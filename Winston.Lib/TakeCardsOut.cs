namespace Winston
{
    public static partial class General
    {
        public static List<CardModel> TakeOutType(List<CardModel> cards, string type = "Basic")
        {
            foreach (var card in cards)
            {
                try
                {
                    if (card.Supertypes[0] == "type")
                    {
                        var gotRemoved = cards.Remove(card);

                        if(!gotRemoved)
                        {
                            throw new Exception("There was a card that was not removed when it should have been");
                        }
                    }
                }
                catch
                {
                }
            }

            return cards;
        }
    }
}