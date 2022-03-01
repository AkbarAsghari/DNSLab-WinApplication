
using dnslabwin.DTOs.DNS;
using dnslabwin.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dnslabwin.Repository
{
    public class DNSRepository 
    {
        private readonly HttpService _httpService;
        public DNSRepository()
        {
            this._httpService = new HttpService(new System.Net.Http.HttpClient());
        }

        public async Task<IEnumerable<HostSummaryDTO>> GetOwnHostsSummary()
        {
            var response = await _httpService.Get<IEnumerable<HostSummaryDTO>>($"/DNS/GetOwnHostsSummary");
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
