#ifndef __EVENT_COMMON_H__
#define __EVENT_COMMON_H__

#include "Resource.h"
#include "AddIn.h"

////////////////////////////////////////////////////////////
// Dispatch IDs for Office and Outlook events
////////////////////
#define BTN_CLICK                   0x0001
#define APP_OPTIONS_PAGES_ADD       0xf005
#define EXP_FOLDER_SWITCH           0xf002
#define EXP_BEFORE_FOLDER_SWITCH    0xf003
#define EXP_VIEW_SWITCH             0xf004   
#define EXP_SELECTION_CHANGE        0xf007
#define INS_CLOSE                   0xf008
#define INSS_NEW_INSPECTOR          0xf001


extern _ATL_FUNC_INFO OnClickButtonInfo;
extern _ATL_FUNC_INFO OnOptionsAddPagesInfo;
extern _ATL_FUNC_INFO OnSelectionChangeInfo;
extern _ATL_FUNC_INFO OnFolderSwitchInfo;
extern _ATL_FUNC_INFO OnCloseInspectorInfo;
extern _ATL_FUNC_INFO OnNewInspectorInfo;

#endif // __EVENT_COMMON_H__