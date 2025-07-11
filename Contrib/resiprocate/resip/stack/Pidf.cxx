#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include "resip/stack/Pidf.hxx"
#include "resip/stack/SipMessage.hxx"
#include "resip/stack/Symbols.hxx"
#include "resip/stack/XMLCursor.hxx"
#include "rutil/Logger.hxx"
#include "rutil/Inserter.hxx"
#include "rutil/WinLeakCheck.hxx"

using namespace resip;
using namespace std;

#define RESIPROCATE_SUBSYSTEM Subsystem::SIP

bool
Pidf::init()
{
   static ContentsFactory<Pidf> factory;
   (void)factory;
   return true;
}

Pidf::Pidf()
   : Contents(getStaticType())
{}

Pidf::Pidf(const Mime& contentType)
   : Contents(getStaticType())
{}

Pidf::Pidf(HeaderFieldValue* hfv, const Mime& contentsType)
   : Contents(hfv, contentsType)
{
}
 
Pidf::Pidf(const Pidf& rhs)
   : Contents(rhs),
     mEntity(rhs.mEntity),
     mNote(rhs.mNote),
     mTuples(rhs.mTuples)
{
}

Pidf::Pidf(const Uri& entity)
   : Contents(getStaticType()),
     mEntity(entity)
{}

Pidf::~Pidf()
{
}

void 
Pidf::setEntity(const Uri& entity)
{
   checkParsed();
   mEntity = entity;
}

const Uri& 
Pidf::getEntity() const
{ 
   checkParsed();
   return mEntity; 
};

std::vector<Pidf::Tuple>&
Pidf::getTuples()
{
   checkParsed();
   return mTuples;
}

const std::vector<Pidf::Tuple>&
Pidf::getTuples() const
{
   checkParsed();
   return mTuples;
}

int
Pidf::getNumTuples() const
{
   checkParsed();
   return mTuples.size();
}

Pidf&
Pidf::operator=(const Pidf& rhs)
{
   if (this != &rhs)
   {
      Contents::operator=(rhs);
      mNote = rhs.mNote;
      mEntity = rhs.mEntity;
      mTuples = rhs.mTuples;
   }
   return *this;
}

Contents* 
Pidf::clone() const
{
   return new Pidf(*this);
}

const Mime& 
Pidf::getStaticType() 
{
   static Mime type("application","pidf+xml");
   return type;
}

std::ostream& 
Pidf::encodeParsed(std::ostream& str) const
{
   //DebugLog(<< "Pidf::encodeParsed " << mText);
   //str << mText;

   str << "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" << Symbols::CRLF;
   str << "<presence xmlns=\"urn:ietf:params:xml:ns:pidf\"" << Symbols::CRLF;
   str << "          entity=\"" << mEntity << "\">"  <<  Symbols::CRLF;
   for (vector<Tuple>::const_iterator i = mTuples.begin(); i != mTuples.end(); ++i)
   {
      Data status( (char*)( (i->status) ? "open" : "closed" ) );
      str << "  <tuple id=\"" << i->id << "\" ";

      XMLCursor::encode(str, i->attributes);
      str << ">" << Symbols::CRLF;
      str << "     <status><basic>" << status << "</basic></status>" << Symbols::CRLF;
      if ( !i->contact.empty() )
      {
         str << "     <contact priority=\"" << i->contactPriority << "\">" << i->contact << "</contact>" << Symbols::CRLF;
      }
      if ( !i->timeStamp.empty() )
      {
         str << "     <timestamp>" << i->timeStamp << "</timestamp>" << Symbols::CRLF;
      }
      if ( !i->note.empty() )
      {
         str << "     <note>" << i->note << "</note>" << Symbols::CRLF;
      }
      str << "  </tuple>" << Symbols::CRLF;
   }
   str << "</presence>" << Symbols::CRLF;
   
   return str;
}

