using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LetterAPI.Core.Entities;
using LetterUI.Models;
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

        public async Task<IActionResult> Download()
        {
            var request = "letters";

            var myclient = _clientFactory.CreateClient("LetterAPI");

            using (var Response = await myclient.GetAsync(request))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    var Templates = JsonSerializer.Deserialize<List<Letter>>(apiResponse);
                    var templatebytes = JsonSerializer.SerializeToUtf8Bytes<List<Letter>>(Templates);
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
    }
}
