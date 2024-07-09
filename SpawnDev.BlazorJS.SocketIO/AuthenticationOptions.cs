using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.SocketIO
{
    public class AuthenticationOptions
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Token { get; set; }
    }
}
