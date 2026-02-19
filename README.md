# SpawnDev.BlazorJS.SocketIO

[![NuGet version](https://badge.fury.io/nu/SpawnDev.BlazorJS.SocketIO.svg?label=SpawnDev.BlazorJS.SocketIO)](https://www.nuget.org/packages/SpawnDev.BlazorJS.SocketIO)

**SpawnDev.BlazorJS.SocketIO** brings the powerful [Socket.IO](https://socket.io/) JavaScript library to Blazor WebAssembly, enabling bidirectional and low-latency communication for every platform.

This library provides strongly-typed C# wrappers around the Socket.IO client API, leveraging [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) for seamless JavaScript interop.

## Features

- **Full Socket.IO Client API Coverage** - Complete strongly-typed access to Socket.IO client functionality
- **Multiple .NET Versions** - Targets .NET 8, .NET 9, and .NET 10
- **Event-Driven Architecture** - Built on `EventEmitter` with easy event subscription using C# delegates
- **Type-Safe Events** - Strongly-typed event handlers with support for generic type parameters
- **Promise-Based Acknowledgements** - Async/await support with `EmitWithAck` methods
- **Connection State Management** - Properties for tracking connection state, socket ID, and recovery status
- **Manager Access** - Direct access to the underlying Engine.IO Manager for advanced scenarios
- **Flexible Configuration** - Comprehensive `IOOptions` for customizing Socket.IO behavior

## Installation

Add the NuGet package to your Blazor WebAssembly project:

```bash
dotnet add package SpawnDev.BlazorJS.SocketIO
```

## Quick Start

### 1. Configure Services

Add the BlazorJSRuntime service in your `Program.cs`:

```cs
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.SocketIO;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register BlazorJSRuntime service
builder.Services.AddBlazorJSRuntime();

// Load Socket.IO library and create a socket instance
await Socket.Init();
var socket = new Socket("http://localhost:3000");
builder.Services.AddSingleton(socket);

await builder.Build().BlazorJSRunAsync();
```

### 2. Load Socket.IO Library

You can load the Socket.IO library in two ways:

**Option A: Script tag in `index.html`**
```html
<script src="_content/SpawnDev.BlazorJS.SocketIO/socket.io.min.js"></script>
```

**Option B: Runtime loading in C#**
```cs
await Socket.Init(); // Uses bundled socket.io library
// Or specify a custom CDN URL:
await Socket.Init("https://cdn.socket.io/4.8.1/socket.io.min.js");
```

### 3. Create and Use a Socket

```cs
using SpawnDev.BlazorJS.SocketIO;

// Create a socket connection
var socket = new Socket("http://localhost:3000");

// Or with options
var socket = new Socket("http://localhost:3000", new IOOptions 
{
    Reconnection = true,
    ReconnectionDelay = 1000
});

// Listen for connection
socket.OnConnect += () => 
{
    Console.WriteLine($"Connected! Socket ID: {socket.Id}");
};

// Listen for custom events
socket.On<string>("message", (data) => 
{
    Console.WriteLine($"Received: {data}");
});

// Emit events
socket.Emit("greeting", "Hello from Blazor!");

// Emit with acknowledgement
var response = await socket.EmitWithAck<string>("getData");
```

## Core Components

### Socket
The fundamental class for interacting with the server. Provides methods for emitting and listening to events.

**Key Properties:**
- `Connected` - Whether the socket is currently connected
- `Disconnected` - Whether the socket is disconnected
- `Id` - Unique identifier for the socket session
- `Active` - Whether the socket will automatically try to reconnect
- `Recovered` - Whether the connection state was recovered during reconnection
- `IO` - Reference to the underlying Manager instance

**Key Methods:**
- `Emit(eventName, args)` - Send an event to the server
- `EmitWithAck<T>(eventName, args)` - Send an event and await acknowledgement
- `On<T>(eventName, callback)` - Listen for events from the server
- `Once<T>(eventName, callback)` - Listen for an event once
- `Off(eventName, callback)` - Remove an event listener
- `Connect()` - Manually connect the socket
- `Disconnect()` - Manually disconnect the socket

### Manager
Manages the Engine.IO client instance which establishes the connection to the server using transports like WebSocket or HTTP long-polling.

**Key Events:**
- `OnError` - Connection errors
- `OnPing` - Ping packets from server
- `OnReconnect` - Successful reconnection
- `OnReconnectAttempt` - Reconnection attempts

### EventEmitter
Base class providing the event system infrastructure, based on Node.js EventEmitter pattern.

### IOOptions
Configuration class for customizing Socket.IO behavior including:
- Connection options (forceNew, multiplex, reconnection settings)
- Transport options (WebSocket, polling)
- Authentication options
- Custom headers and query parameters
- Timeout and delay configuration

## Complete Example: Shared Counter

This example demonstrates a real-time shared counter where all connected clients see the same value update instantly. The demo is included in the repository.

### Blazor Component (`Counter.razor`)

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

### Application Setup (`Program.cs`)

```cs
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.SocketIO;
using SpawnDev.BlazorJS.SocketIO.Demo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register BlazorJSRuntime service
builder.Services.AddBlazorJSRuntime();

// Load Socket.IO library
await Socket.Init();

// Create socket and register as singleton service
var socket = new Socket("http://localhost:3000");
builder.Services.AddSingleton(socket);

// Example: Listen for welcome message
socket.On<string>("welcome", welcomeMessage =>
{
    Console.WriteLine($"Welcome received: {welcomeMessage}");
});

await builder.Build().BlazorJSRunAsync();
```

### Socket.IO Server (Node.js) (`app.js`)

```js
const { createServer } = require("http");
const { Server } = require("socket.io");

const httpServer = createServer();
const io = new Server(httpServer, {
    cors: { origin: '*' }
});

let currentCount = 0;
let countChangedBy = "";

io.on("connection", (socket) => {
    console.log("Client connected:", socket.id);

    // Send welcome message
    socket.emit("welcome", `Welcome! Your ID: ${socket.id}`);

    // Handle disconnect
    socket.on("disconnect", () => {
        console.log("Client disconnected:", socket.id);
    });

    // Handle count increment
    socket.on("incrementCount", () => {
        currentCount++;
        countChangedBy = socket.id;
        io.emit("countChanged", countChangedBy, currentCount);
    });

    // Handle count request
    socket.on("getCount", (callback) => {
        callback([countChangedBy, currentCount]);
    });
});

httpServer.listen(3000, () => {
    console.log("Server listening on port 3000");
});
```

## Event Handling Patterns

### Basic Event Subscription

```cs
// Simple event with no parameters
socket.OnConnect += () => Console.WriteLine("Connected!");

// Event with single parameter
socket.On<string>("message", (msg) => Console.WriteLine(msg));

// Event with multiple parameters
socket.On<string, int>("update", (name, count) => 
{
    Console.WriteLine($"{name}: {count}");
});
```

### One-Time Events

```cs
socket.Once<string>("serverReady", (msg) => 
{
    Console.WriteLine($"Server is ready: {msg}");
});
```

### Removing Event Listeners

```cs
void MyHandler(string data) 
{
    Console.WriteLine(data);
}

socket.On<string>("message", MyHandler);
// Later...
socket.Off<string>("message", MyHandler);
```

## Emitting Events

### Fire and Forget

```cs
// Simple emit
socket.Emit("ping");

// Emit with data
socket.Emit("message", "Hello World");

// Emit with multiple parameters
socket.Emit("update", "username", 42, true);
```

### With Acknowledgements

```cs
// Wait for acknowledgement (void)
await socket.EmitWithAck("save", data);

// Wait for acknowledgement with return value
var result = await socket.EmitWithAck<string>("processData", data);

// With timeout
var result = await socket.EmitWithAck<string>(5000, "getData");
```

## Advanced Configuration

### IOOptions Example

```cs
var socket = new Socket("http://localhost:3000", new IOOptions
{
    // Connection behavior
    Reconnection = true,
    ReconnectionAttempts = 5,
    ReconnectionDelay = 1000,
    ReconnectionDelayMax = 5000,

    // Transport options
    Transports = new[] { "websocket", "polling" },

    // Custom headers
    ExtraHeaders = new Dictionary<string, string>
    {
        ["Authorization"] = "Bearer token123"
    },

    // Query parameters
    Query = new Dictionary<string, string>
    {
        ["userId"] = "user123"
    },

    // Other options
    ForceNew = false,
    Multiplex = true,
    Timeout = 20000
});
```

### Accessing the Manager

```cs
var manager = socket.IO;

manager.OnReconnect += (attempt) => 
{
    Console.WriteLine($"Reconnected after {attempt} attempts");
};

manager.OnReconnectAttempt += (attempt) => 
{
    Console.WriteLine($"Attempting to reconnect... (attempt {attempt})");
};

manager.OnError += (error) => 
{
    Console.WriteLine($"Connection error: {error.Message}");
};
```

## Project Structure

```
SpawnDev.BlazorJS.SocketIO/
├── Socket.cs              # Main Socket class
├── Manager.cs             # Engine.IO Manager
├── EventEmitter.cs        # Event system base class
├── IOOptions.cs           # Configuration options
├── AuthenticationOptions.cs
└── wwwroot/
    └── socket.io.min.js   # Socket.IO JavaScript library

SpawnDev.BlazorJS.SocketIO.Demo/
└── Demo Blazor WebAssembly application

socketio-demo-server/
└── Node.js Socket.IO server example
```

## Requirements

- .NET 8, .NET 9, or .NET 10
- Blazor WebAssembly project
- [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) v3.0.3 or later

## Browser Compatibility

This library works in any browser that supports Blazor WebAssembly and Socket.IO transports (WebSocket and/or HTTP long-polling).

## Links

- **NuGet Package**: [SpawnDev.BlazorJS.SocketIO](https://www.nuget.org/packages/SpawnDev.BlazorJS.SocketIO)
- **GitHub Repository**: [https://github.com/LostBeard/SpawnDev.BlazorJS.SocketIO](https://github.com/LostBeard/SpawnDev.BlazorJS.SocketIO)
- **SpawnDev.BlazorJS**: [https://github.com/LostBeard/SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS)
- **Socket.IO**: [https://socket.io](https://socket.io)
- **Socket.IO Client API Docs**: [https://socket.io/docs/v4/client-api/](https://socket.io/docs/v4/client-api/)

## License

See [LICENSE.txt](LICENSE.txt) for details.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

---

Built with ❤️ using [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS)
