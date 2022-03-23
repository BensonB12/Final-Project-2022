namespace Winston
{
    public static partial class General
    {
        public static List<CardModel> TakeOutType(List<CardModel> cards, string type = "Basic")
        {
            var listOfBasicLands = new List<CardModel>();

            foreach (var card in cards)
            {
                try
                {
                    if (card.Supertypes[0] == $"{type}")
                    {
                        listOfBasicLands.Add(card);
                    }
                }
                catch
                {
                }
            }

            foreach (var basicLand in listOfBasicLands)
            {
                var gotRemoved = cards.Remove(basicLand);

                if (!gotRemoved)
                {
                    throw new Exception("There was a card that was not removed when it should have been");
                }
            }


            return cards;
        }
    }
}