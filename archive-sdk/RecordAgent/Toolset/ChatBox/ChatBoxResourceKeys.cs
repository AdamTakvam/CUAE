using System;

namespace Metreos.Toolset
{
	/// <summary>
	/// Summary description for ChatBoxResourceKeys.
	/// </summary>
	public class ChatBoxResourceKeys
	{
		public static string root = "ChatBox";  

		#region Toolbar tooltips
		public static string SAVE = root + ".SAVE";  
		public static string OPEN  = root + ".OPEN";  
		public static string BOLD = root + ".BOLD";  
		public static string ITALIC = root + ".ITALIC";
		public static string UNDERLINE = root + ".UNDERLINE";  
		public static string STRIKEOUT = root + ".STRIKEOUT";  
		public static string LEFT = root + ".LEFT";  
		public static string CENTER = root + ".CENTER";  
		public static string RIGHT = root + ".RIGHT";  
		public static string UNDO = root + ".UNDO";  
		public static string REDO = root + ".REDO";  
		public static string COLOR = root + ".COLOR";  
		public static string EMOTICONS = root + ".EMOTICONS"; 
		#endregion

		#region Rich Edit Box Context Menu
		public static string CUT = root + ".CUT";  
		public static string COPY = root + ".COPY";  
		public static string PASTE = root + ".PASTE";  
		public static string DELETE = root + ".DELETE"; 
		public static string SELECTALL = root + ".SELECTALL"; 
		#endregion

		public ChatBoxResourceKeys() {}
	}
}

