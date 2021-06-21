using Assets.Scripts;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public ScoreController Score;
    public ConsoleController Console;
    public Text UniqueIdentifier;

    private readonly string dataKey = "score";

    public void ConnectToPlayfab()
    {
        var customId = UniqueIdentifier.text;
        var data = GetCurrentData();
        PlayFabAuthentication.Authentication(customId, data, SetLocalData);
    }

    public void SetLocalData(Dictionary<string, UserDataRecord> data)
    {
        var score = data[dataKey].Value;
        var newIntScore = int.Parse(score);
        Score.Score = newIntScore;
    }

    private Dictionary<string, string> GetCurrentData()
    {
        return new Dictionary<string, string>
        {
            [dataKey] = Score.Score.ToString()
        };
    }
}
