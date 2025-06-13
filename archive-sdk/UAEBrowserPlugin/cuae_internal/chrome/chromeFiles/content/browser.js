/* 
 * Gain access to the Prefences service.
 */
var prefManager = Components.classes["@mozilla.org/preferences-service;1"]
                            .getService(Components.interfaces.nsIPrefBranch);

var username;
var password;
var phoneIP;
var appServerIP;
var allOn;

/*
 * Called when a Firefox Page Loads.  
 */
 
function cuae_internal_init(event)
{
	if (document.getElementById("appcontent"))
	{
		document.getElementById("appcontent").addEventListener("DOMContentLoaded", checkUrl, true);
	}
	
	if (document.getElementById("messagepane"))
	{
		document.getElementById("messagepane").addEventListener("DOMContentLoaded", checkUrl, true);
	}
	
	if (document.getElementById("sidebar"))
    {
		document.getElementById("sidebar").addEventListener("DOMContentLoaded", checkUrl, true);
	}
}

function checkUrl(event)
{
	if(!event)
	{
		// bail out early
		return;
	}
	
	var page = event.originalTarget;
	if(!page)
	{
		// bail out early
		return;
	}
	
	if(page.location)
	{
		if(page.location.host == 'directory.cisco.com' || page.location.host == 'wwwin-tools.cisco.com')
		{
			allOn = prefManager.getBoolPref("extensions.cuae_internal.allOnOff");
			if(allOn)
			{
				// We are in the directory.  Hunt for the 'Telephony' <td> element
				var cells = page.getElementsByTagName("td");
				
				/*username = prefManager.getCharPref("extensions.cuae_internal.username");
				password = prefManager.getCharPref("extensions.cuae_internal.password");
				phoneIP = prefManager.getCharPref("extensions.cuae_internal.primaryPhoneIP");
				appServerIP = prefManager.getCharPref("extensions.cuae_internal.appServerIP");*/
				
				if(cells)
				{
					//alert("has cells");
					
					for(var i = 0; i < cells.length; i++)
					{
						var td = cells[i];
						
						// CHECK FOR NEW CISCO DIRECTORY
						if(td.hasAttribute("scope") && td.getAttribute("scope") == "row" && 
							td.hasAttribute("class") && td.getAttribute("class") == "desc")
						{
							//alert("found row/desc");

							var tdtext = td.firstChild;
							if(tdtext && tdtext.nodeType == Node.TEXT_NODE)
							{
								//alert("found text node, potentially with Work|Mobile|Other");

								if(tdtext.nodeValue == "Work" || tdtext.nodeValue == "Mobile" || tdtext.nodeValue == "Other")
								{
								    //alert("Found " + tdtext.nodeValue);

									var phoneElement = cells[i + 2];
									
									overwriteNumberWithLink(phoneElement);

								}
							}
						}
						
						// CHECK FOR OLD CISCO DIRECTORY
						var tdtext = td.firstChild;
						if(tdtext && tdtext.nodeType == Node.TEXT_NODE)
						{
							if(tdtext.nodeValue == "Telephone" || tdtext.nodeValue == "Mobile Number")
							{
								// We have the telephone cell.  This contains a number like '+ 1 512 378 1377'
								var phoneTd = cells[i + 1];
								
								overwriteNumberWithLink(phoneTd);
							}
						}
					}
				}
				
				
				
			}
		}
	}
}

function overwriteNumberWithLink(phoneNumberNode)
{
	
	var ownerDoc = phoneNumberNode.ownerDocument;
	
	var dirtyPhoneNumber = "NULL";
    var phoneNumberText;
	
	// find if it has strong, if so, use that instead of this tag
	var strongs = phoneNumberNode.getElementsByTagName("strong");
	
	if(strongs && strongs.length > 0)
	{
		phoneNumberText = strongs[0];
		dirtyPhoneNumber = phoneNumberText.firstChild;
		if(dirtyPhoneNumber)
		{
			dirtyPhoneNumber = dirtyPhoneNumber.nodeValue;
		}
	}
	else
	{
		phoneNumberText = phoneNumberNode.firstChild;
		if(phoneNumberText && phoneNumberText.nodeType == Node.TEXT_NODE)
		{
			// snatch phone number, remove all non-digit characters
			dirtyPhoneNumber = phoneNumberText.nodeValue;
		}
	}
		
	if(dirtyPhoneNumber && dirtyPhoneNumber != "NULL" && dirtyPhoneNumber != "")
	{
		var cleanPhoneNumber = dirtyPhoneNumber.replace(/\s+/g, "");
		cleanPhoneNumber = cleanPhoneNumber.replace("+", "");
		
		if(cleanPhoneNumber && cleanPhoneNumber != "")
		{
			var dialablePhoneNumber = "9" + cleanPhoneNumber;
			
			// Create image node phone
			var image = document.createElement("image");
			image.setAttribute("src", "chrome://cuae_internal/content/images/phone_spaced.png");
			image.setAttribute("style", "cursor:pointer");
			image.addEventListener("click", function(e) { showPrompt(e.screenX, e.screenY, dialablePhoneNumber); }, false);
			phoneNumberNode.appendChild(image);
		}
	}
}

function showPrompt(x, y, dialablePhoneNumber)
{
	x = 10 + x;
	y = 10 + y; // scoot down and to the right
	
	username = prefManager.getCharPref("extensions.cuae_internal.username");
	password = prefManager.getCharPref("extensions.cuae_internal.password");
	phoneIP = prefManager.getCharPref("extensions.cuae_internal.primaryPhoneIP");
	appServerIP = prefManager.getCharPref("extensions.cuae_internal.appServerIP");
	
	window.openDialog('chrome://cuae_internal/content/clicktotalkprompt.xul', 'UAE Dialer', 'chrome, screenY=' + y  +' , screenX=' + x, username, password, phoneIP, appServerIP, dialablePhoneNumber);
}