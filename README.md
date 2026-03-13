# CUAE
Cisco Unified Appolication Server 

This repository stands as a historical archive of a product that was far too ahead of its time. As a historical relic, it is noteworthy for many of the code patterns and capabilities that it had long before the libraries and infrastructure existed to make such things easy.

## History

CUAE started life in 2002 as "Samoa", the brainchild of Adam Takvam. But like most inspiraed ideas, this didn't come from nowhere. Prior to Metreos, Adam worked for Ericsson where he was on a team developing a programmable application server targeted specifically at the large Telco operators. One day, while pondering the business side of things, Adam wondered why they would limit themselves to just a handful of customers. There was no reason that this server couldn't be run on commodity hardware and marketed to enteprises to provide customiizable telephony workflows and IVR capabilities. He filed that idea away until the opportunity to build it came along.

Louis Marascio provided exactly that opportunity. Adam replied to an ad that Louis had posted on the Geek Austin message board looking for a chief architect. He had no money to offer, but was willing to offer equity instead with a promise to backpay salary after the company had raised a round of funding. BEing young and optimistic, Adam was fine with those terms.

Upon joining Metreos, it was clear that the company had no solid concept of what they wanted to build. Louis knew he wanted to do something in VoIP. All they had was an MGCP media gateway at that point and the main person who wrote that had quit. That's when Adam saw the opportunity to pitch his idea for a VoIP application server running on commodity hardware. 

Louis and Adam went back and forth on the details and ultimately decided that the fastest way to create it would be to leverage a brand new language called C# that Microsoft had made because it basically had the advantage of seeing what went well and what didn't with Java and not making the same mistakes again. Looking back on it, it may seem odd to choose a language that only ran on Windows when building a server, but at that time Windows server were not uncommon and Linux was still very young and had no shortage of problems of its own. So the choice today is much more clear-cut than it was back then.

James DeCocq was hired to build the media server portion. For perfomance reasons, he opted to implement in C++. He did not use the existing media server as a starting point because it had been designed for an entirely different use case. Jim is most famous for turning every bug report into a bet. If someone internally filed a bug report and was wrong, they had to pay up (usually $5)!

Once funding had been secured, Joel Fontenot was instated as CEO at the behest of investors. This made Louis the CTO and Adam the Chief Architect.

At some point while Adam and Jim were building their respective components Louis had his own brainchild. At that point, the applications were written in a custom Domain Specific Language (DSL) that happened to be based on XML due to the ease of working with that format in early versions of .NET. Louis rightly noted that customers are not going to write scripts in XML for this platform. There needed to be an easier wauy. We needed a graphical designer in which to author the apps. Initially, this would be a "No Code" experience, but later we realized tht the ability to toss in a little code here and there was still necessary. So, it became a minimal code visual application designer. Having basically completed the media server at that point, Jim took point on creating the application designed and Adam remained focused on developing the application server itself.

The first and only paying customer that Metreos ever had was Lehman Brothers. But it was quite a validating moment when that $100,000 check came in the mail (yes, it was a literal paper check!). Adam, Louis, and Joel flew to New Jersey to meet with the IT people and plan the integration. 

All throughout the development, the Cisco Unified Communications Manager was always the primary integration target since it it was the only stable VoIP call signaling platform flexible enough to provide a stable integration. CUAE could be coerced into being a PBX, but that wasn't what it was built for. So, the ideal deployment had CUAE working as an adjunct to either an SS7 or VoIP PBX. In this way, the application server was only invoked as needed (e.g. when a configured triggering event was received). 

When hardware was needed for the Lehman Brothers deployment, Cisco UCX was the natural choice. Louis took point on establishing and evolving that relationship. He also developed an obsessive fascination with designing a custom bezel to make the server look distinctive. The application server and media server shipped on 1U Cisco UCX hardware running Windows 2003 Server.

At this point, it was clear that Metreos was lining itself up to be acquired by Cisco. Louis had made contact with Glenn Inn in the Office of the CTO at Cisco and Glenn became the internal champion for the acquisition. Joel took point on negotiations leading to a sale of Metreos to Cisco for $28m in cash in June 2006.

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
Daniel Bethke - Network Engineer / SysAdmin \
Guy Oliver - Technical Documentation \
Mark Richards - Angel investor
