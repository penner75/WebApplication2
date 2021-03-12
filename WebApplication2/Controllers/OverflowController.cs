using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    //TODO
 
    public class OverflowController : Controller
    {

        private static readonly HttpClient client = new HttpClient();
        // GET: OverflowController
        public IActionResult Index()
        {
            var watch = new Stopwatch();
            watch.Start();
            ViewData["Watch"] = watch;
            return View();
        }


        public static async Task<List<Item>> GetResults(String tag)
        {
            

            long toDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long fromDate = DateTimeOffset.UtcNow.AddDays(-7).ToUnixTimeSeconds();

            String urlVotes = String.Format("https://api.stackexchange.com/2.2/questions?page=1&fromdate={0}&todate={1}&order=desc&sort=votes&tagged={2}&site=stackoverflow&filter=!m)BmTleW)yVK9hCoB)VXD44m4-D59Fv8YFMaWFI5tpBKzwTAjBgkjG2a",fromDate, toDate,tag);
            String urlRecent = String.Format("https://api.stackexchange.com/2.2/questions?page=1&fromdate={0}&todate={1}&order=desc&sort=creation&tagged={2}&site=stackoverflow&filter=!m)BmTleW)yVK9hCoB)VXD44m4-D59Fv8YFMaWFI5tpBKzwTAjBgkjG2a", fromDate, toDate, tag);

            List<Item> mostRecent = await AccessApi(urlRecent);
            List<Item> mostVotes = await AccessApi(urlVotes);

            List<Item> merged = new List<Item>();
            int count = mostRecent.Count;
            if (count > 10)
            {
                count = 10;
            }
            for (int i =0; i<count; i++)
            {
                merged.Add(mostRecent[i]);
            }

            //add top 10 voted
            for (int i =0; i<10; i++)
            {
                bool inserted = false;
                var k = 0;
                var max = merged.Count;
                while (!inserted )//insert at correct spot
                {
                    if (merged[k].title.Equals(mostVotes[i].title))//duplicate
                    {
                        inserted = true;
                    } else if (merged[k].creation_date < mostVotes[i].creation_date)//newer, insert at index
                    {
                        merged.Insert(k, mostVotes[i]);
                        inserted = true;
                    }

                    k++;
                    if (k==max)//we reached the end, insert at back
                    {
                        merged.Insert(k, mostVotes[i]);
                        inserted = true;
                    }
                }

            }

            return merged;
        }

        private static async Task<List<Item>> AccessApi(String url)
        {
            HttpClient client = new HttpClient();
           
            var bytes = await client.GetByteArrayAsync(url);
            var decompressedJson = new StreamReader(new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress)).ReadToEnd();
            var JSON = JsonConvert.DeserializeObject<Root>(decompressedJson);

            return JSON.items;
        }

        public async Task<IActionResult> Search(String tag)
        {
            var watch = new Stopwatch();
            watch.Start();
            List<Item> results = await GetResults(tag);
            ViewData["Items"] = results;
            ViewData["Tag"] = tag;
            ViewData["Watch"] = watch;
            return View();
        }



    }
}
