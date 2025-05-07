using System.Text.Json.Serialization;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowIdentityRequest {

    [JsonPropertyName("grant_type")]
    public string GrantType { get; set; }
    [JsonPropertyName("client_id")]
    public string ClientID { get; set; }
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }
}
