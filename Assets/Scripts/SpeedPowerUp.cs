using DG.Tweening;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(0.5f, 0.5f).From(0f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
