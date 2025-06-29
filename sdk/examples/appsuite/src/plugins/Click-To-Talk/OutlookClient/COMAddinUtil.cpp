#include "StdAfx.h"
#include "comaddinutil.h"

CCOMAddinUtil::CCOMAddinUtil(void)
{
}

CCOMAddinUtil::~CCOMAddinUtil(void)
{
}

HANDLE CCOMAddinUtil::_DDBToDIB( CBitmap& bitmap, DWORD dwCompression, CPalette* pPal )
{
 	/* Taken from http://www.codeguru.com/bitmap/ddb_to_dib.shtml */
 
 	BITMAP bm;
	BITMAPINFOHEADER bi;
	LPBITMAPINFOHEADER lpbi;
	DWORD dwLen;
	HANDLE hDIB;
	HANDLE handle;
	HDC hDC;
	HPALETTE hPal;

	ASSERT( bitmap.GetSafeHandle() );

	// The function has no arg for bitfields
	if ( dwCompression == BI_BITFIELDS )
		return NULL;

	// If a palette has not been supplied use defaul palette
	hPal = (HPALETTE) pPal->GetSafeHandle();
	if (hPal==NULL)
		hPal = (HPALETTE) GetStockObject(DEFAULT_PALETTE);

	// Get bitmap information
	bitmap.GetObject(sizeof(bm),(LPSTR)&bm);

	// Initialize the bitmapinfoheader
	bi.biSize               = sizeof(BITMAPINFOHEADER);
	bi.biWidth              = bm.bmWidth;
	bi.biHeight             = bm.bmHeight;
	bi.biPlanes             = 1;
	bi.biBitCount           = bm.bmPlanes * bm.bmBitsPixel;
	bi.biCompression        = dwCompression;
	bi.biSizeImage          = 0;
	bi.biXPelsPerMeter      = 0;
	bi.biYPelsPerMeter      = 0;
	bi.biClrUsed            = 0;
	bi.biClrImportant       = 0;

	// Compute the size of the  infoheader and the color table
	int nColors = (1 << bi.biBitCount);
	if ( nColors > 256 ) 
		nColors = 0;
	dwLen = bi.biSize + nColors * sizeof(RGBQUAD);

	// We need a device context to get the DIB from
	hDC = ::GetDC(NULL);
	hPal = SelectPalette(hDC,hPal,FALSE);
	RealizePalette(hDC);

	// Allocate enough memory to hold bitmapinfoheader and color table
	hDIB = GlobalAlloc(GMEM_FIXED,dwLen);

	if (!hDIB)
	{
		SelectPalette(hDC,hPal,FALSE);
		::ReleaseDC(NULL,hDC);
		return NULL;
	}

	lpbi = (LPBITMAPINFOHEADER)hDIB;

	*lpbi = bi;

	// Call GetDIBits with a NULL lpBits param, so the device driver 
	// will calculate the biSizeImage field 
	GetDIBits(hDC, (HBITMAP)bitmap.GetSafeHandle(), 0L, (DWORD)bi.biHeight,
		(LPBYTE)NULL, (LPBITMAPINFO)lpbi, (DWORD)DIB_RGB_COLORS);

	bi = *lpbi;

	// If the driver did not fill in the biSizeImage field, then compute it
	// Each scan line of the image is aligned on a DWORD (32bit) boundary
	if (bi.biSizeImage == 0)
	{
		bi.biSizeImage = ((((bi.biWidth * bi.biBitCount) + 31) & ~31) / 8) 
			* bi.biHeight;

		// If a compression scheme is used the result may infact be larger
		// Increase the size to account for this.
		if (dwCompression != BI_RGB)
			bi.biSizeImage = (bi.biSizeImage * 3) / 2;
	}

	// Realloc the buffer so that it can hold all the bits
	dwLen += bi.biSizeImage;
	if (handle = GlobalReAlloc(hDIB, dwLen, GMEM_MOVEABLE))
		hDIB = handle;
	else
	{
		GlobalFree(hDIB);

		// Reselect the original palette
		SelectPalette(hDC,hPal,FALSE);
		::ReleaseDC(NULL,hDC);
		return NULL;
	}

	// Get the bitmap bits
	lpbi = (LPBITMAPINFOHEADER)hDIB;

	// FINALLY get the DIB
	BOOL bGotBits = GetDIBits( hDC, (HBITMAP)bitmap.GetSafeHandle(),
							0L,                             // Start scan line
							(DWORD)bi.biHeight,             // # of scan lines
							(LPBYTE)lpbi                    // address for bitmap bits
							+ (bi.biSize + nColors * sizeof(RGBQUAD)),
							(LPBITMAPINFO)lpbi,             // address of bitmapinfo
							(DWORD)DIB_RGB_COLORS);         // Use RGB for color table

	if( !bGotBits )
	{
		GlobalFree(hDIB);

		SelectPalette(hDC,hPal,FALSE);
		::ReleaseDC(NULL,hDC);
		return NULL;
	}

	SelectPalette(hDC,hPal,FALSE);
	::ReleaseDC(NULL,hDC);
	return hDIB;
} 
  
