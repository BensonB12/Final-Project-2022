namespace Winston
{
    public interface ICardProcessor
    {
        public Task<CardModel> LoadLand(Set enumSet);
        public Task<CardModel> LoadNonLand(Set enumSet);
    }
}