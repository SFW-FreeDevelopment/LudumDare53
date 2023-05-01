using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFadeBehaviourScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeTime = 1f;
    
    private void Start()
    {
        StartCoroutine(FadeOverTime());
    }

    private IEnumerator FadeOverTime()
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < fadeTime)
        {
            float t = elapsedTime / fadeTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor;
    }
}