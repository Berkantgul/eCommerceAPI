using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.DTOs.Facebook
{
    public class FacebookUserAccessTokenValidation_DTO
    {
        [JsonPropertyName("data")]
        public FacebookUserAccessTokenValidationData Data { get; set; }
    }
    public class FacebookUserAccessTokenValidationData
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

    }

}

//{ "data"{ "app_id":"329665416097041","type":"USER","application":"MiniETicaret","data_access_expires_at":1702633827,"expires_at":1694862000,"is_valid":true,"scope":["email","public_profile"],"user_id":"294370569900120"} }
