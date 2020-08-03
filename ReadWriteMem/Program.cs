using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;

/*
 * Issue with MemoryMappedFile.CreateFromFile: https://github.com/dotnet/runtime/issues/16371
 * -> CreateFromFile doesn't use sane defaults. It's suggested to create the filestream yourself and pass
 *    it to CreateFromFile.
 * 
 * Of course, this doesn't matter when you use CreateOrOpen. At that point, you're just trying to make
 * sure the mapName is unique to any other mutex, handle, etc, etc on the system.
 */

namespace ReadWriteMem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string memfile = "{B4ECD457-AF52-45DC-8E2D-790E18BA7A85}";
                using (MemoryMappedFile mmf = HighPerfLogLib.MemoryOperations.CreateFileHandle(memfile, 1500))
                {
                    byte[] buffer = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF };

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        Console.WriteLine(buffer[i]);
                    }

                    HighPerfLogLib.MemoryOperations.WriteShMem(mmf, buffer, 0, buffer.Length);

                    byte[] buffer2 = null;

                    buffer2 = HighPerfLogLib.MemoryOperations.ReadShMem(mmf, 0, buffer.Length);

                    for (int i = 0; i < buffer2.Length; i++)
                    {
                        Console.WriteLine(buffer2[i]);
                    }

                    Console.ReadLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
