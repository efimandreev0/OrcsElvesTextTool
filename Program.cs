using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcsElvesDSTextTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Contains(".txt"))
            {
                Rebuild(args[0]);
            }
            else
            {
                Extract(args[0]);
            }
        }
        public static void Extract(string file)
        {
            var reader = new BinaryReader(File.OpenRead(file));
            List<string> strings = new List<string>();
            reader.ReadByte();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                strings.Add(ReadUntilNullByte(reader, Encoding.UTF8).Replace("\n","<lf>").Replace("\u007f", "<7F>"));
            }
            File.WriteAllLines("file.txt", strings);
        }
        public static void Rebuild(string txt)
        {
            string[] strings = File.ReadAllLines(txt);
            using (BinaryWriter writer = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(txt) + ".bin")))
            {
                writer.Write(new byte());
                for (int i = 0; i < strings.Length; i++)
                {
                    writer.Write(Encoding.UTF8.GetBytes(strings[i].Replace("<lf>", "\n").Replace("<7F>", "\u007f")));
                    writer.Write(new byte());
                }
            }
        }
        public static string ReadUntilNullByte(BinaryReader reader, Encoding encoding)
        {
            byte nullByte = 0x00;
            List<byte> bytes = new List<byte>();

            byte currentByte;
            while ((currentByte = reader.ReadByte()) != nullByte)
            {
                bytes.Add(currentByte);
            }

            string result = encoding.GetString(bytes.ToArray());
            return result;
        }

    }
}
