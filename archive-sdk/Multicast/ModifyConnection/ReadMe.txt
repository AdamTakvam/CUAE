1). INTRODUCTION:

	This application will modify a normal call connection and connect it to a multicast port.

2). How to run the application:

	a). Open the application with CUAD.Under the "OnIncomingCall" tab, click on "ModifyConnection" action and go to its properties window.Set the desired multicast IP and multicast port in the "MediaTxIp" and "MediaTxPort" in the respective fields.


	b). Now click on the "CreateExecute" action and go to its properties window.Set the field URL1 as "RTPMRx:<Multicast IP Address>:<Multicast Port>:100"(This is the volume level of the audio multicast).

	
	c). Then click on the "SendExecute" action and go to its properties window.Set the "Password" field with the password associated with the user of the phone, which will be listening to the mentioned multicast port.In the "URL" field furnish the IP address of the IP phone and in the field named "Username" give the Username to which the IP phone is assigned in CallManager.


	d). Place a call to the Route Patern associated with the App-Server. The connection will be modified and a file will be played to the phone listening to the multicast port.
