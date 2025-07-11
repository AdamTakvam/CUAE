/* Copyright 1998 by the Massachusetts Institute of Technology.
 *
 * Permission to use, copy, modify, and distribute this
 * software and its documentation for any purpose and without
 * fee is hereby granted, provided that the above copyright
 * notice appear in all copies and that both that copyright
 * notice and this permission notice appear in supporting
 * documentation, and that the name of M.I.T. not be used in
 * advertising or publicity pertaining to distribution of the
 * software without specific, written prior permission.
 * M.I.T. makes no representations about the suitability of
 * this software for any purpose.  It is provided "as is"
 * without express or implied warranty.
 */


#include <sys/types.h>
#include <assert.h>

#ifndef WIN32
#include <sys/socket.h>
#include <sys/uio.h>
#include <netinet/in.h>
#ifndef __CYGWIN__
#  include <arpa/nameser.h>
#endif
#include <unistd.h>
#endif

#include <string.h>
#include <stdlib.h>
#include <fcntl.h>
#include <time.h>
#include <errno.h>
#include "ares.h"
#include "ares_dns.h"
#include "ares_private.h"
#include "ares_local.h"

#ifdef WIN32
static int getErrno() { return WSAGetLastError(); }
#else
static int getErrno() { return errno; }
#endif

static void write_tcp_data(ares_channel channel, fd_set *write_fds,
			   time_t now);
static void read_tcp_data(ares_channel channel, fd_set *read_fds, time_t now);
static void read_udp_packets(ares_channel channel, fd_set *read_fds,
			     time_t now);
static void process_timeouts(ares_channel channel, time_t now);
static void process_answer(ares_channel channel, unsigned char *abuf,
			   int alen, int whichserver, int tcp, time_t now);
static void handle_error(ares_channel channel, int whichserver, time_t now);
static void next_server(ares_channel channel, struct query *query, time_t now);
static int open_tcp_socket(ares_channel channel, struct server_state *server);
static int open_udp_socket(ares_channel channel, struct server_state *server);
static int same_questions(const unsigned char *qbuf, int qlen,
			  const unsigned char *abuf, int alen);
static void end_query(ares_channel channel, struct query *query, int status,
		      unsigned char *abuf, int alen);

/* Something interesting happened on the wire, or there was a timeout.
 * See what's up and respond accordingly.
 */
void ares_process(ares_channel channel, fd_set *read_fds, fd_set *write_fds)
{
  time_t now;

  time(&now);
  write_tcp_data(channel, write_fds, now);
  read_tcp_data(channel, read_fds, now);
  read_udp_packets(channel, read_fds, now);
  process_timeouts(channel, now);

  /* See if our local pseudo-db has any results. */
  /* Querying this only on timeouts is OK (is not high-performance) */
  ares_local_process_requests();
}

/* If any TCP sockets select true for writing, write out queued data
 * we have for them.
 */
