
using System;

namespace Lephone.Util.Coding
{
    public class Base64Coding : StringCoding
    {
        public override byte[] Encode(string Src)
        {
            return Convert.FromBase64String(Src);
        }

        public override string Decode(byte[] Src)
        {
            return Convert.ToBase64String(Src);
        }
    }
}
