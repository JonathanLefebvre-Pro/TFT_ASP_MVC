using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Forms;
using DE = WebApp.Domain.Entities;

namespace WebApp.Controllers
{
    public class TaskController : Controller
    {
        IHttpClientFactory _httpClientFactory;

        public TaskController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                Task<HttpResponseMessage> responseTask = client.GetAsync("api/Task/GetAsync");
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    IEnumerable<DE.Task> tasks =
                        await responseTask.Result.Content.ReadFromJsonAsync<IEnumerable<DE.Task>>(
                            options: new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                            }
                        ) ?? Enumerable.Empty<DE.Task>();
                    return View(tasks);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(TaskForm form)
        {
            if (!ModelState.IsValid)
                return View(form);

            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                Task<HttpResponseMessage> responseTask = client.PostAsJsonAsync(
                    "api/Task/Add",
                    form
                );
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }
    }
}
