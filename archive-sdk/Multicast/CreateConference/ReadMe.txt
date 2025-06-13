1). INTRODUCTION:

	This application is to create a conference which will be multicasted on a multicast IP and port.


2). How to run the application:

	a). Open the application with CUAD.Under the "OnGotRequest" tab, click on "CreateConference" action and go to its properties window. Set the desired multicast IP and multicast port in the MediaTxIp and MediaTxPort in the respective fields.


	b). Now click on the "CreateExecute" action and go to its properties window.Set the field URL1 as "RTPMRx:<Multicast IP Address>:<Multicast Port>:100"(This is the volume level of the audio multicast).

	
	c). Then click on the "SendExecute" action and go to its properties window.Set the "Password" field with the password associated with the user of the phone, which will be listening to the mentioned multicast port.In the "URL" field furnish the IP address of the IP phone and in the field named "Username" give the Username to which the IP phone is assigned in CallManager.


	d). Click on the "MakeCall" action and go to its properties window. In the "From" field mention the number of the Route Pattern, from where the call is to be generated.In the "To" field mention the number to which the call is to be made.

	
	e). Open an IE page and type http://<App-Server IP>:8000/CreateConf. The conference will start and you can talk into the called phone and the listening phone will hear.