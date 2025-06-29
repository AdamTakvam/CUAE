#include "StdAfx.h"
#include "EventCommon.h"

////////////////////////////////////////////////////////////
// Event dispatch function information.
////////////////////
_ATL_FUNC_INFO OnClickButtonInfo        = { CC_STDCALL, VT_EMPTY, 2, { VT_DISPATCH, VT_BYREF | VT_BOOL }} ;
_ATL_FUNC_INFO OnOptionsAddPagesInfo    = { CC_STDCALL, VT_EMPTY, 1, { VT_DISPATCH}};
_ATL_FUNC_INFO OnSelectionChangeInfo    = { CC_STDCALL, VT_EMPTY, 0, {}};
_ATL_FUNC_INFO OnFolderSwitchInfo       = { CC_STDCALL, VT_EMPTY, 0, {}};
_ATL_FUNC_INFO OnCloseInspectorInfo     = { CC_STDCALL, VT_EMPTY, 0, {}};
_ATL_FUNC_INFO OnNewInspectorInfo       = { CC_STDCALL, VT_EMPTY, 1, { VT_DISPATCH }};