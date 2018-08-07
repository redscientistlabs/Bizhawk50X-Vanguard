using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RTC
{
	public static class RTC_Filtering
	{
		private static Dictionary<MD5, String[]> guid2LimiterDico = new Dictionary<MD5, string[]>();

	}
}