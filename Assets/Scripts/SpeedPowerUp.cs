using DG.Tweening;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedBoost = 1f;

    [SerializeField] private float speedBoostUpgradeStrength;

    private void OnEnable()
    {
        transform.DOScale(.5f, .5f).From(0f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = GetComponent<PlayerController>();

            transform.DOScale(0, .5f).From(.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                playerController.moveSpeed += speedBoostUpgradeStrength;
                transform.DOKill();  
                Destroy(gameObject);
            });
        }
    }
}
