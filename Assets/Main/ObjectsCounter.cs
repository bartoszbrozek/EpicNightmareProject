using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ObjectsCounter : MonoBehaviour
{

    private string[] tags = new string[] { "Food", "Minion" };

    void Awake()
    {
        this.appendToCsv("Born", "Dead", "Actual Number of Minions");
    }

    void Start()
    {
        InvokeRepeating("countObjects", 0f, 10f);
    }

    void Update()
    {

    }

    void countObjects()
    {
        foreach (string tag in this.tags)
        {
            int length = GameObject.FindGameObjectsWithTag(tag).Length;

            Debug.Log("Counter > " + tag + ": " + length);
        }

        Debug.Log("Born: " + GameManager.numberBorn + " Dead: " + GameManager.numberDead);

        int actualMinionsNumber = GameObject.FindGameObjectsWithTag("Minion").Length;

        this.appendToCsv(GameManager.numberBorn.ToString(), GameManager.numberDead.ToString(), actualMinionsNumber.ToString());
    }

    void appendToCsv(string a, string b, string c)
    {
        var csv = new StringBuilder();
        var newLine = string.Format("{0},{1},{2}", a, b, c);
        csv.AppendLine(newLine);
        string filePath = "minionsSim.csv";

        File.AppendAllText(filePath, csv.ToString());
    }
}
