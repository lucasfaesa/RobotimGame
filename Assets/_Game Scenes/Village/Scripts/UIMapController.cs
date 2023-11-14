using System;
using System.Collections;
using System.Collections.Generic;
using _Game_Scenes.Village.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIMapController : MonoBehaviour
{
    [SerializeField] private QuestManagerSO questManager;
    [Space]
    [SerializeField] private GameObject mapSprite;
    [SerializeField] private List<StreetInfoSO> streetsList;
    [SerializeField] private List<TextMeshProUGUI> streetsText;
    [Space] 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private RectTransform playerLocationSprite;

    private bool mapActive;
    private Sequence scaleSequence;
    private bool inOverlay;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        NameStreetsOnMap();

        scaleSequence = DOTween.Sequence();
        
        scaleSequence.Append(playerLocationSprite.transform.DOScale(new Vector3(1.0619f, 1.0619f, 1.0619f), 0.35f).SetEase(Ease.InOutSine));
        scaleSequence.SetLoops(-1, LoopType.Yoyo);
        scaleSequence.Pause();
    }

    public void InOverlay(bool status)
    {
        inOverlay = status;
    } 
    
    private void CursorLocked(bool status)
    {
        if (status)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    private void NameStreetsOnMap()
    {
        for (int i = 0; i < streetsList.Count; i++)
        {
            streetsText[i].text = "Rua " + streetsList[i].StreetName;
        }
    }
    
    private void Update()
    {
        if (Keyboard.current.mKey.wasReleasedThisFrame)
        {
            ToggleMap(!mapActive);
        }

        if (mapActive)
        {
            UpdatePlayerPositionOnMap();
        }
       
    }

    private void ToggleMap(bool status)
    {
        if (inOverlay) return;
        
        mapActive = status;
        mapSprite.SetActive(status);

        if (status)
        {
            CursorLocked(false);
            MovementToggle.CameraOrbitActive(false);
            scaleSequence.Play();
        }
        else
        {
            CursorLocked(true);
            MovementToggle.CameraOrbitActive(true);
            scaleSequence.Pause();
        }
            
    }

    private void UpdatePlayerPositionOnMap()
    {
        var playerPos = playerTransform.position;
        
        float totalX = 92f;
        float totalZ = 89.39f;
        
        float xRight = 38.7f;
        float zUp = 43.18f;

        float distFromRight =  xRight - (playerPos.x + totalX);
        float distFromTop = zUp - (playerPos.z + totalZ);
        
        var percentageX = (distFromRight / totalX) * -1;
        var percentageY = (distFromTop / totalZ) * -1;
        
        float maxXSprite = 982f;
        float maxYSprite = 955.3f;

        float posX = -488 + (maxXSprite * Mathf.Clamp01(percentageX));
        float posY = -471 + (maxYSprite * Mathf.Clamp01(percentageY));
        
        playerLocationSprite.anchoredPosition = new Vector2(posX, posY);
    }

    private void OnEnable()
    {
        questManager.InOverlay += InOverlay;
    }

    private void OnDisable()
    {
        questManager.InOverlay -= InOverlay;
    }
}
