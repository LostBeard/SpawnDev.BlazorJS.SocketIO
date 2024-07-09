using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.SocketIO
{
    /// <summary>
    /// The Manager manages the Engine.IO client instance, which is the low-level engine that establishes the connection to the server (by using transports like WebSocket or HTTP long-polling).<br/>
    /// https://socket.io/docs/v4/client-api/#manager
    /// </summary>
    public class Manager : EventEmitter
    {
        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="_ref"></param>
        public Manager(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Create a new instance of Manager
        /// </summary>
        public Manager() : base(JS.New("io.Manager")) { }
        /// <summary>
        /// Create a new instance of Manager
        /// </summary>
        public Manager(string url) : base(JS.New("io.Manager", url)) { }
        /// <summary>
        /// Create a new instance of Manager
        /// </summary>
        public Manager(string url, IOOptions options) : base(JS.New("io.Manager", url, options)) { }
        /// <summary>
        /// Fired upon a connection error.
        /// </summary>
        public JSEventCallback<Error> OnError { get => new JSEventCallback<Error>("error", On, RemoveListener); set { } }
        /// <summary>
        /// Fired when a ping packet is received from the server.
        /// </summary>
        public JSEventCallback OnPing { get => new JSEventCallback("ping", On, RemoveListener); set { } }
        /// <summary>
        /// Fired upon a successful reconnection.<br/>
        /// attempt <number> reconnection attempt number
        /// </summary>
        public JSEventCallback<int> OnReconnect { get => new JSEventCallback<int>("reconnect", On, RemoveListener); set { } }
        /// <summary>
        /// Fired upon an attempt to reconnect.<br/>
        /// attempt <number> reconnection attempt number
        /// </summary>
        public JSEventCallback<int> OnReconnectAttempt { get => new JSEventCallback<int>("reconnect_attempt", On, RemoveListener); set { } }
        /// <summary>
        /// Fired upon a reconnection attempt error.<br/>
        /// error <Error> error object
        /// </summary>
        public JSEventCallback<Error> OnReconnectError { get => new JSEventCallback<Error>("reconnect_error", On, RemoveListener); set { } }
        /// <summary>
        /// Fired when couldn't reconnect within reconnectionAttempts.
        /// </summary>
        public JSEventCallback OnReconnectFailed { get => new JSEventCallback("reconnect_failed", On, RemoveListener); set { } }
        /// <summary>
        /// If the manager was initiated with autoConnect to false, launch a new connection attempt.
        /// </summary>
        public void Open() => JSRef!.CallVoid("open");
        /// <summary>
        /// Creates a new Socket for the given namespace. Only auth ({ auth: {key: "value"} }) is read from the options object. Other keys will be ignored and should be passed when instancing a new Manager(nsp, options).
        /// </summary>
        /// <param name="nsp"></param>
        /// <returns></returns>
        public Socket Socket(string nsp) => JSRef!.Call<Socket>("socket", nsp);
        /// <summary>
        /// Creates a new Socket for the given namespace. Only auth ({ auth: {key: "value"} }) is read from the options object. Other keys will be ignored and should be passed when instancing a new Manager(nsp, options).
        /// </summary>
        /// <param name="nsp"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Socket Socket(string nsp, IOOptions options) => JSRef!.Call<Socket>("socket", nsp, options);
    }
}
