using Mediscreen.Data;
using Microsoft.Extensions.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISystemClock, SystemClock>();
builder.Services.AddHttpClient<IPatientService, DemographicsPatientService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalServices:DemographicsWebApi:BaseAddress"]);
});
builder.Services.AddHttpClient<IPatientNoteService, HistoryPatientNoteService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalServices:HistoryWebApi:BaseAddress"]);
});
builder.Services.AddTransient<ITriggerTermCounter, TriggerTermsCounter>();
builder.Services.AddTransient<IDiabetesRiskAnalyzer, DiabetesRiskAnalyzer>();
builder.Services.AddTransient<IPatientDiabetesAssessmentService, PatientDiabetesAssessmentService>();

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