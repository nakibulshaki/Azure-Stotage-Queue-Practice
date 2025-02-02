using AzureStotageQueuePractice.Models;
using AzureStotageQueuePractice.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzureStotageQueuePractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AzureQueueService _queueService;
        public HomeController(ILogger<HomeController> logger,
                              AzureQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var queueMessages = await _queueService.PeekMessages(10);
            return View(queueMessages.ToList());
        }

        public async Task<IActionResult> AddTestStorageQueues()
        {
            for (int i = 0; i < 10; i++)
            {
                await _queueService.SendMessage($"This is test message no {i}");
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
