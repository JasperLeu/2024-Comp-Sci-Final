using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingStat : MonoBehaviour
{
    public string stat;
    public TextMeshProUGUI text;
    public float duration = 1f;
    float timer = 0;
    public Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = stat;
        timer += Time.deltaTime;

        // Fade out the text over time
        Color c = text.color;
        c.a = Mathf.Lerp(1, 0, timer / duration);
        text.color = c;
        transform.position += offset * Time.deltaTime;

        if (timer >= duration)
        {
            Destroy(gameObject); // Destroy the object after the duration
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        text.rectTransform.position = screenPos;
    }
}
