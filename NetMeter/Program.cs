using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace NetMeter
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var requestCount = int.Parse(args[0]);
            var url = args[1];

            var taskList = new List<Task>();
            var s = new Stopwatch();
            s.Start();
            for (int i = 0; i < requestCount; i++)
            {
               taskList.Add(GetElaspedTime(url));
            }

            await Task.WhenAll(taskList);
            s.Stop();

            Console.WriteLine("RPS : " + (requestCount*1000) / s.ElapsedMilliseconds);


        }

        static async Task GetElaspedTime(string url)
        {
            var client = new HttpClient();    
            await client.GetAsync(url);
          
        }
    }
}
