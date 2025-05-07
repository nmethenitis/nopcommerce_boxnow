using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Nop.Core.Caching;
using Nop.Plugin.Shipping.BoxNow.Helpers;
using Nop.Plugin.Shipping.BoxNow.Models;

namespace Nop.Plugin.Shipping.BoxNow.Services;
public class BoxNowService {

    private readonly BoxNowSettings _boxNowSettings;
    private readonly MemoryCacheManager _cache;

    public BoxNowService(BoxNowSettings boxNowSettings, MemoryCacheManager cache) {
        _boxNowSettings = boxNowSettings;
        _cache = cache;
    }

    public async Task<BoxNowDeliveryResponse> DeliveryRequest(BoxNowDeliveryRequest request) {
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri(GetApiBaseUrl());
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetTokenAsync()}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(BoxNowDefaults.DeliveryRequestPath, content);
            if (httpResponseMessage.IsSuccessStatusCode) {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<BoxNowDeliveryResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                return response;
            } else {
                throw new Exception($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
            }
        }
    }

    public async Task<string> GetTokenAsync() {
        //TODO set and get access token from cache
        using (var client = new HttpClient()) {
            var authRequest = new BoxNowIdentityRequest() {
                GrantType = "client_credentials",
                ClientID = _boxNowSettings.ClientID,
                ClientSecret = _boxNowSettings.ClientSecret
            };
            client.BaseAddress = new Uri(GetApiBaseUrl());
            var payload = JsonSerializer.Serialize(authRequest);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(BoxNowDefaults.AuthPath, content);
            if (httpResponseMessage.IsSuccessStatusCode) {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<BoxNowIdentityResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                return response.AccessToken;
            } else {
                throw new Exception($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
            }
        }
    }

    private string GetApiBaseUrl() {
        return _boxNowSettings.IsStaging ? BoxNowDefaults.ApiUrl.Staging : BoxNowDefaults.ApiUrl.Production;
    }
}