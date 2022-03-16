namespace Winston
{
    public class CardResultModel
    {
        public CardModel[] cards { get; set; }
    }

    public class CardModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string ImageUrl { get; set; }
        public string[] Types { get; set; }
        public string[] Supertypes { get; set; }
    }
}