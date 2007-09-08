
#region usings

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Specialized;

using NUnit.Framework;

using Lephone.Util;
using Lephone.Util.Setting;

#endregion

namespace Lephone.UnitTest.util
{
    [TestFixture]
    public class NameValueHandlerTest
    {
        [Test]
        public void Test1()
        {
            NameValueSectionHandler h = new NameValueSectionHandler();
            string s = ResourceHelper.ReadToEnd(this.GetType(), "UnitTest.config.xml");
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bs = Encoding.Default.GetBytes(s);
                ms.Write(bs, 0, bs.Length);
                ms.Flush();
                ms.Position = 0;
                XmlDocument xd = new XmlDocument();
                xd.Load(ms);
                NameValueCollection l = (NameValueCollection)h.Create(null, null, xd["configuration"].ChildNodes[0]);
                Assert.AreEqual("Lephone.Util.Logging.ConsoleMessageRecorder, Lephone.Util", l["SqlLogRecorder"]);
                Assert.AreEqual("@Access : @~test.mdb", l["1.DataBase"]);
            }
        }
    }
}
