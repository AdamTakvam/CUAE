using System;

namespace Metreos.Types.CiscoIpPhone.Tests
{
	public class TypesTest
	{
		public TypesTest()
		{}

        [csUnit.Test]
        public void testMenu()
        {
            Menu menu = new Menu();
            menu.type = ICiscoPhone.DIRECTORY;
            menu.menu.Title = "This is a sample title";
            menu.menu.Prompt = "This is a sample prompt";

            CiscoIPPhoneSoftKeyType softkey = new CiscoIPPhoneSoftKeyType();
            softkey.Name = "SoftKey name";
            softkey.Position = 12;
            softkey.URL = "http://www.softkey.com";
            object obj = softkey;
            menu.Assign(obj);

            CiscoIPPhoneMenuItemType menuItem = new CiscoIPPhoneMenuItemType();
            menuItem.Name = "MenuItem name";
            menuItem.URL = "http://www.menuitem.com";
            menu.Assign(menuItem);

            string xml = menu.ToString();

            System.IO.StringReader reader = new System.IO.StringReader(xml);
            System.Xml.Serialization.XmlSerializer serializer = 
                new System.Xml.Serialization.XmlSerializer(typeof(CiscoIPPhoneMenuType));
            CiscoIPPhoneMenuType newMenu = (CiscoIPPhoneMenuType) serializer.Deserialize(reader);

            csUnit.Assert.True(menu.menu.Title == newMenu.Title);
            csUnit.Assert.True(menu.menu.SoftKeyItem.Length == newMenu.SoftKeyItem.Length);
            csUnit.Assert.True(menu.menu.SoftKeyItem[0].URL == newMenu.SoftKeyItem[0].URL);
        }
	}
}
