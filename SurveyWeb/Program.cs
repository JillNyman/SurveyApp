using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
    

var builder = WebApplication.CreateBuilder(args);



    //L�gger till httpclient f�r att peka p� API i surveyapi
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
    app.UseDeveloperExceptionPage(); //Felhantering i utvecklingsl�ge
}
else
{
    app.UseExceptionHandler("/Error");   //Felhantering i produktionsl�ge
    app.UseHsts(); //Anv�nd HTTP Strict Transport Security
}

app.UseHttpsRedirection(); //Vidarebef HTTP till HTTPS
app.UseStaticFiles(); //Anv�nd statiska filer (js, css)

app.UseRouting();

app.UseAuthorization();

//app.UseSession();

app.MapRazorPages();


app.Run();
