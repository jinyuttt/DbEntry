﻿using System;
using Lephone.Util;
using Lephone.Util.Text;
using NUnit.Framework;

namespace Lephone.UnitTest.util
{
	[TestFixture]
	public class StringHelperTester
	{
        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Test()
        {
            const string s = "";
            StringHelper.GetStringLeft(s);
        }

	    [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Test1()
	    {
	        const string s = "abc";
	        StringHelper.GetStringLeft(s, 3);
	    }

	    [Test]
		public void Test2()
		{
			string s = "abc";
			s = StringHelper.GetStringLeft(s);
			Assert.AreEqual(s, "ab");
		}

		[Test]
		public void Test3()
		{
			string s = "a,b,c,";
			s = StringHelper.GetStringLeft(s);
			Assert.AreEqual(s, "a,b,c");
		}

		[Test]
		public void Test4()
		{
			const string s = "_abc";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test5()
		{
			const string s = "_ab123c";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test6()
		{
			const string s = "ab123c";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test7()
		{
			const string s = "__abc12";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test8()
		{
			const string s = "a";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test9()
		{
			const string s = "_";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test10()
		{
			const string s = "1abc12";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test11()
		{
			const string s = "1";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test12()
		{
			const string s = "%";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test13()
		{
			const string s = "ab%";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test14()
		{
			const string s = "";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test15()
		{
			const string s = "ab\ncd";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test16()
		{
			const string s = "ab\tcd";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test17()
		{
			const string s = "ab cd";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(false, b);
		}

		[Test]
		public void Test18()
		{
			const string s = " abcd ";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test19()
		{
			const string s = "\t abcd";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

		[Test]
		public void Test20()
		{
			const string s = "abcd\t";
			bool b = StringHelper.IsIndentityName(s);
			Assert.AreEqual(true, b);
		}

        [Test]
        public void Test21()
        {
            const string s = "aaaa";
            string[] ss = StringHelper.Split(s, ':', 2);
            Assert.AreEqual(2, ss.Length);
            Assert.AreEqual("aaaa", ss[0]);
            Assert.AreEqual("", ss[1]);
        }

        [Test]
        public void Test22()
        {
            const string s = "aa:a:a";
            string[] ss = StringHelper.Split(s, ':', 2);
            Assert.AreEqual(2, ss.Length);
            Assert.AreEqual("aa", ss[0]);
            Assert.AreEqual("a:a", ss[1]);
        }

        [Test]
        public void TestByteArray()
        {
            var b1 = new byte[] { 1, 2, 3, 4 };
            var b2 = new byte[] { 1, 2, 3, 4 };
            Assert.IsFalse(b1 == b2);
        }

        [Test]
        public void TestAreEqual()
        {
            var b1 = new byte[] { 1, 2, 3, 4 };
            var b2 = new byte[] { 1, 2, 3, 4 };
            var ret = CommonHelper.AreEqual(b1, b2);
            Assert.IsTrue(ret);
            var b3 = new byte[] { 1, 2, 3, 5 };
            Assert.IsFalse(CommonHelper.AreEqual(b1, b3));
        }

    }
}
