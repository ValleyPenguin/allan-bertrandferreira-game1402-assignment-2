using UnityEngine;
using UnityEngine.InputSystem;

public class SMGShooter : Shooter
{
    [SerializeField] private float fireRate = 0.1f;
    private float _nextFireTime;
    private bool _isFiring;

    protected override void OnEnable()
    {
        base.OnEnable();
        shootInput.canceled += StopFiring;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        shootInput.canceled -= StopFiring;
    }

    private void Update()
    {
        if (_isFiring && Time.time >= _nextFireTime)
        {
            FireOnce();
            _nextFireTime = Time.time + fireRate;
        }
    }

    protected override void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;
        _isFiring = true;
        _nextFireTime = 0f;
    }

    private void StopFiring(InputAction.CallbackContext context)
    {
        _isFiring = false;
    }

    private void FireOnce()
    {
        muzzleFlash.Play();
        audioSource.pitch = Random.Range(0.95f, 1.67f);
        audioSource.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        if ((Physics.Raycast(ray, out RaycastHit hit, 1000f, hitLayers)))
        {
            rayHitPosition = hit.point;
        }
        else
        {
            rayHitPosition = ray.GetPoint(50f);
        }   

        _shootDirection = (rayHitPosition - shootPoint.position).normalized;
        bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(_shootDirection));
        bullet.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
    }
}