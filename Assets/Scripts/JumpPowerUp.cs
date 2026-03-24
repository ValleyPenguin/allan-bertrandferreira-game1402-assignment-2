using DG.Tweening;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour
{

    [SerializeField] private float jumpBoostUpgradeStrength;

    private void OnEnable()
    {
        transform.DOScale(.5f, .5f).From(0f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            transform.DOScale(0, .1f).From(.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                playerController.jumpVelocity += jumpBoostUpgradeStrength;
                transform.DOKill();
                Destroy(gameObject);
            });
        }
    }
}
