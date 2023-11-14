using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;
    
    void Start()
    {
        versionText.text = Application.version;
    }

}
