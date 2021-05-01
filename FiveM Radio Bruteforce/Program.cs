using NAudio.CoreAudioApi;
using System;
using System.Linq;
using System.Threading;
using AutoIt;
using System.IO;
using System.Drawing;
using Console = Colorful.Console;

namespace FiveM_Radio_Bruteforce
{
    class Program
    {
        static bool Listening = false;
        static int RadioDelay = 90000;
        static void Main(string[] args)
        {
            MMDeviceEnumerator en = new MMDeviceEnumerator();
            AutoItX.AutoItSetOption("SendKeyDownDelay", 200);
            var devices = en.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

            ASCII();
            Console.Title = "Radio Bruteforcer";

            int deviceId = 1;
            foreach(var device in devices)
            {
                Console.Write("[", Color.White); Console.Write(deviceId, Color.MediumPurple); Console.Write("] " + device + Environment.NewLine, Color.White);
                deviceId++;
            }
            Console.Write("\nDevice", Color.White); Console.Write(": ", Color.MediumPurple);
            deviceId = int.Parse(Console.ReadLine());
            var singleDevice = devices[deviceId - 1];

            Console.Clear();
            ASCII(); 

            Console.Write("Time to Spend on Each Channel", Color.White); Console.Write(": ", Color.MediumPurple);
            RadioDelay = int.Parse(Console.ReadLine());

            Console.Clear();
            ASCII();

            var rnd = new Random();
            var randomRadios = Enumerable.Range(14, 200).OrderBy(x => rnd.Next()).Distinct().ToList(); //Radio channels, 14 - 200 in a randomized order

            Thread.Sleep(10000);
            for (int i = 14; i < 200; i++)
            {
                ChangeRadio(randomRadios[i]);
                Thread t = new Thread(Counter);
                t.Start();

                while (Listening)
                {
                    int audioLevel = (int)(singleDevice.AudioMeterInformation.MasterPeakValue * 100);
                    if (audioLevel == 6)
                    {
                        File.AppendAllText("hits.txt", $"Radio: {randomRadios[i]} | Heard a sound\n");
                        Console.Write("[", Color.White); Console.Write(DateTime.Now.ToLongTimeString(), Color.MediumPurple); Console.Write("] Radio ", Color.White); Console.Write(randomRadios[i], Color.MediumPurple); Console.Write(" | Heard a sound\n", Color.White);
                        t.Abort();
                        break;
                    }
                }
            }
        }
        
        static void ChangeRadio(int number)
        {
            Thread.Sleep(500);           
            AutoItX.Send("{LSHIFT}+{F2}");
            Thread.Sleep(2000);           
            AutoItX.Send("e");
            Thread.Sleep(2000);         
            AutoItX.Send("{ENTER}");
            Thread.Sleep(2000);            
            AutoItX.Send("{BS 4}");
            Thread.Sleep(2000);           
            AutoItX.Send(number.ToString());
            Thread.Sleep(2000);            
            AutoItX.Send("{ENTER}");
            Thread.Sleep(2000);           
            AutoItX.Send("e");
            Thread.Sleep(2000);           
            AutoItX.Send("{LSHIFT}+{F2}");
            Thread.Sleep(1000);          
            AntiAFK();
            Thread.Sleep(1000);
            Listening = true;
        }

        static void AntiAFK()
        {
            AutoItX.Send("w"); 
        }
        static void Counter()
        {
            Listening = true;
            Thread.Sleep(RadioDelay);
            Listening = false;
        }

        static void ASCII()
        {
            Console.WriteLine();
            Console.WriteLine("   ██▀███   ▄▄▄      ▓█████▄  ██▓ ▒█████      ▄▄▄▄    ██▀███   █    ██ ▄▄▄█████▓▓█████ ", Color.MediumPurple);
            Console.WriteLine("  ▓██ ▒ ██▒▒████▄    ▒██▀ ██▌▓██▒▒██▒  ██▒   ▓█████▄ ▓██ ▒ ██▒ ██  ▓██▒▓  ██▒ ▓▒▓█   ▀ ", Color.MediumPurple);
            Console.WriteLine("  ▓██ ░▄█ ▒▒██  ▀█▄  ░██   █▌▒██▒▒██░  ██▒   ▒██▒ ▄██▓██ ░▄█ ▒▓██  ▒██░▒ ▓██░ ▒░▒███   ", Color.MediumPurple);
            Console.WriteLine("  ▒██▀▀█▄  ░██▄▄▄▄██ ░▓█▄   ▌░██░▒██   ██░   ▒██░█▀  ▒██▀▀█▄  ▓▓█  ░██░░ ▓██▓ ░ ▒▓█  ▄ ", Color.MediumPurple);
            Console.WriteLine("  ░██▓ ▒██▒ ▓█   ▓██▒░▒████▓ ░██░░ ████▓▒░   ░▓█  ▀█▓░██▓ ▒██▒▒▒█████▓   ▒██▒ ░ ░▒████▒", Color.MediumPurple);
            Console.WriteLine("  ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ▒▒▓  ▒ ░▓  ░ ▒░▒░▒░    ░▒▓███▀▒░ ▒▓ ░▒▓░░▒▓▒ ▒ ▒   ▒ ░░   ░░ ▒░ ░", Color.MediumPurple);
            Console.WriteLine("    ░▒ ░ ▒░  ▒   ▒▒ ░ ░ ▒  ▒  ▒ ░  ░ ▒ ▒░    ▒░▒   ░   ░▒ ░ ▒░░░▒░ ░ ░     ░     ░ ░  ░", Color.MediumPurple);
            Console.WriteLine("    ░░   ░   ░   ▒    ░ ░  ░  ▒ ░░ ░ ░ ▒      ░    ░   ░░   ░  ░░░ ░ ░   ░         ░   ", Color.MediumPurple);
            Console.WriteLine("     ░           ░  ░   ░     ░      ░ ░      ░         ░        ░                 ░  ░", Color.MediumPurple);
            Console.WriteLine("                      ░                            ░                                   ", Color.MediumPurple);
        }
    }
}
