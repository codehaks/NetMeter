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

            var requestCount = 1000;//int.Parse(args[0]);
            var url = "http://localhost:5000/api/sync";// args[1];

            var taskList = new List<Task<Tuple<int, long,int>>>();
            var s = new Stopwatch();
            s.Start();
            for (int i = 0; i < requestCount; i++)
            {
                 url = "http://localhost:5000/api/sync/" + i;
                taskList.Add(GetElaspedTime(url,i));
            }

            await Task.WhenAll(taskList);
            s.Stop();

          

            foreach (var task in taskList)
            {
                var data = await task;
                //Console.WriteLine($" {data.Item1} -> {data.Item2}");
                System.IO.File.AppendAllText("d:\\logs.txt",$"{data.Item1},{data.Item2},{data.Item3}\n");
            }

            Console.WriteLine("RPS : " + (requestCount * 1000) / s.ElapsedMilliseconds);


        }

        static async Task<Tuple<int, long,int>> GetElaspedTime(string url,int number)
        {
            var s = new Stopwatch();
            s.Start();
            var client = new HttpClient();
            var result = await client.GetAsync(url);
            s.Stop();
            var threadCount=int.Parse(await result.Content.ReadAsStringAsync());
            Console.WriteLine($"req:{number} | Threads: {threadCount} | Duration: {s.ElapsedMilliseconds}");
            return new Tuple<int, long,int>(
                threadCount,
                s.ElapsedMilliseconds,number);

        }
    }
}
