using UnityEngine;
using UnityEngine.UI;
using VFolders.Libs;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image damageFlashImage;

    [SerializeField] private float playerDamageInterval;

    private float opacityTimer = 0f;

    public static PlayerHealth Instance { get; private set; }

    public int playerHealth;

    public bool didPlayerDie;

    private bool isBeingDamaged;

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
        isBeingDamaged = false;
        opacityTimer = 0f;
        didPlayerDie = false;
        damageFlashImage.color = new Color(1f, 1f, 1f, 0f);
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

        if(opacityTimer >= 0)
        {
            opacityTimer -= Time.deltaTime;
        }

        damageFlashImage.color = new Color(1f, 1f, 1f, opacityTimer);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && isBeingDamaged == false)
        {
            Debug.Log("Collided with enemy!");
            //playerHealth--;
            StartCoroutine(DamagePlayerOnAnInterval(playerDamageInterval));
            opacityTimer = 1f;
        }
    }

    private IEnumerator DamagePlayerOnAnInterval(float interval)
    {
        isBeingDamaged = true;
        playerHealth--;
        yield return new WaitForSeconds(interval);
        isBeingDamaged = false;
    }
}
