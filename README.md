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

