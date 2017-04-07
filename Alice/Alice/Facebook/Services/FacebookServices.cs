using System.Net.Http;
using System.Threading.Tasks;
using Alice.Facebook.Models;
using Newtonsoft.Json;

namespace Alice.Facebook.Services
{
    public class FacebookServices
    {

        public async Task<FacebookProfile> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

            return facebookProfile;
        }
    }
}
