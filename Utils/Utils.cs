using System;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;

namespace BBY_Bypass
{
    internal class Utils
    {
        public static string Time()
        {
            DateTime time = DateTime.Now;
            return time.ToString();
        }
        public static void Leave()
        {
            Console.WriteLine($"[{Utils.Time()}] Finished. Press enter to leave...");
            Console.ReadLine();
            Process.GetCurrentProcess().Kill();
        }
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static void CheckForTokens()
        {
            string path = Environment.CurrentDirectory + @"\Tokens.txt";
            if (!File.Exists(path))
            {
                string content = "";
                File.WriteAllText(path, content);
            }
        }

    }
}
