using Newtonsoft.Json;
using System;
using System.Linq;

namespace Citadel.Infrastructure
{
    public class ClaimValueType
    {
        public ClaimValueType() { }

        public static ClaimValueType Create(Type t)
        {
            var cvt = new ClaimValueType();
            if (new[]
            {
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(string),
                typeof(byte)
            }.Contains(t))
            {
                cvt.IsComplexType = false;
                cvt.IsBase64 = false;
            }
            else if (t == typeof(byte[]))
            {
                cvt.IsComplexType = false;
                cvt.IsBase64 = true;
            }
            else
            {
                cvt.IsComplexType = true;
                cvt.IsBase64 = false;
            }

            cvt.FullName = t.FullName;

            return cvt;
        }


        public bool IsComplexType { get; set; }
        public bool IsBase64 { get; set; }
        public string FullName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ClaimValueType FromJson(string value)
        {
            return JsonConvert.DeserializeObject<ClaimValueType>(value);
        }

        public static implicit operator ClaimValueType(string s)
        {
            return JsonConvert.DeserializeObject<ClaimValueType>(s);
        }

        public static implicit operator string(ClaimValueType s)
        {
            return JsonConvert.SerializeObject(s);
        }
    }
}
