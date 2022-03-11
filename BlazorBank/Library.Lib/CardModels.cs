namespace Winston
{
    public class CardResultModel
    {
        public CardModel[] cards { get; set; }
    }

    public class CardModel
    {
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Number { get; set; }
    }
}