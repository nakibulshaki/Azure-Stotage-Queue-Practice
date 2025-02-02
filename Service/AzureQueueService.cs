using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStotageQueuePractice.Service
{
    public class AzureQueueService
    {
        public readonly QueueClient _queueClient;
        public AzureQueueService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }
        public async Task SendMessage(string msg)
        {
          await _queueClient.SendMessageAsync(msg);
        }
        public async Task<ICollection<PeekedMessage>> PeekMessages(int noOfMsgToRecive)
        {
            PeekedMessage[] pickMsgs = await _queueClient.PeekMessagesAsync(noOfMsgToRecive);
            return pickMsgs;
        }
    }
}
