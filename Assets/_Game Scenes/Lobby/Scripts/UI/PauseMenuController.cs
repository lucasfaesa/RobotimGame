using System;
using System.Collections;
using System.Collections.Generic;
using _MeteorShower.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private ActionsTogglerSO actionsToggler;
    [Space]
    [SerializeField] private GameObject content;
    
    private bool inPauseMenu;
    private bool canPause;

    private void OnEnable()
    {
        gameManager.gameEnded += GameEndedPreventPauseFunction;
    }

    private void OnDisable()
    {
        gameManager.gameEnded -= GameEndedPreventPauseFunction;
    }

    private void GameEndedPreventPauseFunction()
    {
        if(content.activeSelf)
            content.SetActive(false);
        inPauseMenu = false;
        canPause = false;
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasReleasedThisFrame)
        {
            TogglePauseMenu();
        }    
    }

    public void TogglePauseMenu()
    {
        inPauseMenu = !inPauseMenu;

        if (inPauseMenu)
        {
            content.SetActive(true);
            actionsToggler.EnableCursor(true);
            actionsToggler.MovementToggle(false);
            actionsToggler.OrbitToggle(false);
        }
        else
        {
            content.SetActive(false);
            actionsToggler.EnableCursor(false);
            actionsToggler.MovementToggle(true);
            actionsToggler.OrbitToggle(true);
        }
    }
}
