using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace contacts
{
    public class MyOptions {
        public string ApiBaseUri { get; set; }
    }
    public class Program
    {
        public static async Task Main(string[] args)
        {
            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<contacts.App>("#app");
            builder.Services.AddSingleton(async i => {
                using var a = new HttpClient();
                var res = await a.GetStringAsync($"{builder.HostEnvironment.BaseAddress}/Options.json");
                return Newtonsoft.Json.JsonConvert.DeserializeObject<MyOptions>(res);
            });
            builder.Services.AddSingleton(async sp => {
                var myO = await sp.GetService<Task<MyOptions>>();
                var hc = new HttpClient { BaseAddress = new Uri(myO.ApiBaseUri) };
                return hc;
            });
            builder.Services.AddSingleton(async sp => { 
                var myO = await sp.GetService<Task<MyOptions>>();
                var myClient = await sp.GetService<Task<HttpClient>>();
                return new swaggerClient(myO.ApiBaseUri, myClient);
            });
            await builder.Build().RunAsync();
        }
    }
}
