#pragma once

class CCOMAddinUtil
{
	private:
		CCOMAddinUtil(void);
		~CCOMAddinUtil(void);
 	 	static HANDLE _DDBToDIB( CBitmap& bitmap, DWORD dwCompression, CPalette* pPal );

	public:
		static void CopyTransBitmap(HBITMAP hSrcBmp);

		 
};