void 
Pidf::parse(ParseBuffer& pb)
{
   DebugLog(<< "Pidf::parse(" << Data(pb.start(), int(pb.end()-pb.start())) << ") ");

   XMLCursor xml(pb);

   if (xml.getTag() == "presence")
   {
      XMLCursor::AttributeMap::const_iterator i = xml.getAttributes().find("entity");
      if (i != xml.getAttributes().end())
      {
         mEntity = Uri(i->second);
      }
      else
      {
         DebugLog(<< "no entity!");
      }
      
      if (xml.firstChild())
      {
         do
         {
            if (xml.getTag() == "tuple")
            {
               Tuple t;
               t.attributes = xml.getAttributes();
               XMLCursor::AttributeMap::const_iterator i = xml.getAttributes().find("id");
               if (i != xml.getAttributes().end())
               {
                  t.id = i->second;
                  t.attributes.erase("id");
               }
               
               // look for status, contacts, notes -- take last of each for now
               if (xml.firstChild())
               {
                  do
                  {
                     if (xml.getTag() == "status")
                     {
                        // look for basic
                        if (xml.firstChild())
                        {
                           do
                           {
                              if (xml.getTag() == "basic")
                              {
                                 if (xml.firstChild())
                                 {
                                    t.status = (xml.getValue() == "open");
                                    xml.parent();
                                 }
                              }
                           } while (xml.nextSibling());
                           xml.parent();
                        }
                     }
                     else if (xml.getTag() == "contact")
                     {
                        XMLCursor::AttributeMap::const_iterator i = xml.getAttributes().find("priority");
                        if (i != xml.getAttributes().end())
                        {
                           t.contactPriority = float( i->second.convertDouble() );
                        }
                        if (xml.firstChild())
                        {
                           t.contact = xml.getValue();
                           xml.parent();
                        }
                     }
                     else if (xml.getTag() == "note")
                     {
                        if (xml.firstChild())
                        {
                           t.note = xml.getValue();
                           xml.parent();
                        }
                     }
                     else if (xml.getTag() == "timestamp")
                     {
                        if (xml.firstChild())
                        {
                           t.timeStamp = xml.getValue();
                           xml.parent();
                        }
                     }
                  } while (xml.nextSibling());
                  xml.parent();
               }
               
               mTuples.push_back(t);
            }
         } while (xml.nextSibling());
         xml.parent();
      }
   }
   else
   {
      DebugLog(<< "no presence tag!");
   }
}

void 
Pidf::setSimpleId(const Data& id)
{
   checkParsed();
   if (mTuples.empty())
   {
      Tuple t;
      mTuples.push_back(t);
   }
   mTuples[0].id = id;
}

void 
Pidf::setSimpleStatus( bool online, const Data& note, const Data& contact )
{
   checkParsed();
   if (mTuples.empty())
   {
      Tuple t;
      mTuples.push_back(t);
   }

   mTuples[0].status = online;
   mTuples[0].contact = contact;
   mTuples[0].contactPriority = 1.0;
   mTuples[0].note = note;
   mTuples[0].timeStamp = Data::Empty;
}

bool 
Pidf::getSimpleStatus(Data* note) const
{
   checkParsed();

   if (!mTuples.empty())
   {
      if (note)
      {
         *note = mTuples[0].note;
      }
   
      return mTuples[0].status;
   }
   return false;
}

void 
Pidf::merge(const Pidf& other)
{
   vector<Tuple>& tuples = getTuples();
   tuples.reserve(tuples.size() + other.getTuples().size());

   setEntity(other.mEntity);

   for (vector<Tuple>::const_iterator i = other.getTuples().begin();
        i != other.getTuples().end(); ++i)
   {
      bool found = false;
      for (vector<Tuple>::iterator j = getTuples().begin();
           j != getTuples().end(); ++j)
      {
         if (i->id == j->id)
         {
            found = true;
            *j = *i;
            break;
         }
      }
      if (!found)
      {
         tuples.push_back(*i);
      }
   }
}

std::ostream& 
resip::operator<<(std::ostream& strm, const Pidf::Tuple& tuple)
{
   strm << "Tuple [" 
        << " status=" << tuple.status
        << " id=" << tuple.id
        << " contact=" << tuple.contact
        << " attributes=" << Inserter(tuple.attributes);
   return strm;
}


/* ====================================================================
 * The Vovida Software License, Version 1.0 
 * 
 * Copyright (c) 2000 Vovida Networks, Inc.  All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 * 
 * 3. The names "VOCAL", "Vovida Open Communication Application Library",
 *    and "Vovida Open Communication Application Library (VOCAL)" must
 *    not be used to endorse or promote products derived from this
 *    software without prior written permission. For written
 *    permission, please contact vocal@vovida.org.
 *
 * 4. Products derived from this software may not be called "VOCAL", nor
 *    may "VOCAL" appear in their name, without prior written
 *    permission of Vovida Networks, Inc.
 * 
 * THIS SOFTWARE IS PROVIDED "AS IS" AND ANY EXPRESSED OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND
 * NON-INFRINGEMENT ARE DISCLAIMED.  IN NO EVENT SHALL VOVIDA
 * NETWORKS, INC. OR ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT DAMAGES
 * IN EXCESS OF $1,000, NOR FOR ANY INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
 * OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 * 
 * ====================================================================
 * 
 * This software consists of voluntary contributions made by Vovida
 * Networks, Inc. and many individuals on behalf of Vovida Networks,
 * Inc.  For more information on Vovida Networks, Inc., please see
 * <http://www.vovida.org/>.
 *
 */