void CCOMAddinUtil::CopyTransBitmap(HBITMAP hSrcBmp)
{
	/* Based on Microsoft Knowledge Base Article - 288771  */

	BITMAP  bitmap;
 	HBITMAP hbmMask;
 	
	// Get the BITMAP 
	GetObject(hSrcBmp, sizeof(BITMAP), &bitmap);
  	 
	// Get a device context
	HDC hDC = GetDC(0);
 
	// *********************************************************
	// Step #1: Create the Bitmap MASK 
	// *********************************************************

     // Create memory DCs to work with.
    HDC hdcMask = CreateCompatibleDC(hDC);
    HDC hdcImage = CreateCompatibleDC(hDC);
    
	// Create a monochrome bitmap for the mask.
	hbmMask = CreateBitmap(bitmap.bmWidth, bitmap.bmHeight, 1, 1, NULL);
    
	// Select the mono bitmap into its DC.
    HBITMAP hbmOldMask = (HBITMAP)SelectObject(hdcMask, hbmMask);
    
	// Select the image bitmap into its DC.
    HBITMAP hbmOldImage = (HBITMAP)SelectObject(hdcImage, hSrcBmp);
    
	// Set the transparency color to be the top-left pixel.
    SetBkColor(hdcImage, GetPixel(hdcImage, 0, 0));
	SetTextColor(hdcImage, RGB(255,255,255));

    // Make the mask.
    BitBlt(hdcMask, 0, 0, bitmap.bmWidth, bitmap.bmHeight, hdcImage, 0, 0, SRCCOPY);
    
	// Cleaup up
    SelectObject(hdcMask, hbmOldMask);
    SelectObject(hdcImage, hbmOldImage);
    DeleteDC(hdcMask);
    DeleteDC(hdcImage);
 
	// *********************************************************
	// Step #2: Copy a DIB of the Source Image to the clipboard
	// *********************************************************
  
	// Open the clipboard
	OpenClipboard(0);

	// Empty Clipboard
	EmptyClipboard();
  
	// Covert BITMAP (DDB) to DIB 
	CBitmap tmpBitmap;
	tmpBitmap.Attach(hSrcBmp);
	HANDLE hDIB = _DDBToDIB(tmpBitmap, FALSE, NULL);
	tmpBitmap.Detach();
 
	// Copy the DIB to the Clipboard
   	HANDLE hMemTmp1 = SetClipboardData(CF_DIB, hDIB);
  
	// *********************************************************
	// Step #3: Populate Outlooks Special Clipboard formats
	// *********************************************************
 
	// ' Get the cf for button face and mask.
	long cfBtnFace = RegisterClipboardFormat("Toolbar Button Face");
	long cfBtnMask = RegisterClipboardFormat("Toolbar Button Mask");

	BITMAPINFO bi; 
	BYTE hbuf[50];
  
	// Initialize the header.
	ZeroMemory(&bi, sizeof(BITMAPINFO));
	bi.bmiHeader.biSize = sizeof(BITMAPINFO);
  
	// Get the BITMAPHEADERINFO for the mask.
	GetDIBits(hDC, hbmMask, 0, 0, NULL, (BITMAPINFO*)&bi, 0);
 	CopyMemory(&hbuf[0], &bi.bmiHeader, 40);
 
	// Allocate space for the MASK Bitmap Bits
	BYTE* pBMDataBuffer = (BYTE*)malloc(bi.bmiHeader.biSizeImage + 4);
	ZeroMemory(pBMDataBuffer, bi.bmiHeader.biSizeImage + 4);
 
	// Get handle to the Source DIB recently created 
	HANDLE hMemTmp = GetClipboardData(CF_DIB);

	// Copy the Source DIB to clipboard using the Button Face Format
	if (hMemTmp) 
	{
		SIZE_T cbSize    = GlobalSize(hMemTmp);
		HANDLE hGMemFace = GlobalAlloc(GMEM_ZEROINIT | GMEM_SHARE | GMEM_FIXED, cbSize);

		if (hGMemFace)
		{
 			LONG* lpData  = (LONG*)GlobalLock(hMemTmp);
			LONG* lpData2 = (LONG*)GlobalLock(hGMemFace);
			CopyMemory(lpData2, lpData, cbSize);
			GlobalUnlock(hGMemFace);
			GlobalUnlock(hMemTmp);
			
			if (SetClipboardData(cfBtnFace, hGMemFace) == 0) 
				GlobalFree(hGMemFace);
  		}
	}
 
	// Now get the mask bits and the rest of the header.
	GetDIBits(hDC, hbmMask, 0, bi.bmiHeader.biSizeImage, pBMDataBuffer, (BITMAPINFO*)&hbuf[0], 0);
 
	// Copy them to global memory and set it on the clipboard.
	HANDLE hGMemMask = GlobalAlloc(GMEM_ZEROINIT | GMEM_SHARE | GMEM_FIXED, bi.bmiHeader.biSizeImage + 60);
 
	// Copy the Mask DIB to clipboard using the Button Face Format
	if (hGMemMask)
	{
 		BYTE* lpData = (BYTE*)GlobalLock(hGMemMask);
		 
		CopyMemory(lpData, &hbuf[0], 48);
  		CopyMemory(&lpData[48], pBMDataBuffer, bi.bmiHeader.biSizeImage);
		GlobalUnlock(hGMemMask);
			 
 		if (SetClipboardData(cfBtnMask, hGMemMask) == 0) 
			GlobalFree(hGMemMask);
  	}
 
	// Cleanup the mask and source
 	DeleteObject(hbmMask);

	// Close the clipboard
	CloseClipboard();

	// Release the mask bitmap buffer
	if (pBMDataBuffer)
		delete pBMDataBuffer;
}
