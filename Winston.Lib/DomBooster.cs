namespace Winston
{
    public class DOMBooster : MTGBooster
    {
        protected override Task<List<CardModel>> _Booster { set; get; }
        private List<CardModel> _Legendary { set; get; }
        public List<CardModel> legendary
        {
            get
            {
                return _Legendary;
            }
        }
        public DOMBooster() : base(Set.DOM)
        {
            _Booster = GetLegendary();
        }

        private async Task<List<CardModel>> GetLegendary()
        {
            var legendary = await CardProcessor.LoadNonLand(Set.DOM, Rarity.rare, 1, "empty", "Legendary");

            var numberOfUnCommonsAndRares = FindOutRarity(legendary);

            _Legendary = legendary;

            return await PickXCardsFromSet(Set.DOM, 10, numberOfUnCommonsAndRares.Item1, numberOfUnCommonsAndRares.Item2);
        }
    }
}