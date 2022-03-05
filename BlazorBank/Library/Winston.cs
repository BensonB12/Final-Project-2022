//using MtgApiManager.Lib.Core;
            // IOperationResult<List<string>> result = service.AllAsync();

            // if (result.IsSuccess)
            // {
            //     var value = result.Value;
            // }
            // else
            // {
            //     var exception = result.Exception;
            // }
// IOperationResult<List<ICard>> result = service.AllAsync();

// if (result.IsSuccess)
// {
// var value = result.Value;
// }
// else
// {
// var exception = result.Exception;
// }
//This was used in the Github example
//Github Link: https://github.com/MagicTheGathering/mtg-sdk-dotnet

using MtgApiManager.Lib.Service;


namespace Winston
{
    //Booster packs have 10 common, 3 uncommon, 1 rare/mythicRare, and 1 non-basic land if there is any in the set. (DOM had basics and WAR had one common land)

    public enum Set
    {
        WAR,
        NEO,
        DOM
    }

    public enum Rarity
    {
        commmon,
        uncommon,
        rare,
        mythicRare,
        land
    }

    public enum cardType
    {
        planeswalker,
        legendary
    }
    public abstract class MakeBooster
    {
        static IMtgServiceProvider serviceProvider = new MtgServiceProvider();

        static ICardService service = serviceProvider.GetCardService();
        private Set set;
        private int NumberOfCards = 15;
        private string[] Cards;
        public MakeBooster(Set set)
        {
            this.set = set;
        }
        public MakeBooster(Set set, int numberOfCards) : this(set)
        {
            this.NumberOfCards = numberOfCards;
        }

        private void PickXCardsFromSet()
        {
        }
    }

    public class NormalBooster : MakeBooster
    {
        public NormalBooster(Set set) : base(set)
        {
        }
    }

    public class WARBooster : MakeBooster
    {
        public WARBooster() : base(Set.WAR)
        {
        }
    }

    public class DOMBooster : MakeBooster
    {
        public DOMBooster() : base(Set.DOM)
        {
        }
    }
}