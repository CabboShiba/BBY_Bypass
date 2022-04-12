using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;

namespace BBY_Bypass
{
    #region json stuff
    public class Response
    {
        public string location { get; set; }
    }
    public class Application
    {
        public string id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public string summary { get; set; }
        public object type { get; set; }
        public bool hook { get; set; }
        public string guild_id { get; set; }
        public bool bot_public { get; set; }
        public bool bot_require_code_grant { get; set; }
        public string terms_of_service_url { get; set; }
        public string privacy_policy_url { get; set; }
        public string custom_install_url { get; set; }
        public string verify_key { get; set; }
        public List<string> tags { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public List<string> scopes { get; set; }
        public Application application { get; set; }
    }
    #endregion
    internal class DiscordClient
    {
        public static string X_Super_Properties = "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiRGlzY29yZCBDbGllbnQiLCJyZWxlYXNlX2NoYW5uZWwiOiJzdGFibGUiLCJjbGllbnRfdmVyc2lvbiI6IjAuMS45Iiwib3NfdmVyc2lvbiI6IjEwLjAuMTkwNDMiLCJvc19hcmNoIjoieDY0Iiwic3lzdGVtX2xvY2FsZSI6Iml0IiwiY2xpZW50X2J1aWxkX251bWJlciI6MTIzNDU1LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==";
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) discord/0.1.9 Chrome/83.0.4103.122 Electron/9.4.4 Safari/537.36";

        public static string AuthDiscordBot(string link, string Token)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(link);
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.Method = "POST";
                req.Headers.Add("name", "discord.com");
                req.KeepAlive = true;
                req.Headers.Add("X-Super-Properties", DiscordClient.X_Super_Properties);
                req.Headers.Add("X-Discord-Locale", "it");
                req.Headers.Add("X-Debug-Options", "bugReporterEnabled");
                req.Headers.Add("Accept-Language", "it,it-IT;q=0.9");
                req.Headers.Add("Authorization", Token);
                req.UserAgent = DiscordClient.UserAgent;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Origin", "https://discord.com");
                req.Headers.Add("Sec-Fetch-Site", "same-origin");
                req.Headers.Add("Sec-Fetch-Mode", "cors");
                req.Headers.Add("Sec-Fetch-Dest", "empty");
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string json = "{\"permissions\":\"0\",\"authorize\":true}";

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)req.GetResponse();

                string result;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                if (result.Contains("location"))
                {
                    string responsefinal = Regex.Replace(result.Trim('"').Replace("\\\"", "\""), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                    var VerifyLink = JsonConvert.DeserializeObject<Response>(responsefinal);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{Utils.Time()}] Succesfully obtained Verify Link:\n{VerifyLink.location}.  Token: {Token.Substring(0, 30)}");
                    Console.ResetColor();
                    return VerifyLink.location;
                }
                else
                {
                    Console.WriteLine($"[{Utils.Time()}] Error. Could not obtain Verify Link. Response message {result}");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{Utils.Time()}] Error during Discord Bot Auth process.");
                Console.WriteLine($"[{Utils.Time()}] {ex.Message}");
                return "";
            }
        }

        public static string GetDiscordBot(string Token)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/oauth2/tokens");
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.Method = "GET";
                req.Headers.Add("name", "discord.com");
                req.KeepAlive = true;
                req.Headers.Add("X-Super-Properties", DiscordClient.X_Super_Properties);
                req.Headers.Add("X-Discord-Locale", "it");
                req.Headers.Add("X-Debug-Options", "bugReporterEnabled");
                req.Headers.Add("Accept-Language", "it,it-IT;q=0.9");
                req.Headers.Add("Authorization", Token);
                req.UserAgent = DiscordClient.UserAgent;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Origin", "https://discord.com");
                req.Headers.Add("Sec-Fetch-Site", "same-origin");
                req.Headers.Add("Sec-Fetch-Mode", "cors");
                req.Headers.Add("Sec-Fetch-Dest", "empty");
                var httpResponse = (HttpWebResponse)req.GetResponse();

                string result;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                result = result.Trim().Trim('[', ']');
                string responsefinal = Regex.Replace(result.Trim('"').Replace("\\\"", "\""), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
                //https://www.newtonsoft.com/json/help/html/ReadMultipleContentWithJsonReader.htm
                IList<Root> roles = new List<Root>();
                JsonTextReader reader = new JsonTextReader(new StringReader(responsefinal));
                reader.SupportMultipleContent = true;
                while (true)
                {
                    if (!reader.Read())
                    {
                        break;
                    }
                    JsonSerializer serializer = new JsonSerializer();
                    Root role = serializer.Deserialize<Root>(reader);

                    roles.Add(role);
                }

                foreach (Root role in roles)
                {
                    string a = role.application.name;
                    if (a.Contains("Bby"))
                    {
                        Program.BotID = role.id;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[{Utils.Time()}] Succesfully obtained BbyStealer ID. Token: {Token.Substring(0, 30)}");
                        Console.ResetColor();
                        return Program.BotID;
                    }
                    else
                    {
                    }
                }
                return "N/A";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{Utils.Time()}] Error during Discord Bot scraping process.");
                Console.WriteLine($"[{Utils.Time()}] {ex.Message}");
                return "N/A";
            }

        }
        public static void RemoveDiscordBot(string Token)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/oauth2/tokens/" + Program.BotID);
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.Method = "DELETE";
                req.Headers.Add("name", "discord.com");
                req.KeepAlive = true;
                req.Headers.Add("X-Super-Properties", DiscordClient.X_Super_Properties);
                req.Headers.Add("X-Discord-Locale", "it");
                req.Headers.Add("X-Debug-Options", "bugReporterEnabled");
                req.Headers.Add("Accept-Language", "it,it-IT;q=0.9");
                req.Headers.Add("Authorization", Token);
                req.UserAgent = DiscordClient.UserAgent;
                req.ContentType = "application/json";
                req.Accept = "*/*";
                req.Headers.Add("Origin", "https://discord.com");
                req.Headers.Add("Sec-Fetch-Site", "same-origin");
                req.Headers.Add("Sec-Fetch-Mode", "cors");
                req.Headers.Add("Sec-Fetch-Dest", "empty");
                var httpResponse = (HttpWebResponse)req.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{Utils.Time()}] Succesfully removed BBY Bot from your authed application. Token: {Token.Substring(0, 30)}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"[{Utils.Time()}] Error during Discord Bot removing process. Status Code {httpResponse.StatusCode}.  Token: {Token.Substring(0, 30)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{Utils.Time()}] Error during Discord Bot removing process. Token: {Token.Substring(0, 30)}");
                Console.WriteLine($"[{Utils.Time()}] {ex.Message}");
            }
        }


    }
}
