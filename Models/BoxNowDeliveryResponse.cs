using System.Text.Json.Serialization;

namespace Nop.Plugin.Shipping.BoxNow.Models;
public class BoxNowDeliveryResponse {
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("parcels")]
    public List<Parcel> Parcels { get; set; }
}

public class Parcel {
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
