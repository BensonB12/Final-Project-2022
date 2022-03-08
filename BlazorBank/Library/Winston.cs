//Github Link: https://github.com/MagicTheGathering/mtg-sdk-dotnet
//Booster packs have 10 common, 3 uncommon, 1 rare/mythicRare, and 1 non-basic land if there is any in the set. (DOM had basics and WAR had one common land)

using System;
using System.IO;
using System.Net.Http;
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
        private int NumberOfCards = 15;
        private IAsyncEnumerable<List<CardModel>> booster { get; }

        public IAsyncEnumerable<List<CardModel>> GetBooster()
        {
            return booster;
        }

        public MakeBooster(Set set)
        {
            this.set = set;
            booster = (IAsyncEnumerable<List<CardModel>>)PickXCardsFromSet(set);
        }
        public MakeBooster(Set set, int numberOfCards) : this(set)
        {
            this.NumberOfCards = numberOfCards;
        }

        private async static IAsyncEnumerator<List<CardModel>> PickXCardsFromSet(Set enumSet, int numberOfCommons = 10, int numberOfUncommons = 3, int numberOfRares = 1, int numberOfLands = 1)
        {
            Random random = new Random();
            List<CardModel> boosterPack = null;
            for (int i = 0; i < numberOfLands; i++)
            {
                CardModel card = await CardProcessor.LoadLand(enumSet);
                boosterPack.Add(card);
            }
            for (int i = 0; i < numberOfCommons; i++)
            {
                boosterPack.Add(await CardProcessor.LoadNonLand(enumSet, Rarity.commmon));
            }
            for (int i = 0; i < numberOfUncommons; i++)
            {
                boosterPack.Add(await CardProcessor.LoadNonLand(enumSet, Rarity.uncommon));
            }

            int decider = random.Next(101);

            if (decider < 72)
            {
                for (int i = 0; i < numberOfRares; i++)
                {
                    boosterPack.Add(await CardProcessor.LoadNonLand(enumSet, Rarity.rare));
                }
            }
            else
            {
                for (int i = 0; i < numberOfRares; i++)
                {
                    boosterPack.Add(await CardProcessor.LoadNonLand(enumSet, Rarity.mythicRare));
                }
            }

            yield return boosterPack;
        }

        public static class ApiHelper
        {
            //Maybe not static, (only one browser)
            public static HttpClient ApiClient { get; set; }
            public static void InitializeClient()
            {
                ApiClient = new HttpClient();
                ApiClient.BaseAddress = new Uri("https://api.magicthegathering.io/v1/");
                ApiClient.DefaultRequestHeaders.Accept.Clear();
                //Give us Json
                ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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
                    CardModel[] cards = await response.Content.ReadAsAsync<CardModel[]>();
                    Random random = new Random();
                    cards = cards.OrderBy(x => random.Next()).ToArray();
                    return cards[0];
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        //Neo does not have common lands, WAR has one, DOM none
        public async static Task<CardModel> LoadNonLand(Set enumSet, Rarity enumRarity)
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

            string url = $"{BaseUrl.BU()}cards?rarity={stringRarity}&set={stringSet}";

            using (HttpResponseMessage response = await MakeBooster.ApiHelper.ApiClient.GetAsync(url))
            {

                if (response.IsSuccessStatusCode)
                {
                    CardResultModel cards = await response.Content.ReadAsAsync<CardResultModel>();
                    Random random = new Random();
                    CardModel[] specificData = cards.cards;
                    specificData = specificData.OrderBy(x => random.Next()).ToArray();
                    return CheckForOutliers(specificData, enumSet);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }


            static CardModel CheckForOutliers(CardModel[] cards, Set enumSet)
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

                foreach (CardModel card in cards)
                {
                    if (card.Number < maxNumber)
                    {
                        return card;
                    }
                }

                throw new Exception("In 'CheckForOutliers' there was no card that was less than the maxNumber.");
            }
        }
    }

    public class CardResultModel
    {
        public CardModel[] cards {get; set;}
    }

    public class CardModel
    {
        public string ImageUrl { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
    }

    public class MainWindow
    {
        public async Task LoadImage()
        {
            var card = await CardProcessor.LoadNonLand(Set.WAR, Rarity.uncommon);

            var uriSource = new Uri(card.ImageUrl, UriKind.Absolute);
            //cardImage.Source = new BitmapImage(uriSource);
        }
    }

}