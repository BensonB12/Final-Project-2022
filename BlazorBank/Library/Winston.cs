//Github Link: https://github.com/MagicTheGathering/mtg-sdk-dotnet
//Booster packs have 10 common, 3 uncommon, 1 rare/mythicRare, and 1 non-basic land if there is any in the set. (DOM had basics and WAR had one common land)

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MtgApiManager.Lib.Core;
using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;

namespace Winston
{
    public static class BaseUrl
    {
        public static string BU()
        {
            return "https://api.magicthegathering.io/v1/";
        }
    }

    public enum Set
    {
        //War = 250 is the first basic land. NEO 282, DOM 250
        WAR,
        NEO,
        DOM
    }

    public enum Rarity
    {
        commmon,
        uncommon,
        rare,
        mythicRare
    }

    public enum cardType
    {
        planeswalker,
        legendary,
        land
    }
    public class MakeBooster
    {
        private Set set;
        public Task<List<CardModel>> booster { get; }
        public Task<CardModel> singleCard { get; }

        public MakeBooster(Set set)
        {
            this.set = set;
            //this.singleCard = CardProcessor.LoadNonLand(set, Rarity.commmon);
            booster = PickXCardsFromSet(set);
        }

        private async static Task<List<CardModel>> PickXCardsFromSet(Set enumSet, int numberOfCommons = 10, int numberOfUncommons = 3, int numberOfRares = 1, int numberOfLands = 1)
        {
            Random random = new Random();
            List<CardModel> boosterPack = new List<CardModel>();
            for (int i = 0; i < numberOfLands; i++)
            {
                CardModel card = await CardProcessor.LoadLand(enumSet);
                boosterPack.Add(card);
            }

            foreach (var item in await CardProcessor.LoadNonLand(enumSet, Rarity.commmon, numberOfCommons))
            {
                boosterPack.Add(item);
            }

            foreach (var item in await CardProcessor.LoadNonLand(enumSet, Rarity.uncommon, numberOfUncommons))
            {
                boosterPack.Add(item);
            }

            int decider = random.Next(101);

            if (decider < 72)
            {
                foreach (var item in await CardProcessor.LoadNonLand(enumSet, Rarity.rare, numberOfRares))
                {
                    boosterPack.Add(item);
                }
            }
            else
            {
                foreach (var item in await CardProcessor.LoadNonLand(enumSet, Rarity.mythicRare, numberOfRares))
                {
                    boosterPack.Add(item);
                }
            }

            return boosterPack;
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
    }

    // public class NormalBooster : MakeBooster
    // {
    //     public NormalBooster(Set set) : base(set)
    //     {
    //     }
    // }

    // public class WARBooster : MakeBooster
    // {
    //     public WARBooster() : base(Set.WAR)
    //     {
    //     }
    // }

    // public class DOMBooster : MakeBooster
    // {
    //     public DOMBooster() : base(Set.DOM)
    //     {
    //     }
    // }

    public static class CardProcessor
    {
        public async static Task<CardModel> LoadLand(Set enumSet)
        {
            string stringSet;
            switch (enumSet)
            {
                case Set.WAR:
                    stringSet = "war";
                    break;
                case Set.DOM:
                    stringSet = "dom";
                    break;
                case Set.NEO:
                    stringSet = "neo";
                    break;
                default:
                    throw new Exception("The set in 'LoadNonLand' is not 'war', 'dom', nor 'neo'.");
            }

            string url = $"{BaseUrl.BU()}cards?rarity=common&set={stringSet}&type=land";

            using (HttpResponseMessage response = await MakeBooster.ApiHelper.ApiClient.GetAsync(url))
            {

                if (response.IsSuccessStatusCode)
                {
                    CardResultModel result = await response.Content.ReadAsAsync<CardResultModel>();
                    Random random = new Random();
                    CardModel[] specificData = result.cards;
                    specificData = specificData.OrderBy(x => random.Next()).ToArray();
                    return specificData[0];
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //Neo does not have common lands, WAR has one, DOM none
        public async static Task<List<CardModel>> LoadNonLand(Set enumSet, Rarity enumRarity, int cardsWanted)
        {
            string stringSet;

            switch (enumSet)
            {
                case Set.WAR:
                    stringSet = "war";
                    break;
                case Set.DOM:
                    stringSet = "dom";
                    break;
                case Set.NEO:
                    stringSet = "neo";
                    break;
                default:
                    throw new Exception("The set in 'LoadNonLand' is not 'war', 'dom', nor 'neo'.");
            }

            string stringRarity;
            switch (enumRarity)
            {
                case Rarity.commmon:
                    stringRarity = "common";
                    break;
                case Rarity.uncommon:
                    stringRarity = "uncommon";
                    break;
                case Rarity.rare:
                    stringRarity = "rare";
                    break;
                case Rarity.mythicRare:
                    stringRarity = "mythic";
                    break;
                default:
                    throw new Exception("The set in 'LoadCard' is not 'common', 'uncommon', 'rare', nor 'mythicRare'.");
            }

            var list = new List<CardModel>();

            string url = $"{BaseUrl.BU()}cards?rarity={stringRarity}&set={stringSet}";

            using (HttpResponseMessage response = await MakeBooster.ApiHelper.ApiClient.GetAsync(url))
            {

                if (response.IsSuccessStatusCode)
                {
                    CardResultModel result = await response.Content.ReadAsAsync<CardResultModel>();
                    Random random = new Random();
                    var writer = new StreamWriter("results.txt");
                    writer.WriteLine("we have made it to here");
                    writer.WriteLine(result);
                    writer.Close();
                    CardModel[] specificData = result.cards;
                    specificData = specificData.OrderBy(x => random.Next()).ToArray();
                    int i = 0;
                    while(true)
                    {
                        var worthyCard = CheckForOutliers(specificData, enumSet, i);
                        if (worthyCard != null)
                        {
                            list.Add(worthyCard);
                            i++;
                        }
                        if(i >= cardsWanted)
                        {
                            break;
                        }
                    }

                    return list;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }


            static CardModel CheckForOutliers(CardModel[] cards, Set enumSet, int i)
            {
                int maxNumber;
                switch (enumSet)
                {
                    case Set.NEO:
                        maxNumber = 282;
                        break;
                    case Set.DOM:
                    case Set.WAR:
                        maxNumber = 250;
                        break;
                    default:
                        throw new Exception("We got a diffrent set somehow in 'CheckForLands'");
                }

                int number = int.Parse(cards[i].Number.Trim(), CultureInfo.InvariantCulture);

                if (number < maxNumber)
                {
                    return cards[i];
                }
                else
                {
                    return null;
                }

                throw new Exception("In 'CheckForOutliers' there was no card that was less than the maxNumber.");
            }
        }
    }

    public class CardResultModel
    {
        public CardModel[] cards { get; set; }
    }

    public class CardModel
    {
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Number { get; set; }
    }

    // public class MainWindow
    // {
    //     public async Task LoadImage()
    //     {
    //         var card = await CardProcessor.LoadNonLand(Set.WAR, Rarity.uncommon);

    //         var uriSource = new Uri(card.ImageUrl, UriKind.Absolute);
    //         //cardImage.Source = new BitmapImage(uriSource);
    //     }
    // }

}