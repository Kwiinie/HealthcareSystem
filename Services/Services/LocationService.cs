using BusinessObjects.LocationModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Services.Services
{
    public class LocationService : ILocationService
    {
        private readonly HttpClient _httpClient;

        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<District>> GetCities(string proCode)
        {
            var apiUrl = $"https://provinces.open-api.vn/api/p/{proCode}?depth=2";

            var response = await _httpClient.GetStringAsync(apiUrl);

            var province = JsonConvert.DeserializeObject<Province>(response);

            return province.Districts;
        }

        public async Task<List<Province>> GetProvinces()
        {
            var apiUrl = "https://provinces.open-api.vn/api/p/";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var provinces = JsonConvert.DeserializeObject<List<Province>>(response);

            return provinces;
        }

        public async Task<List<Ward>> GetWards(string cityCode)
        {
            var apiUrl = $"https://provinces.open-api.vn/api/d/{cityCode}?depth=2";

            var response = await _httpClient.GetStringAsync(apiUrl);
            var district = JsonConvert.DeserializeObject<District>(response);

            return district.Wards;
        }

    }
}