static void write_tcp_data(ares_channel channel, fd_set *write_fds, time_t now)
{
  struct server_state *server;
  struct send_request *sendreq;
#ifdef WIN32
  WSABUF *vec;
#else
  struct iovec *vec;
#endif
  int i, n, count;

  for (i = 0; i < channel->nservers; i++)
    {
      /* Make sure server has data to send and is selected in write_fds. */
      server = &channel->servers[i];
      if (!server->qhead || server->tcp_socket == -1
	  || !FD_ISSET(server->tcp_socket, write_fds))
	continue;

      /* Count the number of send queue items. */
      n = 0;
      for (sendreq = server->qhead; sendreq; sendreq = sendreq->next)
	n++;

#ifdef WIN32
      /* Allocate iovecs so we can send all our data at once. */
      vec = malloc(n * sizeof(WSABUF));
      if (vec)
	{
		int err;
	  /* Fill in the iovecs and send. */
	  n = 0;
	  for (sendreq = server->qhead; sendreq; sendreq = sendreq->next)
	    {
	      vec[n].buf = (char *) sendreq->data;
	      vec[n].len = sendreq->len;
	      n++;
	    }
	  err = WSASend(server->tcp_socket, vec, n, &count,0,0,0 );
	  if ( err == SOCKET_ERROR )
	  {
		  count =-1;
	  }
	  free(vec);
#else
	     /* Allocate iovecs so we can send all our data at once. */
      vec = malloc(n * sizeof(struct iovec));
      if (vec)
	{
			// int err;
	  /* Fill in the iovecs and send. */
	  n = 0;
	  for (sendreq = server->qhead; sendreq; sendreq = sendreq->next)
	    {
	      vec[n].iov_base = (char *) sendreq->data;
	      vec[n].iov_len = sendreq->len;
	      n++;
	    }
	  count = writev(server->tcp_socket, vec, n);
	  free(vec);
#endif

	  if (count < 0)
	    {
	      handle_error(channel, i, now);
	      continue;
	    }

	  /* Advance the send queue by as many bytes as we sent. */
	  while (count)
	    {
	      sendreq = server->qhead;
	      if (count >= sendreq->len)
		{
		  count -= sendreq->len;
		  server->qhead = sendreq->next;
		  if (server->qhead == NULL)
		    server->qtail = NULL;
		  free(sendreq);
		}
	      else
		{
		  sendreq->data += count;
		  sendreq->len -= count;
		  break;
		}
	    }
	}
      else
	{
	  /* Can't allocate iovecs; just send the first request. */
	  sendreq = server->qhead;
#ifndef UNDER_CE
	  count = write(server->tcp_socket, sendreq->data, sendreq->len);
#else
          count = send(server->tcp_socket, sendreq->data, sendreq->len,0);
#endif
	  if (count < 0)
	    {
	      handle_error(channel, i, now);
	      continue;
	    }

	  /* Advance the send queue by as many bytes as we sent. */
	  if (count == sendreq->len)
	    {
	      server->qhead = sendreq->next;
	      if (server->qhead == NULL)
		server->qtail = NULL;
	      free(sendreq);
	    }
	  else
	    {
	      sendreq->data += count;
	      sendreq->len -= count;
	    }
	}
    }
}

/* If any TCP socket selects true for reading, read some data,
 * allocate a buffer if we finish reading the length word, and process
 * a packet if we finish reading one.
 */
static void read_tcp_data(ares_channel channel, fd_set *read_fds, time_t now)
{
  struct server_state *server;
  int i, count;

  for (i = 0; i < channel->nservers; i++)
    {
      /* Make sure the server has a socket and is selected in read_fds. */
      server = &channel->servers[i];
      if (server->tcp_socket == -1 || !FD_ISSET(server->tcp_socket, read_fds))
	continue;

      if (server->tcp_lenbuf_pos != 2)
	{
	  /* We haven't yet read a length word, so read that (or
	   * what's left to read of it).
	   */
#if defined UNDER_CE || defined WIN32
      count = recv(server->tcp_socket,
                       server->tcp_lenbuf + server->tcp_lenbuf_pos,
                       2 - server->tcp_lenbuf_pos,0);
#else
      count = read(server->tcp_socket,
		       server->tcp_lenbuf + server->tcp_lenbuf_pos,
		       2 - server->tcp_lenbuf_pos);
#endif
	  if (count <= 0)
	    {
	      handle_error(channel, i, now);
	      continue;
	    }

	  server->tcp_lenbuf_pos += count;
	  if (server->tcp_lenbuf_pos == 2)
	    {
	      /* We finished reading the length word.  Decode the
               * length and allocate a buffer for the data.
	       */
	      server->tcp_length = server->tcp_lenbuf[0] << 8
		| server->tcp_lenbuf[1];
	      server->tcp_buffer = malloc(server->tcp_length);
	      if (!server->tcp_buffer)
		handle_error(channel, i, now);
	      server->tcp_buffer_pos = 0;
	    }
	}
      else
	{
	  /* Read data into the allocated buffer. */
#if defined UNDER_CE || defined WIN32
      count = recv(server->tcp_socket,
		       server->tcp_buffer + server->tcp_buffer_pos,
		       server->tcp_length - server->tcp_buffer_pos,0);
#else
      count = read(server->tcp_socket,
		       server->tcp_buffer + server->tcp_buffer_pos,
		       server->tcp_length - server->tcp_buffer_pos);
#endif

	  if (count <= 0)
	    {
	      handle_error(channel, i, now);
	      continue;
	    }

	  server->tcp_buffer_pos += count;
	  if (server->tcp_buffer_pos == server->tcp_length)
	    {
	      /* We finished reading this answer; process it and
               * prepare to read another length word.
	       */
	      process_answer(channel, server->tcp_buffer, server->tcp_length,
			     i, 1, now);
	      free(server->tcp_buffer);
	      server->tcp_buffer = NULL;
	      server->tcp_lenbuf_pos = 0;
	    }
	}
    }
}

/* If any UDP sockets select true for reading, process them. */
static void read_udp_packets(ares_channel channel, fd_set *read_fds,
			     time_t now)
{
  struct server_state *server;
  int i, count;
  unsigned char buf[PACKETSZ + 1];

  for (i = 0; i < channel->nservers; i++)
    {
      /* Make sure the server has a socket and is selected in read_fds. */
      server = &channel->servers[i];
      if ( (server->udp_socket == -1) || (!FD_ISSET(server->udp_socket, read_fds) ))
	  {
	     continue;
	  }

	  assert( server->udp_socket != -1 );
	  
      count = recv(server->udp_socket, buf, sizeof(buf), 0);
      if (count <= 0)
	  {
#if defined(WIN32)
		//int err;
		//err = WSAGetLastError();
		//err = errno;
		switch (getErrno())
		{
			case WSAEWOULDBLOCK: 
               FD_CLR(server->udp_socket, read_fds);
               continue;
			case WSAECONNABORTED:
				break;
			case WSAECONNRESET: // got an ICMP error on a previous send 
				break;
		}
#endif
		handle_error(channel, i, now);
	  }
	  else
	  {
		process_answer(channel, buf, count, i, 0, now);
	  }
    }
}

/* If any queries have timed out, note the timeout and move them on. */
static void process_timeouts(ares_channel channel, time_t now)
{
  struct query *query, *next;

  for (query = channel->queries; query; query = next)
    {
      next = query->next;
      if (query->timeout != 0 && now >= query->timeout)
	{
	  query->error_status = ARES_ETIMEOUT;
	  next_server(channel, query, now);
	}
    }
}

/* Handle an answer from a server. */
static void process_answer(ares_channel channel, unsigned char *abuf,
			   int alen, int whichserver, int tcp, time_t now)
{
  int id, tc, rcode;
  struct query *query;

  /* If there's no room in the answer for a header, we can't do much
   * with it. */
  if (alen < HFIXEDSZ)
    return;

  /* Grab the query ID, truncate bit, and response code from the packet. */
  id = DNS_HEADER_QID(abuf);
  tc = DNS_HEADER_TC(abuf);
  rcode = DNS_HEADER_RCODE(abuf);

  /* Find the query corresponding to this packet. */
  for (query = channel->queries; query; query = query->next)
    {
      if (query->qid == id)
	break;
    }
  if (!query)
    return;

  /* If we got a truncated UDP packet and are not ignoring truncation,
   * don't accept the packet, and switch the query to TCP if we hadn't
   * done so already.
   */
  if ((tc || alen > PACKETSZ) && !tcp && !(channel->flags & ARES_FLAG_IGNTC))
    {
      if (!query->using_tcp)
	{
	  query->using_tcp = 1;
	  ares__send_query(channel, query, now);
	}
      return;
    }

  /* Limit alen to PACKETSZ if we aren't using TCP (only relevant if we
   * are ignoring truncation.
   */
  if (alen > PACKETSZ && !tcp)
    alen = PACKETSZ;

  /* If we aren't passing through all error packets, discard packets
   * with SERVFAIL, NOTIMP, or REFUSED response codes.
   */
  if (!(channel->flags & ARES_FLAG_NOCHECKRESP))
    {
      if (rcode == SERVFAIL || rcode == NOTIMP || rcode == REFUSED)
	{
	  query->skip_server[whichserver] = 1;
	  if (query->server == whichserver)
	    next_server(channel, query, now);
	  return;
	}
      if (!same_questions((unsigned char*)query->qbuf, query->qlen, abuf, alen))
	{
	  if (query->server == whichserver)
	    next_server(channel, query, now);
	  return;
	}
    }

  end_query(channel, query, ARES_SUCCESS, abuf, alen);
}

static void handle_error(ares_channel channel, int whichserver, time_t now)
{
  struct query *query;

  /* Reset communications with this server. */
  ares__close_sockets(&channel->servers[whichserver]);

  /* Tell all queries talking to this server to move on and not try
   * this server again.
   */
  for (query = channel->queries; query != 0; query = query->next)
    {
		assert( query != 0 );
		assert( channel->queries != 0 );

      if (query->server == whichserver)
	{
	  query->skip_server[whichserver] = 1;
#if 0 // !cj! - this seem to corrput memory when it is called 
	  next_server(channel, query, now);
#endif
	}
    }
}

static void next_server(ares_channel channel, struct query *query, time_t now)
{
  /* Advance to the next server or try. */
  query->server++;
  for (; query->itry < channel->tries; query->itry++)
    {
      for (; query->server < channel->nservers; query->server++)
	{
	  if (!query->skip_server[query->server])
	    {
	      ares__send_query(channel, query, now);
	      return;
	    }
	}
      query->server = 0;

      /* Only one try if we're using TCP. */
      if (query->using_tcp)
	break;
    }
  end_query(channel, query, query->error_status, NULL, 0);
}

void ares__send_query(ares_channel channel, struct query *query, time_t now)
{
  struct send_request *sendreq;
  struct server_state *server;

  server = &channel->servers[query->server];
  if (query->using_tcp)
    {
      /* Make sure the TCP socket for this server is set up and queue
       * a send request.
       */
      if (server->tcp_socket == -1)
	{
	  if (open_tcp_socket(channel, server) == -1)
	    {
	      query->skip_server[query->server] = 1;
	      next_server(channel, query, now);
	      return;
	    }
	}
      sendreq = malloc(sizeof(struct send_request));
      if (!sendreq)
	end_query(channel, query, ARES_ENOMEM, NULL, 0);
      sendreq->data = query->tcpbuf;
      sendreq->len = query->tcplen;
      sendreq->next = NULL;
      if (server->qtail)
	server->qtail->next = sendreq;
      else
	server->qhead = sendreq;
      server->qtail = sendreq;
      query->timeout = 0;
    }
  else
    {
      if (server->udp_socket == -1)
	{
	  if (open_udp_socket(channel, server) == -1)
	    {
	      query->skip_server[query->server] = 1;
	      next_server(channel, query, now);
	      return;
	    }
	}
      if (send(server->udp_socket, query->qbuf, query->qlen, 0) == -1)
	{
	  query->skip_server[query->server] = 1;
	  next_server(channel, query, now);
	  return;
	}
      query->timeout = now
	  + ((query->itry == 0) ? channel->timeout
	     : channel->timeout << query->itry / channel->nservers);
    }
}

static int open_tcp_socket(ares_channel channel, struct server_state *server)
{
  int s;
  struct sockaddr_in sin;
#ifdef USE_IPV6
  struct sockaddr_in6 sin6;
#endif

  /* Acquire a socket. */
#ifdef USE_IPV6
  assert(server->family == AF_INET || server->family == AF_INET6);
  s = socket(server->family, SOCK_STREAM, 0);
#else
  s = socket(AF_INET, SOCK_STREAM, 0);
#endif
  if (s == -1)
    return -1;

  /* Set the socket non-blocking. */
#ifdef WIN32
  {
	unsigned long noBlock = 1;
	int errNoBlock = ioctlsocket( s, FIONBIO , &noBlock );
	if ( errNoBlock != 0 )
	{
		return -1;
	}
  }
#else
  {
	  int flags;
  if (fcntl(s, F_GETFL, &flags) == -1)
    {
      ares__kill_socket(s);
      return -1;
    }
  flags |= O_NONBLOCK; // Fixed evil but here - used to be &=
  if (fcntl(s, F_SETFL, flags) == -1)
    {
      ares__kill_socket(s);
      return -1;
    }
}
#endif

#ifdef WIN32
#define PORTABLE_INPROGRESS_ERR WSAEWOULDBLOCK
#else
#define PORTABLE_INPROGRESS_ERR EINPROGRESS
#endif

  /* Connect to the server. */
#ifdef USE_IPV6
  if (server->family == AF_INET6)
  {
    memset(&sin6, 0, sizeof(sin6));
    sin6.sin6_family = AF_INET6;
    sin6.sin6_addr = server->addr6;
    sin6.sin6_port = channel->tcp_port;
    sin6.sin6_flowinfo = 0;
    sin6.sin6_scope_id = 0;
    // do i need to explicitly set the length?

    if (connect(s, (const struct sockaddr *) &sin6 , sizeof(sin6)) == -1
           && getErrno() != PORTABLE_INPROGRESS_ERR)
    {
      ares__kill_socket(s);
      return -1;
    }
  }
  else // IPv4 DNS server
  {
    memset(&sin, 0, sizeof(sin));
    sin.sin_family = AF_INET;
    sin.sin_addr = server->addr;
    sin.sin_port = channel->tcp_port;    

    if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) == -1 && getErrno() != PORTABLE_INPROGRESS_ERR)
    {
      ares__kill_socket(s);
      return -1;
    }
  }
