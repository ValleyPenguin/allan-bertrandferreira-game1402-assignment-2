using UnityEngine;
using VFolders.Libs;

public class BulletDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroyBullet", 10f);
    }

    void DestroyBullet()
    {
        gameObject.Destroy();
        Debug.Log("Bullet Destroyed!");
    }
}
