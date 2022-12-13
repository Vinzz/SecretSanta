/*
 * Created by SharpDevelop.
 * Author: Vincent Tollu 
 * vinzz@altern.org
 * Date: 26/10/2005
 * Time: 22:50
 * 
 * Distributed under the General Public License
 */

using System;

namespace KDoNoel
{
	/// <summary>
	/// Object Friend
	/// </summary>
	public class Friend
	{
		private static int teamIndex;

		private string team = string.Empty;
		public string Team
		{
			get
            {
				return team;
            }

			set
			{ if (string.IsNullOrEmpty(value))
				{
					++teamIndex;
					team = teamIndex.ToString();
				}
				else team = value; 
			}
		}
		public string  Name { get; set; }
		public string Mail { get; set; }
		public string Lover { get; set; }
		public bool HasAGift { get; set; }
		public int GivesTo { get; set; }


		//Constructor
		public Friend()
		{
			GivesTo = 0;
		}
	}

}
