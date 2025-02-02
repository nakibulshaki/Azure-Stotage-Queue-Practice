using AzureStotageQueuePractice.Models;
using AzureStotageQueuePractice.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStotageQueuePractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AzureQueueService _queueService;

        public HomeController(ILogger<HomeController> logger, AzureQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var messageCount = await _queueService.GetQueueMessageCountAsync();
            ViewBag.NoItemInQueue = messageCount;

            var queueMessages = await _queueService.PeekMessagesAsync(10);
            return View(queueMessages.ToList());
        }

        public async Task<IActionResult> AddTestStorageQueues()
        {
            _logger.LogInformation("Adding 10 test messages to the queue.");
            var tasks = Enumerable.Range(0, 10)
                                  .Select(i => _queueService.SendMessageAsync($"This is test message no {i}"));

            await Task.WhenAll(tasks);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromStorageQueues(int? noOfMessages)
        {
            _logger.LogInformation($"Removing {noOfMessages ?? 1} messages from the queue.");
            await _queueService.RemoveFromQueueAsync(noOfMessages ?? 1);
            return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SendObjectAsync()
        {
            _logger.LogInformation("Sending individual object messages to the queue.");
            await _queueService.SendObjectDataAsMessageAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendObjectListAsync()
        {
            _logger.LogInformation("Sending a list of object messages to the queue.");
            await _queueService.SendListObjectDataAsMessageAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
