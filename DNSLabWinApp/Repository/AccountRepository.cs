﻿using DNSLabWinApp.DTOs.User;
using DNSLabWinApp.Services;
using System;
using System.Threading.Tasks;

namespace DNSLabWinApp.Repository
{
    public class AccountRepository
    {
        private readonly HttpService _httpService;
        public AccountRepository()
        {
            _httpService = new HttpService(new System.Net.Http.HttpClient());
        }

        public async Task<string> Login(AuthenticateDTO userInfo)
        {
            var response = await _httpService.Post<AuthenticateDTO, AuthUserDTO>($"/Auth/authenticate", userInfo);
            if (!response.Success)
            {
                return String.Empty;
            }
            else
            {
                return response.Response.Token;
            }
        }

        public async Task<UserInfo> Get()
        {
            var response = await _httpService.Get<UserInfo>($"/Auth/get");
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
