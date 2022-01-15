using Mediscreen.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPatientNotesService>(
    new AzureTableStoragePatientNotesService(builder.Configuration["Data:AzureTableStorage:ConnectionString"]));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");

        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();