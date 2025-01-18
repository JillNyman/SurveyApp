using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using surveyapi.Data;
using surveyapi.Services;
using surveyapi.Models;


var builder = WebApplication.CreateBuilder(args);

// L�gger till DbContext f�r databas
builder.Services.AddDbContext<SurveyContext>(options => options.UseSqlite("Data Source=surveys.db"));

// L�gger till m�jlighet att testa endpoints med Swagger
builder.Services.AddEndpointsApiExplorer();

//L�gger till tj�nster/services 
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();

//L�gger till Controllers och Json
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; //Matchar f�ltnamnen exakt
    });

//L�gger till Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "surveyapi",
        Version = "v1",
        Description = "API som hanterar enk�ter och fr�gor."
    });
});

//L�gger till CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//L�gger till Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

//Anv�nd migrations vid startup
using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SurveyContext>();
    db.Database.Migrate();                //k�r migrationer till databasen
}

// Konfigurerar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Konfigurerar Swagger f�r korrekt server-URL
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        swaggerDoc.Servers = new List<OpenApiServer>
        {
            new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}" }
        };
    });
});

//Aktiverar HTTPS och CORS
app.UseHttpsRedirection();
app.UseCors("AllowAll");

//Routing och RazorPages
app.UseRouting();
app.MapRazorPages();

app.UseAuthorization();

//Kartl�gger controllers, f�r endpoints i API
app.MapControllers();

//Starta applikationen
app.Run();

 

