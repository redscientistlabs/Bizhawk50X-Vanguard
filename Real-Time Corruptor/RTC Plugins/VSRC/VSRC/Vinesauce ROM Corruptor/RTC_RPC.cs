using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace RTC
{

    public static class RTC_RPC
    { // localhost communication to the RTC program
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        /*

         
         >>>> Steps for adding RTC plugin stuff
        
         Step 1: transfer main(args[]) to RTC.RPG.args

         Step 2: Call the Plugin_Loaded method at the end of your main form Load method 
         
            try
            {
                if (RTC.RTC_RPC.args[0] == "-RTC")
                    RTC.RTC_RPC.Plugin_Loaded(this);
            }
            catch { }

         Step 3: Fill the Plugin_Set and Plugin_Corrupt using your form. If your corruption program uses the
                 values in the control of your main form, you'll be able to call them by making them public and
                 by using the corruptorForm form object that is stored in this class.
                 If not, you'll have to edit things in order to provide access to them.

         Step 4 (facultative): hide the "Original file", "Corrupted file" and "External corruptor" components of your
                               main form if the program was called with the -RTC argument. That way, it will look a bit
                               more lighter and feel more like a proper plugin. 

         Step 5 : put a file in your exe's file directory called RTC.dat that contains the name of your main executable (ex: Corruptor.exe)

         The program will be executed as a plugin when put in the PLUGNS folder of RTC. Take note that the name of the plugin which
         will appear is gonna be the name of the folder. RTC will use the provided functions to interract with your software.
         
         
         
        */


        //Fill these functions below.

        private static void Plugin_Set(string OriginalFile, string CorruptedFile, string ExternalCorruptor)
        {
            //use the param variables to set the settings in your corruptor

            Vinesauce_ROM_Corruptor.MainForm.SelectedROM = new Vinesauce_ROM_Corruptor.RomId(OriginalFile);
            (corruptorForm as Vinesauce_ROM_Corruptor.MainForm).EnforceAutoEnd(); //required by vsrc
            (corruptorForm as Vinesauce_ROM_Corruptor.MainForm).textBox_SaveLocation.Text = CorruptedFile;
            (corruptorForm as Vinesauce_ROM_Corruptor.MainForm).textBox_EmulatorToRun.Text = ExternalCorruptor;
        }

        private static void Plugin_Corrupt()
        {
            //call your Corrupt method here

            (corruptorForm as Vinesauce_ROM_Corruptor.MainForm).button_Run_Click(new object(), new EventArgs());
        }



        public static void Plugin_Loaded(Form _form)
        {
            //The corruptor's main form must call THIS method at the end of the Form Load method
            //Feed the form parameter with the form itself (this).

            Start(_form);
            SendToRTC("RTC|ASK_PLUGIN_SET");
        }



        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------



        public static string[] args;                                            //To keep the program's main() args[]
        public static volatile bool Running = false;                            //Enables or disables the program
        private static string ip = "127.0.0.1";                                 //Localhost address
        private static int listenPort = 56664;                                  //Plugin's listen adress
        private static int sendPort = 56665;                                    //RTC'S plugin listen adress

        private static System.Windows.Forms.Timer time;                         //Message timer
        private static Thread t;                                                //thread for udp loop
        private static volatile Queue<String> messages = new Queue<string>();   //volatile message queue
        private static UdpClient pluginSender = new UdpClient(ip, sendPort);    //sender client
        private static Form corruptorForm = null;                               // should contain the external corruptor main form





        private static void Start(Form _corruptorForm)
        {
            Running = true;
            corruptorForm = _corruptorForm;

            time = new System.Windows.Forms.Timer();
            time.Interval = 200; //default message check every 200ms
            time.Tick += new EventHandler(CheckMessages);
            time.Start();

            t = new Thread(new ThreadStart(Listen));
            t.IsBackground = true;
            t.Start();
        }

        private static void Listen()
        {
            bool done = false;

            UdpClient pluginListener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ip), listenPort);

            try
            {
                while (!done)
                {
                    if (!Running)
                        break;

                    byte[] bytes = pluginListener.Receive(ref groupEP);

                    messages.Enqueue(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                pluginListener.Close();
            }
        }

        private static void CheckMessages(object sender, EventArgs e)
        {
            string msg = "";
            while (messages.Count != 0)
            {
                msg = messages.Dequeue();
                string[] splits = msg.Split('|');

                switch (splits[0])
                {
                    default:
                        break;
                    case "RTC_Plugin":

                        switch (splits[1])
                        {
                            case "SET":
                                Plugin_Set(splits[2], splits[3], splits[4]);
                                break;
                            case "CLOSE":
                                Application.Exit();
                                break;
                            case "CORRUPT":
                                Plugin_Corrupt();
                                break;
                        }

                        break;
                }
            }

        }

        private static void SendToRTC(string msg)
        {
            if (!Running)
                return;

            Byte[] sdata = Encoding.ASCII.GetBytes(msg);
            pluginSender.Send(sdata, sdata.Length);
        }

    }
}
