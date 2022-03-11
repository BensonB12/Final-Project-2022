namespace Winston 
{
    public class StackOCards
    {
        public async Task<List<CardModel>> MixAllBoosters(int numberOfDom, int numberOfWar, int numberOfNeo)
        {
            List<CardModel> finalStack = new List<CardModel>();
            MakeBooster booster;

            for (int i = 0; i < numberOfDom; i++)
            {
                booster = new MakeBooster(Set.DOM);
                finalStack.AddRange(await booster.booster);

            }
            for (int i = 0; i < numberOfWar; i++)
            {
                booster = new MakeBooster(Set.WAR);
                finalStack.AddRange(await booster.booster);
            }
            for (int i = 0; i < numberOfNeo; i++)
            {
                booster = new MakeBooster(Set.NEO);
                finalStack.AddRange(await booster.booster);
            }

            return finalStack;
        }
    }
}