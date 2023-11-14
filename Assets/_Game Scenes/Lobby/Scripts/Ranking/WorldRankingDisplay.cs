using TMPro;
using UnityEngine;

namespace _Lobby.Scripts.Ranking
{
    public class WorldRankingDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshPro playerName;
        [SerializeField] private TextMeshPro playerScore;

        public void SetInfos(string name, string score)
        {
            playerName.text = name;
            playerScore.text = score;
        }
    }
}
