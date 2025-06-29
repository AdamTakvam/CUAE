/************************************************************
 *      Copyright (c) 1990-1998 by Dialogic Corp.           *
 *                                                          *
 ************************************************************/
// REVISION HISTORY:
// 01/11/2001  [AC] Added D42LIB_DEFS int d42_IndicatorsEx()
// 01/11/2001  [AC] Added TD42_NEWMESSAGEARRIVED,D4CH_MESG_Q,ED42_QEMPTY
// 01/11/2001  [AC] Added prototype for GetNewMessage()
// 06/12/2001  [AC] PTR21543 fix - changed the value of D4CH_LC_LAMP to 0x019


#ifndef D42LIB_IMPORT
#define D42LIB_DEFS __declspec(dllimport)
#else
#define D42LIB_DEFS __declspec(dllexport)
#endif

#ifndef __D42LIB_H__
#define __D42LIB_H__

/*
 * General Definitions
 */
#define  VER_D42DRIVER          0x0000
#define  VER_D42LIB             0x0003
#define  VER_D42FIRMWARE        0x0004
#define  MAXDBTYPE              0x0012

/*
 * Board Types Return Values
 */
#define TYP_NONE                 0x0000   /* No D/42  Devices   */
#define TYP_SL                   0x0001   /* D/42-SL  Device    */
#define TYP_SX                   0x0002   /* D/42-SX  Device    */
#define TYP_NS                   0x0003   /* D/42-NS  Device    */
#define TYP_NE2                  0x0004   /* D/42-NE2 Device    */
#define TYP_M1                   0x0005   /* D/42-M1  Device    */
#define TYP_DVC                  0x0006   /* D/42-DVC Device    */
#define TYP_NE2PBX               0x0007   /* D/42-NE2PBX Device */
#define TYP_NE2KTS               0x0008   /* D/42-NE2KTS Device */
#define TYP_PA                   0x0009   /* D/42-PA  Device    */
#define TYP_D82L4						0x000A	/* d/82 Lucent 4 wire */
#define TYP_D82L2						0x000B	/* D/82 Lucent 2 wire */
#define TYP_D82SR						0x000C	/* D/82 Seimens/ROLM	 */
#define TYP_D82SH						0x000D	/* D/82 Seimens/Hicom */
#define TYP_D82SX						0x000E	/* D/82 Mitel SX		 */
#define TYP_D82NS						0x000F	/* D/82 Nortel NS		 */
#define TYP_D82M1						0x0010	/* D/82 NorTel Meridan*/
#define TYP_D82NE2PBX				0x0011	/* D/82 NEC PBX		 */
#define TYP_D82NE2KTS				0x0012	/* D/82 NEC KTS		 */

/*
 * The following equates are parameter numbers
 */
#define D4BD_TYPE             0x0070   /* Indicates a Board Parm     */
#define D4CH_TYPE             0x00F0   /* Indicates a channel parm   */

/*
 * Board level Parameters for D/42-SL Only
 */
#define D4BD_RINGON           0x0002   /* ring on duration           */
#define D4BD_RINGOFF          0x0003   /* ring off duration          */

/*
 * Board level Parameters for the D/42-NS
 */
#define D4BD_CALLID						0x0001      /* calling number ID enable   */

/*
 * Channel level Parameters for the D/42-NS only
 */
#define D4CH_CALLERIDAVAILABLE			0x0015		/* Asynch Caller ID available */
#define D4CH_CHANNELSTATUS 			    0x0013		/* Asynch Channel Status */
#define D4CH_MESG_Q						0x0016      /* Message Queue parameter */

/*
 * Channel level Parameters for the D/42-SL only
 */
#define D4CH_PDNKEY           0x0089   /* PDN or ICI key location    */
#define D4CH_XFERKEY          0x008A   /* transfer key location      */
#define D4CH_SENDKEY          0x008B   /* send message key location  */
#define D4CH_CANCELKEY        0x008C   /* cancel message key location */
#define D4CH_DNKEY            0x008D   /* DN or outbound line location */
#define D4CH_RELEASEKEY       0x008E   /* release key location       */

/*
 * Board level Parameters for D/42-SX Only
 */
#define D4BD_SPMODE           0x0007   /* enable/disable speakerphone */

/*
 * Board level Parameters for D/42-NE2 KTS and D/42-NE2 PBX Only
 */
#define D4BD_LONGBEEPDUR      0x0002   /* Long beep duration          */
#define D4BD_GETSWITCHTYPE    0x000D   /* Retrieves the switch type   */

/*
 * Channel level Parameters for D/42-NE2 KTS and D/42-NE2 PBX Only
 */
