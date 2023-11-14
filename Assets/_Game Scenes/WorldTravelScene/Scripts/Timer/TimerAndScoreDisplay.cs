using TMPro;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Timer
{
    public class TimerAndScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtNumber;
        [SerializeField] private TextMeshProUGUI txtTime;
        [SerializeField] private TextMeshProUGUI txtPoints;

        public void SetData(int number, float time, int points)
        {
            txtNumber.text = $"{number}º";
            txtTime.text = ToCorrectTimeString(time);
            txtPoints.text = ($"+{points}");
        }
        
        private string ToCorrectTimeString(float time)
        {
            var intTime = time;
            var minutes = intTime / 60;
            var seconds = intTime % 60;
            var fraction = time * 1000;
            fraction = fraction % 1000;
            var timeText = $"{minutes:00}:{seconds:00}:{fraction:00}";
            return timeText[..8]; //returning only 8 characters
        }
    }
}
