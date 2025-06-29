using System;
using System.Text;
using Metreos.RecordAgent.sforce;

namespace Metreos.RecordAgent
{
	public class CallerInfo
	{
		private SforceServiceWrapper binding;
		private string _urlToShow;
		private string _titleText;
		private string _contentText;
		private string _phoneText;

		public string PhoneText
		{
			get { return _phoneText; }
			set { _phoneText = value; }
		}
		public string ContentText
		{
			get { return _contentText; }
			set { _contentText = value; }
		}
		public string TitleText 
		{
			get { return _titleText; }
			set { _titleText = value; }
		}
			public string urlToShow
		{
			get { return _urlToShow; }
			set { _urlToShow = value;}
		}
		public CallerInfo(SforceServiceWrapper binding)
		{
			this.binding = binding;
		}

		private string CleanPhoneNumber(string phoneNumber) 
		{
			Encoding ascii = Encoding.ASCII;

			byte[] phonebyte = ascii.GetBytes(phoneNumber);
			StringBuilder sb = new StringBuilder();
			for (int i=0;i<phonebyte.Length;i++) 
			{
				if (phonebyte[i] > 47 && phonebyte[i] < 58) 
				{
					sb.Append(ascii.GetString(phonebyte, i, 1));
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Does a search operation given a phone number and returns
		/// </summary>
		/// <param name="phoneNumber">The number to find</param>
		/// in a sosl statement.  ie "Contact(field1, field2,...), Lead(field1, field2, ...)"
		///   This will not be parsed, just used as is to construct a sosl statement.</param>
		/// <param name="?"></param>
		public string DoSearchByPhoneNumber(string phoneNumber, string objectReturns) 
		{
			phoneNumber = CleanPhoneNumber(phoneNumber);
			string SOSL = "FIND {" + phoneNumber + "} IN PHONE FIELDS";
			if (objectReturns.Length > 0)
				SOSL = SOSL + " RETURNING " + objectReturns;

			sforce.SearchResult sr = binding.searchWrapped(SOSL);
			if (sr.searchRecords == null) 
				return null;            // if we want screen pop all the time then, do not return null and let it process.
			else 
    			return ProcessSearchResults(sr, phoneNumber);
		}

        public void DoScreenPop(object state)
        {
            string phoneNumber = state as String;
            string objectReturns = "Contact(Id, FirstName, LastName, AccountId), Lead(Id, FirstName, LastName, Company), Account(Id, Name, Type, TickerSymbol)";
            if (DoSearchByPhoneNumber(phoneNumber, objectReturns) != null)
                System.Diagnostics.Process.Start(this.urlToShow);
        }

		private string ProcessSearchResults(sforce.SearchResult sr, string searchValue) 
		{
			
			System.Uri uri = new Uri(binding.Url);
			string appURL = "https://" + uri.Host.Replace("-api.", ".");
			appURL = appURL + "/secur/frontdoor.jsp?sid=" + binding.SessionHeaderValue.sessionId + "&retURL=";
			
			System.Collections.ArrayList accounts = new System.Collections.ArrayList();
			System.Collections.ArrayList contacts = new System.Collections.ArrayList();
			System.Collections.ArrayList leads = new System.Collections.ArrayList();

			if (sr.searchRecords == null) 
			{
				//return the not found case
				this.TitleText = "Not Found";
				this.ContentText = "Click the for search page.";
				this.PhoneText = FormatPhone(searchValue);
				string retURL = appURL + "/srch/advsearch.jsp%3Fstr%3D" + searchValue + "%26searchType%3D1"; 
				this.urlToShow = retURL;
				System.Diagnostics.Trace.WriteLine(retURL);
				return retURL;
			} 
			else 
			{
				for (int i=0;i<sr.searchRecords.Length;i++) 
				{
					sforce.SearchRecord rec = sr.searchRecords[i];
					if (rec.record.type.ToLower().Equals("account")) 
						accounts.Add(rec.record);
					else if (rec.record.type.ToLower().Equals("contact"))
						contacts.Add(rec.record);
					else if (rec.record.type.ToLower().Equals("lead"))
						leads.Add(rec.record);
				}

				//For this implementation, we will check to see if the following scenarios exist.
				// 1.  account(s) and conact(s) returned.  One or more accounts and one or more contacts.
				// 2.  multiple accounts returned.
				// 3.  multiple contacts with the same account ids
				// 4.  multiple contacts with different account ids

				if (sr.searchRecords.Length == 1) 
				{
					//Found a single object
					GetItemDetail(sr.searchRecords[0].record, searchValue);
					return appURL + "/" + sr.searchRecords[0].record.Id;
				} 
				else if (accounts.Count == 1 && contacts.Count == 1) 
				{
					GetItemDetail((sforce.sObject)accounts[0], searchValue);
					this.urlToShow = appURL + "/" + ((sforce.sObject)accounts[0]).Id;
					return appURL + "/" + ((sforce.sObject)accounts[0]).Id;
				}
				else 
				{
					string retURL = appURL + "%2Fsrch%2Fadvsearchresults.jsp%3FsearchType%3D2%26sbstr%3D" + searchValue + "%26str%3D" + searchValue; 
					System.Diagnostics.Trace.WriteLine(retURL);
					this.TitleText = "Found multiple objects";
					this.urlToShow = retURL;
					this.ContentText = "Click the link above to see search page.";
					this.PhoneText = FormatPhone(searchValue);
					return retURL;
				}
			}
		}

		private string GetFieldValue(string fieldName, sforce.sObject obj) 
		{
			string returnval = "";
			for (int i=0;i<obj.Any.Length;i++) 
			{
				if (obj.Any[i].LocalName.ToLower().Equals(fieldName.ToLower()))
				{
					if (obj.Any[i].InnerText.Length == 0 || obj.Any[i].InnerText == null)
						returnval = "";
					else
						returnval = obj.Any[i].InnerText;
				}
			}
			return returnval;
		}
		private string LookupAccountName(string accountId) 
		{
			sforce.sObject[] recs = binding.retrieve("Name", "Account", new string[] {accountId});
			if (recs != null) 
				return GetFieldValue("Name", recs[0]);
			else
				return "No account association.";
		}

		private string FormatPhone(string phone) 
		{
			if (phone.Length == 7)
				return phone.Substring(0, 3) + "-" + phone.Substring(3);
			else if (phone.Length == 10)
				return "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6);
			else if (phone.Length > 10) 
			{
				string temp = phone.Substring(phone.Length - 10);
				string temp2 = phone.Substring(0, phone.Length - 10);
				return temp2 + "+ (" + temp.Substring(0, 3) + ") " + temp.Substring(3, 3) + "-" + temp.Substring(6);
			} else return phone;
		}

		private void GetItemDetail(sforce.sObject obj, string searchValue)
		{
			string objId = obj.Id;
			string idPrefix = objId.Substring(0, 3);

			System.Uri uri = new Uri(binding.Url);
			string appUrl = "https://" + uri.Host.Replace("-api.", ".");
			appUrl = appUrl + "/secur/frontdoor.jsp?sid=" + binding.SessionHeaderValue.sessionId + "&retURL=";

			if (idPrefix.Equals("003") )
			{
				//Get the contact information
				this.TitleText = GetFieldValue("FirstName", obj) + ((string)(" " + GetFieldValue("LastName", obj))).Trim() + " (C)";
				this.ContentText = LookupAccountName(GetFieldValue("AccountId", obj));
				this.PhoneText = FormatPhone(searchValue);
				this.urlToShow = appUrl + "/" + obj.Id;
			} 
			else if (idPrefix.Equals("00Q") )
			{
				//get the lead information
				this.TitleText = GetFieldValue("FirstName", obj) + ((string)(" " + GetFieldValue("LastName", obj))).Trim() + " (L)";
				this.ContentText = GetFieldValue("Company", obj);
				if (this.ContentText == "") this.ContentText = "No company name given.";
				this.PhoneText = FormatPhone(searchValue);
				this.urlToShow = appUrl + "/" + obj.Id;
			} 
			else if (idPrefix.Equals("001")) 
			{
				//get the account information
				this.TitleText = GetFieldValue("Name", obj);
				this.ContentText = GetFieldValue("Type", obj);
				string ticker = GetFieldValue("TickerSymbol", obj);
				if (ticker.Length > 0)
					this.ContentText = this.ContentText + " (" + ticker + ")";
				this.PhoneText = FormatPhone(searchValue);
				this.urlToShow = appUrl + "/" + obj.Id;
			}
		}
	}
}
