using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common
{
    public class RandomHelper
    {
        private static int Minimum { get; set; } = 1;
        private static int Maximal { get; set; } = 999;
        private static int RandomLength { get; set; } = 4;

        private const string RANDOMSTRING  = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";
        private static Random _random = new Random(DateTime.Now.Second);

        public RandomHelper(int minimum, int maximal, int randomLength)
        {
            Minimum = minimum;
            Maximal = maximal;
            RandomLength = randomLength;
        }

        /// <summary>
        /// 产生随机字符串
        /// </summary>
        /// <param name="stringLength">字符串长度</param>
        /// <returns></returns>
        public static string GetRandomString(int stringLength)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < stringLength; i++)
            {
                int r = _random.Next(0, RANDOMSTRING.Length - 1);
                returnValue += RANDOMSTRING[r];
            }
            return returnValue;
        }
        public static string GetRandomString()
        {
            return GetRandomString(RandomLength);
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        /// <returns></returns>
        public static int GetRandom(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber);
        }
        public static int GetRandom()
        {
            return _random.Next(Minimum, Maximal);
        }

    }
}
