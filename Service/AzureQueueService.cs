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
        public async Task ReceiveMessage()
        {
           var result  = await _queueClient.ReceiveMessageAsync();
        }
        public async Task ReceiveMessages(int noOfMsgToRecive)
        {
            await _queueClient.ReceiveMessagesAsync(noOfMsgToRecive);
        }
        public async Task RemoveFromQueue()
        {
            var item = await _queueClient.ReceiveMessageAsync();
             await _queueClient.DeleteMessageAsync(item.Value.MessageId,item.Value.PopReceipt);
        }
        public async Task RemoveFromQueue(int noOfMsgToDel)
        {
            var queues = await _queueClient.ReceiveMessagesAsync(maxMessages: noOfMsgToDel);
            foreach (var item in queues.Value)
            {
                await _queueClient.DeleteMessageAsync(item.MessageId, item.PopReceipt);
            }
        }
    }
}
