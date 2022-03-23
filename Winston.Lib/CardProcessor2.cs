using System.Globalization;

namespace Winston
{
    public static partial class CardProcessor
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
}