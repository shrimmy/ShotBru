using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace ShotBru
{
    public class Camera : IDisposable
    {
        private OutputPort shutter;

        public Camera(Cpu.Pin shutterPin)
        {
            shutter = new OutputPort(shutterPin, false);
        }

        public void TakePhoto()
        {
            shutter.Write(true);
            Thread.Sleep(100);
            shutter.Write(false);
        }

        public void Dispose()
        {
            shutter.Dispose();
        }
    }
}
