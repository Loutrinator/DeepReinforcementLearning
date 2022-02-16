
using UnityEngine;
using Random = System.Random;

public enum CoinSide { Head,Tail }
public class Coin
{
    private float riggedCoeff;
    private int nbOfTosses = 0;
    private float proba = 0;
    private Random rand = new Random();
    public Coin(float percentOfHead)
    {
        riggedCoeff = percentOfHead;
    }
    public CoinSide ThrowCoin()
    {
        double random = rand.NextDouble();
        if (random <= riggedCoeff)
        {
            proba = (proba * nbOfTosses + 1) / (nbOfTosses + 1);
            nbOfTosses++;
            return CoinSide.Head;
        }
        else
        {
            proba = (proba * nbOfTosses) / (nbOfTosses + 1);
            return CoinSide.Tail;
        }
    }

    public double GetProbabilityOfWin()
    {
        return proba;
    }
    public double GetRiggedCoeff()
    {
        return riggedCoeff;
    }
    public double GetNbOfTosses()
    {
        return nbOfTosses;
    }
}