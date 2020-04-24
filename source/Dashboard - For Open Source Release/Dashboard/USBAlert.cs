using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Management;
using System.Windows.Forms;

namespace VistA_Nurses_Dashboard
{
    
    class USBAlert
    {
        public static SerialPort currentPort;

        private static string GetArduinoPort()
        {
            string thisCaption;
            string thisCom;
            thisCom = "";
            int i;
            int j;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    thisCaption = queryObj["Caption"].ToString();
                    if (thisCaption.Contains("(COM"))
                    {
                        //if (thisCaption.Contains("Arduino Uno"))
                        if (thisCaption.Contains("Silicon Labs CP210x USB to UART Bridge"))
                        {
                            //MessageBox.Show(queryObj["Caption"].ToString());
                            i = thisCaption.IndexOf("(COM");
                            if (i > -1)
                            {
                                thisCom = thisCaption.Substring(i + 1);
                                j = thisCom.IndexOf(")");
                                if (j > -1)
                                {
                                    thisCom = thisCom.Substring(0, j);
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show(e.Message);
            }
            return thisCom;
        }

        public static string GetUSBAlertComPort()
        {
            string portName;
            string arduinoPort;
            portName = "";
            try
            {                         
                
              
                string[] ports = SerialPort.GetPortNames();

                arduinoPort = GetArduinoPort();
                if (arduinoPort == "")
                {
                    return portName;
                }
                               
                foreach (string port in ports)
                    if (port == arduinoPort)
                    {
                        {
                            currentPort = new SerialPort(port, 115200);
                            
                            
                                if (DetectArduino())
                                {
                                    GlobalVars.portFound = true;
                                    GlobalVars.USBAlertComPort = currentPort;
                                    portName = currentPort.PortName;
                                    break;
                                }
                                else
                                {
                                    GlobalVars.portFound = false;
                                }
                            
                        }
                    }
            }
            catch (Exception e)
            {
                currentPort.Close();
                return portName;
            }
            currentPort.Close();
            return portName;
        }
        public static void TurnOnRedLight() //Consults
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(253);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void TurnOnYellowLight() //Labs
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(254);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void TurnOnGreenLight()  //Orders
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(255);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void TurnOffAllLights()
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(0);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void TurnOffAllLightsAndLCD()
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(220);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void TurnOffAllLightsSetLCDtoReady()
        {
            byte[] buffer = new byte[5];
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(127);
            buffer[2] = Convert.ToByte(4);
            buffer[3] = Convert.ToByte(200);
            buffer[4] = Convert.ToByte(4);
            SendMessage(buffer);
        }

        public static void SendMessage(byte[] message)
        {
            try
            {
                GlobalVars.USBAlertComPort.Open();
                GlobalVars.USBAlertComPort.Write(message, 0, 5);
                Thread.Sleep(300);
                GlobalVars.USBAlertComPort.Close();
            }
            catch
            {
            }
        }

        public static bool DetectArduino()
        {
            try
            {
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[5];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);
                buffer[2] = Convert.ToByte(0);
                buffer[3] = Convert.ToByte(0);
                buffer[4] = Convert.ToByte(4);

                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;

                
                currentPort.Open();
                currentPort.Write(buffer, 0, 5);
                Thread.Sleep(1500);

                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                    count--;
                }
                if (returnMessage.Contains("ARDUINO"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
