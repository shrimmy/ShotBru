using System;
using Microsoft.SPOT;

namespace ShotBru
{
    public class Utility
    {
        public static string Pad(int number, int digits)
        {
            string str = number.ToString();
            if (str.Length < digits)
            {
                for (int i = 0; i < digits + 1 - str.Length; i++)
                {
                    str = "0" + str;
                }
            }
            return str;
        }

        public static string PadEndWithSpace(string str, int characters)
        {
            string strTemp = str;
            int length = strTemp.Length;
            if (strTemp.Length < characters)
            {
                for (int i = 0; i < characters + 1 - length; i++)
                {
                    strTemp += " ";
                }
            }
            return strTemp;
        }
    }
}
