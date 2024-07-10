using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.SocketIO;
using SpawnDev.BlazorJS.SocketIO.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// init SpawnDev.BlazorJS.BlazorJSRuntime service
builder.Services.AddBlazorJSRuntime();

// Load the Socket.IO Javascript library. Can be loaded using a <script> tag in the index.html instead
await Socket.Init();
// A Socket can be created when needed.
// Here we are creating a socket.io Socket at startup and registering it as a service
// below tells the socket the uri of our socketio-demo-server
var socket = new Socket("http://localhost:3000");
builder.Services.AddSingleton(socket);
// example of listening for a message on a Socket
socket.On<string>("welcome", welcomeMessage =>
{
    Console.WriteLine($"Welcome received: {welcomeMessage}");
});

await builder.Build().RunAsync();
