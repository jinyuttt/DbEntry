
#region usings

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Specialized;
using System.Security;

using Lephone.Util.Text;

#endregion

namespace Lephone.Util
{
	public static class SystemHelper
	{
		private static string _ExeFileName;
        private static string _BaseDirectory;

        public static string ExeFileName
        {
            get { return _ExeFileName; }
        }

        public static string BaseDirectory
        {
            get { return _BaseDirectory; }
        }

		static SystemHelper()
		{
			try
			{
                AppDomainSetup s = AppDomain.CurrentDomain.SetupInformation;
                _ExeFileName     = s.ApplicationName;
                _BaseDirectory   = s.ApplicationBase;
            }
			catch {}
		}

		public static string GetDateTimeString()
		{
			return GetDateTimeString(DateTime.Now);
		}

		public static string GetDateTimeString(DateTime dt)
		{
			return dt.ToString("yyMMddHHmmss");
		}

		#region Get StackTrack Function Name

		public static string CurrentFunctionName
		{
			get
			{
				return GetStackTrackFunctionName(0);
			}
		}

		public static string CallerFunctionName
		{
			get
			{
				return GetStackTrackFunctionName(1);
			}
		}

		private static string GetStackTrackFunctionName(int Index)
		{
			Index += 2;
			StackTrace st = new StackTrace(true);
			if ( st.FrameCount > Index )
			{
				StackFrame sf = st.GetFrame(Index);
				return GetMothodDesc(sf, false);
			}
			else
			{
				return null;
			}
		}

		private static StringCollection GetMothodLineCollection(StackTrace st)
		{
			return GetMothodLineCollection(st, false);
		}

		private static StringCollection GetMothodLineCollection(StackTrace st, bool NeedLineNo)
		{
			StringCollection sc = new StringCollection();
			for (int i = 0; i < st.FrameCount; i++)
			{
				StackFrame sf = st.GetFrame(i);
				sc.Add( GetMothodDesc(sf, NeedLineNo) );
			}
			return sc;
		}

		private static string GetMothodDesc(StackFrame sf, bool NeedLineNo)
		{
			MethodBase mb = sf.GetMethod();
			if ( NeedLineNo )
			{
				return GetMothodLine(mb) + GetCurLine(sf);
			}
			else
			{
				return GetMothodLine(mb);
			}
		}

		private static string GetMothodLine(MethodBase mb)
		{
			StringBuilder sb = new StringBuilder();
			Type t = mb.DeclaringType;
			if (t != null)
			{
				string s = t.Namespace;
				if (s != null)
				{
					sb.Append(s);
					if (sb != null)
					{
						sb.Append(".");
					}
				}
				sb.Append(t.Name);
				sb.Append(".");
			}
			sb.Append(mb.Name);
			sb.Append("(");
			ParameterInfo[] pis = mb.GetParameters();
			for (int j = 0; j < pis.Length; j++)
			{
				string s = "<UnknownType>";
				if (pis[j].ParameterType != null)
				{
					s = pis[j].ParameterType.Name;
				}
				sb.Append(((j != 0) ? ", " : "") + s + " " + pis[j].Name);
			}
			sb.Append(")");
			return sb.ToString();
		}

		private static string GetCurLine(StackFrame sf)
		{
			if (sf.GetILOffset() != -1)
			{
				string s = null;
				try
				{
					s = sf.GetFileName();
				}
				catch (SecurityException)
				{
				}
				if (s != null)
				{
					return string.Format(" in {0}:line {1}", s, sf.GetFileLineNumber());
				}
			}
			return "";
		}

		#endregion
	}
}
