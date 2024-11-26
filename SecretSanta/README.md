# kdonoel

Program that deals with Christmas parties where any guest is supposed to give a guest to another aka Secret Santa. It consumes an xml file (well, it was 2004 at the time) to get a list of the guests, along with smtp server infos, then roll dices to select for anybody somebody to give a gift to.
Integrated contraints:

    * One can't give a gift to himself (nifty, isn't it?)
    * If one's lover is given in the xml file, then this one can't have his own lover to give a gift to.
    * If Teams are provided, a member of a team won't give a gift to a teammate
    * A 'sentmail' file will be generated so as to fix any mail issue, even if it may spoil the surprise.

Off course, this program is provided "as is", without warranty of any kind.

Note: As of 2023, uses the Mailjet service so as to send mails

So don't bother me if your ISP blacklists you for spamming, if your friends won't talk to you anymore cause their gifts sucked, or if your computer breaks while using this program.
See provided sample.xml file for infos on the datafile.

**Important**: If you use special characters like 'é' 'à' or other international ones, be sure to encode your xml input file in UTF-8. Otherwise, those special characters won't be well read.
