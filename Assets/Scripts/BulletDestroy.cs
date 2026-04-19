using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyBullet", 10f);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
        Debug.Log("Bullet Destroyed!");
    }
}
