using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LetterUI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using LetterAPI.Core.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using LetterUI.Converter;

namespace LetterUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Configure;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _Configure = configuration;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //PropertyNameCaseInsensitive = true
                //Converters = { new StringConverter() }
            };


            var request = "letters/100";

            var myclient = _clientFactory.CreateClient("LetterAPI");

            using (var Response = await myclient.GetAsync(request))
            {
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await Response.Content.ReadAsStringAsync();
                    Letter letter = JsonSerializer.Deserialize<Letter>(apiResponse, serializerOptions);
                    ViewData["LetterName"] = letter.LetterName;
                    ViewData["Letter"] = letter.LetterContent;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
