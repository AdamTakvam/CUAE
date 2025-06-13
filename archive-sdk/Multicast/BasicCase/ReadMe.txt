1). INTRODUCTION:-

	This application is to enable sending and recieving on a multicast IP address.


2). How to run the application:

	a).Open the application with CUAD.Under the "OnGotRequest" tab, click on "CreateConnection" action.Set the desired multicast IP and multicast port in the "MediaTxIp" and "MediaTxPort" in the respective fields in its properties window.
	
	b).Now click on the "CreateExecute" action and go to its properties window.Set the field URL1 as "RTPMRx:<Multicast IP Address>:<Multicast Port>:100"(This is the volume level of the audio multicast).

	c).Then click on the "SendExecute" action and go to its properties window.Set the "Password" field with the password associated with the user of the phone, which will be listening to the mentioned multicast port.In the "URL" field furnish the IP address of the IP phone and in the field named "Username" give the Username to which the IP phone is associated in CallManager.

	d).Build and deploy the application.

	e).Open an IE page and type http://<App-Server IP>:8000/playmulticast.The audio file being played will be listened on the phone, which is listening to the mentioned multicast port.