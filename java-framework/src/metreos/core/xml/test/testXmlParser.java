/* $Id: testXmlParser.java,v 1.7 2005/09/16 22:20:50 wert Exp $
 *
 * Created by wert on Jun 5, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.xml.test;

import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import metreos.core.xml.ParseException;
import metreos.core.xml.XmlParser;

/**
 * Description of testXmlParser
 */
public class testXmlParser extends XmlParser
{
	/**
	 * @param args
	 * @throws ParseException 
	 * @throws InterruptedException 
	 */
	public static void main( String[] args ) throws ParseException, InterruptedException
	{
		testXmlParser xp = new testXmlParser();

		parse( xp, "<a>" );
		parse( xp, "</a>" );

		parse( xp, "<a:d>" );
		parse( xp, "</a:d>" );

		parse( xp, "<abc>" );
		parse( xp, "<abc:def>" );
		
		parse( xp, "<abc >" );
		parse( xp, "<abc:def >" );
		
		parse( xp, "<abc/>" );
		parse( xp, "<abc:def/>" );
		
		parse( xp, "<abc />" );
		parse( xp, "<abc:def />" );
		
		parse( xp, "</abc:def>" );
		parse( xp, "</abc>" );
		
		parse( xp, "</abc:def >" );
		parse( xp, "</abc >" );

		parse( xp, "<a x='1'/>" );
		parse( xp, "<a x ='1'/>" );
		parse( xp, "<a x= '1'/>" );
		parse( xp, "<a x = '1'/>" );
		
		parse( xp, "<a xyz='123'/>" );
		parse( xp, "<a x='1' y='2'/>" );

		parse( xp, "<a q:x='1'/>" );
		parse( xp, "<a q:xyz='123'/>" );
		parse( xp, "<a q:x='1' q:y='2'/>" );

		parse( xp, "<a x=\"1\"/>" );
		parse( xp, "<a xyz=\"123\"/>" );
		parse( xp, "<a x=\"1\" y=\"2\"/>" );
		
		xp.clear();
		
		xp.parseOne( TEST_MSG );
		
		xp.dump();
		
		if (false)
		{
			int m = 5;
			int n = 50000*4;
			for (int j = 0; j < m; j++)
			{
				if (j != 0)
					Thread.sleep( 5000 );
				
				long t0 = System.currentTimeMillis();
				for (int i = 0; i < n; i += 4)
				{
					xp.parse( TEST_MSG );
					xp.parse( TEST_MSG );
					xp.parse( TEST_MSG );
					xp.parse( TEST_MSG );
				}
				long t1 = System.currentTimeMillis();
				
				double t = (t1-t0)/1000.0;
				double r = n / t;
				System.out.println( "took "+t+" for "+n+" iterations ("+r+" per second)" );
			}
		}
	}

	private final static String TEST_MSG =
		"<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
		"<message xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
			"<field name=\"heartbeatInterval\">10</field>" +
			"<field name=\"heartbeatPayload\">mediaResources</field>" +
			"<field name=\"machineName\">JAVAMAN</field>" +
			"<field name=\"queueName\">NULL</field>" +
			"<field name=\"serverId\">1</field>" +
			"<field name=\"transactionId\">3</field>" +
			"<field name=\"serverId\">1</field>" +
			"<field name=\"clientId\">NULL</field>" +
			"<messageId>connect</messageId>" +
		"</message>";

	private static void parse( XmlParser xp, String s ) throws ParseException
	{
		System.out.println( "trying '"+s+"'..." );
		System.out.flush();
		
		xp.parse( s );
		
		System.out.println( "... ok!" );
		System.out.flush();
		
		xp.dump();
	}
	
	@Override
	public TagElement addRootTag( QName qName, Map<QName, String> attrs )
	{
		MessageProcessor mp = new MessageProcessor();
		pushTagElement( mp );
		return mp.addTag( qName, attrs );
	}
	
	/**
	 * Description of MessageProcessor
	 */
	public class MessageProcessor implements TagElement
	{
		/**
		 */
		public MessageProcessor()
		{
			// nothing to do
		}

		public boolean matches( String qualifier, String name )
		{
			return false;
		}

		public void addCdata( String cdata )
		{
			if (cdata.trim().length() > 0)
				throw new RuntimeException( "cdata not allowed in message processor: "+cdata );
		}
		
		public TagElement addTag( QName childQName, Map<QName, String> childAttrs )
		{
			if (childQName.matches( MESSAGE_QUALIFIER, MESSAGE_NAME ))
				return new Message( childAttrs );
			
			return new DefaultTagElement( childQName, childAttrs );
		}

		public void finish()
		{
			// nothing to do
		}
		
