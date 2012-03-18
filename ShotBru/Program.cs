﻿using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace ShotBru
{
    public class Program
    {
        public static void Main()
        {
            new Controller().Start();
        }

    }
}
