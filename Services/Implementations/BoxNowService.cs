using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nop.Core;
using Nop.Plugin.Shipping.BoxNow.Helpers;
using Nop.Plugin.Shipping.BoxNow.Models;
using Nop.Plugin.Shipping.BoxNow.Services.Interfaces;

namespace Nop.Plugin.Shipping.BoxNow.Services.Implementations;
public class BoxNowService : IBoxNowService {

    private readonly BoxNowSettings _boxNowSettings;
    private readonly ILogger<BoxNowService> _logger;

    public BoxNowService(BoxNowSettings boxNowSettings, ILogger<BoxNowService> logger) {
        _boxNowSettings = boxNowSettings ?? throw new ArgumentNullException(nameof(boxNowSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<BoxNowDeliveryResponse> DeliveryRequest(BoxNowDeliveryRequest request) {
        try {
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
                    var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<BoxNowErrorResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                    throw new Exception($"Box Now Delivery Request Order: {request.OrderNumber}, Code: {response.Code} - {BoxNowErrorCodes.Errors.GetValueOrDefault(response.Code)}");
                }
            }
        } catch (Exception ex) {
            _logger.LogError($"Error: {ex.Message}");
            throw new Exception($"Error: {ex.Message}");
        }
    }

    public async Task<byte[]> ParcelRequest(BoxNowParcelRequest request) {
        try {
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
                    var httpResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<BoxNowErrorResponse>(httpResponseContent, JsonSerializerOptionDefaults.GetDefaultSettings());
                    throw new Exception($"Box Now Parcel Request Order: {request.ParcelId}, Code: {response.Code} - {BoxNowErrorCodes.Errors.GetValueOrDefault(response.Code)}");
                }
            }
        } catch (Exception ex) {
            _logger.LogError($"Box Now Parcel Request Error: {ex.Message}");
            throw new Exception($"Error: {ex.Message}");
        }
    }

    public async Task<string> GetTokenAsync() {
        //TODO set and get access token from cache
        try {
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
                    throw new Exception($"Box Now Token Request : {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}");
                }
            }
        } catch (Exception ex) {
            _logger.LogError($"Error: {ex.Message}");
            throw new Exception($"Error: {ex.Message}");
        }
    }

    private string GetApiBaseUrl() {
        return _boxNowSettings.IsStaging ? BoxNowDefaults.ApiUrl.Staging : BoxNowDefaults.ApiUrl.Production;
    }
}