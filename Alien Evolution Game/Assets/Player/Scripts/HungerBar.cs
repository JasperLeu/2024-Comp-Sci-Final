using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [Header("Hunger Bar Info")]
    public Sprite filled;
    public Sprite empty;
    public float segLength;
    public GameObject segmentAsset;
    public GameObject[] segments;

    [Header("Hunger")]
    public int food;
    public int maxHunger;

    [Header("Warning Flash")]
    public float flashingSpeed;
    public Vector2 flashRange;
    public GameObject flash;
    float flashTimer;

    void Start()
    {
        food = Mathf.RoundToInt(maxHunger / 2);
        initializeBar();
    }

    void Update()
    {
        // Warning Flash
        flashTimer += Time.deltaTime * flashingSpeed;
        float scale = Mathf.Sin(flashTimer) / 2 + .5f;
        flash.transform.localScale = Vector3.one * (scale * (flashRange[1] - flashRange[0]) + flashRange[0]);

        for (int i = 0; i < maxHunger; i++)
        {
            Sprite img = empty;
            if (i < food)
            {
                img = filled;
            }
            // Flashing on segment
            if (i == food - 1)
            {
                if (flashingSpeed > 0)
                {
                    flash.SetActive(true);
                    flash.transform.position = segments[i].transform.position;
                }
            }
            segments[i].GetComponentInChildren<Image>().sprite = img;
        }
        // disabling flashing
        if (food == 0 || flashingSpeed == 0)
        {
            flash.SetActive(false);
        }
    }
    public void initializeBar()
    {
        foreach (GameObject seg in segments)
        {
            Destroy(seg);
        }
        segments = new GameObject[maxHunger];
        float startPos = -segLength * maxHunger / 2;
        for (int i = 0; i < maxHunger; i++)
        {
            segments[i] = Instantiate(segmentAsset, Vector3.zero, Quaternion.identity, transform);
            segments[i].transform.localPosition = new Vector3(startPos + i * segLength, 0, 0);
        }
    }
}
