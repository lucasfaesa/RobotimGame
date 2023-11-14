using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImageMap : MonoBehaviour
{
    [SerializeField] public float rayDistance;

    [field:SerializeField] public Texture2D imageMap;
    
    private int FindIndexFromColor(Color color)
    {
        if (color.r >= 0.5)
            return 0;
        
        return 1;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, rayDistance))
        {
            ImageMap map = hit.transform.GetComponent<ImageMap>();
            Texture2D texture = map.imageMap;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= texture.width;
            pixelUV.y *= texture.height;
            
            Color color = imageMap.GetPixel(Mathf.FloorToInt(pixelUV.x), Mathf.FloorToInt(pixelUV.y));
            
            int index = FindIndexFromColor(color);
        }
    }
}
