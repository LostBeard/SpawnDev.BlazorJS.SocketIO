# SpawnDev.BlazorJS.SocketIO
Bidirectional and low-latency communication for every platform.

[![NuGet version](https://badge.fury.io/nu/SpawnDev.BlazorJS.SocketIO.svg?label=SpawnDev.BlazorJS.SocketIO)](https://www.nuget.org/packages/SpawnDev.BlazorJS.SocketIO)

**SpawnDev.BlazorJS.SocketIO** brings the amazing [socket.io](https://github.com/socketio/socket.io) library to Blazor WebAssembly.

**SpawnDev.BlazorJS.SocketIO** uses [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) for Javascript interop allowing strongly typed, full usage of the [socket.io](https://github.com/socket.io/socket.io) Javascript library. 

## Setup
**Create a new Blazor WebAssembly project**  
In the folder you created for your new project:
```dotnet
dotnet new blazorwasm
```

**Add the Nuget package**  
```nuget
dotnet add package SpawnDev.BlazorJS.SocketIO
```

**Add BlazorJSRuntime service**  
Add to `Program.cs`
```cs
builder.Services.AddBlazorJSRuntime();
```

**Add SocketIO Javascript Library**  
Add to `index.html`  
```html
<script src="_content/SpawnDev.BlazorJS.SocketIO/socket.io.min.js"></script>
```
(Alternatively `await Socket.Init()` can be used in C# to load the SocketIO library at runtime)  

**Create a Socket**
```cs
var socket = new Socket(socketIoServerUrl);
```

## Counter example from repo
In this example, when any user changes the counter value by clicking the button all users see the value change.

Program.cs
```cs
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
builder.Services.AddSingleton(new Socket("http://localhost:3000"));

await builder.Build().RunAsync();
```

Counter.razor
```cs
@page "/counter"
@inject Socket Socket
@implements IDisposable

<PageTitle>Shared Counter</PageTitle>

<h1>Shared Counter</h1>

<p role="status">Current count: @currentCount</p>
<p role="status">Changed by: @countChangedBy</p>

<button disabled="@Socket.Disconnected" class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;
    private string countChangedBy = "";

    private void IncrementCount()
    {
        Socket.Emit("incrementCount");
    }
    protected override void OnInitialized()
    {
        Socket.OnConnect += Socket_OnConnect;
        Socket.OnDisconnect += Socket_OnDisconnect;
        Socket.On<string, int>("countChanged", Socket_OnCountChanged);
        _ = TryUpdateCount();
    }
    void Socket_OnCountChanged(string changedBy, int newValue)
    {
        currentCount = newValue;
        countChangedBy = changedBy;
        StateHasChanged();
    }
    async Task TryUpdateCount()
    {
        if (!Socket.Connected) return;
        try
        {
            (countChangedBy, currentCount) = await Socket.EmitWithAck<(string, int)>("getCount");
            StateHasChanged();
        }
        catch { }
    }
    void Socket_OnConnect()
    {
        _ = TryUpdateCount();
    }
    void Socket_OnDisconnect()
    {
        StateHasChanged();
    }
    public void Dispose()
    {
        Socket.OnConnect -= Socket_OnConnect;
        Socket.OnDisconnect -= Socket_OnDisconnect;
        Socket.Off<string, int>("countChanged", Socket_OnCountChanged);
    }
}
```
