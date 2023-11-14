using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "ActionsToggler", menuName = "ScriptableObjects/ActionsToggler/ActionsToggler")]
public class ActionsTogglerSO : ScriptableObject
{
    public event Action<bool> ToggleMovement;
    public event Action<bool> ToggleOrbit;
    public event Action<bool> ToggleCursor;

    public void EnableCursor(bool status)
    {
        Debug.Log($"cursor status {status}");
        ToggleCursor?.Invoke(status);
    }

    public void MovementToggle(bool status)
    {
        ToggleMovement?.Invoke(status);
    }

    public void OrbitToggle(bool status)
    {
        ToggleOrbit?.Invoke(status);
    }
}
