using UnityEngine;
using VFolders.Libs;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public int playerHealth;

    public bool didPlayerDie;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        didPlayerDie = false;
    }

    private void Update()
    {
        if (playerHealth <= 0)
        {
            didPlayerDie = true;
            Destroy(gameObject);
            Toast.Instance.ShowToast("Ya lost!");
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with spider!");
            playerHealth--;
        }
    }
}