		@Override
		public String toString()
		{
			return "mp";
		}

		public Iterator<Element> getChildren()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getCdataValue()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public Iterator<QName> getAttrNames()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getAttr( String qualifier, String name )
		{
			// TODO Auto-generated method stub
			return null;
		}
	}
	
	/**
	 * Description of Message.
	 */
	public class Message implements TagElement
	{
		/**
		 * Constructs the Message.
		 *
		 * @param attrs
		 */
		public Message( Map<QName, String> attrs )
		{
			type = attrs.get( new QName( null, "t" ) );
		}
		
		private final String type;

		public boolean matches( String qualifier, String name )
		{
			return QName.matches( qualifier, MESSAGE_QUALIFIER, name, MESSAGE_NAME );
		}

		public void addCdata( String cdata )
		{
			if (cdata.trim().length() != 0)
				throw new RuntimeException( "cdata not allowed in message body" );
		}

		public TagElement addTag( QName childQName, Map<QName, String> childAttrs )
		{
			// only allow parameter tags
			if (!childQName.matches( PARAMETER_QUALIFIER, PARAMETER_NAME ))
				throw new RuntimeException( "tag "+childQName+" not allowed in message body" );
			
			Parameter p = new Parameter( childAttrs );
			params.add( p );
			return p;
		}

		public void finish()
		{
			// nothing to do.
			//System.out.println( "message finished: "+this );
		}
		
		@Override
		public String toString()
		{
			return type + ": "+params;
		}
		
		private List<Parameter> params = new Vector<Parameter>();

		public Iterator<Element> getChildren()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getCdataValue()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public Iterator<QName> getAttrNames()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getAttr( String qualifier, String name )
		{
			// TODO Auto-generated method stub
			return null;
		}
	}
	
	/**
	 * Description of Parameter.
	 */
	public static class Parameter implements TagElement
	{
		/**
		 * Constructs the Parameter.
		 *
		 * @param attrs
		 */
		public Parameter( Map<QName, String> attrs )
		{
			n = attrs.get( new QName( null, "n" ) );
			if (n == null)
				throw new RuntimeException( "n attribute not specified" );
			
			t = attrs.get( new QName( null, "t" ) );
			if (t == null)
				throw new RuntimeException( "t attribute not specified" );
			
			setValue( attrs.get( new QName( null, "v" ) ) );
		}
		
		private final String n;
		
		private final String t;

		public boolean matches( String qualifier, String name )
		{
			return QName.matches( qualifier, PARAMETER_QUALIFIER, name, PARAMETER_NAME );
		}

		public void addCdata( String cdata )
		{
			setValue( cdata );
		}

		public TagElement addTag( QName childQName, Map<QName, String> childAttrs )
		{
			throw new RuntimeException( "subordinate elements are not yet supported" );
		}

		public void finish()
		{
			if (v == null)
				throw new RuntimeException( "parameter "+n+" was not given a value" );
		}
		
		/**
		 * @param name
		 * @return true if name matches this parameter's name
		 */
		public boolean matchesName( String name )
		{
			return n.equals( name );
		}
		
		/**
		 * @return the name of this parameter
		 */
		public String getName()
		{
			return n;
		}
		
		/**
		 * @return the value of this parameter.
		 */
		public Object getValue()
		{
			return v;
		}
		
		private void setValue( String s )
		{
			if (s == null)
				return;
			
			//System.out.println( "setValue: name '"+n+"' type '"+t+"' value '"+s+"'" );
			
			if (v != null)
				throw new RuntimeException( "parameter "+n+" already has a value" );
			
			if (t.equals( "s" ))
				v = s;
			else if (t.equals( "i" ))
				v = Integer.parseInt( s );
			else if (t.equals( "d" ))
				v = Double.parseDouble( s );
			else
				throw new RuntimeException( "parameter "+n+" type "+t+" not supported" );
		}
		
		@Override
		public String toString()
		{
			return n+" =("+t+") "+v;
		}
		
		private Object v;

		public Iterator<Element> getChildren()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getCdataValue()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public Iterator<QName> getAttrNames()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public String getAttr( String qualifier, String name )
		{
			// TODO Auto-generated method stub
			return null;
		}
	}
	
	/**
	 * Description of MESSAGE_QUALIFIER.
	 */
	public final static String MESSAGE_QUALIFIER = null;
	
	/**
	 * Description of MESSAGE_NAME.
	 */
	public final static String MESSAGE_NAME = "m";
	
	/**
	 * Description of PARAMETER_QUALIFIER.
	 */
	public final static String PARAMETER_QUALIFIER = null;
	
	/**
	 * Description of PARAMETER_NAME.
	 */
	public final static String PARAMETER_NAME = "p";
}
