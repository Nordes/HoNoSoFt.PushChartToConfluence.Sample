using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoNoSoFt.PushChartToConfluence.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public ImagesController(IHttpClientFactory clientFactory) {
            var confluenceHttpClient = clientFactory.CreateClient("confluence");
            confluenceHttpClient.BaseAddress = new Uri("https://confluence.kmiservicehub.com/api");
        }

        /// <summary>
        /// This will save the image in your local "temp" folder.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Created)]
        public async Task<IActionResult> PostAsync(IFormCollection formCollection)
        {
            var files = formCollection.Files;
            long size = files.Sum(f => f.Length);
            List<string> fileList = new List<string>();

            foreach (var formFile in files)
            {
                // full path to file in temp location
                var filePath = Path.GetTempFileName();
                fileList.Add(filePath);
                if (formFile.Length > 0)
                {
                    Console.WriteLine($"form file length: {formFile.Length}");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Console.WriteLine($"Copy to file...");
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, fileList});
        }

        /// <summary>
        /// This will save the image in your local "temp" folder.
        /// </summary>
        /// <remarks>
        /// More details can be found at https://developer.atlassian.com/server/confluence/confluence-rest-api-examples/
        /// </remarks>
        [HttpPost]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Created)]
        public async Task<IActionResult> PostToConfluence(IFormCollection formCollection)
        {
            var files = formCollection.Files;
            long size = files.Sum(f => f.Length);
            List<string> fileList = new List<string>();

            foreach (var formFile in files)
            {
                // full path to file in temp location
                if (formFile.Length > 0)
                {
                    Console.WriteLine($"Form file length to send: {formFile.Length}");

                    // using (var stream = new FileStream(filePath, FileMode.Create))
                    // {
                    //     Console.WriteLine($"Copy to file...");
                    //     await formFile.CopyToAsync(stream);
                    // }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, fileList});
        }
    }
}
