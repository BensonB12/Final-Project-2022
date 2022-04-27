using System.Net.Http.Headers;

namespace Winston
{
    public static partial class General
    {
        public static string DeleteNonNumbersInString(string number)
        {
            var input = number.ToCharArray();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] < '0' || input[i] > '9')
                {
                    return null;
                }
            }

            return number;
        }
        public static class ApiHelper
        {
            public static HttpClient ApiClient { get; set; }
            public static void InitializeClient()
            {
                ApiClient = new HttpClient();
                //ApiClient.BaseAddress = new Uri("https://api.magicthegathering.io/v1/");
                ApiClient.DefaultRequestHeaders.Accept.Clear();
                //Give us Json
                ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public static string BaseUrl()
        {
            return "https://api.magicthegathering.io/v1/";
        }
    }
}