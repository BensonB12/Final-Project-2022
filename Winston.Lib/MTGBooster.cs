namespace Winston
{
    public abstract class MTGBooster : IBooster
    {
        protected abstract Task<List<CardModel>> _Booster { set; get; }
        private Set set;
        public Task<List<CardModel>> booster
        {
            get
            {
                return _Booster;
            }
        }

        public MTGBooster(Set set)
        {
            this.set = set;
            _Booster = PickXCardsFromSet(set);
        }

        protected async static Task<List<CardModel>> PickXCardsFromSet(Set enumSet, int numberOfCommons = 10, int numberOfUncommons = 3, int numberOfRares = 1, int numberOfLands = 1)
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

            if (numberOfRares > 0)
            {
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
            }
            
            return boosterPack;
        }

        protected (int, int) FindOutRarity(List<CardModel> card)
        {
            int numberOfUncommons = 3;
            int numberOfRares = 1;

            switch (card[0].Rarity)
            {
                case "Uncommon":
                    numberOfUncommons--;
                    break;
                case "Rare":
                case "Mythic":
                    numberOfRares--;
                    break;
                default:
                    throw new Exception("SwitchStatement fell through to default in 'HelpSecialCases'");
            }

            return (numberOfUncommons, numberOfRares);
        }
    }
}