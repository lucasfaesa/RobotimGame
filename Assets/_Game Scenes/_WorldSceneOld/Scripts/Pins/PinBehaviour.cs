using System;
using UnityEngine;

public class PinBehaviour : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private PinsManagerSO pinsManager;
    [Header("Color Settings")] 
    [SerializeField] private Material[] colors;
    [SerializeField] private Renderer renderer;
    
    [field:Space]
    [field:SerializeField] public PinsController PinsController { get; set; }
    
    [field:SerializeField] [field:ReadOnly] public PinInfo PinInfo { get; private set; }

    public void SetPinInfo(PinInfo info)
    {
        PinInfo = info;
        
        if (info.completed)
            renderer.material = colors[1];
        else
            renderer.material = colors[0];
    }

    public void SelectPin() //used in event trigger on click
    {
        pinsManager.SelectPin(PinInfo);
    }
    
}

[Serializable]
public class PinInfo
{
    public string objectives;
    public string location;
    public string subject;
    public string difficulty;
    public bool completed;
    public Sprite imagePreview;

    public Material pinColor;
    public Transform parentTransform;

    public PinInfo(string objective, string loc, string subj, string diff, bool compl, Sprite preview, Transform parent)
    {
        this.objectives = objective;
        this.location = loc;
        this.subject = subj;
        this.difficulty = diff;
        this.completed = compl;
        this.imagePreview = preview;
        
        this.parentTransform = parent;
    }

}