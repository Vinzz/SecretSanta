/*
 * Created by SharpDevelop.
 * Author: Vincent Tollu 
 * vinzz@altern.org
 * Date: 05/11/2005
 * Time: 09:55
 * 
 * Distributed under the General Public License
 */

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace KDoNoel
{
	public struct ServerInfo
	{
		public string subject;
		public string message;
		public string SenderMail;
	}
		
	/// <summary>
	/// Various tools used
	/// </summary>
	public struct KDoTools
	{
		public static void PrintUsage()
		{
			Console.WriteLine("Usage:\n" +
			                  "SecretSanta FriendFilesPath.xml [-Go] \n" +
			                  "see the provided sample.xml for data file syntax\n" +
			                  "if the flag -Go is provided, the mails will really be sent\n");
			Console.Read();
		}
		
		//Fills the friends collection
		public static void FillFriendsColl(string stDataFilePath, ref ArrayList aFriendsColl, ref ServerInfo oServerInfo)
		{
			try
			{
				Friend fPal = null;
			
				//File access...
				XmlDocument doc = new XmlDocument();
				try
				{
					StreamReader sr = new StreamReader(stDataFilePath, true);
    				doc.Load(sr);
				}
				catch(System.Exception e)
				{
					Console.WriteLine("Opening Document Error: " + e.Message);
					throw;
				}
				
    			XmlNode docElement = doc.DocumentElement;
    			
    			XmlNode node;
    			
    			node = docElement.SelectSingleNode("/KDONoel/mail_title");
    			oServerInfo.subject =node.InnerText;
    			
    			node = docElement.SelectSingleNode("/KDONoel/mail_message");
    			oServerInfo.message =node.InnerText;
    			
    			node = docElement.SelectSingleNode("/KDONoel/SenderMail");
    			oServerInfo.SenderMail =node.InnerText;
    			
    			XmlNodeList NodesFriends = docElement.SelectNodes("/KDONoel/Friend");
    			
    			foreach(XmlNode n in NodesFriends)
    			{
    				if (CheckMail(n.SelectSingleNode("adress").InnerText) == false)
    					throw new System.ArgumentException("The adress '" + 
    					                                   n.SelectSingleNode("adress").InnerText +
					                            		   "' seems invalid. Please check it\n" +
					                                       "and retry\n");
    						
    				fPal = new Friend()
					{
						Name = n.SelectSingleNode("name").InnerText,
						Mail = n.SelectSingleNode("adress").InnerText,
						Lover = n.SelectSingleNode("lover").InnerText,
						Team = n.SelectSingleNode("team").InnerText
					};
    				aFriendsColl.Add(fPal); //store it in the coll
    			}
			}
			catch(System.Exception e)
			{
				Console.WriteLine("Error in the data file parsing.\n " +
				                  "One data item might be invalid\n");
				Console.WriteLine(e.Message);
				throw;
			}

		}

        // Sends mails... What a surprise!
        public static async Task SendMail(string stTo, string stFrom, string stSubject, string stBody)
        {
			var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            var client = new SendGridClient(apiKey);
			var from = new EmailAddress("moulinette.a.vincent@gmail.com", "moulinette-??-vincent??");
			var subject = stSubject;
            var to = new EmailAddress(stTo);
            var plainTextContent = stBody;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, string.Empty);
            var response = await client.SendEmailAsync(msg);

            if(response.StatusCode != HttpStatusCode.Accepted)
            {
                throw new Exception("Houston! We have a problem");
            }
        }

		
		public static bool CheckMail(string adress)
		{
			//I _had_ to check that ;o)
            // And guess what? Regex based email validation sucks

            try
            {
                MailAddress mail = new MailAddress(adress);
                return true;
            }
            catch (FormatException)
            {
                //address is invalid
                return false;
            }
		}
		
		public static bool RollTheDices(ref ArrayList aFriendsColl)
		{
			try
			{
				Random r = new  Random();
				
				//For each guest
				for(int iCurrent=0;iCurrent<aFriendsColl.Count;++iCurrent)
				{
					int spy=0; //Infinite loop proof
					int index=0;
					Friend pFCurrent = ((Friend) aFriendsColl[iCurrent]);
					Friend pFReceiver;
					
					//look for another one that don't have any gift yet,
					//and who's not himself, or his lover, or a teammate
					//But don't search for too long!
					do 
					{
						index = r.Next(0,aFriendsColl.Count); 
						++spy;
						if(spy==1000) break;
					}
					while (
					       (((Friend) aFriendsColl[index]).HasAGift == true) ||
					        (index == iCurrent) ||
					        (((Friend) aFriendsColl[index]).Name == pFCurrent.Lover) ||
							(((Friend)aFriendsColl[index]).Team == pFCurrent.Team)
						  );
					
					if(spy<999)
						pFReceiver = ((Friend) aFriendsColl[index]);
					else return false;
					
					//We now have a giver and a receiver!
					//Set the receiver flag as true
					pFCurrent.GivesTo = index;
					pFReceiver.HasAGift = true;
				}
				return true;
			}
			catch(System.Exception e)
			{
				Console.WriteLine("Error in the shuffle component\n");
				Console.WriteLine(e.Message);
			}
			return false;
		}
		public static string ComputeMessage(string stMessageTemplate, string stSender, string stReceiver)
		{
			string stbuf = stMessageTemplate;
			if (stbuf.IndexOf("%sender%") != -1)
				stbuf = stbuf.Replace("%sender%", stSender);
			
			if (stbuf.IndexOf("%receiver%") != -1)
				stbuf = stbuf.Replace("%receiver%", stReceiver);
	
			return stbuf;
		}
	}
}
