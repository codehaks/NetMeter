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

            var logPath = "";
            if (args.Length == 3)
            {
                logPath = args[2];
            }

            var taskList = new List<Task<Tuple<long, int>>>();
            var totalDuration = new Stopwatch();

            totalDuration.Start();
            for (int i = 0; i < requestCount; i++)
            {
                taskList.Add(GetElaspedTime(url, i));
            }

            await Task.WhenAll(taskList);
            totalDuration.Stop();



            foreach (var task in taskList)
            {
                var data = await task;
                System.IO.File.AppendAllText(logPath, $"{data.Item1},{data.Item2} \n");
            }

            Console.WriteLine("RPS : " + (requestCount * 1000) / totalDuration.ElapsedMilliseconds);


        }

        static async Task<Tuple<long, int>> GetElaspedTime(string url, int number)
        {
            var requestDuration = new Stopwatch();
            requestDuration.Start();
            var client = new HttpClient();
            var result = await client.GetAsync(url);
            requestDuration.Stop();


            Console.WriteLine($"req:{number} | Duration: {requestDuration.ElapsedMilliseconds}");
            return new Tuple<long, int>(
                requestDuration.ElapsedMilliseconds,
                number);
        }
    }
}
