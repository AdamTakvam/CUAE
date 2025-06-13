#ifndef __QUEUE__H__INCLUDED_
#define __QUEUE__H__INCLUDED_

#include "stdio.h" // for NULL declaration
/*
Queue.h - header file for queue class ( FIFO list )
SB Software 1998-2006

  Usage :
  Queue<CString> MyQueue;
  ...
  MyQueue.Push("Mr. ");
  MyQueue.Push("Bill ");
  MyQueue.Push("Gates ");
  ...
  CString sName=MyQueue.Pop()+MyQueue.Pop()+MyQueue.Pop()+" !!!";

  // sName="Mr. Bill Gates  !!!"

*/


template <class Type> class Queue;			// forward declaration
template <class Type> class _queue_item {
	Type value;
	_queue_item *next;
	_queue_item() { next=NULL;};
	_queue_item(const Type &val) { 
		value=val;
		next=NULL;
	};
	~_queue_item() {};
	void DeleteQueue() {					// delete all items
		if(next!=NULL) next->DeleteQueue();
		delete this;
	};
	friend class Queue<Type>;				// the one and the only one friend 
											// who can use this class
};

template <class Type> class Queue {
	_queue_item<Type> *first;				// head pointer
	_queue_item<Type> *last;				// Tail pointer
	typedef int BOOL;
#ifndef TRUE
	enum
	{
		FALSE=0,
		TRUE=1
	};
#endif
public:
	BOOL Push(const Type & item) {					// Add element at the end of queue
		_queue_item<Type> *next=new _queue_item<Type> (item);
		if (first==NULL) 
			last=first=next;
		else {
			last->next=next;
			last=next;
		}
		return TRUE;
	};
	Type Pop() {							// Retrieve element from the head of queue, and remove it from queue
		Type val;
		_queue_item<Type> *oldFirst=first;
		if(first==NULL)
			throw "SB Queue underflow !";
		val=first->value;
		first=first->next;
		delete oldFirst;
		return val;
	};
	BOOL IsEmpty() {						// check if queue is empty
		return (first==NULL);
	};
	void Reset() {							// reset contents of the queue
		if(first!=NULL) {
			first->DeleteQueue();
			first=NULL;
		}
	};
	Queue() { first=last=NULL;};
	~Queue() {								// destructor - deletes all
		if(first!=NULL)
			first->DeleteQueue();
	};
};

#endif // __QUEUE__H__INCLUDED_
