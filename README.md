# CUAE
Cisco Unified Application Environment 

This repository stands as a historical archive of a product that was far too ahead of its time. As a historical relic, it is noteworthy for many of the code patterns and capabilities that it had long before the libraries and infrastructure existed to make such things easy.

## Metreos

The product that came to be known as CUAE started life in 2002 as "Samoa", the brainchild of Adam Takvam. But like most inspired ideas, this didn't come from nowhere. Prior to Metreos, Adam worked for Ericsson where he was on a team developing a programmable application server called Call Signaling Control Function (CSCF) targeted specifically at the large telco providers. Adam wondered why they would limit themselves to just a handful of customers. There was no reason that this server couldn't be run on commodity hardware and marketed to enterprises to provide customizable telephony workflows and IVR capabilities. He filed that idea away until the opportunity to build it came along.

Louis Marascio provided exactly that opportunity. Adam replied to an ad that Louis had posted on the Geek Austin message board looking for a chief architect. He had no money to offer, but was willing to offer equity instead with a promise to backpay salary after the company had raised a round of funding. Being young and optimistic, Adam was fine with those terms.

Upon joining Metreos, it was clear that the company had no solid concept of what they wanted to build. Louis knew he wanted to do something in VoIP. All they had was an MGCP media gateway and the main person who wrote that had quit. That's when Adam saw the opportunity to pitch his idea for a VoIP application server running on commodity hardware. 

Louis and Adam went back and forth on the details and ultimately decided that the fastest way to create it would be to leverage a brand new language called C# that Microsoft had made because it basically had the advantage of seeing what went well and what didn't with Java and not making the same mistakes again. Looking back on it, it may seem odd to choose a language that only ran on Windows when building a server, but at that time Windows server were not uncommon and Linux was still very young and had no shortage of problems of its own. So the choice today is much more clear-cut than it was back then.

James DeCocq was hired to build the media server portion. For performance reasons, he opted to implement in C++. He did not use the existing media server as a starting point because it had been designed for an entirely different use case. Jim is most famous for turning every bug report into a bet. If someone internally filed a bug report and was wrong, they had to pay up (usually $5)!

Once funding had been secured, Joel Fontenot was instated as CEO at the behest of investors. This made Louis the CTO and Adam the Chief Architect.

At some point while Adam and Jim were building their respective components Louis had his own brainchild. At that point, the applications were written in a custom Domain Specific Language (DSL) that happened to be based on XML due to the ease of working with that format in early versions of .NET. Louis rightly noted that customers were not going to write scripts in XML for this platform. There needed to be an easier way. The product needed a graphical designer in which to author the apps. Initially, this would be a "No Code" experience, but later it became evident that the ability to add snippets of code here and there was still necessary. So, it became a minimal code visual application designer. Having basically completed the media server at that point, Jim took point on creating the application designer and Adam remained focused on developing the application server.

The first and only paying customer that Metreos ever had was Lehman Brothers. But it was quite a validating moment when that $100,000 check came in the mail (yes, it was a literal paper check!). Adam, Louis, and Joel flew to New Jersey to meet with the IT people and plan the integration. 

All throughout the development, the Cisco Unified Communications Manager was always the primary integration target since it was the only stable VoIP call signaling platform flexible enough to provide a stable integration. CUAE could be coerced into being a PBX, but that wasn't what it was built for. So, the ideal deployment had CUAE working as an adjunct to either an SS7 or VoIP PBX. In this way, the application server was only invoked as needed (e.g. when a configured triggering event was received). 

When hardware was needed for the Lehman Brothers deployment, Cisco UCX was the natural choice. Louis took point on establishing and evolving that relationship. He also developed an obsessive fascination with designing a custom bezel to make the server look distinctive. The application server and media server shipped on 1U Cisco UCX hardware running Windows 2003 Server.

At this point, it was clear that Metreos was lining itself up to be acquired by Cisco. Louis had made contact with Glenn Inn in the Office of the CTO at Cisco and Glenn became the internal champion for the acquisition. Joel took point on negotiations leading to a sale of Metreos to Cisco for $28m in cash in June 2006. What Cisco - and most Metreos employees - didn't know was that Metreos was about 2 months away from running out of capital when the deal was signed!

## Cisco Unified Application Environment

If memory serves, everyone on the Metreos team continued on with Cisco after the acquisition. Once the team had established their footing and internalized the product direction mandates from Cisco leadership, they entered a period of intensive development tightening the CUCM integration as well as making up for a lot of documentation shortfalls.

By late 2007, a few things became clear: 

1. The CUAE did not have traction within the Cisco sales organization because it was too much of an unknown. At that time, Cisco products were "Big Iron" that one would plug in, configure, and then leave alone. They ran Cisco's proprietary IOS operating system. They didn't act as endpoints to anything. The CUAE was something entirely different. It ran on Windows with no hope of changing. It required "programming" or installing pre-built applications before it would do anything at all. It acted as a call signaling endpoint. It was just too different for them to figure out how to position it and too untested in the field for them to put their reputations on the line to endorse it. 

2. Cisco TAC had no idea what to make of it. It came in with startup-level documentation and that's it. No one on the team was designated as a TAC liaison to train TAC engineers or handle escalations, so if I case was created for it, TAC engineers had to just email Louis or Adam for guidance. The product just didn't have the robust support it needed to gain traction in the Cisco ecosystem.

3. Radianta, based in Irvine CA, was the only Cisco integration partner to embrace CUAE and they were having difficulties understanding how to develop an application portfolio for it.

Seeing the writing on the walls, Adam decided to leave Cisco and join Radianta as their VP of Product Management. This allowed Radianta to make some solid wins in the market including a large deployment at Ford. But it wasn't enough to keep CUAE alive.

Sometime in mid-2008 the CUAE was dropped from Cisco's product catalog. Due to the small number of active customers, there was no end-of-life announcement.

## The Metreos Team

Joel Fontenot - CEO \
Louis Marascio - CTO \
John Curreri - CFO \
David Wilson - VP Marketing \
Adam Takvam - Chief Architect \
James DeCocq - Lead Engineer \
James Dixson - Engineering Manager \
Vaughan Stanford - Sales and Marketing \
Seth Call - Customer Support & Integrations Engineer \
Scott Comer - Sr. Software Engineer \
Jan Capps - Software Engineer \
Hung Nguyen - Software Engineer \
John Zdunczyk - Software Engineer \
Daniel Bethke - Network Engineer / Systems Administrator \
Guy Oliver - Technical Documentation \
Mark Richards - Angel investor
