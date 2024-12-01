using KDoNoel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KdoNoel5
{
    class Program
    {
        static void Main(string[] args)
        {
			try
			{
				//Instantiate the Friend container
				ArrayList aFriendsColl = new ArrayList();
				ServerInfo oServerInfo = new ServerInfo();
				bool bTest = true; //Should the mails really be sent?
				if ((args.Length == 0) || (args.Length > 2)) //No file provided, or to much arguments
				{
					KDoTools.PrintUsage();
					return;
				}

				if ((args.Length > 1) && (args[1] == "-Go")) bTest = false;

				KDoTools.FillFriendsColl(args[0], ref aFriendsColl, ref oServerInfo);

				bool bAns = false;
				int spy = 0;
				int limit = 250;
				do
				{
					++spy;
					bAns = KDoTools.RollTheDices(ref aFriendsColl);
					if (spy == limit) break;
				} while (bAns == false);



				if (spy == limit)
				{
					throw new ArgumentException("\nError in present picking.\n " +
												"You seem to have not enough guests...\n" +
												"You can cross your fingers\n" +
												"and retry, if you wish\n" +
												"Don't panic!... No mails were sent!");
				}

                FileInfo input = new FileInfo(args[0]);

				TextWriter tw = new StreamWriter($"SentMails{Path.GetFileNameWithoutExtension(input.Name)}");
				for (int iCurrent = 0; iCurrent < aFriendsColl.Count; ++iCurrent)
				{
					Friend pFCurrent = ((Friend)aFriendsColl[iCurrent]);

					string stProcessedMessage =
							KDoTools.ComputeMessage(oServerInfo.message,
												  pFCurrent.Name,
												  ((Friend)aFriendsColl[pFCurrent.GivesTo]).Name);

					string stProcessedSubject =
							KDoTools.ComputeMessage(oServerInfo.subject,
												  pFCurrent.Name,
												  ((Friend)aFriendsColl[pFCurrent.GivesTo]).Name);

					//Save the sent messages
					tw.WriteLine(pFCurrent.Name + " offre à " + ((Friend)aFriendsColl[pFCurrent.GivesTo]).Name);

					if (!bTest)
						KDoTools.SendMail(pFCurrent.Mail,
										   oServerInfo.SenderMail,
										   stProcessedSubject,
										   stProcessedMessage, oServerInfo.DisplaySenderMail).Wait();
					else
					{
						Console.WriteLine("<!-- Test Mode, Mail won't be sent -->");
						Console.WriteLine("To: " + pFCurrent.Mail);
						Console.WriteLine("From: " + oServerInfo.SenderMail);
                        Console.WriteLine("As: " + oServerInfo.DisplaySenderMail);
                        Console.WriteLine("Subject: " + stProcessedSubject);
						Console.WriteLine("Message: " + stProcessedMessage + "\n");
					}
				}
				tw.Close();
				Console.WriteLine("Bon, ben voilà. A l'année prochaine!");
				Console.Read();
			}
			catch (System.Exception e)
			{
				Console.WriteLine("Error in the program");
				Console.WriteLine(e.Message);
				Console.Write(e.ToString() + "\n");
				Console.Read();
			}
		}
    }
}
