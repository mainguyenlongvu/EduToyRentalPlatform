﻿using EduToyRentalPlatform.SignalR;
using System.Runtime.CompilerServices;
using ToyShop;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddRazorPages();
builder.Services.AddConfig(builder.Configuration);
DependencyInjection.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

// Thêm điều hướng đến Home/Index khi truy cập root URL

app.MapGet("/", context =>
{
    context.Response.Redirect("/Admin");
    return Task.CompletedTask;
});

app.MapHub<NotificationHub>("notification-hub");
app.MapHub<MessageHub>("message-hub");

app.Run();