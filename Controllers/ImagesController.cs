using HoNoSoFt.PushChartToConfluence.Sample.Configurations;
using HoNoSoFt.PushChartToConfluence.Sample.Models.Confluence;
using HoNoSoFt.PushChartToConfluence.Sample.Models.Confluence.Attachment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoNoSoFt.PushChartToConfluence.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly HttpClient _confluenceHttpClient;

        public ImagesController(IHttpClientFactory clientFactory, IOptions<ConfluenceConfig> confluenceConfig)
        {
            _confluenceHttpClient = clientFactory.CreateClient("confluence");

            _confluenceHttpClient.BaseAddress = confluenceConfig.Value.BaseApiUri;
            _confluenceHttpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {confluenceConfig.Value.GetBase64Token()}");
            _confluenceHttpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
        }

        /// <summary>
        /// This will save the image in your local "temp" folder.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
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
                    // Form file length: {formFile.Length}
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        // Save to file...
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Example of processed uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { count = files.Count, size, fileList });
        }

        /// <summary>
        /// This will save the image in your local "temp" folder.
        /// </summary>
        /// <remarks>
        /// More details can be found at https://developer.atlassian.com/server/confluence/confluence-rest-api-examples/
        /// </remarks>
        [HttpPost("confluence/{pageId}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostToConfluence(IFormCollection formCollection, int pageId)
        {
            var files = formCollection.Files;
            long size = files.Sum(f => f.Length);
            List<string> fileList = new List<string>();
            var forwardAttachmentTasks = new List<Task<FileTransferResult>>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    forwardAttachmentTasks.Add(ForwardFileToConfluence(pageId, formFile));
                }
            }

            // In case we had multiple tasks at the same time.
            await Task.WhenAll(forwardAttachmentTasks.ToArray()).ConfigureAwait(false);
            await UpdatePage(pageId, forwardAttachmentTasks).ConfigureAwait(false);

            return StatusCode((int)HttpStatusCode.InternalServerError, new { count = files.Count, size, fileList });
        }

        private async Task UpdatePage(int pageId, List<Task<FileTransferResult>> forwardTasks)
        {
            if (forwardTasks.Any())
            {
                // Update the page
                var pageContentResult = await _confluenceHttpClient.GetAsync($"content/{pageId}?expand=version,body.storage");
                var pageContentData = JsonConvert.DeserializeObject<Models.Confluence.Content.ContentStorage>(await pageContentResult.Content.ReadAsStringAsync());

                string newContent = string.Empty;
                foreach (var sentAttachmentTask in forwardTasks)
                {
                    var sentAttachment = await sentAttachmentTask;
                    if (sentAttachment != null && sentAttachment.Results.Any())
                    {
                        // Add the file if not present on the page.
                        var fileName = sentAttachment.Results.First().Title;
                        if (pageContentData.Body.Storage.Value.IndexOf($"<ri:attachment ri:filename=\"{fileName}\" />") == -1)
                        {
                            newContent += $"<h2>You've just pushed: {fileName}</h2><p><ac:image><ri:attachment ri:filename=\"{fileName}\" /></ac:image></p>";
                        }
                        // Else: Nothing to do, it's already on the page somewhere.
                    }
                }

                // Update object (camelCase mandatory, so use a proper serializer in real life scenario)
                var updateQuery = new
                {
                    id = pageContentData.Id,
                    title = pageContentData.Title,
                    status = pageContentData.Status,
                    type = pageContentData.Type,
                    version = new { number = pageContentData.Version.Number + 1 },
                    body = new
                    {
                        storage = new
                        {
                            value = pageContentData.Body.Storage.Value + newContent,
                            representation = "storage"
                        }
                    }
                };

                var result = await _confluenceHttpClient.PutAsJsonAsync($"content/{pageId}", updateQuery);
            }
        }

        private async Task<FileTransferResult> ForwardFileToConfluence(int pageId, IFormFile formFile)
        {
            // Start getting if attachment exists
            var getIfAttachmentExists = _confluenceHttpClient.GetAsync($"content/{pageId}/child/attachment?filename={formFile.FileName}&expand=version").ConfigureAwait(false);
            // While previous request goes on, let's get the file.
            byte[] data;
            using (var br = new BinaryReader(formFile.OpenReadStream()))
            {
                data = br.ReadBytes((int)formFile.OpenReadStream().Length);
            }

            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multipartContent = new MultipartFormDataContent();
            multipartContent.Add(bytes, "file", formFile.FileName);

            var attachmentRequestData = await getIfAttachmentExists;
            if (attachmentRequestData.IsSuccessStatusCode && attachmentRequestData.StatusCode == HttpStatusCode.OK)
            {
                // Page exists and no errors...
                var attachmentRequestContent = await attachmentRequestData.Content.ReadAsStringAsync().ConfigureAwait(false);
                var attachmentData = JsonConvert.DeserializeObject<FileSearch>(attachmentRequestContent);

                HttpResponseMessage putAttachmentResponse;
                // Update existing data.
                if (attachmentData.Size == 1)
                {
                    multipartContent.Add(new StringContent($"Automatic update/upload from TestApplication ;)."), "comment");
                    putAttachmentResponse = await _confluenceHttpClient.PostAsync(
                        $"content/{pageId}/child/attachment/{attachmentData.Results.First().Id}/data",
                        multipartContent);

                    // Result is 1 "item"
                    var content = await putAttachmentResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<Models.Confluence.Result>(content);

                    return new FileTransferResult() { Results = new Models.Confluence.Result[] { result }, Size = 1 };
                }
                else
                {
                    // Create the attachment
                    multipartContent.Add(new StringContent($"Automatic upload from TestApplication ;)."), "comment");
                    putAttachmentResponse = await _confluenceHttpClient.PostAsync(
                        $"content/{pageId}/child/attachment",
                        multipartContent);

                    // Result is a list of item.
                    var content = await putAttachmentResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<FileTransferResult>(content);
                }
            }

            return default(FileTransferResult);
        }
    }
}
