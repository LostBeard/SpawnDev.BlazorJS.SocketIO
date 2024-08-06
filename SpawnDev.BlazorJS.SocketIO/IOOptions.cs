using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.SocketIO
{
    /// <summary>
    /// IOOptions
    /// </summary>
    public class IOOptions
    {
        #region IO factory options
        // https://socket.io/docs/v4/client-options/#io-factory-options
        /// <summary>
        /// Whether to create a new Manager instance.<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ForceNew { get; set; }
        /// <summary>
        /// The opposite of forceNew: whether to reuse an existing Manager instance.<br/>
        /// Default value: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Multiplex { get; set; }
        /// <summary>
        /// The trailing slash which was added by default can now be disabled:
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AddTrailingSlash { get; set; }
        /// <summary>
        /// Whether to (silently) close the connection when the beforeunload event is emitted in the browser.<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CloseOnBeforeunload { get; set; }
        /// <summary>
        /// Additional headers (then found in socket.handshake.headers object on the server-side).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string,string>? ExtraHeaders { get; set; }
        /// <summary>
        /// With autoUnref set to true, the Socket.IO client will allow the program to exit if there is no other active timer/TCP socket in the event system (even if the client is connected):<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AutoUnref { get; set; }
        /// <summary>
        /// Whether to force base64 encoding for binary content sent over WebSocket (always enabled for HTTP long-polling).<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ForceBase64 { get; set; }
        /// <summary>
        /// It is the name of the path that is captured on the server side.<br/>
        /// Default value: /socket.io/
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Path { get; set; }
        /// <summary>
        /// Either a single protocol string or an array of protocol strings. These strings are used to indicate sub-protocols, so that a single server can implement multiple WebSocket sub-protocols (for example, you might want one server to be able to handle different types of interactions depending on the specified protocol).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Protocols { get; set; }
        /// <summary>
        /// Additional query parameters (then found in socket.handshake.query object on the server-side).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object>? Query { get; set; }
        /// <summary>
        /// If true and if the previous WebSocket connection to the server succeeded, the connection attempt will bypass the normal upgrade process and will initially try WebSocket. A connection attempt following a transport error will use the normal upgrade process. It is recommended you turn this on only when using SSL/TLS connections, or if you know that your network does not block websockets.<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RememberUpgrade { get; set; }
        /// <summary>
        /// The name of the query parameter to use as our timestamp key.<br/>
        /// Default value: "t"
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TimestampParam { get; set; }
        /// <summary>
        /// Whether to add the timestamp query param to each request (for cache busting).<br/>
        /// Default value: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TimestampRequests { get; set; }
        /// <summary>
        /// Transport-specific options.<br/>
        /// Default value: {}
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, Dictionary<string, object>>? TransportOptions { get; set; }
        /// <summary>
        /// The low-level connection to the Socket.IO server can either be established with:<br/>
        /// HTTP long-polling: successive HTTP requests (POST for writing, GET for reading)<br/>
        /// WebSocket<br/>
        /// WebTransport<br/>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Transports { get; set; }
        /// <summary>
        /// Whether the client should try to upgrade the transport from HTTP long-polling to something better.<br/>
        /// Default value: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Upgrade { get; set; }
        /// <summary>
        /// Whether the cross-site requests should be sent including credentials such as cookies, authorization headers or TLS client certificates. Setting withCredentials has no effect on same-site requests.<br/>
        /// Default value: false
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WithCredentials { get; set; }
        #endregion
        #region Manager options
        // https://socket.io/docs/v4/client-options/#manager-options
        /// <summary>
        /// Whether to automatically connect upon creation. If set to false, you need to manually connect:<br/>
        /// Default value: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AutoConnect { get; set; }
        /// <summary>
        /// Whether reconnection is enabled or not. If set to false, you need to manually reconnect:<br/>
        /// Default value: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Reconnection { get; set; }
        /// <summary>
        /// The timeout in milliseconds for each connection attempt.<br/>
        /// Default value: 20000
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Timeout { get; set; }
        #endregion
        #region Socket options
        // https://socket.io/docs/v4/client-options/#socket-options
        /// <summary>
        /// Credentials that are sent when accessing a namespace (see also here).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AuthenticationOptions? Auth { get; set; }
        /// <summary>
        /// The maximum number of retries. Above the limit, the packet will be discarded.<br/>
        /// socket emit retry count
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Retries { get; set; }
        #endregion
    }
}
