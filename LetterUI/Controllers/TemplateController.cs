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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LetterUI.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly IConfiguration _Configure;
        private readonly IHttpClientFactory _clientFactory;

        public TemplateController(ILogger<TemplateController> logger, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _Configure = configuration;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            TemplateListViewModel model = new TemplateListViewModel()
            {
                PageTitle = "Templates"
            };

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var request = "letters";

            var myclient = _clientFactory.CreateClient("LetterAPI");

            using (var Response = await myclient.GetAsync(request))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    model.Templates = JsonSerializer.Deserialize<List<Letter>>(apiResponse, serializerOptions);
                    return View(model);
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    return View(model);
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

        [HttpGet]
        public IActionResult Upload()
        {
            TemplateUploadViewModel model = new TemplateUploadViewModel()
            {
                PageTitle = "Templates"
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(TemplateUploadViewModel model)
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
