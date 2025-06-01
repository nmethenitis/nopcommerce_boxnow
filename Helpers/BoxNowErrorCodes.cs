namespace Nop.Plugin.Shipping.BoxNow.Helpers;
public static class BoxNowErrorCodes {
    public static readonly Dictionary<string, string> Errors = new(){
        { "P400", "Invalid request data. Make sure you are sending the request according to the documentation." },
        { "P401", "Invalid request origin location reference. Make sure you are referencing a valid location ID from Origins endpoint or valid address." },
        { "P402", "Invalid request destination location reference. Make sure you are referencing a valid location ID from Destinations endpoint or valid address." },
        { "P403", "You are not allowed to use AnyAPM-SameAPM delivery. Contact support if you believe this is a mistake." },
        { "P404", "Invalid import CSV. See error contents for additional info." },
        { "P405", "Invalid phone number. Make sure you are sending the phone number in full international format, e.g. +30 xx x xxx xxxx." },
        { "P406", "Invalid compartment/parcel size. Make sure you are sending one of required sizes 1, 2 or 3 (Small, Medium or Large)." },
        { "P407", "Invalid country code. Make sure you are sending country code in ISO 3166-1 alpha-2 format, e.g. GR." },
        { "P408", "Invalid amountToBeCollected amount. Make sure you are sending amount in the valid range of (0, 5000)." },
        { "P409", "Invalid delivery partner reference. Make sure you are referencing a valid delivery partner ID from Delivery partners endpoint." },
        { "P410", "Order number conflict. You are trying to create a delivery request for order ID that has already been created." },
        { "P411", "You are not eligible to use Cash-on-delivery payment type. Use another payment type or contact our support." },
        { "P412", "You are not allowed to create customer returns deliveries. Contact support if you believe this is a mistake." },
        { "P413", "Invalid return location reference. Make sure you are referencing a valid location warehouse ID from Origins endpoint or valid address." },
        { "P414", "Unauthorized parcel access. You are trying to access information to parcel/s that don't belong to you." },
        { "P415", "You are not allowed to create delivery to home address. Contact support if you believe this is a mistake." },
        { "P416", "You are not allowed to use COD payment for delivery to home address. Contact support if you believe this is a mistake." },
        { "P417", "You are not allowed to use q parameter. It is forbidden for server partner accounts." },
        { "P420", "Parcel not ready for cancel. You can cancel only new, undelivered, or parcels that are not returned or lost." },
        { "P421", "Invalid parcel weight. Make sure you are sending value between 0 and 10^6." },
        { "P422", "Address not found. Try to call just with postal code and country." },
        { "P423", "Nearby locker not found." },
        { "P424", "Invalid region format. Please ensure the format includes a language code followed by a country code in ISO 3166-1 alpha-2 format, e.g. el-GR." },
        { "P430", "Parcel not ready for AnyAPM confirmation. Parcel is probably already confirmed or being delivered." },
        { "P440", "Ambiguous partner. Your account is linked to multiple partners. Send X-PartnerID header with the ID of the partner you want to manage." },
        { "P441", "Invalid X-PartnerID header. The value is either invalid or references a partner you don't have access to." },
        { "P442", "Invalid limit query parameter. The query limit for this API has been exceeded. Max allowed is 100." }
    };
}

