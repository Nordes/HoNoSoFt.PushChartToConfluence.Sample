﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HoNoSoFt.PushChartToConfluence.Sample.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HoNoSoFt.PushChartToConfluence.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
    private readonly HttpClient _confluenceHttpClient;

    public ImagesController(IHttpClientFactory clientFactory, IOptions<ConfluenceConfig> confluenceConfig) {
            _confluenceHttpClient = clientFactory.CreateClient("confluence");
            
            _confluenceHttpClient.BaseAddress = confluenceConfig.Value.BaseApiUri;
            _confluenceHttpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {confluenceConfig.Value.GetBase64Token()}");
            _confluenceHttpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
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
        [HttpPost("bis")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Created)]
        public IActionResult PostToConfluence(IFormCollection formCollection)
        {
            var files = formCollection.Files;
            long size = files.Sum(f => f.Length);
            List<string> fileList = new List<string>();

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            foreach (var formFile in files)
            {
                // full path to file in temp location
                if (formFile.Length > 0)
                {
                    byte[] data;
                    using (var br = new BinaryReader(formFile.OpenReadStream()))
                        data = br.ReadBytes((int)formFile.OpenReadStream().Length);

                    ByteArrayContent bytes = new ByteArrayContent(data);


                    Console.WriteLine($"Form file length to send: {formFile.Length}");

                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("comment", "Automatic Upload"), 
                    });

                    multiContent.Add(formContent);
                    multiContent.Add(bytes, "file", formFile.Name);
                }
            }

            var result = _confluenceHttpClient.PostAsync("content/67657373/child/attachment", multiContent).Result;

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, fileList});
        }
    }
}
