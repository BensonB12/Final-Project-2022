namespace Winston
{
    public class StackOCards
    {
        public async Task<List<CardModel>> MixAllBoosters(int numberOfDom, int numberOfWar, int numberOfNeo)
        {
            List<CardModel> finalStack = new List<CardModel>();
            MTGBooster booster;

            for (int i = 0; i < numberOfDom; i++)
            {
                var domBooster = new DOMBooster();
                finalStack.AddRange(await domBooster.booster);
                finalStack.AddRange(domBooster.legendary);
            }
            for (int i = 0; i < numberOfWar; i++)
            {
                var warBooster = new WARBooster();
                finalStack.AddRange(await warBooster.booster);
                finalStack.AddRange(warBooster.planeswalker);
            }
            for (int i = 0; i < numberOfNeo; i++)
            {
                booster = new NormalBooster(Set.NEO);
                finalStack.AddRange(await booster.booster);
            }

            var random = new Random();
            var shuffledStack = finalStack.OrderBy(a => random.Next()).ToList();

            return shuffledStack;
        }
    }
}