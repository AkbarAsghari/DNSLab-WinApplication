using DNSLabWinApp.DTOs.IP;
using DNSLabWinApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSLabWinApp.Repository
{
    public class IPRepository
    {
        private readonly HttpService _httpService;
        public IPRepository()
        {
            this._httpService = new HttpService(new System.Net.Http.HttpClient());
        }

        public async Task<IPDTO> GetIP()
        {
            var response = await _httpService.Get<IPDTO>($"/IP");
            if (!response.Success)
            {
                return null;
            }
            else
            {
                return response.Response;
            }
        }
    }
}
