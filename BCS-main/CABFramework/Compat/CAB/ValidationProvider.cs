/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Text.RegularExpressions;

namespace LNG.Framework.Utility
{
	/// <summary>
	/// This class is used to caontain the all validation method.
	/// </summary>
	public class ValidationProvider
	{
		/// <summary>
		/// This method is used to validate pin number.
		/// </summary>
		/// <param name="pin">string</param>
		/// <returns>bool</returns>
		public static bool CheckPin(string pin)
		{

			if (string.IsNullOrEmpty(pin))
				return false;
			else if (pin.Length > 6)
				return false;
			else if (Int32.Parse(pin).ToString().Length < 6)
				return false;
			else
				return true;
		}

		/// <summary>
		/// This method is used to validate the User Name, Password & login id. 
		/// With the help of regular expression.
		/// </summary>
		/// <param name="name">string</param>
		/// <param name="constant">string</param>
		/// <returns></returns>
		public static bool ValidateData(string name, string constant)
		{
			return Regex.Match(name, constant).Success;
		}

		public static int GetDivisionValue(int primary, int secondary)
		{
			int value = 0;
			try
			{
				value = primary / secondary;
			}
			catch (Exception)
			{ 

			}
			return value;
		}
	}
}

