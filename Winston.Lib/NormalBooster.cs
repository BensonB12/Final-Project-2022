namespace Winston 
{
    public class NormalBooster : MTGBooster
    {
        protected override Task<List<CardModel>> _Booster { set; get; }
        public NormalBooster(Set set) : base(set)
        {
            _Booster = PickXCardsFromSet(set);
            CutBackNameOff();
        }

        public async void CutBackNameOff()
        {
            foreach (var card in await _Booster)
            {
                var words = card.Name.Split(' ');

                if (words.Contains("//") == true)
                {
                    card.Name = "";

                    foreach (var word in words)
                    {
                        if(word != "//")
                        {
                            card.Name = card.Name + word + " ";
                        }
                        else
                        {
                            card.Name = card.Name.TrimEnd();
                            break;
                        }
                    }
                }
            }
        }
    }
}