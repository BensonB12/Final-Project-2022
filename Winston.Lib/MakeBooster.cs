namespace Winston
{
    public abstract class MTGBooster
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

    }

    public class NormalBooster : MTGBooster
    {
        protected override Task<List<CardModel>> _Booster { set; get; }
        public NormalBooster(Set set) : base(set)
        {
            _Booster = PickXCardsFromSet(set);
        }
    }

    public class WARBooster : MTGBooster
    {
        //I cannot figure out how to add the planeswalker to the _Booster
        protected override Task<List<CardModel>> _Booster { set; get; }
        private List<CardModel> _Planeswalker { set; get; }
        public List<CardModel> planeswalker
        {
            get
            {
                return _Planeswalker;
            }
        }
        public WARBooster() : base(Set.WAR)
        {
            _Booster = GetPlaneswalker();
        }

        private async Task<List<CardModel>> GetPlaneswalker()
        {
            //Rarity does not do anything when you pass a special Type
            var planeswalker = await CardProcessor.LoadNonLand(Set.WAR, Rarity.rare, 1, "planeswalker");

            int numberOfUncommons = 3;
            int numberOfRares = 1;

            switch (planeswalker[0].Rarity)
            {
                case "Uncommon":
                    numberOfUncommons--;
                    break;
                case "Rare":
                case "Mythic":
                    numberOfRares--;
                    break;
                default:
                    throw new Exception("SwitchStatement fell through to default in 'GetPlaneswalker'");
            }

            _Planeswalker = planeswalker;

            return await PickXCardsFromSet(Set.WAR, 10, numberOfUncommons, numberOfRares);
        }
    }

    public class DOMBooster : MTGBooster
    {
        protected override Task<List<CardModel>> _Booster { set; get; }
        public DOMBooster() : base(Set.DOM)
        {
            _Booster = PickXCardsFromSet(Set.DOM);
        }
    }
}