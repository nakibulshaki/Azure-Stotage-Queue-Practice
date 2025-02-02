using Azure.Storage.Queues;
using AzureStotageQueuePractice.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var storageConfig = builder.Configuration.GetSection("AzureStorage");
var connectionString = storageConfig["ConnectionString"] ?? throw new InvalidOperationException("Azure Storage connection string not found.");
var queueName = storageConfig["QueueName"] ?? "default-queue";

// Register Queue clients
builder.Services.AddSingleton(new QueueServiceClient(connectionString));
builder.Services.AddSingleton(sp => new QueueClient(connectionString, queueName));

builder.Services.AddScoped<AzureQueueService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
