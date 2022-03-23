using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Winston;

namespace TestingBlazor.Tests;

public class Tests
{
    //This is a 6 pack of dominaria
    List<CardModel> CardStack = new List<CardModel>();
    List<CardModel> WarBooster = new List<CardModel>();
    List<CardModel> DomBooster = new List<CardModel>();
    List<CardModel> NeoBooster = new List<CardModel>();
    int j = 0;
    int War = 2;
    int Dom = 2;
    int Neo = 2;

    int common = 0;
    int unCommon = 0;
    int rare = 0;
    int mythic = 0;
    int land = 0;

    [SetUp]
    public async Task Setup()
    {
        if (j == 0)
        {
            Winston.General.ApiHelper.InitializeClient();
            Winston.StackOCards aHeapOCards = new Winston.StackOCards();
            var stackOCards = await aHeapOCards.MixAllBoosters(Dom, War, Neo);
            CardStack.AddRange(stackOCards);

            var aHeapOfCardsToBe = new WARBooster();
            WarBooster = await aHeapOfCardsToBe.booster;
            WarBooster.AddRange(aHeapOfCardsToBe.planeswalker);

            var newHeapOfCards = new DOMBooster();
            DomBooster = await newHeapOfCards.booster;
            DomBooster.AddRange(newHeapOfCards.legendary);

            var thirdHeapOfCards = new NormalBooster(Set.NEO);
            NeoBooster = await thirdHeapOfCards.booster;

            foreach (var card in CardStack)
            {
                if (card.Rarity == "Common")
                {
                    common++;

                    if (card.Types[0] == "Land")
                    {
                        land++;
                    }
                }
                else if (card.Rarity == "Uncommon")
                {
                    unCommon++;
                }
                else if (card.Rarity == "Rare")
                {
                    rare++;
                }
                else if (card.Rarity == "Mythic")
                {
                    mythic++;
                }
            }

            j++;
        }
    }

    [Test]
    public void StacksLength()
    {
        Assert.AreEqual(90, CardStack.Count);
    }

    [Test]
    public void WarsLength()
    {
        Assert.AreEqual(15, WarBooster.Count);
    }

    [Test]
    public void DomsLength()
    {
        Assert.AreEqual(15, DomBooster.Count);
    }

    [Test]
    public void NeosLength()
    {
        Assert.AreEqual(15, NeoBooster.Count);
    }


    [Test]
    public void ArentFancyCardsInDom()
    {
        int i = 0;

        foreach (var card in DomBooster)
        {
            //This does not count basic lands
            if (int.Parse(card.Number) < 250)
            {
                i++;
            }
        }

        Assert.LessOrEqual(14, i);
    }

    [Test]
    public void ArentFancyCardsInWar()
    {
        int i = 0;

        foreach (var card in WarBooster)
        {
            //This does not count basic lands
            if (int.Parse(card.Number) < 250)
            {
                i++;
            }
        }

        Assert.LessOrEqual(14, i);
    }

    [Test]
    public void ArentFancyCardsInNeo()
    {
        int i = 0;

        foreach (var card in NeoBooster)
        {
            //This does not count basic lands
            if (int.Parse(card.Number) < 283)
            {
                i++;
            }
        }

        Assert.LessOrEqual(14, i);
    }

    [Test]
    public void WarBoosterContainsPlaneswalker()
    {
        int i = 0;

        foreach (var card in WarBooster)
        {
            if (card.Types[0] == "Planeswalker")
            {
                i++;
            }
        }

        Assert.AreEqual(1, i);
    }

    [Test]
    public void StackContainsPlaneswalkers()
    {
        int i = 0;

        foreach (var card in CardStack)
        {
            if (card.Types[0] == "Planeswalker")
            {
                i++;
            }
        }

        Assert.GreaterOrEqual(i, War);
    }

    [Test]
    public void DomBoosterContainsLegend()
    {
        int i = 0;

        foreach (var card in DomBooster)
        {
            try
            {
                if (card.Supertypes[0] == "Legendary")
                {
                    i++;
                }
            }
            catch
            {
            }
        }

        Assert.GreaterOrEqual(i, 1);
    }

    [Test]
    public void CommonCount()
    {

        Assert.AreEqual(66, common);
    }

    [Test]
    public void UncommonCount()
    {
        Assert.AreEqual(18, unCommon);
    }

    public void RaresAndMythicsCount()
    {
        Assert.AreEqual(6, rare + mythic);
    }

    [Test]
    public void CommonLandCount()
    {
        Assert.GreaterOrEqual(land, 6);
    }
}