using System.Globalization;

namespace Winston
{
    public static partial class CardProcessor
    {
        public async static Task<List<CardModel>> LoadNonLand(Set enumSet, Rarity enumRarity, int cardsWanted, string specialType = "empty", string specialSuperType = "empty" )
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
            else if (specialSuperType != "empty")
            {
                url = $"{General.BaseUrl()}cards?set={stringSet}&supertypes={specialSuperType}";
            }
            else if (enumSet == Set.WAR)
            {
                url = $"{General.BaseUrl()}cards?type=land&rarity={stringRarity}&set=war&type=creature|artifact|enchantment|sorcery|instant";
            }
            else if (enumSet == Set.NEO && stringRarity == "common")
            {
                url = $"{General.BaseUrl()}cards?rarity=common&set=neo&type=creature|enchantment";

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

                url = $"{General.BaseUrl()}cards?rarity=common&set=neo&type=sorcery|instant|artifact";
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
        }
    }
}