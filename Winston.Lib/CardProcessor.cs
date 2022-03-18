using System.Globalization;

namespace Winston
{
    public static class CardProcessor
    {
        public async static Task<CardModel> LoadLand(Set enumSet)
        {
            string stringSet = SwitchEnumSetToString(enumSet);

            string url = $"{General.BaseUrl()}cards?rarity=common&set={stringSet}&type=land";

            using (HttpResponseMessage response = await General.ApiHelper.ApiClient.GetAsync(url))
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
        public async static Task<List<CardModel>> LoadNonLand(Set enumSet, Rarity enumRarity, int cardsWanted, string specialType = "empty")
        {
            string stringSet = SwitchEnumSetToString(enumSet);

            string stringRarity = enumRarity switch
            {
                Rarity.commmon => "common",
                Rarity.uncommon => "uncommon",
                Rarity.rare => "rare",
                Rarity.mythicRare => "mythic",
                _ => throw new Exception("The set in 'LoadCard' is not 'common', 'uncommon', 'rare', nor 'mythicRare'.")
            };

            var list = new List<CardModel>();
            string url;
            CardModel[] halfTheCommons = new CardModel[100];
            CardModel[] allTheCards = new CardModel[200];

            if (specialType != "empty")
            {
                url = $"{General.BaseUrl()}cards?set={stringSet}&type={specialType}";
            }
            else if (enumSet == Set.WAR)
            {
                url = $"{General.BaseUrl()}cards?type=land&rarity={stringRarity}&set=war&type=creature|artifact|enchantment|sorcery|instant";
            }
            else if (enumSet == Set.NEO && stringRarity == "common")
            {
                url = $"{General.BaseUrl()}cards?rarity=common&set=neo&type=creature|artifact|enchantment";

                using (HttpResponseMessage response = await General.ApiHelper.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        CardResultModel result = await response.Content.ReadAsAsync<CardResultModel>();
                        halfTheCommons = result.cards;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }

                url = $"{General.BaseUrl()}cards?rarity=common&set=neo|type=sorcery|instant";
            }
            else
            {
                url = $"{General.BaseUrl()}cards?rarity={stringRarity}&set={stringSet}";
            }

            using (HttpResponseMessage response = await General.ApiHelper.ApiClient.GetAsync(url))
            {

                if (response.IsSuccessStatusCode)
                {
                    CardResultModel result = await response.Content.ReadAsAsync<CardResultModel>();
                    Random random = new Random();
                    var specificData = result.cards;

                    if (enumSet == Set.NEO && stringRarity == "common")
                    {
                        int k = 0;
                        foreach (var card in halfTheCommons)
                        {
                            allTheCards[k] = card;
                            k++;
                        }

                        foreach (var card in specificData)
                        {
                            allTheCards[k] = card;
                            k++;
                        }
                    }
                    else
                    {
                        allTheCards = specificData;
                    }

                    allTheCards = allTheCards.Where(c => c != null).ToArray();
                    allTheCards = allTheCards.OrderBy(x => random.Next()).ToArray();

                    int cardsSeen = 0;
                    int WorthyCardCount = 0;

                    while (true)
                    {
                        var worthyCard = CheckForOutliers(allTheCards, enumSet, cardsSeen);

                        if (worthyCard != null)
                        {
                            list.Add(worthyCard);
                            WorthyCardCount++;
                        }

                        cardsSeen++;

                        if (WorthyCardCount >= cardsWanted)
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

            static CardModel CheckForOutliers(CardModel[] cards, Set enumSet, int placeInCardsArray)
            {
                if (enumSet == Set.NEO)
                {
                    foreach (var letter in cards[placeInCardsArray].Name)
                    {
                        if (letter == '/')
                        {
                            if (cards[placeInCardsArray].Types.Contains("Creature") == true)
                            {
                                return null;
                            }
                        }
                    }
                }

                int maxNumber = enumSet switch
                {
                    Set.NEO => 282,
                    var e when e == Set.WAR || e == Set.DOM => 250,
                    _ => throw new Exception("We got a diffrent set somehow in 'CheckForUnknowns'")
                };

                int number = 1000;

                string toParse = General.DeleteNonNumbersInString(cards[placeInCardsArray].Number);

                if (toParse == null)
                {
                    return null;
                }

                number = int.Parse(toParse, CultureInfo.InvariantCulture);

                if (number < maxNumber)
                {
                    return cards[placeInCardsArray];
                }
                else
                {
                    return null;
                }

                throw new Exception("In 'CheckForOutliers' there was no card that was less than the maxNumber.");
            }
        }
        public static string SwitchEnumSetToString(Set set)
        {
            string stringSet = set switch
            {
                Set.NEO => "neo",
                Set.WAR => "war",
                Set.DOM => "dom",
                _ => throw new Exception("The set in 'LoadNonLand' is not 'war', 'dom', nor 'neo'.")
            };

            return stringSet;
        }
    }
}