namespace Winston
{
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

            var numberOfUnCommonsAndRares = FindOutRarity(planeswalker);

            _Planeswalker = planeswalker;

            return await PickXCardsFromSet(Set.WAR, 10, numberOfUnCommonsAndRares.Item1, numberOfUnCommonsAndRares.Item2);
        }
    }
}