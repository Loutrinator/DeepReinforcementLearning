using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadOrTail : MonoBehaviour
{
    private List<Coin> coins = new List<Coin>(); //le pourcentage d'obtention de pile
    private int currentCoin = 0;
    [SerializeField] private float epsilon = 0.2f;
    [SerializeField] private bool enableDecay;
    [SerializeField] private int nbOfTurnsMax = 5000;

    private float moneyCount;
    private int iterations;
    private float currentEpsilon;
    
    private System.Random rand = new System.Random();

    [SerializeField] private Text coinsLabel;
    [SerializeField] private Text chosenCoinLabel;
    [SerializeField] private Text iterationsLabel;
    [SerializeField] private Text moneyLabel;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
       if(moneyCount > 0 && iterations < nbOfTurnsMax)
       {
           if (enableDecay) UpdateEpsilon();
            ChooseCoin();
            Play();
        }
    }

    private void UpdateEpsilon()
    {
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }
    //Démarre le jeu et l'initialise
    private void StartGame()
    {
        coins = new List<Coin>();
        coins.Add(new Coin(0.25f));
        coins.Add(new Coin(0.5f));
        coins.Add(new Coin(0.75f));

        ShuffleCoins();

        moneyCount = 20;
        int iterations = 0;
        currentEpsilon = epsilon;

    }

    private void UpdateUI()
    {
        coinsLabel.text = "Coins : \n";
        coinsLabel.text += "Coin A (" + coins[0].GetRiggedCoeff() + ") -> " + Math.Round(coins[0].GetProbabilityOfWin(),3) + " : choosen " + coins[0].GetNbOfTosses() + " times\n";
        coinsLabel.text += "Coin B (" + coins[1].GetRiggedCoeff() + ") -> " + Math.Round(coins[1].GetProbabilityOfWin(),3) + " : choosen " + coins[1].GetNbOfTosses() + " times\n";
        coinsLabel.text += "Coin C (" + coins[2].GetRiggedCoeff() + ") -> " + Math.Round(coins[2].GetProbabilityOfWin(),3) + " : choosen " + coins[2].GetNbOfTosses() + " times\n";
        switch (currentCoin)
        {
            case 0 :
                chosenCoinLabel.text = "Current coin : A";
                break;
            case 1 :
                chosenCoinLabel.text = "Current coin : B";
                break;
            case 2 :
                chosenCoinLabel.text = "Current coin : C";
                break;
        }

        iterationsLabel.text = iterations + " itterations";
        moneyLabel.text = moneyCount + "€";
    }

    private void ShuffleCoins()
    {
        int n = coins.Count;  
        while (n > 1) {  
            n--;  
            int k = rand.Next( n + 1); 
            Debug.Log("k : " + k);
            Coin value = coins[k];  
            coins[k] = coins[n];  
            coins[n] = value;  
        }  
    }

    //Joue un tour
    private void ChooseCoin()
    {
        double r = rand.NextDouble();
        if (r < epsilon)
        {
            PickCoin(rand.Next(3));
        }
        else
        {
            float bestProba = 0f;
            int bestCoinId = 0;
            for (int i = 0; i < coins.Count; i++)
            {
                if (coins[i].GetProbabilityOfWin() > bestProba)
                {
                    bestProba = (float) coins[i].GetProbabilityOfWin();
                    bestCoinId = i;
                }
            }

            PickCoin(bestCoinId);
        }
    }
    private void PickCoin(int coin)
    {
        currentCoin = coin;
    }
    //Joue un tour
    private void Play()
    {
        moneyCount -= 1;
        CoinSide side = coins[currentCoin].ThrowCoin();
        if (side == CoinSide.Head)
        {
            Debug.Log("Win !");
            moneyCount += 2;
        }
        else
        {
            
            Debug.Log("Lost !");
        }

        iterations++;
    }
}
