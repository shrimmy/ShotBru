using System;
using Microsoft.SPOT;
using System.Text;

namespace ShotBru
{
    public class Utility
    {
        public static string ZeroFill(string number, int digits)
        {
            bool Negative = false;
            if (number.Substring(0, 1) == "-")
            {
                Negative = true;
                number = number.Substring(1);
            }

            for (int Counter = number.Length; Counter < digits; ++Counter)
            {
                number = "0" + number;
            }
            if (Negative) number = "-" + number;
            return number;
        }

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

        public static string PadEnd(string text, int length, char character = ' ')
        {
            StringBuilder newText = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                if (text.Length > i)
                    newText.Append(text.Substring(i, 1));
                else
                    newText.Append(character);
            }

            return newText.ToString();
        }
    }
}
