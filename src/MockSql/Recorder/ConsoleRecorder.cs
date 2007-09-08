
#region usings

using System;

#endregion

namespace Lephone.MockSql.Recorder
{
    public class ConsoleRecorder : IRecorder
    {
        public void Write(string Msg, params object[] os)
        {
            Console.WriteLine(Msg, os);
        }
    }
}
