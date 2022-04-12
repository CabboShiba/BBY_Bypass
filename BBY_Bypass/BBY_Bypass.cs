using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace BBY_Bypass
{
    internal class BBY_Bypass
    {
        public static void Bypass(string verifylink, string Token)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(verifylink);
                req.Method = "GET";
                req.Headers.Add("name", "superfuniestindianparty.rip");
                req.KeepAlive = true;
                req.Headers.Add("sec-ch-ua", " Not A;Brand\"; v = \"99\", \"Chromium\"; v = \"100\", \"Google Chrome\"; v = \"100");
                req.Headers.Add("sec-ch-ua-mobile", "?0");
                req.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                req.Headers.Add("Upgrade-Insecure-Requests", "1");
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.88 Safari/537.36";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                req.Headers.Add("Sec-Fetch-Site", "none");
                req.Headers.Add("Sec-Fetch-Mode", "navigate");
                req.Headers.Add("Sec-Fetch-User", "?1");
                req.Headers.Add("Sec-Fetch-Dest", "document");
                req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                req.Headers.Add("Accept-Language", "it-IT,it;q=0.9,en;q=0.8,en-US;q=0.7");
                var httpResponse = (HttpWebResponse)req.GetResponse();

                string result;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                Console.WriteLine($"[{Utils.Time()}] Response Code: {httpResponse.StatusCode}");
                Console.WriteLine($"[{Utils.Time()}] Sent request to BBY Server. Response: {result}");
                if (result.Contains("done"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{Utils.Time()}] Succesfully bypassed BBY Bot. Token: {Token.Substring(0, 30)}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[{Utils.Time()}] Error during BBY Bot bypass.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[{Utils.Time()}] Error during BBY Bypass process.");
                Console.WriteLine($"[{Utils.Time()}] {ex.Message}");
            }
        }
    }
}
