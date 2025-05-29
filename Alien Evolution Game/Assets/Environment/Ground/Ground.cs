using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    public Texture2D fertilityTexture;
    public Material groundMat;
    [Header("Fertility Decay")]
    public float fertilityDecayRate;
    public float decayDelayTime = 1f;
    float decayTimer = 0f;

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
                float val = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) / 2 + .5f;
                fertilityTexture.SetPixel(x, y, new Color(1, 0, 0));
            }
        }
        fertilityTexture.Apply();
        groundMat.SetTexture("_Fertility", fertilityTexture);
    }

    private void Update()
    {
        // Update fertility decay timer
        decayTimer += Time.deltaTime;

        if (decayTimer >  decayDelayTime)
        {
            decayTimer = 0f;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = fertilityTexture.GetPixel(x, y);
                    float decay = fertilityDecayRate * decayDelayTime;
                    float newValue = Mathf.Clamp(pixelColor.r - decay, 0, 1);
                    fertilityTexture.SetPixel(x, y, new Color(newValue, 0, 0));
                }
            }
            fertilityTexture.Apply();
        }
    }

    public void FertilizeArea(Vector2 pos, float value, float radius)
    {
        // translate global position to texture coordinates
        Vector2 newPos = pos / 60 * new Vector2(width, height) + new Vector2(width / 2, height / 2);
        Debug.Log("Pos: " + newPos);
        // Fertilize in a circular area
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Vector2.Distance(new Vector2(x, y), newPos) < radius)
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
