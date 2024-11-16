using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TGBApi;
using TGBApi2StructC;

namespace TGBApi2DC
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri tgscraper = new Uri("https://tgscraper.sys001.xyz/schemas/botapi.json");
            
            using (var webClient = new System.Net.WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json;charset=utf-8");
                string json = webClient.DownloadString(tgscraper);// = System.IO.File.ReadAllText(args[0]);
                // Now parse with JSON.Net
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(rootJson));
                rootJson result = (dcjs.ReadObject(ms)) as rootJson;
                Stopwatch stopw = new Stopwatch();
                stopw.Start();
                TGBApi2DC.GenerateContracts(result);
                //TGBApi2StructC.TGBApi2StructC.GenerateContracts(result);
                Directory.CreateDirectory(@".\methods");
                foreach (Methods method in result.methods)
                {

                    break;
                }

                stopw.Stop();
                Console.Write("Time Taken to Generate: ");
                Console.WriteLine(stopw.ElapsedMilliseconds);
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();

            }
        }
    }
}
