using UnityEngine;
using VFolders.Libs;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public int playerHealth;

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

    void Start()
    {
        
    }

    void Update()
    {
        if (playerHealth <= 0)
        {
            gameObject.Destroy();
            Toast.Instance.ShowToast("Ya lost!");
            Time.timeScale = 1f;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with spider!");
            playerHealth--;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with spider!");
            playerHealth--;
        }
    }
}
