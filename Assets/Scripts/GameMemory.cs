using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu]

[SerializeField]
public class GameMemory : ScriptableObject
{
    public bool SoundEnabled;
    public long Pontuation;
    public long BestScore;
    public bool IsGameOver;
    private string jSONFileName;
    public int Retrys;
    public int MaxRetrysToPub;
    public int Level;

    public bool IsToReward;

    public GameMemory()
    {
        Level = 1;
        Pontuation = 0;
        IsGameOver = true;
        jSONFileName = "/Data.json";
        SoundEnabled = true;
        BestScore = 0;
        Retrys = 0;
        MaxRetrysToPub = 3;
        IsToReward = false;
    }

    public bool ToJSOnFile()
    {
        string JsonString = JsonUtility.ToJson(this);

        if (string.IsNullOrEmpty(JsonString))
            return false;

        File.WriteAllText(Application.persistentDataPath + this.jSONFileName, JsonString);

        return true;
    }

    public GameMemory FromJsonFile()
    {
        string JsonString = "";
        if (!File.Exists(Application.persistentDataPath + this.jSONFileName))
        {
            JsonString = JsonUtility.ToJson(new GameMemory());
            File.WriteAllText(Application.persistentDataPath + this.jSONFileName, JsonString);
        }

        JsonString = File.ReadAllText(Application.persistentDataPath + this.jSONFileName);

        JsonUtility.FromJsonOverwrite(JsonString, this);

        return this;
    }
}

