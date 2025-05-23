using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public Texture2D fertilityTexture;
    public Material groundMat;
    public float fertilityDecayRate;

    void Start()
    {
        fertilityTexture = new Texture2D(width, height, TextureFormat.RFloat, false);
        fertilityTexture.filterMode = FilterMode.Point; // Pixel-perfect
        fertilityTexture.wrapMode = TextureWrapMode.Clamp;

        // Initialize to full fertility
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                fertilityTexture.SetPixel(x, y, new Color(0, 0, 0));
            }
        }
        fertilityTexture.Apply();

        groundMat.SetTexture("_Fertility", fertilityTexture);
        FertilizeArea(100, 100, 1, 300);
    }

    private void Update()
    {
        // decay fertility over time
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        Color pixelColor = fertilityTexture.GetPixel(x, y);
        //        float newValue = Mathf.Clamp(pixelColor.r - Time.deltaTime * fertilityDecayRate, 0, 1);
        //        fertilityTexture.SetPixel(x, y, new Color(newValue, 0, 0));
        //    }
        //}
        //fertilityTexture.Apply();
    }

    public void FertilizeArea(int xPos, int yPos, float value, float radius)
    {
        // Fertilize in a circular area
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), new Vector2(xPos, yPos)) < radius)
                {
                    Color pixelColor = fertilityTexture.GetPixel(x, y);
                    float newValue = Mathf.Clamp(pixelColor.r + value, 0, 1);
                    fertilityTexture.SetPixel(x, y, new Color(newValue, 0, 0));
                }
            }
        }
        // Apply the changes to the texture
        fertilityTexture.Apply();
    }
}
