// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using System;
using Microsoft.SPOT.Hardware;

namespace MicroLiquidCrystal
{
    public class Shifter74Hc595LcdTransferProvider : BaseShifterLcdTransferProvider, IDisposable
    {
        private readonly BitOrder _bitOrder;
        private readonly SPI _spi;
        private bool _disposed;

        public Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin, BitOrder bitOrder)
        {
            _bitOrder = bitOrder;

            var spiConfig = new SPI.Configuration(
                latchPin, 
                false, // active state
                0,     // setup time
                0,     // hold time 
                false, // clock idle state
                true,  // clock edge
                1000,   // clock rate
                spiBus);

            _spi = new SPI(spiConfig);
        }

        public Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin)
            : this(spiBus, latchPin, BitOrder.MSBFirst)
        { }

        ~Shifter74Hc595LcdTransferProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _spi.Dispose();
                _disposed = true;
            }
            if (disposing)
                GC.SuppressFinalize(this);
        }

        protected override void SendByte(byte output)
        {
            if (_bitOrder == BitOrder.LSBFirst)
                output = ReverseBits(output);

            _spi.Write(new[] { output });
        }

        public enum BitOrder
        {
            MSBFirst, LSBFirst
        }

        private static byte ReverseBits(byte v)
        {
            byte r = v; // r will be reversed bits of v; first get LSB of v
            int s = 8 - 1; // extra shift needed at end

            for (v >>= 1; v != 0; v >>= 1)
            {
                r <<= 1;
                r |= (byte)(v & 1);
                s--;
            }
            r <<= s; // shift when v's highest bits are zero
            return r;
        }
    }
}