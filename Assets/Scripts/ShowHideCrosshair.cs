using UnityEngine;

public class ShowHideCrosshair : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;

    public static ShowHideCrosshair Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowCrosshair()
    {
        crosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        crosshair.SetActive(false);
    }
}
