using UnityEngine;
using DG.Tweening;

public class GrowPowerUp : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOScale(0.5f, 0.65f).From(0f).SetEase(Ease.OutBack);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
