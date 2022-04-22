using static Winston.StaticPileOfCards;

namespace Winston
{
    public static class StaticGenerals
    {
        public static void ClearFOF()
        {
            PileOfCards = null;
            Player = 0;
            Using = false;
            BoolPopUpCards = false;
            otherPlayerCanSee = false;
            chosenPlayer = 1;
            onlyOnePlayer = true;
        }

        public static void PlrChosePile(int pile, ref List<CardModelHTML> CardsForChoice)
        {
            otherPlayerCanSee = false;
            CardsForChoice = new List<CardModelHTML>();

            if (chosenPlayer == 1)
            {
                if (pile == 1)
                {
                    Player1Cards.AddRange(PopUpCards.Item1);
                    Player2Cards.AddRange(PopUpCards.Item2);
                }
                else
                {
                    Player1Cards.AddRange(PopUpCards.Item2);
                    Player2Cards.AddRange(PopUpCards.Item1);
                }
            }
            else
            {
                if (pile == 1)
                {
                    Player2Cards.AddRange(PopUpCards.Item1);
                    Player1Cards.AddRange(PopUpCards.Item2);
                }
                else
                {
                    Player2Cards.AddRange(PopUpCards.Item2);
                    Player1Cards.AddRange(PopUpCards.Item1);
                }
            }

            if (PileOfCards.Count == 0)
            {
                GameOver = true;
            }

            PopUpCards.Item1.Clear();
            PopUpCards.Item2.Clear();
        }

        public static void ShowCardsToOtherPlayer(List<CardModelHTML> CardsForChoice)
        {
            foreach (var HTMLCard in CardsForChoice)
            {
                if (HTMLCard.Status == CardStatus.Pile1FaceUp)
                {
                    StaticPileOfCards.PopUpCards.Item1.Add(HTMLCard.card);
                }
                else
                {
                    StaticPileOfCards.PopUpCards.Item2.Add(HTMLCard.card);
                }
            }

            StaticPileOfCards.chosenPlayer++;

            if (StaticPileOfCards.chosenPlayer == 3)
            {
                StaticPileOfCards.chosenPlayer = 1;
            }

            StaticPileOfCards.BoolPopUpCards = false;

            StaticPileOfCards.otherPlayerCanSee = true;
        }

        public static void HandleStatusUpdated()
        {
        }
    }

}