using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Nop.Plugin.Shipping.BoxNow.Helpers;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Plugin.Shipping.BoxNow.Services.Interfaces;

namespace Nop.Plugin.Shipping.BoxNow.Services.Implementations;
public class BoxNowService : IBoxNowService {

    private readonly BoxNowSettings _boxNowSettings;

    public BoxNowService(BoxNowSettings boxNowSettings) {
        _boxNowSettings = boxNowSettings;
    }

    public async Task<BoxNowDeliveryResponse> DeliveryRequest(BoxNowDeliveryRequest request) {
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri(GetApiBaseUrl());
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetTokenAsync()}");
            client.DefaultRequestHeaders.Add("X-PartnerID", $"{_boxNowSettings.PartnerID}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = JsonSerializer.Serialize(request, JsonSerializerOptionDefaults.GetDefaultSettings());
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var httpResponseMessage = await client.PostAsync(BoxNowDefaults.DeliveryRequestPath, content);
            if (httpResponseMessage.IsSuccessStatusCode) {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<BoxNowDeliveryResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                return response;
            } else {
                try {
                    var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<BoxNowErrorResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                    throw new Exception($"Box Now Delivery Request Error: {response.Code} - {BoxNowErrorCodes.Errors.GetValueOrDefault(response.Code)}");
                } catch (Exception ex) {
                    throw new Exception($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
                }
            }
        }
    }

    public async Task<byte[]> ParcelRequest(BoxNowParcelRequest request) {
        using (var client = new HttpClient()) {
            client.BaseAddress = new Uri(GetApiBaseUrl());
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await GetTokenAsync()}");
            client.DefaultRequestHeaders.Add("X-PartnerID", $"{_boxNowSettings.PartnerID}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var httpResponseMessage = await client.GetAsync($"{BoxNowDefaults.ParcelRequestPath}/{request.ParcelId}/label.{request.Type}");
            if (httpResponseMessage.IsSuccessStatusCode) {
                var httpResponseContent = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                return httpResponseContent;
            } else {
                try {
                    var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<BoxNowErrorResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                    throw new Exception($"Box Now Delivery Request Error: {response.Code} - {BoxNowErrorCodes.Errors.GetValueOrDefault(response.Code)}");
                } catch (Exception ex) {
                    throw new Exception($"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
                }
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