using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Forms;
using DE = WebApp.Domain.Entities;

namespace WebApp.Controllers
{
    public class TaskController : Controller
    {
        IHttpClientFactory _httpClientFactory;
        ILogger<TaskController> _logger;

        public TaskController(IHttpClientFactory httpClientFactory, ILogger<TaskController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                HttpResponseMessage response = await client.GetAsync("api/Task/GetAsync");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<DE.Task> tasks =
                        await response.Content.ReadFromJsonAsync<IEnumerable<DE.Task>>(
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
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Task/Add", form);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            //_logger.Log(LogLevel.Debug, "Delete id : " + id);

            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                //Check if we can get the item from the DB
                HttpResponseMessage response = await client.GetAsync($"api/Task/GetAsync/{id}");
                if (response.IsSuccessStatusCode)
                {
                    /*DE.Task? task = await response.Content.ReadFromJsonAsync<DE.Task>(
                        options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                    Console.WriteLine(task?.Title);*/
                    response = await client.DeleteAsync($"api/Task/Delete/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id)
        {
            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                //Check if we can get the item from the DB
                HttpResponseMessage response = await client.GetAsync($"api/Task/GetAsync/{id}");
                if (response.IsSuccessStatusCode)
                {
                    DE.Task? task = await response.Content.ReadFromJsonAsync<DE.Task>(
                        options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    //Convert Task to TaskFormEdit
                    TaskFormEdit form = task?.ToTaskForm()!;

                    return View(form);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> ValidateEdit(TaskFormEdit form)
        {
            if (!ModelState.IsValid)
                return View(form);

            using (HttpClient client = _httpClientFactory.CreateClient("api"))
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(
                    $"api/Task/Update/{form.Id}",
                    form.ToTask(),
                    options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
                else
                    return View("Edit", form);
            }
        }
    }
}
