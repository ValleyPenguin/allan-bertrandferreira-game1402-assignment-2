using UnityEngine;
using DG.Tweening;

public class GrowPowerUp : MonoBehaviour
{
    [SerializeField] private float playerSizeUpgradeStrength;

    private void OnEnable()
    {
        transform.DOScale(.5f, .75f).From(0f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            transform.DOScale(0, .1f).From(.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                Toast.Instance.ShowToast("Upgraded Player Size!");
                playerController.playerTransform.localScale *= playerSizeUpgradeStrength;
                transform.DOKill();
                Toast.Instance.HideToastWithDelay(1.5f);
                Destroy(gameObject);
            });
        }
    }
}
