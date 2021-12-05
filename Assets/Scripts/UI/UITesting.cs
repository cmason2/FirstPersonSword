using UnityEngine;
using UnityEngine.UI;

public class UITesting : MonoBehaviour
{
    public Button easy;
    public Button medium;
    public Button hard;
    public Button back;

    // Start is called before the first frame update
    void Start()
    {
        easy.gameObject.SetActive(false);
        medium.gameObject.SetActive(false);
        hard.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
    }
}
