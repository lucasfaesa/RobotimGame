using System;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    public class ProjectionFlickering : MonoBehaviour
    {
        [SerializeField] private GameManagerSO gameManagerSo;
        [SerializeField] private Renderer projectionRenderer;
        [Space] 
        [SerializeField] private List<Color32> colors;

        private Sequence colorSequence;
        private Sequence flickSequence;

        private float targetAlpha = 150f;
        
        private int cont = 0;
        
        private void OnEnable()
        {
            gameManagerSo.gameEnded += KillTweens;
        }

        private void OnDisable()
        {
            gameManagerSo.gameEnded -= KillTweens;
        }

        private void KillTweens()
        {
            colorSequence.Kill();
            flickSequence.Kill();
        }
        
        void Start()
        {
            //AnimateColor();
            AnimateFlickering();
        }

        private void AnimateColor()
        {
            int randomRange = Random.Range(0, colors.Count);
            Color32 newColor = colors[randomRange];
            Color32 alpha = projectionRenderer.material.color;
            projectionRenderer.material.color = new Color32(newColor.r, newColor.g, newColor.b, alpha.a);
        }

        
        private void AnimateFlickering()
        {
            if (cont++ == 45)
            {
                cont = 0;
                AnimateColor();
            }
            
            targetAlpha = targetAlpha == 150f ? 120f : 150f;
            Color32 newColor = projectionRenderer.material.color;
            float randomRange = Random.Range(0.1f, 0.3f);
            colorSequence = DOTween.Sequence().Append(projectionRenderer.material.DOColor(new Color32(newColor.r,newColor.g,newColor.b,(byte)targetAlpha), randomRange).SetEase(Ease.Linear));
            colorSequence.OnComplete(AnimateFlickering);
        }
        
    }
}
