using System;
using _Game_Scenes.SpaceshipCombat.Scripts;
using DG.Tweening;
using UnityEngine;

namespace _SpaceshipCombat.Scripts
{
    public class AnswerAnimationController : MonoBehaviour
    {
        [Header("SO")] 
        [SerializeField] private QuestionsManagerSO questionsManager;
        [Space]
        [SerializeField] private EnemySpaceshipAnim enemySpaceshipAnim;
        [SerializeField] private ParticleSystem moreSpeedParticle;
        [SerializeField] private ParticleSystem plusSignsParticle;
        [SerializeField] private ParticleSystem xSignsParticle;
        [SerializeField] private CameraAnim cameraAnim;

        private void OnEnable()
        {
            questionsManager.correctAnswer += CorrectAnswerEffects;
            questionsManager.wrongAnswer += WrongAnswerEffects;
        }

        private void OnDisable()
        {
            questionsManager.correctAnswer -= CorrectAnswerEffects;
            questionsManager.wrongAnswer -= WrongAnswerEffects;
        }

        public void CorrectAnswerEffects(int totalCorrectAnswers)
        {
            var speedParticleSettings = moreSpeedParticle.main;
            var plusParticleSettings = plusSignsParticle.main;
        
            plusParticleSettings.startLifetime = 4f;
            speedParticleSettings.startLifetime = 4f;
        
            plusSignsParticle.gameObject.SetActive(true);
            moreSpeedParticle.gameObject.SetActive(true);
        
            plusSignsParticle.Play();
            moreSpeedParticle.Play();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(x => plusParticleSettings.startLifetime = x, 4f, 0, 0.7f).SetEase(Ease.InOutSine).SetDelay(1f));
            sequence.Append(DOTween.To(x => speedParticleSettings.startLifetime = x, 4f, 0, 0.7f).SetEase(Ease.InOutSine));
            sequence.OnComplete(() =>
            {
                plusSignsParticle.Stop();
                moreSpeedParticle.Stop();
            });
            cameraAnim.Shake(1.7f);
            //cameraAnim.ZoomOutAndIn(0.5f,0.5f, 0.7f);
            cameraAnim.MoveRightAndBack(0.5f,0.5f,0.7f);
        
            enemySpaceshipAnim.MoveEnemyBackwards(totalCorrectAnswers);
        }

        public void WrongAnswerEffects(int totalWrongAnswers)
        {
            var particleSettings = xSignsParticle.main;
            particleSettings.startLifetime = 4f;
            xSignsParticle.gameObject.SetActive(true);
            xSignsParticle.Play();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(x => particleSettings.startLifetime = x, 4f, 0, 0.7f).SetEase(Ease.InOutSine)
                .SetDelay(1f));
            sequence.OnComplete(() =>
            {
                xSignsParticle.Stop();
            });
            cameraAnim.Shake(1.7f);
            cameraAnim.ZoomInAndOut(0.5f,0.5f, 0.7f);
            cameraAnim.MoveLeftAndBack(0.5f,0.5f,0.7f);
        
            enemySpaceshipAnim.MoveEnemyForwards(totalWrongAnswers);
        }
    }
}
