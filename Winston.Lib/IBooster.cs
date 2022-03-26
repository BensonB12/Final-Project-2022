namespace Winston
{
    public interface IBooster
    {
        //This has to be async and abstract
        public Task<List<CardModel>> PickXCardsFromSet(Set enumSet, int numberOfCommons, int numberOfUncommons, int numberOfRares, int numberOfLands);

        public (int, int) FindOutRarity(List<CardModel> card);
    }
}