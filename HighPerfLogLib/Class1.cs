using System;
using System.IO;
using System.Security.Cryptography;
using System.IO.MemoryMappedFiles;

namespace HighPerfLogLib
{
    public static class MemoryOperations
    {
        public static MemoryMappedFile CreateFileHandle(string file, int length)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("The input file cannot be empty.");
            if (length < 0)
                throw new ArgumentException("The input length cannot be less than zero.");

            MemoryMappedFileAccess access = MemoryMappedFileAccess.ReadWrite;
            MemoryMappedFileOptions options = MemoryMappedFileOptions.None;
            HandleInheritability inheritability = HandleInheritability.Inheritable;

            return MemoryMappedFile.CreateOrOpen(file, length, access, options, inheritability);
        }

        public static void WriteShMem(MemoryMappedFile file, byte[] buffer, int startPos, int length)
        {
            if (MemoryMappedFile.ReferenceEquals(file, null))
                throw new ArgumentNullException(nameof(file), "The argument cannot be null");

            using (MemoryMappedViewStream stream = file.CreateViewStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(buffer, startPos, length);
                }
            }
        }

        public static byte[] ReadShMem(MemoryMappedFile file, int startPos, int length)
        {
            if (MemoryMappedFile.ReferenceEquals(file, null))
                throw new ArgumentException("The input file cannot be empty.");
            if (startPos < 0)
                throw new ArgumentException("The input startPos cannot be less than zero.");
            if (length < 0)
                throw new ArgumentException("The input length cannot be less than zero.");

            byte[] bytes = new byte[length];
            
            using (MemoryMappedViewStream stream = file.CreateViewStream())
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.Read(bytes, startPos, length);
                }
            }

            return bytes;
        }
    }
}
