using Nop.Plugin.Shipping.BoxNow.Models;

namespace Nop.Plugin.Shipping.BoxNow.Services.Interfaces;
public interface IBoxNowService {
    Task<BoxNowDeliveryResponse> DeliveryRequest(BoxNowDeliveryRequest request);
    Task<byte[]> ParcelRequest(BoxNowParcelRequest request);
}
