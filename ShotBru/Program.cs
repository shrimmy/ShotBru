using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using MicroLiquidCrystal;
using ShotBru.Models;
using ShotBru.Modes;

namespace ShotBru
{
    public class Program
    {
        #region Local Varaibles
        // variables
        private ShotModel model;
        private Timer displayTimer;
        private ModeController modeController;

        // pin defenitions
        private Cpu.Pin keyPad_Input = Pins.GPIO_PIN_A0;
        private Cpu.Pin sensor1_Input = Pins.GPIO_PIN_A1;
        //private Cpu.Pin sensor1_Tip = Pins.GPIO_PIN_A2;
        private Cpu.Pin sensor1_Power = Pins.GPIO_PIN_D0;
        private Cpu.Pin camera1_Shutter = Pins.GPIO_PIN_D1;
        private Cpu.Pin lcd_RS = Pins.GPIO_PIN_D2;
        private Cpu.Pin lcd_EN = Pins.GPIO_PIN_D3;
        private Cpu.Pin lcd_D4 = Pins.GPIO_PIN_D4;
        private Cpu.Pin lcd_D5 = Pins.GPIO_PIN_D5;
        private Cpu.Pin lcd_D6 = Pins.GPIO_PIN_D6;
        private Cpu.Pin lcd_D7 = Pins.GPIO_PIN_D7;

        // drivers
        private KeyPad keyPad;
        private Lcd lcd;
        private Camera camera1;
        private Sensor sensor1;
        #endregion

        public static void Main()
        {
            new Program().Start();
        }

        public void Start()
        {
            Initialize();

            Thread.Sleep(Timeout.Infinite);
        }

        private void Initialize()
        {
            lcd = new Lcd(new GpioLcdTransferProvider(lcd_RS, lcd_EN, lcd_D4, lcd_D5, lcd_D6, lcd_D7));
            lcd.Begin(16, 2);
            DisplayLine("Shot Bru (V1)");
            DisplayLine("Initializing...", 1);

            model = new ShotModel();
            modeController = new ModeController(new Mode[] 
                {
                    new HomeMode(model),
                    new LightMode(model),
                });

            keyPad = new KeyPad(keyPad_Input);
            keyPad.KeyPressed += new KeyPressedEventHandler(keyPad_KeyPressed);

            sensor1 = new Sensor(model, sensor1_Input, sensor1_Power);
            sensor1.Triggered += new TriggerEventHandler(sensor1_Triggered);

            camera1 = new Camera(camera1_Shutter);
            
            // setup the display timer to fire every 250mS,
            displayTimer = new Timer(new TimerCallback(displayTimer_Fired), model, 250, 250);

            // wait for 1 second, gives sensors a chance to start up
            Thread.Sleep(1000);
            //sensor1.Start();
        }

        /// <summary>
        /// Displays a line on the lcd display. Empty space will be added to the end of the line to remove any previous text
        /// </summary>
        /// <param name="line">Text to display</param>
        /// <param name="row">Optional row number</param>
        private void DisplayLine(string line, int row = 0)
        {
            lcd.SetCursorPosition(0, row);
            lcd.Write(Utility.PadEndWithSpace(line, 16));
        }

        private void displayTimer_Fired(object state)
        {
            Mode v = modeController.Current;

            lcd.SetCursorPosition(0, 0);
            lcd.Write(Utility.PadEnd(v.Line1, 16));
            lcd.SetCursorPosition(0, 1);
            lcd.Write(Utility.PadEnd(v.Line2, 16));
        }

        private void sensor1_Triggered(int value)
        {
            if (model.TriggerDelay > 0) { Thread.Sleep(model.TriggerDelay * 1000); } // delay before taking photo
            model.TriggerCount++;
            camera1.TakePhoto();
            modeController.Current.Hide();
            if (model.IsAutoReset && model.AutoResetDelay > 0) 
            { 
                // delay before activating again
                Thread.Sleep(model.AutoResetDelay * 1000);
                model.IsPaused = false;
            }
            modeController.Current.Show();
        }

        private void keyPad_KeyPressed(Key key)
        {
            if (key == Key.Menu)
            {
                if (model.IsExecuting)
                {
                    // immediately stop executing
                    model.IsExecuting = false;
                    modeController.ResetCurrent();
                }
                else
                {
                    modeController.Next();
                }
            }
            else
            {
                if (!model.IsExecuting)
                {
                    // route all other keys, but only if we are not currently executing
                    modeController.Current.RouteKeyPressed(key);
                }
            }
        }
    }
}
