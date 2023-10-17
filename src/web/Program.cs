using System.Reflection;
using CloudNotes.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

const string defaultBaseUrl = "http://localhost:5275";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazorBootstrap();

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var serviceUrl = builder.Configuration["serviceUrl"];

builder.Services.AddHttpClient("default", (serviceProvider, httpClient) =>
{
    httpClient.BaseAddress = new Uri(serviceUrl ?? defaultBaseUrl);
});



await builder.Build().RunAsync();