#else
  memset(&sin, 0, sizeof(sin));
  sin.sin_family = AF_INET;
  sin.sin_addr = server->addr;
  sin.sin_port = channel->tcp_port;

  if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) == -1
         && getErrno() != PORTABLE_INPROGRESS_ERR)
  {
    ares__kill_socket(s);
    return -1;
  }
#endif

  server->tcp_socket = s;
  return 0;
}

static int open_udp_socket(ares_channel channel, struct server_state *server)
{
  int s;

#ifdef USE_IPV6
  // added by Rohan 7-Sept-2004
  // should really replace sockaddr_in6 with sockaddr_storage
  struct sockaddr_in6 sin6;
  struct sockaddr_in sin;


  /* Acquire a socket. */
  assert(server->family == AF_INET || server->family == AF_INET6);
  s = socket(server->family, SOCK_DGRAM, 0);
#ifdef WIN32
  {   
	 unsigned long errNoBlock = 1;
     errNoBlock = ioctlsocket( s, FIONBIO , &errNoBlock );
     if ( errNoBlock != 0 )
     {
        return -1;
     }
  }
#endif
  if (s == -1)
    return -1;

  /* Connect to the server. */
  if (server->family == AF_INET6)
  {
    memset(&sin6, 0, sizeof(sin6));
    sin6.sin6_family = AF_INET6;
    sin6.sin6_addr = server->addr6;
    sin6.sin6_port = channel->udp_port;
    sin6.sin6_flowinfo = 0;
    sin6.sin6_scope_id = 0;
    // do i need to explicitly set the length?

    if (connect(s, (const struct sockaddr *) &sin6, sizeof(sin6)) == -1)
    {
      ares__kill_socket(s);
      return -1;
    }
  }
  else // IPv4 DNS server
  {
    memset(&sin, 0, sizeof(sin));
    sin.sin_family = AF_INET;
    sin.sin_addr = server->addr;
    sin.sin_port = channel->udp_port;

    if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) == -1)
    {
      ares__kill_socket(s);
      return -1;
    }
  }

