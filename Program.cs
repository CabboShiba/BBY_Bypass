using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Diagnostics;

namespace BBY_Bypass
{
    internal class Program
    {
        public static string link = "https://discord.com/api/oauth2/authorize?client_id=962011271010594847&redirect_uri=https%3A%2F%2Fsuperfuniestindianparty.rip%2Fverify&response_type=code&scope=identify%20guilds.join";
        public static string BotID;
        public static string Token;
        static void Main(string[] args)
        {
            Utils.CheckForTokens();
            string path = Environment.CurrentDirectory + @"\Tokens.txt";
            //string readtext = File.ReadAllText(path);
            var numberOfCharacters = File.ReadAllLines(path).Sum(s => s.Length);
            Console.Title = $"[{Utils.Time()}] BBY Bypass by Cabbo";
            Stopwatch TaskTimer = new Stopwatch();
            TaskTimer.Start();
            if (File.Exists(path) && numberOfCharacters >= 60)
            {
                var lines = File.ReadLines(path);
                foreach (string Token in lines)
                {
                    string verifylink = DiscordClient.AuthDiscordBot(link, Token);
                    if (verifylink.Contains("https://superfuniestindianparty.rip/verify"))
                    {
                        BBY_Bypass.Bypass(verifylink, Token);
                    }
                    else
                    {
                    }
                    string result = DiscordClient.GetDiscordBot(Token);
                    if (result != "N/A")
                    {
                        DiscordClient.RemoveDiscordBot(Token);
                    }
                    else
                    {
                        Console.WriteLine($"[{Utils.Time()}] Could not obtain BbyStealer ID. Failed to remove it.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"[{Utils.Time()}] Could not find Tokens. Please load your tokens here: {path}");
            }
            TaskTimer.Stop();
            TimeSpan TotalTime = TaskTimer.Elapsed;
            string FinalTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            TotalTime.Hours, TotalTime.Minutes, TotalTime.Seconds,
            TotalTime.Milliseconds / 10);
            Console.WriteLine($"[{Utils.Time()}] Succesfully completed all tasks in: " + FinalTime);
            Utils.Leave();
        }
    }
}
