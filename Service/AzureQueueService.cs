using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureStotageQueuePractice.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureStotageQueuePractice.Service
{
    public class AzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<ICollection<PeekedMessage>> PeekMessagesAsync(int count)
        {
            var peekedMessages = await _queueClient.PeekMessagesAsync(count);
            return peekedMessages.Value;
        }

        public async Task<QueueMessage> ReceiveMessageAsync()
        {
            var message = await _queueClient.ReceiveMessageAsync();
            return message.Value;
        }

        public async Task<IEnumerable<QueueMessage>> ReceiveMessagesAsync(int count)
        {
            var messages = await _queueClient.ReceiveMessagesAsync(count);
            return messages.Value;
        }

        public async Task RemoveFromQueueAsync()
        {
            var message = await ReceiveMessageAsync();
            if (message != null)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }

        public async Task RemoveFromQueueAsync(int count)
        {
            var messages = await ReceiveMessagesAsync(count);
            foreach (var message in messages)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }

        public async Task<int> GetQueueMessageCountAsync()
        {
            var properties = await _queueClient.GetPropertiesAsync();
            return properties.Value.ApproximateMessagesCount;
        }

        public async Task SendObjectDataAsMessageAsync()
        {
            var cars = GetSampleCars();
            foreach (var car in cars)
            {
                await SendMessageAsync(JsonSerializer.Serialize(car));
            }
        }

        public async Task SendListObjectDataAsMessageAsync()
        {
            var cars = GetSampleCars();
            await SendMessageAsync(JsonSerializer.Serialize(cars));
        }

        private static List<Car> GetSampleCars() => new()
        {
            new Car { Id = 1, Name = "Toyota Corolla", BuildYear = 2020 },
            new Car { Id = 2, Name = "Honda Civic", BuildYear = 2019 },
            new Car { Id = 3, Name = "Ford Mustang", BuildYear = 2021 },
            new Car { Id = 4, Name = "Chevrolet Camaro", BuildYear = 2018 },
            new Car { Id = 5, Name = "BMW 3 Series", BuildYear = 2022 },
            new Car { Id = 6, Name = "Audi A4", BuildYear = 2017 },
            new Car { Id = 7, Name = "Mercedes-Benz C-Class", BuildYear = 2023 },
            new Car { Id = 8, Name = "Tesla Model 3", BuildYear = 2021 },
            new Car { Id = 9, Name = "Nissan Altima", BuildYear = 2020 },
            new Car { Id = 10, Name = "Hyundai Elantra", BuildYear = 2019 }
        };
    }
}
