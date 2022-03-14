using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stats
{
    public Stats(int level)
    {
        this.level = level;
    }

    public int level;
    public List<TimeSpan> executionTimes = new List<TimeSpan>();
    public string MinExecutionTime;
    public string MaxExecutionTime;
    public string AverageExecutionTime;
    public string TotalExecutionTime;
    public int NbExecution;
    public List<string> allExecutionTime = new List<string>();

    public void AddTime(TimeSpan execTime)
    {
        executionTimes.Add(execTime);
        allExecutionTime.Add(execTime.ToString());
        MinExecutionTime = executionTimes.Min().ToString();
        MaxExecutionTime = executionTimes.Max().ToString();
        AverageExecutionTime = new TimeSpan(Convert.ToInt64(executionTimes.Average(t => t.Ticks))).ToString();
        TotalExecutionTime =new TimeSpan(Convert.ToInt64(executionTimes.Sum(t => t.Ticks))).ToString();
        NbExecution = executionTimes.Count;
    }

    public static void ToJson(Stats stats, string additionalPath = "")
    {
        string json = JsonUtility.ToJson(stats);
        string path = Application.persistentDataPath + "/" + DateTime.Now.ToString("MM-dd-yyyy-HH-mm") +
                      additionalPath + ".json";
        Debug.Log(path);
        System.IO.File.WriteAllText(path, json);
    }
}