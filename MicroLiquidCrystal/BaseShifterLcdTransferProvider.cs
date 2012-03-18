// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

namespace MicroLiquidCrystal
{
    public abstract class BaseShifterLcdTransferProvider : ILcdTransferProvider
    {
        /// <summary>
        /// Implement this method on derived class.
        /// </summary>
        /// <param name="output"></param>
        protected abstract void SendByte(byte output);

        // bytes are send in this order
        // +--------- 0x80 d7
        // |+-------- 0x40 d6
        // ||+------- 0x20 d5
        // |||+------ 0x10 d4
        // |||| +---- 0x08 enable  
        // |||| |+--- 0x04 rw  
        // |||| ||+-- 0x02 rs  
        // |||| |||+- 0x01 backlight
        // 7654 3210

        private const byte BackLightMask = 0x01;
        private const byte RSMask = 0x02;
        private const byte RWMask = 0x04;
        private const byte EnableMask = 0x08;

        public void Send(byte data, bool mode, bool backlight)
        {
            int output = data & 0xF0;                   // set the first four data-bits
            output |= (backlight) ? BackLightMask : 0;  // set LSB to 1 when back light is On
            output |= (mode) ? RSMask : 0;              // set R/S HIGH
            output &= ~RWMask;                          // set RW LOW
            PulseEnable((byte) output);

            output &= 0x0F;                             // keep the control bits
            output |= (data << 4) & 0xF0;               // set HByte to zero 
            PulseEnable((byte) output);
        }

        private void PulseEnable(int output)
        {
            output &= ~EnableMask;      // set Enable LOW
            SendByte((byte) output);
            output |= EnableMask;       // set Enable HIGH
            SendByte((byte) output);
            output &= ~EnableMask;      // set Enable LOW
            SendByte((byte) output);
        }

        public bool FourBitMode
        {
            get { return true; }
        }

    }
}