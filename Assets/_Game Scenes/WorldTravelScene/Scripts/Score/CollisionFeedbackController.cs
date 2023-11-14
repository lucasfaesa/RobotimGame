using _WorldTravelScene.ScriptableObjects.Planet;
using _WorldTravelScene.Scripts.Countries;
using DG.Tweening;
using Inside;
using TMPro;
using UnityEngine;

namespace _WorldTravelScene.Scripts.Score
{
    public class CollisionFeedbackController : MonoBehaviour
    {
        [SerializeField] private CountriesManagerSO countriesManager;
        [SerializeField] private LandSeaCollisionManagerSO landSeaCollisionManager;
        [Space] 
        [SerializeField] private DisplayObject scoreRef;
        [SerializeField] private DisplayObject errorRef;
        [Space]
        [SerializeField] private TextMeshPro countryText;
        [SerializeField] private TextMeshPro scoreText;
        [SerializeField] private TextMeshPro errorText;

        private Sequence showScoreSequence;
        private Sequence hideScoreSequence;

        private Sequence showMissSequence;
        private Sequence hideMissSequence;

        private void Start()
        {
            showScoreSequence = DOTween.Sequence().Append(scoreRef.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack)).SetAutoKill(false).AppendInterval(3.5f).Pause();
            hideScoreSequence = DOTween.Sequence().Append(scoreRef.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)).SetAutoKill(false).Pause();
            
            showMissSequence = DOTween.Sequence().Append(errorRef.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack)).SetAutoKill(false).AppendInterval(3.5f).Pause();
            hideMissSequence = DOTween.Sequence().Append(errorRef.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)).SetAutoKill(false).Pause();
            
        }

        private void OnEnable() { 
            countriesManager.targetHitCorrectly += ShowScoreFeedback;
            countriesManager.targetHitIncorrectly += ShowErrorFeedback;
            landSeaCollisionManager.collidedWithLand += ShowMissFeedback;
        }

        private void OnDisable() { 
            countriesManager.targetHitCorrectly -= ShowScoreFeedback;
            countriesManager.targetHitIncorrectly -= ShowErrorFeedback;
            landSeaCollisionManager.collidedWithLand -= ShowMissFeedback;
        }

        private void ShowMissFeedback(bool onLand)
        {
            if (hideMissSequence.IsPlaying()) { hideMissSequence.Pause();}
            
            errorRef.transform.localScale = Vector3.zero;

            if (onLand)
            {
                errorText.text = "x Errou x";
            }
            else
            {
                errorText.text = "x Na água x";
            }
            
            errorText.color = Color.yellow;
            errorRef.Solo();

            showMissSequence.Restart();
            showMissSequence.OnComplete(() =>
            {
                hideMissSequence.Restart();
                hideMissSequence.OnComplete(() =>
                {
                    errorRef.gameObject.SetActive(false);
                });
            });
        }
        
        private void ShowErrorFeedback(CountrySO countrySo, TargetDivision.TargetDivisionType targetDivisionType, int arg3)
        {
            if (hideScoreSequence.IsPlaying()) { hideScoreSequence.Pause(); }
            
            scoreRef.transform.localScale = Vector3.zero;
            
            countryText.text = countrySo.CountryName;
            scoreText.text = "x País errado x";
            scoreText.color = Color.black;
            
            scoreRef.Solo();

            showScoreSequence.Restart();
            showScoreSequence.OnComplete(() =>
            {
                hideScoreSequence.Restart();
                hideScoreSequence.OnComplete(() =>
                {
                    scoreRef.gameObject.SetActive(false);
                });
            });
        }
        
        private void ShowScoreFeedback(CountrySO country, TargetDivision.TargetDivisionType division, int pointsAmount)
        {
            if (hideScoreSequence.IsPlaying()) { hideScoreSequence.Pause(); }
            
            scoreRef.transform.localScale = Vector3.zero;
            countryText.text = country.CountryName;
            scoreText.text = "+" + pointsAmount + " Pontos";

            switch (division)
            {
                case TargetDivision.TargetDivisionType.Inner:
                    scoreText.color = Color.red;
                    break;
                case TargetDivision.TargetDivisionType.Middle:
                    scoreText.color = new Color32((byte)0f, (byte)141f,(byte)226f, (byte)255f);
                    break;
                case TargetDivision.TargetDivisionType.Outer:
                    scoreText.color = new Color32((byte)0f, (byte)0f,(byte)152f, (byte)255f);
                    break;
            }
            
            scoreRef.Solo();
            
            showScoreSequence.Restart();
            showScoreSequence.OnComplete(() =>
            {
                hideScoreSequence.Restart();
                hideScoreSequence.OnComplete(() =>
                {
                    scoreRef.gameObject.SetActive(false);
                });
            });
        }
    }
}