#define D4CH_RECBEEPDUR       0x0003   /* Record beep duration        */
#define D4CH_RECBEEPREP       0x0004   /* Record beep repetition      */
#define D4CH_TIMETOFIRSTBEEP  0x0005   /* Time to start first beep    */

/*
 * Board level Parameters for D/42-NE2 PBX Only
 */
#define D4BD_MSGACCESSON      0x000A   /* Message waiting on          */
#define D4BD_MSGACCESSOFF     0x000B   /* Message waiting off         */
#define D4BD_RESETRINGCNT     0x000C   /* Resets ring count           */

/*
 * D82 specific parms
*/
#define D4BD_REPORT_RESET		0x000E	/* turns on or off reporting of reset error */

/*
 * Param mask for enabling events during a Feature Session
 */
#define D42_EVT_SOFTKEY						0x01
#define D42_EVT_ASYNCCLOSEFEATSESSION	0x02

/*
 * Board and channel parameters that control async event notification
 */
#define D42LIBPARM 0x1000

// Available on: NE2, PA
//		Control: TRUE = Report async channel status, FALSE - discard message
#define D4CH_ASYNCCHSTATUS 			(D42LIBPARM | 0x0013)

// Available on: PA
//    Control: TRUE = Report async live record message, FALSE - discard message
#define D4CH_LIVERECORD					(D42LIBPARM | 0x0018)

// Available on: NE2
//      Control: TRUE = Report PBX message available, FALSE - discard message
#define D4CH_GETPBXMESSAGE		0x1010

//Available on the D/82
// [AC] PTR21543 fix - changed the value of D4CH_LC_LAMP to 0x019
// #define D4CH_LC_LAMP			0x016 // change the loop current lamp
#define D4CH_LC_LAMP			0x019 // change the loop current lamp
#define D4CH_CHANNELUPDATE	0x017 // Allow async indicator and display messages

/*
 *	Asynchronous events codes
 */
#define TD42_ASYNCCHSTATUS				0x00D0
#define TD42_ASYNCCALLID				0x00D1
#define TD42_SOFTKEYINPUT				0x00D2
#define TD42_LIVERECORDENABLE			0x00D3
#define TD42_LIVERECORDDISABLE		0x00D4
#define TD42_ASYNCCLOSEFEATSESSION	0x00D5
#define TD42_GETPBXMESSAGE		      0x00D6
#define TD42_ASYNCDISP					0x00DA
#define TD42_ASYNCLINE					0x00DB
#define TD42_NEWMESSAGEARRIVED          0x00DC 


/*
 * D42 event data values for the async event TD42_ASYNCCHSTATUS
 * These are retrieved by calling sr_getevtdatap()
 */
#define D42_CH_STATUS_OFF	0x00
#define D42_CH_STATUS_ON	0x01


/*
 * D42 events returned to the application
 */
#define  TD42_D42ERROR        0x0D1    /* Error event for D42 */


/*
 * D/82 D4BD_GETSWITCHTYPE results
 */
#define PBX_L4						0x0A
#define PBX_L2						0x0B
#define PBX_NS						0x0C
#define PBX_M1						0x0D
#define PBX_SR						0x0E
#define PBX_SH						0x0F
#define PBX_SX						0x10
#define PBX_SX2					0x11
#define PBX_NE2K					0x12
#define PBX_NE2P					0x13

/*
 *  D42 Library error codes returned by ATDV_LASTERR( )
 */
#define ED42_NOERROR						0x0000  /* No Error                         */
#define ED42_NOTIMP						0x0500  /* Function not implemented         */
#define ED42_MAXCHAN						0x0501  /* Max chan capacity reached        */
#define ED42_INVALARG					0x0502  /* Illegal argument to function     */
#define ED42_BADPARM						0x0503  /* Invalid value for d42 parameter  */
#define ED42_UNSUPPORTED				0x0504  /* Unsupported feature              */
#define ED42_RDFWVER						0x0505  /* Error reading firmware version   */
#define ED42_UNKNOWNBOARD				0x0506  /* Unknown D/42 Board Type          */
#define ED42_BADDEVICE					0x0507  /* Invalid or wrong device handle   */
#define ED42_DLLINIT						0x0508  /* Unable to initialize the DLL     */
#define ED42_SYSTEM						0x0509  /* System Error                     */
#define ED42_NOCOMM						0x050A  /* No Communication With Switch     */
#define ED42_NOTIDLE						0x050B  /* Device is not idle               */
#define ED42_FEATSESSIONALREADYOPEN	0x050C  /* Feature session is already open  */	
#define ED42_NOFEATURESESSION			0x050D  /* No feature session available     */
#define ED42_FWREQFAILURE  			0x050E  /* Firmware request failed          */
#define ED42_MEMORY						0x050F  /* Buffer is to small					  */
#define ED42_NOCOMMUNICATIONS			0x0510  /* Lost Communication with Font End */
#define ED42_REGAINCOM					0x0511  /* regained Communications			  */
#define ED42_QEMPTY						0x0512  /* Message Q is empty               */

