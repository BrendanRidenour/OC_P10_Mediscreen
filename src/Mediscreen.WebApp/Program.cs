using Mediscreen;
using Mediscreen.Mocks;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISystemClock, SystemClock>();
builder.Services.AddTransient<IPatientService, InMemoryPatientService>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/ServerError");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithReExecute("/Errors/StatusCodeError", queryFormat: "?code={0}");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();