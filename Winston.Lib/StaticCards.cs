namespace Winston
{
    public static class StaticPileOfCards
    {
        private static int player = 1;
        public static int Player
        { 
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }
        public static bool Using { get; set; }
        public static List<CardModel> PileOfCards { get; set; }
        public static List<CardModel> PopUpCards = new List<CardModel>();
        public static bool BoolPopUpCards = false;
        public static int chosenPlayer = 1;
        public static bool otherPlayerCanSee = false;
    }
}