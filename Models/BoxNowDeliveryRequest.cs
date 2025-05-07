using System.Text.Json.Serialization;

namespace Nop.Plugin.Shipping.BoxNow.Models;

public class BoxNowDeliveryRequest {
    [JsonPropertyName("orderNumber")]
    public string OrderNumber { get; set; }

    [JsonPropertyName("paymentMode")]
    public string PaymentMode { get; set; }

    [JsonPropertyName("invoiceValue")]
    public string InvoiceValue { get; set; }

    [JsonPropertyName("amountToBeCollected")]
    public string AmountToBeCollected { get; set; }

    [JsonPropertyName("origin")]
    public LocationModel Origin { get; set; }

    [JsonPropertyName("destination")]
    public LocationModel Destination { get; set; }

    [JsonPropertyName("items")]
    public List<ItemModel> Items { get; set; }

    [JsonPropertyName("overwriteSenderShippingLabelInfo")]
    public ShippingLabelInfo OverwriteSenderShippingLabelInfo { get; set; }

    [JsonPropertyName("typeOfService")]
    public string TypeOfService { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("allowReturn")]
    public bool AllowReturn { get; set; }

    [JsonPropertyName("showRecipientInformation")]
    public bool ShowRecipientInformation { get; set; }

    [JsonPropertyName("notifyOnAccepted")]
    public string NotifyOnAccepted { get; set; }

    [JsonPropertyName("notifySMSOnAccepted")]
    public string NotifySMSOnAccepted { get; set; }

    [JsonPropertyName("additionalInformation")]
    public string AdditionalInformation { get; set; }

    [JsonPropertyName("returnLocation")]
    public LocationModel ReturnLocation { get; set; }
}

public class LocationModel {
    [JsonPropertyName("locationId")]
    public string LocationId { get; set; }

    [JsonPropertyName("contactNumber")]
    public string ContactNumber { get; set; }

    [JsonPropertyName("contactEmail")]
    public string ContactEmail { get; set; }

    [JsonPropertyName("contactName")]
    public string ContactName { get; set; }

    [JsonPropertyName("deliveryPartnerId")]
    public string DeliveryPartnerId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("addressLine1")]
    public string AddressLine1 { get; set; }

    [JsonPropertyName("addressLine2")]
    public string AddressLine2 { get; set; }

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; }
}

public class ItemModel {
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("weight")]
    public decimal Weight { get; set; }

    [JsonPropertyName("compartmentSize")]
    public int CompartmentSize { get; set; }

    [JsonPropertyName("originDeliveryParcelId")]
    public string OriginDeliveryParcelId { get; set; }

    [JsonPropertyName("destinationDeliveryParcelId")]
    public string DestinationDeliveryParcelId { get; set; }
}

public class ShippingLabelInfo {
    [JsonPropertyName("row1")]
    public string Row1 { get; set; }

    [JsonPropertyName("row2")]
    public string Row2 { get; set; }

    [JsonPropertyName("row3")]
    public string Row3 { get; set; }

    [JsonPropertyName("row4")]
    public string Row4 { get; set; }
}
