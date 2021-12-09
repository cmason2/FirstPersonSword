using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    public IEnumerator Fade(bool fadeIn, float fadeTime)
    {
        if (fadeIn)
        {
            fadeImage.color = new Color(0, 0, 0, 1);
            for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
            {
                fadeImage.color = new Color(0, 0, 0, i / fadeTime);
                yield return null;
            }
        }
        else
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            for (float i = 0; i <= fadeTime; i += Time.deltaTime)
            {
                fadeImage.color = new Color(0, 0, 0, i / fadeTime);
                yield return null;
            }
        }
    }
}
