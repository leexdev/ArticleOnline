using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ArticleOnline.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Provincesz>> GetProvincesAsync()
        {
            var response = await _httpClient.GetAsync("https://provinces.open-api.vn/api/p");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var provinces = JsonConvert.DeserializeObject<List<ProvinceModel>>(content);
                return provinces;
            }

            return null;
        }

        public async Task<List<DistrictModel>> GetDistrictsByProvinceAsync(string provinceCode)
        {
            var url = $"https://provinces.open-api.vn/api/p/{provinceCode}?depth=2";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var districts = JsonConvert.DeserializeObject<List<DistrictModel>>(content);
                return districts;
            }

            return null;
        }
    }

}