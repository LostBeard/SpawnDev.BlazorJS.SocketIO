using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.SocketIO;
using SpawnDev.BlazorJS.SocketIO.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazorJSRuntime(out var JS);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load the SimplePeer Javascript library. Can be loaded using a <script> tag in the index.html instead
await Socket.Init();
// A Socket can be created when needed.
// Here we are creating it at startup and registering it as a service (not required.)
var socket = new Socket("http://localhost:3000");
JS.Set("socket", socket);
builder.Services.AddSingleton(socket);
socket.On<string>("welcome", welcomeMessage =>
{
    Console.WriteLine($"Welcome received: {welcomeMessage}");
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().BlazorJSRunAsync();
