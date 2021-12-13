using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeInOut : MonoBehaviour
{
    TextMeshProUGUI infoText;
    
    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator DisplayTextFade(string text, float displayTime, float fadeTime)
    {
        infoText.text = text;
        for (var t = 0f; t < 1; t += Time.deltaTime / fadeTime)
        {
            infoText.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        
        yield return new WaitForSeconds(displayTime);

        for (var t = 0f; t < 1; t += Time.deltaTime / fadeTime)
        {
            infoText.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        infoText.alpha = 0;
    }
}