#else
  struct sockaddr_in sin;

  /* Acquire a socket. */
  s = socket(AF_INET, SOCK_DGRAM, 0);

  if (s == -1)
    return -1;

  /* Connect to the server. */
  memset(&sin, 0, sizeof(sin));
  sin.sin_family = AF_INET;
  sin.sin_addr = server->addr;
  sin.sin_port = channel->udp_port;

  if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) == -1)
    {
      ares__kill_socket(s);
      return -1;
    }
#endif
  if(channel->socket_function)
  {
     channel->socket_function(s, 1, __FILE__, __LINE__);
  }
    
  server->udp_socket = s;
  return 0;
}

static int same_questions(const unsigned char *qbuf, int qlen,
			  const unsigned char *abuf, int alen)
{
  struct {
    const unsigned char *p;
    int qdcount;
    char *name;
    int namelen;
    int type;
    int dnsclass;
  } q, a;
  int i, j;

  if (qlen < HFIXEDSZ || alen < HFIXEDSZ)
    return 0;

  /* Extract qdcount from the request and reply buffers and compare them. */
  q.qdcount = DNS_HEADER_QDCOUNT(qbuf);
  a.qdcount = DNS_HEADER_QDCOUNT(abuf);
  if (q.qdcount != a.qdcount)
    return 0;

  /* For each question in qbuf, find it in abuf. */
  q.p = qbuf + HFIXEDSZ;
  for (i = 0; i < q.qdcount; i++)
    {
      /* Decode the question in the query. */
      if (ares_expand_name(q.p, qbuf, qlen, &q.name, &q.namelen)
	  != ARES_SUCCESS)
	return 0;
      q.p += q.namelen;
      if (q.p + QFIXEDSZ > qbuf + qlen)
	{
	  free(q.name);
	  return 0;
	}
      q.type = DNS_QUESTION_TYPE(q.p);
      q.dnsclass = DNS_QUESTION_CLASS(q.p);
      q.p += QFIXEDSZ;

      /* Search for this question in the answer. */
      a.p = abuf + HFIXEDSZ;
      for (j = 0; j < a.qdcount; j++)
	{
	  /* Decode the question in the answer. */
	  if (ares_expand_name(a.p, abuf, alen, &a.name, &a.namelen)
	      != ARES_SUCCESS)
	    {
	      free(q.name);
	      return 0;
	    }
	  a.p += a.namelen;
	  if (a.p + QFIXEDSZ > abuf + alen)
	    {
	      free(q.name);
	      free(a.name);
	      return 0;
	    }
	  a.type = DNS_QUESTION_TYPE(a.p);
	  a.dnsclass = DNS_QUESTION_CLASS(a.p);
	  a.p += QFIXEDSZ;

	  /* Compare the decoded questions. */
	  if (strcasecmp(q.name, a.name) == 0 && q.type == a.type
	      && q.dnsclass == a.dnsclass)
	    {
	      free(a.name);
	      break;
	    }
	  free(a.name);
	}

      free(q.name);
      if (j == a.qdcount)
	return 0;
    }
  return 1;
}

static void end_query(ares_channel channel, struct query *query, int status,
		      unsigned char *abuf, int alen)
{
  struct query **q;
  int i;

  query->callback(query->arg, status, abuf, alen);
  for (q = &channel->queries; *q; q = &(*q)->next)
    {
      if (*q == query)
	break;
    }
  *q = query->next;
  free(query->tcpbuf);
  free(query->skip_server);
  free(query);

  /* Simple cleanup policy: if no queries are remaining, close all
   * network sockets unless STAYOPEN is set.
   */
  if (!channel->queries && !(channel->flags & ARES_FLAG_STAYOPEN))
    {
      for (i = 0; i < channel->nservers; i++)
	ares__close_sockets(&channel->servers[i]);
    }
}

void ares__kill_socket(int s)
{
#ifdef WIN32
   closesocket(s);
#else
   close(s);
#endif
}

