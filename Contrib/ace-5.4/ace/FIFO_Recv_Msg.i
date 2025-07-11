/* -*- C++ -*- */
// FIFO_Recv_Msg.i,v 4.6 2003/11/01 11:15:12 dhinton Exp

// FIFO_Recv_Msg.i

#include "ace/Min_Max.h"

ASYS_INLINE ssize_t
ACE_FIFO_Recv_Msg::recv (ACE_Str_Buf &recv_msg)
{
  ACE_TRACE ("ACE_FIFO_Recv_Msg::recv");
#if defined (ACE_HAS_STREAM_PIPES)
  int i = 0;
  if (ACE_OS::getmsg (this->get_handle (),
                      (strbuf *) 0,
                      (strbuf *) &recv_msg,
                      &i) == -1)
    return -1;
  else 
    return recv_msg.len;
#else /* Do the ol' 2-read trick... */
  if (ACE_OS::read (this->get_handle (), 
		    (char *) &recv_msg.len, 
		    sizeof recv_msg.len) != sizeof recv_msg.len)
    return -1;
  else
    {
      size_t remaining = ACE_static_cast (size_t, recv_msg.len);
      size_t requested = ACE_static_cast (size_t, recv_msg.maxlen);
      ssize_t recv_len = ACE_OS::read (this->get_handle (), 
                                       (char *) recv_msg.buf, 
                                       ACE_MIN (remaining, requested));
      if (recv_len == -1)
        return -1;
      // Tell caller what's really in the buffer.
      recv_msg.len = ACE_static_cast (int, recv_len);

      // If there are more bytes remaining in the message, read them and
      // throw them away. Leaving them in the FIFO would make it difficult
      // to find the start of the next message in the fifo.
      // Since the ACE_HAS_STREAM_PIPES version of this method doesn't
      // return getmsg()'s indication of "data remaining", don't worry about
      // saving the indication here either to read the remainder later.
      size_t total_msg_size = remaining;
      remaining -= recv_len;
      while (remaining > 0)
        {
          const size_t throw_away = 1024;
          char dev_null[throw_away];
          recv_len = ACE_OS::read (this->get_handle (),
                                   dev_null,
                                   ACE_MIN (remaining, throw_away));
          if (recv_len == -1)
            break;
          remaining -= recv_len;
        }
      return total_msg_size;
    }
#endif /* ACE_HAS_STREAM_PIPES */
}

ASYS_INLINE ssize_t
ACE_FIFO_Recv_Msg::recv (void *buf, size_t max_len)
{
  ACE_TRACE ("ACE_FIFO_Recv_Msg::recv");
  ACE_Str_Buf recv_msg ((char *) buf, 0, ACE_static_cast (int, max_len));

  return this->recv (recv_msg);
}

#if defined (ACE_HAS_STREAM_PIPES)
ASYS_INLINE ssize_t
ACE_FIFO_Recv_Msg::recv (ACE_Str_Buf *data,
			 ACE_Str_Buf *cntl,
			 int *flags)
{
  ACE_TRACE ("ACE_FIFO_Recv_Msg::recv");
  if (ACE_OS::getmsg (this->get_handle (), 
                      (strbuf *) cntl,
                      (strbuf *) data,
                      flags) == -1)
    return -1;
  else
    return (cntl == 0 ? 0 : cntl->len) + (data == 0 ? 0 : data->len);
}

ASYS_INLINE ssize_t
ACE_FIFO_Recv_Msg::recv (int *band,
			 ACE_Str_Buf *data,
			 ACE_Str_Buf *cntl,
			 int *flags)
{
  ACE_TRACE ("ACE_FIFO_Recv_Msg::recv");
  if (ACE_OS::getpmsg (this->get_handle (), 
                       (strbuf *) cntl,
                       (strbuf *) data,
                       band,
                       flags) == -1)
    return -1;
  else
    return (cntl == 0 ? 0 : cntl->len) + (data == 0 ? 0 : data->len);
}
#endif /* ACE_HAS_STREAM_PIPES */
