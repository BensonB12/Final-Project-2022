using System;

namespace Winston
{
    public class CardModelHTML
    {
        private CardStatus status = CardStatus.Pile1FaceUp;
        public string Id { get; set; }
        public CardModel card { get; set; }
        public CardStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        // public string Description { get; set; }
        public string StringURL { get; set; }
        public string BackOfCardURL => "BackOfMTGCard.jpg";
        //public DateTime LastUpdated { get; set; }
    }

    public enum CardStatus
    {
        Pile1FaceUp,
        Pile2FaceDown
    }
}
