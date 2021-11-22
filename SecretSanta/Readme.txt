KDoNoel v0.3
Author: Vincent Tollu 
vinzz@altern.org
 
Program that deals with Christmas parties where any guest  is supposed to give a guest to another.
It consumes an xml file to get a list of the guests, along with smtp server infos, then roll dices to select for anybody
somebody to give a gift to. Eventually, mails are sent for each guest with gift instructions.

Integrated contraints:
- One can't give a gift to himself (nifty, isn't it?)
- If one's lover is given in the xml file, then one can't have
  his own lover to give a gift to.
- A sentmail file will be generated so as to fix any mail issue, even if it may spoils
  the surprise.
  
Off course, this program is provided "as is", without warranty of any kind.
So don't bother me if youre ISP blacklists you for spamming, if youre friends 
won't talk to you anymore cause their gifts sucked, or if youre computer breaks 
while using this program.

As of 2019, this program uses the SendGrid service

This program is distributed under the GNU General Public License, whose text is provided.

See provided sample.xml file for infos on the datafile.

Important: If you use special characters like 'é' 'à' or other international ones, 
be sure to encode your xml input file in UTF-8. Otherwise, those special characters won't be 
well read.

Syntax:
KDoNoel.exe ListOfFriends.xml [-Go]
If the -Go file is specified, the mails will indeed be sent! Be warned.

Roadmap:
- Play with GTK# to shape an editor to produce the xml file.

History:
25/11/2006 v0.3 Added a .\sentmail txt file so as to have a backup of the sent mails if an error happens,
cuz of course such an error happened last year.

12/02/2005 v0.2 Slightly modified the datafile syntax, including the %sender% 
and the %receiver% parameters. These two parameters will be translated if present 
in mail subject and/or in mail message.

11/19/2005 C# version. Made with #Develop (http://www.icsharpcode.net/OpenSource/SD/)
Runs on win32 with both Ms .Net 1.1 and Mono 1.1 (http://www.mono-project.com/Main_Page)
frameworks.

11/2004 First version without the lover constraint, written in C++ with a txt ('|' separated info)
input file.
