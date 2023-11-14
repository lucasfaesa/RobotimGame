using System;
using System.Collections;
using System.Collections.Generic;
using _Lobby._LevelSelector.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissionBoardController : MonoBehaviour
{
    [Header("Respective UI")] 
    [SerializeField] private LevelsSelector respectiveMissionBoardUI;
    [Header("Animation")] 
    [SerializeField] private Transform planeTransform;
    [SerializeField] private float animationTime;
    [Header("Texture Mover")] 
    [SerializeField] private Renderer planeRenderer;
    [SerializeField] private float scrollSpeed;

    private bool canScrollTexture;
    private float offsetY = 0;
    private bool shown;
    private bool insideMissionBoardArea;

    public void PlayerInsideBoardArea(bool status)
    {
        insideMissionBoardArea = status;
    }
    
    public void ShowMissionBoardAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(planeTransform.DOScaleZ(0.2063594f, animationTime).SetEase(Ease.OutBack, 4f));
        canScrollTexture = true;
    }

    public void HideMissionBoardAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(planeTransform.DOScaleZ(0f, animationTime).SetEase(Ease.InBack, 4f));
        sequence.OnComplete(() =>
        {
            canScrollTexture = false;
        });
    }
    
    void Update()
    {
        if (canScrollTexture)
        {
            offsetY += scrollSpeed * Time.deltaTime;

            if (offsetY >= 1f)
                offsetY = 0f;

            planeRenderer.material.mainTextureOffset = new Vector2(0, offsetY);
        }

        if (insideMissionBoardArea)
        {
            if(Keyboard.current.eKey.wasPressedThisFrame)
                respectiveMissionBoardUI.ToggleContent(true);
        }
    }
}
