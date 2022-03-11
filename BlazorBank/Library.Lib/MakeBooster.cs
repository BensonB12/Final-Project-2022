namespace Winston
{
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

    }
}