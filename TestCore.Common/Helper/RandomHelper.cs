using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Helper
{
    public static class RandomHelper
    {
        static int counter = 0;
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + counter;
            unchecked
            {
                counter++;
            }
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> counter)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }
        /// <summary>
        /// 生成随机数字+字母组合
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + counter;
            unchecked
            {
                counter++;
            }

            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> counter)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        #region 随机生成制定常数序列码
        /// <summary>
        /// 随机生成制定常数序列码
        /// </summary>
        /// <param name="VcodeNum"></param>
        /// <returns></returns>
        public static string RndChar(int VcodeNum)
        {
            string Vchar = "2,3,4,5,6,7,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(',');
            byte i;
            Vchar = "";
            Random rnd = new Random(GetRandomSeed());
            for (i = 0; i < VcodeNum; i++)
            {
                Vchar += VcArray[rnd.Next(VcArray.Length)];
            }
            return Vchar;
        }


        public static string GetHexString(int VcodeNum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,9,A,B,C,D,E,F";
            string[] VcArray = Vchar.Split(',');
            byte i;
            Vchar = "";
            Random rnd = new Random(GetRandomSeed());
            for (i = 0; i < VcodeNum; i++)
            {
                Vchar += VcArray[rnd.Next(VcArray.Length)];
            }
            return Vchar;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalWidth"></param>
        /// <param name="charArray"></param>
        /// <returns></returns>
        public static string GetRandomString(int totalWidth, char[] charArray)
        {
            return GetRandomString(totalWidth, 1, charArray).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalWidth">随机数位数</param>
        /// <param name="genCount">几个随机数</param>
        /// <param name="charArray">源字符集</param>
        /// <returns></returns>
        public static List<string> GetRandomString(int totalWidth, int genCount, char[] charArray)
        {
            List<string> list = new List<string>();

            if (totalWidth > 0 && genCount > 0 && charArray.Length > 0)
            {
                Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));

                for (int i = 0; i < genCount; i++)
                {
                    StringBuilder sb = new StringBuilder();

                    while (sb.Length < totalWidth)
                    {
                        sb.Append(charArray[rnd.Next(charArray.Length)].ToString());
                    }

                    list.Add(sb.ToString());
                }
            }

            return list;
        }

        /// <summary>
        /// 从guid中获取随机数（不包括'-'）
        /// </summary>
        /// <param name="totalWidth"></param>
        /// <returns></returns>
        public static string GetRandomString(int totalWidth)
        {
            if (totalWidth <= 0)
                return string.Empty;
            else
            {
                string result = string.Empty;

                if (totalWidth > 32)
                {
                    result = System.Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 32);
                    result += GetRandomString(totalWidth - 32);
                }
                else
                {
                    result = System.Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, totalWidth);
                }

                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalWidth"></param>
        /// <returns></returns>
        public static string GetRandomNum(int totalWidth)
        {
            int outVal;
            if (!Int32.TryParse(
                string.Format("1{0}", PadString(string.Empty, '0', totalWidth)),
                out outVal))
            {
                return string.Empty;
            }

            Random rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            return PadString(rnd.Next(outVal - 1).ToString(), '0', totalWidth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="paddingChar"></param>
        /// <param name="totalWidth"></param>
        /// <returns></returns>
        public static string PadString(string input, char paddingChar, int totalWidth)
        {
            if (input == null)
                return input;

            return input.PadLeft(totalWidth, paddingChar);
        }


        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static string GetRandom(int totalWidth, bool OnlyDigits=false)
        {
            string[] STRINGS = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] DIGITS = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string rtn = string.Empty;
            Random rnd = new Random(GetRandomSeed());

            while (rtn.Length < totalWidth)
            {
                if (OnlyDigits)
                {
                    rtn += DIGITS[rnd.Next(DIGITS.Count())];
                }
                else
                {
                    rtn += STRINGS[rnd.Next(STRINGS.Count())];
                }
            }
            return rtn;
        }
    }
}