/*
 * Connection type for CALLIDEX
 */
#define CALL_TYP_NULL		0x00	//None
#define CALL_TYP_INTERNAL	0x01	//internal call
#define CALL_TYP_EXTERNAL	0x02	//external call

/*
 *Following definition can define reason code for call forward in CALLIDEX.
 */
#define RSN_NULL            0x00	//None
#define RSN_DIRECT          0x01	//Direct call
#define RSN_FBUSY           0x02	//call Forwarded because busy
#define RSN_FNOANS          0x03	//call forwarded because no answer
#define RSN_SUPR_XFER		0x04	//call transfer because
#define RSN_UNSUPR_XFER		0x05 	//call transfer because

/*
 * Structures
 */
typedef struct _tagLineIndicator
{
	short Indicator;
	short Status;
} LINEINDICATOR;

#define CALLIDLEN 16
typedef struct tagCALLIDEX
{
	char called_id[CALLIDLEN];
	char calling_id[CALLIDLEN];
	int  call_type;
	int  reason_code;
} CALLIDEX;


/*
 * Function prototypes
 */

#ifdef __cplusplus
extern "C" {            // C Plus Plus function bindings
#endif

#if (defined (__cplusplus) || defined( __STDC__ ) || defined (__BORLANDC__))
   D42LIB_DEFS int __cdecl ATD4_CHTYPE(int);
   D42LIB_DEFS int __cdecl ATD4_BDTYPE(int);
   D42LIB_DEFS int __cdecl d42_gtcallid(int, char *);
   D42LIB_DEFS int __cdecl d42_display(int, char *);
   D42LIB_DEFS int __cdecl d42_displayex(int, char *, int);
   D42LIB_DEFS int __cdecl d42_indicators(int, char *);
   D42LIB_DEFS int __cdecl d42_indicatorsex(int, char *, int);
   D42LIB_DEFS int __cdecl d42_setparm(int, int, void *);
   D42LIB_DEFS int __cdecl d42_getparm(int, int, void *);
   D42LIB_DEFS int __cdecl d42_lcdprompt(int, char *);
   D42LIB_DEFS int __cdecl d42_getver(int, char *, int);
   D42LIB_DEFS int __cdecl d42_brdstatus(int, char *, char *);
   D42LIB_DEFS int __cdecl d42_flags(int, char *);
   D42LIB_DEFS int __cdecl d42_chnstatus(int, char *, char *);
   D42LIB_DEFS int __cdecl d42_openfeaturesession(int, char*, int*, int);
   D42LIB_DEFS int __cdecl d42_writetodisplay(int, char*);
   D42LIB_DEFS int __cdecl d42_closefeaturesession(int);
   D42LIB_DEFS int __cdecl d42_putpbxmsg(int, char*, char);
   D42LIB_DEFS int __cdecl d42_longbeep(int);
   D42LIB_DEFS int __cdecl d42_getnewmessage(int, char*);
   D42LIB_DEFS int __cdecl d42_gtcallidex( int, CALLIDEX *);
#else	// old K&R 
   D42LIB_DEFS int __cdecl ATD4_CHTYPE();
   D42LIB_DEFS int __cdecl ATD4_BDTYPE();
   D42LIB_DEFS int __cdecl d42_gtcallid();
   D42LIB_DEFS int __cdecl d42_display();
   D42LIB_DEFS int __cdecl d42_displayex();
   D42LIB_DEFS int __cdecl d42_indicators();
   D42LIB_DEFS int __cdecl d42_indicatorsex();
   D42LIB_DEFS int __cdecl d42_setparm();
   D42LIB_DEFS int __cdecl d42_getparm();
   D42LIB_DEFS int __cdecl d42_lcdprompt();
   D42LIB_DEFS int __cdecl d42_getver();
   D42LIB_DEFS int __cdecl d42_brdstatus();
   D42LIB_DEFS int __cdecl d42_flags();
   D42LIB_DEFS int __cdecl d42_chnstatus();
   D42LIB_DEFS int __cdecl d42_openfeaturesession();
   D42LIB_DEFS int __cdecl d42_writetodisplay();
   D42LIB_DEFS int __cdecl d42_closefeaturesession();
   D42LIB_DEFS int __cdecl d42_putpbxmsg();
   D42LIB_DEFS int __cdecl d42_longbeep();
   D42LIB_DEFS int __cdecl d42_getnewmessage();
   D42LIB_DEFS int __cdecl d42_gtcallidex();
#endif

#ifdef __cplusplus
}           // C Plus Plus function bindings
#endif

#endif

