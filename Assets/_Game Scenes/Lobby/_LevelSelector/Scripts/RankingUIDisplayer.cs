using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUIDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerScore;

    public void SetInfos(string name, string score)
    {
        playerName.text = name;
        playerScore.text = score;
    }
}
