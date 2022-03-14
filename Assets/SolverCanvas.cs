using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolverCanvas : MonoBehaviour
{
    [SerializeField] private Text States;
    [SerializeField] private Text Temps;
    [SerializeField] private Text Algo;
    [SerializeField] private Button ShowButton;
    [SerializeField] private Image Loader;

    public void Start()
    {
        Loader.gameObject.SetActive(false);
        ShowButton.gameObject.SetActive(false);
        States.gameObject.SetActive(false);
        Temps.gameObject.SetActive(false);
        States.text = "";
        Temps.text = "";
    }

    public void BeginSolving()
    {
        Loader.gameObject.SetActive(true);
    }

    public void Solved(float solvingTimeInSeconds, int stateCount)
    {
        Loader.gameObject.SetActive(false);
        
        States.gameObject.SetActive(true);
        Temps.gameObject.SetActive(true);
        ShowButton.gameObject.SetActive(true);
        Temps.text = "Temps d'exécution : " + solvingTimeInSeconds + " s";
        States.text = "Nombre d'états possibles : " + stateCount;
    }

    public void SetAlgo(SolvingAlgorithm algorithm)
    {
        string name = "";

        name = algorithm.ToString();
        
        Algo.text = name;
    }
    
}
