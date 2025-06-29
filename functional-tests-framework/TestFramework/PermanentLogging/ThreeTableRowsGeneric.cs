using System;
using System.Text;
using System.Collections.Specialized;

namespace Metreos.Samoa.FunctionalTestFramework
{
	/// <summary>
	/// Generates simple table to display polls of the MSMQ's size at different times in a test
	/// </summary>
	public class ThreeTableRowsGeneric: TestLogCreationBase
	{
        public StringCollection first;
        public StringCollection second;
        public StringCollection third;
        public string firstTitle;
        public string secondTitle;
        public string thirdTitle;
        public bool sizeMismatchFlag;

		public ThreeTableRowsGeneric(StringCollection first, StringCollection second, StringCollection third, string title, string firstTitle, string secondTitle, string thirdTitle )
		{
            this.first = first;
            this.second = second;
            this.third = third;
            this.firstTitle = firstTitle;
            this.secondTitle = secondTitle;
            this.thirdTitle = thirdTitle;

            sizeMismatchFlag = false;

            if(first == null)
            {
                this.first = new StringCollection();
                this.first.Add("Error: Results came in as null.  Perhaps threading violation?");
                
            }
  
            if(second == null)
            {
                this.second = new StringCollection();
                this.second.Add("Error: Results came in as null.  Perhaps threading violation?");
                
            }

            if(third == null)
            {
                this.third = new StringCollection();
                this.third.Add("Error: Results came in as null.  Perhaps threading violation?");
                
            }
            
            if((this.first.Count != this.second.Count) && (this.first.Count != this.third.Count))
            {
                sizeMismatchFlag = true;
            }         
                
		}

        public override string CreateXML()
        {
            //Call XML serializer
            string results = null;
            return results;
        }

        public override string CreateXHTML()
        {
            XHTMLBuilder = new StringBuilder("<div class='threeTableRowsGeneric'><span class='title'>" + title + "</span>");
            if(sizeMismatchFlag)
            {
                XHTMLBuilder.Append("<p class='error'>All sampling data did not contain the same amount of fields.</p>");
            }
            else
            {
                XHTMLBuilder.Append("<table border='0' class='threeTableRowsGeneric'>");
                XHTMLBuilder.Append("<th class='threeTableRowsGeneric'>" + firstTitle + "</th><th class='threeTableRowsGeneric'>" + secondTitle + "</th><th class='threeTableRowsGeneric'>" + thirdTitle + "</th>");

                for(int i = 0; i < first.Count; i++)
                {
                    int evenOrOdd = i%2;

                    if(evenOrOdd == 1)
                    {
                        XHTMLBuilder.Append("<tr class='threeTableRowsGeneric'><td class='even'>" + first[i] + "</td><td class='even'>" + second[i] + "</td><td class='even'>" + third[i] + "</td></tr>");
                    }
                    else
                    {
                        XHTMLBuilder.Append("<tr class='threeTableRowsGeneric'><td class='odd'>" + first[i] + "</td><td class='odd'>" + second[i] + "</td><td class='odd'>" + third[i] + "</td></tr>");
                    }
                }
                XHTMLBuilder.Append("</table>");
            }
           

            XHTMLBuilder.Append("</div>");

            return XHTMLBuilder.ToString();
        }

	}
}
