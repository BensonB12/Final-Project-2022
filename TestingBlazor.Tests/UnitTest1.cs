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
    int j = 0;
    int War = 4;
    int Dom = 2;

    [SetUp]
    public async Task Setup()
    {
        if (j == 0)
        {
            Winston.General.ApiHelper.InitializeClient();
            Winston.StackOCards aHeapOCards = new Winston.StackOCards();
            var stackOCards = await aHeapOCards.MixAllBoosters(Dom, War, 0);
            CardStack.AddRange(stackOCards);

            var aHeapOfCardsToBe = new MakeBooster(Set.WAR);
            WarBooster = await aHeapOfCardsToBe.booster;

            var newHeapOfCards = new MakeBooster(Set.DOM);
            DomBooster = await newHeapOfCards.booster;

            j++;
        }
    }

    [Test]
    public void StacksLength()
    {
        Assert.AreEqual(90, CardStack.Count);
    }

    [Test]
    public void CardsArentFancySetBoosters()
    {
        int i = 0;

        foreach (var card in CardStack)
        {
            if (int.Parse(card.Number) < 250)
            {
                i++;
            }
        }

        Assert.LessOrEqual(84, i);
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

        Assert.AreEqual(War, i);
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

        Assert.GreaterOrEqual(i, Dom);
    }
}