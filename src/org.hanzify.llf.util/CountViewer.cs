
#region usings

using System;
using Lephone.Util.Logging;

#endregion

namespace Lephone.Util
{
	public class CountViewer
	{
		private string Teamplate;
		private int OutputCount;
		private int nMax;
		private int n;

		public CountViewer(string Teamplate, int OutputCount, int nMax)
		{
			this.Teamplate = Teamplate;
			this.OutputCount = OutputCount;
			this.nMax = nMax;
			n = 0;
		}

		public void Output()
		{
			n++;
			if ( (n % OutputCount) == 0 || n == nMax )
			{
				Logger.Default.Info(string.Format(Teamplate, n, nMax));
			}
		}
	}
}
