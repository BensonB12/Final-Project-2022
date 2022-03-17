using System.Net.Http.Headers;

namespace Winston
{
    public static class General
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
//Github Link: https://github.com/MagicTheGathering/mtg-sdk-dotnet
//Booster packs have 10 common, 3 uncommon, 1 rare/mythicRare, and 1 non-basic land if there is any in the set. (DOM had basics and WAR had one common land)


//     public async Task LoadImage()
//     {
//         var card = await CardProcessor.LoadNonLand(Set.WAR, Rarity.uncommon);

//         var uriSource = new Uri(card.ImageUrl, UriKind.Absolute);
//         //cardImage.Source = new BitmapImage(uriSource);
//     }


// var writer = new StreamWriter("Testing.txt");
                        // foreach (var card in halfTheCommons)
                        // {
                        //     writer.WriteLine($"{card.Name}");
                        // }
                        // writer.Close();