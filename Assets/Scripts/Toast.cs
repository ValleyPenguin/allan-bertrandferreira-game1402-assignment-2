using TMPro;
using UnityEngine;

public class Toast : MonoBehaviour
{
    public static Toast Instance;

    [SerializeField] private GameObject toastUI;
    [SerializeField] private TMP_Text toastText;

    void Awake()
    {
        //simple singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }

    void Start()
    {
        toastUI.SetActive(false);
    }

    public void ShowToast(string textValue)
    {
        toastUI.SetActive(true);
        toastText.SetText(textValue);
    }

    public void HideToast()
    {
        toastUI.SetActive(false);
    }

    public void HideToastWithDelay(float delay)
    {
        Invoke("HideToast", delay);
    }
}
