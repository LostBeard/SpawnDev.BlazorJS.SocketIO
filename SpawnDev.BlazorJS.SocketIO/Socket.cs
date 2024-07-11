using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.SocketIO
{
    /// <summary>
    /// A Socket is the fundamental class for interacting with the server. A Socket belongs to a certain Namespace (by default /) and uses an underlying Manager to communicate.<br/>
    /// https://socket.io/docs/v4/client-api/#socket
    /// </summary>
    public class Socket : EventEmitter
    {
        // CDN 
        // https://cdn.socket.io/
        static Task? _Init = null;
        /// <summary>
        /// Returns a Task that completes after loading the socket.io library
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Task Init(string? src = null) => _Init ??= BlazorJSRuntime.JS.LoadScript(!string.IsNullOrEmpty(src) ? src : "_content/SpawnDev.BlazorJS.SocketIO/socket.io.min.js", "io");
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Socket(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Create a new instance of Socket using io() call
        /// </summary>
        public Socket() : base(JS.Call<IJSInProcessObjectReference>("io")) { }
        /// <summary>
        /// Create a new instance of Socket using io() call
        /// </summary>
        public Socket(string url) : base(JS.Call<IJSInProcessObjectReference>("io", url)) { }
        /// <summary>
        /// Create a new instance of Socket using io() call
        /// </summary>
        public Socket(string url, IOOptions options) : base(JS.Call<IJSInProcessObjectReference>("io", url, options)) { }
        /// <summary>
        /// Whether the socket will automatically try to reconnect.
        /// </summary>
        public bool Active => JSRef!.Get<bool>("active");
        /// <summary>
        /// Whether the socket is currently connected to the server.
        /// </summary>
        public bool Connected => JSRef!.Get<bool>("connected");
        /// <summary>
        /// Whether the socket is currently disconnected from the server.
        /// </summary>
        public bool Disconnected => JSRef!.Get<bool>("disconnected");
        /// <summary>
        /// A unique identifier for the socket session. Set after the connect event is triggered, and updated after the reconnect event.
        /// </summary>
        public string Id => JSRef!.Get<string>("id");
        /// <summary>
        /// Socket namespace
        /// </summary>
        public string Nsp => JSRef!.Get<string>("nsp");
        /// <summary>
        /// A reference to the underlying Manager.
        /// </summary>
        public Manager IO => JSRef!.Get<Manager>("io");
        /// <summary>
        /// Whether the connection state was successfully recovered during the last reconnection.
        /// </summary>
        public bool Recovered => JSRef!.Get<bool>("recovered");
        /// <summary>
        /// This event is fired by the Socket instance upon connection and reconnection.
        /// </summary>
        public JSEventCallback OnConnect { get => new JSEventCallback("connect", On, RemoveListener); set { } }
        /// <summary>
        /// This event is fired upon connection failure.
        /// </summary>
        public JSEventCallback<Error> OnError { get => new JSEventCallback<Error>("error", On, RemoveListener); set { } }
        /// <summary>
        /// This event is fired upon disconnection.
        /// </summary>
        public JSEventCallback OnDisconnect { get => new JSEventCallback("disconnect", On, RemoveListener); set { } }
        /// <summary>
        /// Manually disconnects the socket. In that case, the socket will not try to reconnect.
        /// </summary>
        public void Disconnect() => JSRef!.CallVoid("disconnect");
        /// <summary>
        /// Manually connects the socket.
        /// </summary>
        public void Connect() => JSRef!.CallVoid("connect");
        /// <summary>
        /// Sets a modifier for a subsequent event emission that the callback will be called with an error when the given number of milliseconds have elapsed without an acknowledgement from the server:
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>this instance to allow call chaining</returns>
        public Socket Timeout(int timeout)
        {
            JSRef!.CallVoid("timeout", timeout);
            return this;
        }
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task EmitWithAck(int timeout, string eventName, params object?[]? args)
        {
            Timeout(timeout);
            return args == null || args.Length == 0 ? JSRef!.CallVoidAsync("emitWithAck", eventName) : JSRef!.CallApplyVoidAsync("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        }
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task<T> EmitWithAck<T>(int timeout, string eventName, params object?[]? args)
        {
            Timeout(timeout);
            return args == null || args.Length == 0 ? JSRef!.CallAsync<T>("emitWithAck", eventName) : JSRef!.CallApplyAsync<T>("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        }
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task EmitWithAckApply(int timeout, string eventName, object?[]? args)
        {
            Timeout(timeout);
            return args == null || args.Length == 0 ? JSRef!.CallVoidAsync("emitWithAck", eventName) : JSRef!.CallApplyVoidAsync("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        }
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task<T> EmitWithAckApply<T>(int timeout, string eventName, object?[]? args)
        {
            Timeout(timeout);
            return args == null || args.Length == 0 ? JSRef!.CallAsync<T>("emitWithAck", eventName) : JSRef!.CallApplyAsync<T>("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        }
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task EmitWithAck(string eventName, params object?[]? args)
        => args == null || args.Length == 0 ? JSRef!.CallVoidAsync("emitWithAck", eventName) :
            JSRef!.CallApplyVoidAsync("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task<T> EmitWithAck<T>(string eventName, params object?[]? args)
        => args == null || args.Length == 0 ? JSRef!.CallAsync<T>("emitWithAck", eventName) :
            JSRef!.CallApplyAsync<T>("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task EmitWithAckApply(string eventName, object?[]? args)
        => args == null || args.Length == 0 ? JSRef!.CallVoidAsync("emitWithAck", eventName) :
            JSRef!.CallApplyVoidAsync("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        /// <summary>
        /// Promised-based version of emitting and expecting an acknowledgement from the server:
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task<T> EmitWithAckApply<T>(string eventName, object?[]? args)
        => args == null || args.Length == 0 ? JSRef!.CallAsync<T>("emitWithAck", eventName) :
            JSRef!.CallApplyAsync<T>("emitWithAck", new object[] { eventName }.Concat(args).ToArray());
        /// <summary>
        /// Emits an event to the socket identified by the string name. Any other parameters can be included. All serializable data structures are supported, including Buffer.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void EmitApply(string eventName, object?[]? args)
        {
            if (args == null || args.Length == 0) JSRef!.CallVoid("emit", eventName);
            else JSRef!.CallApplyVoid("emit", new object[] { eventName }.Concat(args).ToArray());
        }
        /// <summary>
        /// Emits an event to the socket identified by the string name. Any other parameters can be included. All serializable data structures are supported, including Buffer.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void Emit(string eventName, params object?[]? args)
        {
            if (args == null || args.Length == 0) JSRef!.CallVoid("emit", eventName);
            else JSRef!.CallApplyVoid("emit", new object[] { eventName }.Concat(args).ToArray());
        }
    }
}
