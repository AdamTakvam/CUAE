/* -*- C++ -*- */
// MEM_IO.i,v 4.7 2003/11/01 11:15:13 dhinton Exp

// MEM_IO.i

#include "ace/OS_NS_string.h"

ASYS_INLINE
ACE_Reactive_MEM_IO::ACE_Reactive_MEM_IO ()
{
}

#if defined (ACE_WIN32) || !defined (_ACE_USE_SV_SEM)
ASYS_INLINE
ACE_MT_MEM_IO::Simple_Queue::Simple_Queue (void)
  : mq_ (0),
    malloc_ (0)
{
}

ASYS_INLINE
ACE_MT_MEM_IO::ACE_MT_MEM_IO ()
{
  this->recv_channel_.sema_ = 0;
  this->recv_channel_.lock_ = 0;
  this->send_channel_.sema_ = 0;
  this->send_channel_.lock_ = 0;
}

ASYS_INLINE
ACE_MT_MEM_IO::Simple_Queue::Simple_Queue (MQ_Struct *mq)
  : mq_ (mq),
    malloc_ (0)
{
}

ASYS_INLINE int
ACE_MT_MEM_IO::Simple_Queue::init (MQ_Struct *mq,
                                   ACE_MEM_SAP::MALLOC_TYPE *malloc)
{
  if (this->mq_ != 0)
    return -1;

  this->mq_ = mq;
  this->malloc_ = malloc;
  return 0;
}
#endif /* ACE_WIN32 || !_ACE_USE_SV_SEM */

ASYS_INLINE ssize_t
ACE_Reactive_MEM_IO::get_buf_len (const off_t off, ACE_MEM_SAP_Node *&buf)
{
#if !defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
  ACE_TRACE ("ACE_Reactive_MEM_IO::get_buf_len");
#endif /* ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS */

  if (this->shm_malloc_ == 0)
    return -1;

  ssize_t retv = 0;

  ACE_SEH_TRY
    {
      buf = ACE_reinterpret_cast (ACE_MEM_SAP_Node *,
                                  (ACE_static_cast(char *,
                                                   this->shm_malloc_->base_addr ())
                                   + off));
      retv = buf->size ();
    }
  ACE_SEH_EXCEPT (this->shm_malloc_->memory_pool ().seh_selector (GetExceptionInformation ()))
    {
    }

  return retv;
}

// Send an n byte message to the connected socket.
ASYS_INLINE
ACE_MEM_IO::ACE_MEM_IO (void)
  : deliver_strategy_ (0),
    recv_buffer_ (0),
    buf_size_ (0),
    cur_offset_ (0)
{
  // ACE_TRACE ("ACE_MEM_IO::ACE_MEM_IO");
}

ASYS_INLINE ssize_t
ACE_MEM_IO::fetch_recv_buf (int flag, const ACE_Time_Value *timeout)
{
  ACE_TRACE ("ACE_MEM_IO::fetch_recv_buf");

  if (this->deliver_strategy_ == 0)
    return -1;

  // This method can only be called when <buf_size_> == <cur_offset_>.
  ACE_ASSERT (this->buf_size_ == this->cur_offset_);

  // We have done using the previous buffer, return it to malloc.
  if (this->recv_buffer_ != 0)
    this->deliver_strategy_->release_buffer (this->recv_buffer_);

  this->cur_offset_ = 0;
  int retv = 0;

  if ((retv = this->deliver_strategy_->recv_buf (this->recv_buffer_,
                                                 flag,
                                                 timeout)) > 0)
    this->buf_size_ = retv;
  else
    this->buf_size_ = 0;

  return retv;
}

ASYS_INLINE
ACE_MEM_IO::~ACE_MEM_IO (void)
{
  delete this->deliver_strategy_;
}

ASYS_INLINE ssize_t
ACE_MEM_IO::send (const void *buf,
                  size_t len,
                  int flags,
                  const ACE_Time_Value *timeout)
{
  ACE_TRACE ("ACE_MEM_IO::send");
  if (this->deliver_strategy_ == 0)
    return 0;

  ACE_MEM_SAP_Node *sbuf = this->deliver_strategy_->acquire_buffer (len);
  if (sbuf == 0)
    return -1;                  // Memory buffer not initialized.
  ACE_OS::memcpy (sbuf->data (), buf, len);

  ///

  sbuf->size_ = len;

  return this->deliver_strategy_->send_buf (sbuf,
                                            flags,
                                            timeout);
}

ASYS_INLINE ssize_t
ACE_MEM_IO::recv (void *buf,
                  size_t len,
                  int flags,
                  const ACE_Time_Value *timeout)
{
  ACE_TRACE ("ACE_MEM_IO::recv");

  size_t count = 0;

//    while (len > 0)
//      {
      size_t buf_len = this->buf_size_ - this->cur_offset_;
      if (buf_len == 0)
        {
          ssize_t blen =         // Buffer length
            this->fetch_recv_buf (flags, timeout);
          if (blen <= 0)
            return blen;
          buf_len = this->buf_size_;
        }

      size_t length = (len > buf_len ? buf_len : len);

      ACE_OS::memcpy ((char *) buf + count,
                      (char *) this->recv_buffer_->data () + this->cur_offset_,
                      length);
      this->cur_offset_ += length;
//        len -= length;
      count += length;
//      }

  return count;
}

ASYS_INLINE ssize_t
ACE_MEM_IO::send (const void *buf, size_t n, int flags)
{
  ACE_TRACE ("ACE_MEM_IO::send");
  return this->send (buf, n, flags, 0);
}

// Recv an n byte message from the connected socket.

ASYS_INLINE ssize_t
ACE_MEM_IO::recv (void *buf, size_t n, int flags)
{
  ACE_TRACE ("ACE_MEM_IO::recv");
  return this->recv (buf, n, flags, 0);
}

// Send an n byte message to the connected socket.

ASYS_INLINE ssize_t
ACE_MEM_IO::send (const void *buf, size_t n)
{
  ACE_TRACE ("ACE_MEM_IO::send");
  return this->send (buf, n, 0);
}

// Recv an n byte message from the connected socket.

ASYS_INLINE ssize_t
ACE_MEM_IO::recv (void *buf, size_t n)
{
  ACE_TRACE ("ACE_MEM_IO::recv");

  return this->recv (buf, n, 0);
}

ASYS_INLINE ssize_t
ACE_MEM_IO::recv (void *buf,
                  size_t len,
                  const ACE_Time_Value *timeout)
{
  ACE_TRACE ("ACE_MEM_IO::recv");
  return this->recv (buf, len, 0, timeout);
}

ASYS_INLINE ssize_t
ACE_MEM_IO::send (const void *buf,
                  size_t len,
                  const ACE_Time_Value *timeout)
{
  ACE_TRACE ("ACE_MEM_IO::send");
  return this->send (buf, len, 0, timeout);
}
