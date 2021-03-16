using System;
using System.IO;
using System.Threading;
namespace real_i8080_runner
{
    class Program
    {
        static void Main(string[] args)
        {
            string twodigitshex(byte x) => x.ToString("X").Length >= 2 ? x.ToString("X") : "0" + (x.ToString("X"));

            bool debuginfo = true;
            Console.WriteLine("i8080 Emulator made by someone who doesnt know how i8080 works.\ncopyright janx 2069 TM. all rights reserved\n\nEnter ROM name to load:");
            string romName = Console.ReadLine();
            Console.WriteLine("\nOffset (in hex):");
            string romOffset = Console.ReadLine();
            Console.WriteLine("\nClock Rate (in Hertz):");
            string clockrate_b = Console.ReadLine();
            Console.WriteLine("Initializing 2^16 bytes of memory");
            byte[] memory = new byte[(int)Math.Pow(2, 16)];
            byte[] registers = new byte[]
            {
                0x00, // a(ccumulator)
                0b00000010, // flags (SZ0A0P1C)
                0x00, // b
                0x00, // c
                0x00, // d
                0x00, // e
                0x00, // h
                0x00, // l
            };
            Console.WriteLine("Done");
            Console.WriteLine("Reading and Inserting ROM");
            Console.WriteLine(Convert.ToInt32(romOffset, 16));

            int ii = 0;
            foreach (var item in System.IO.File.ReadAllBytes(romName))
            {
                memory[Convert.ToInt32(romOffset, 16) + ii] = item;
                ii++;
            }

            Console.WriteLine("Done");
            Thread.Sleep(500);
            Console.Clear();
            int iii = 0;
            int clockrate = int.Parse(clockrate_b);
            //reusable functions
            // decided to scrap them cuz fuck productivity
            string printmemmy(int f)
            {
                string stringy = "";
                for (int i = 0; i < 0x10; i++)
                {
                    stringy += twodigitshex(memory[f+i])+" ";
                }
                return stringy;
            }
            while (true)
            {
                Console.Clear();
                switch (memory[iii])
                {
                    case 0x00: //NOP
                        break;
                    case 0x01: //LXI B,d16
                        registers[2] = memory[iii + 2]; // low endian makes it +2,+1 not +1,+2
                        registers[3] = memory[iii + 1];
                        iii += 2;
                        break;
                    case 0x02: //STAX B
                        // fuck stax and ldax tHEWYRE WRIHUAHRGHSD8  
                        break;
                    default:
                        Console.WriteLine("Illegal opcode "+memory[iii].ToString("X")+" at "+iii.ToString("X"));
                        break;
                }
                if(debuginfo == true)
                {
                    Console.WriteLine("a:"+registers[0].ToString("X") + " b: " + registers[2].ToString("X") + " c: " + registers[3].ToString("X") + " d: " + registers[4].ToString("X") + " e: " + registers[5].ToString("X") + " h: " + registers[6].ToString("X") + " l: " + registers[7].ToString("X") + "");
                    Console.WriteLine("   x0 x1 x2 x3 x4 x5 x6 x7 x8 x9 xA xB xC xD xE xF");
                    Console.WriteLine("xf " + printmemmy(0));
                }
                Thread.Sleep(1000 / clockrate);
                iii += 1; 
            }
        }
    }
}
