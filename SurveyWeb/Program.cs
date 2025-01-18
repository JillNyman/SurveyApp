using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
    

var builder = WebApplication.CreateBuilder(args);



    //Lägger till httpclient för att peka på API i surveyapi
    builder.Services.AddHttpClient("surveyapi", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5093/api/");
    });
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddSession();





//builder.Services.AddTransient<SurveyApiService>();

//builder.Services.AddDistributedMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); //Felhantering i utvecklingsläge
}
else
{
    app.UseExceptionHandler("/Error");   //Felhantering i produktionsläge
    app.UseHsts(); //Använd HTTP Strict Transport Security
}

app.UseHttpsRedirection(); //Vidarebef HTTP till HTTPS
app.UseStaticFiles(); //Använd statiska filer (js, css)

app.UseRouting();

app.UseAuthorization();

//app.UseSession();

app.MapRazorPages();


app.Run();
