﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.DTOs.Facebook
{
    public class FacebookAccessTokenResponse_DTO
    {
        [JsonPropertyName("access_token")]
        public string AccessTken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } 
    }
}
