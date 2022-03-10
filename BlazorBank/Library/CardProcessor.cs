using System.Globalization;

namespace Winston
{
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

            string url = $"{General.BaseUrl()}cards?rarity={stringRarity}&set={stringSet}";

            using (HttpResponseMessage response = await General.ApiHelper.ApiClient.GetAsync(url))
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
                    int j = 0;
                    while (true)
                    {
                        var worthyCard = CheckForOutliers(specificData, enumSet, i);
                        if (worthyCard != null)
                        {
                            list.Add(worthyCard);
                            j++;
                        }

                        i++;

                        if (j >= cardsWanted)
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
                int number = 1000;

                string toParse = General.DeleteNonNumbersInString(cards[i].Number);

                if (toParse == null)
                {
                    return null;
                }

                number = int.Parse(toParse, CultureInfo.InvariantCulture);

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
}