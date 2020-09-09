using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LetterAPI.Core.Entities;
using LetterUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LetterUI.Controllers
{
    public class LettersController : Controller
    {
        private readonly ILogger<LettersController> _logger;
        private readonly IConfiguration _Configure;
        private readonly IHttpClientFactory _clientFactory;

        public LettersController(ILogger<LettersController> logger, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _Configure = configuration;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            LetterListViewModel model = new LetterListViewModel()
            {
                PageTitle = "Templates"
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var myclient = _clientFactory.CreateClient("LetterAPI");

            model.Sections = await GetSections(serializerOptions, myclient);
            model.Letters = await GetLetters(serializerOptions, myclient);

            model.SelectLetters = new SelectList(model.Letters, "LetterName", "LetterName");

            return View(model);
        }

        private static async Task<List<Section>> GetSections(JsonSerializerOptions serializerOptions, HttpClient myclient)
        {
            using (var Response = await myclient.GetAsync("sections"))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Section>>(apiResponse, serializerOptions);
                }
                else
                {
                    return new List<Section>();
                }
            }
        }

        private static async Task<List<Letter>> GetLetters(JsonSerializerOptions serializerOptions, HttpClient myclient)
        {
            using (var Response = await myclient.GetAsync("letters"))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Letter>>(apiResponse, serializerOptions);
                }
                else
                {
                    return new List<Letter>();
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Download()
        {
            var request = "letters";

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var myclient = _clientFactory.CreateClient("LetterAPI");

            using (var Response = await myclient.GetAsync(request))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    var Templates = JsonSerializer.Deserialize<List<Letter>>(apiResponse, serializerOptions);
                    var templatebytes = JsonSerializer.SerializeToUtf8Bytes<List<Letter>>(Templates, serializerOptions);
                    return File(templatebytes, "application/json", "mytemplates.json");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Download(LetterListViewModel model)
        {
            _logger.LogInformation("Post to Download action");
            _logger.LogInformation(model.PageTitle);

            var request = "letters";

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var myclient = _clientFactory.CreateClient("LetterAPI");

            using (var Response = await myclient.GetAsync(request))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    var Templates = JsonSerializer.Deserialize<List<Letter>>(apiResponse, serializerOptions);
                    var templatebytes = JsonSerializer.SerializeToUtf8Bytes<List<Letter>>(Templates, serializerOptions);
                    return File(templatebytes, "application/json", "mytemplates.json");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult Upload()
        {
            LetterUploadViewModel model = new LetterUploadViewModel()
            {
                PageTitle = "Templates"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(LetterUploadViewModel model)
        {
            try
            {
                if (model.JsonFile.Length > 0)
                {
                    var serializerOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var jsonString = new StringBuilder();
                    using (var reader = new StreamReader(model.JsonFile.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            jsonString.AppendLine(reader.ReadLine());
                    }

                   /* string jsonString;
                    //byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        model.JsonFile.CopyTo(ms);
                        byte[] fileBytes = ms.ToArray();
                        jsonString = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }*/

                    var content = new StringContent(jsonString.ToString(), System.Text.Encoding.UTF8, "application/json");
                    //deserialize
                    //var Templates = JsonSerializer.Deserialize<List<Letter>>(fileBytes, serializerOptions);
                    //post vs put

                    var request = "letters/bundle";

                    var myclient = _clientFactory.CreateClient("LetterAPI");

                    using (var Response = await myclient.PutAsync(request, content))
                    {
                        if (Response.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            return View();
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                            return View();
                        }
                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        /*public byte[] ReadBytesAsync(this IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
                //string s = Convert.ToBase64String(fileBytes);
                // act on the Base64 data
            }

            return fileBytes;
        }*/
    }
}
