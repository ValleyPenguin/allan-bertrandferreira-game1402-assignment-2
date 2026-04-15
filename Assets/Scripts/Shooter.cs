using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] protected InputAction shootInput;

    [SerializeField] protected Transform shootPoint;

    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected float shootForce;

    [SerializeField] protected Transform aimTrack;

    [SerializeField] protected ParticleSystem muzzleFlash;

    [SerializeField] protected Camera cam;

    protected AudioSource audioSource;

    protected Vector3 rayHitPosition;

    protected GameObject bullet;

    protected Vector3 _shootDirection;
    protected PlayerState _currentState;

    protected PlayerController _playerController;

    [SerializeField] protected LayerMask hitLayers;


    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        audioSource = GetComponentInParent<AudioSource>();
    } 

    protected virtual void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;

        if (_playerController != null) _playerController.OnStateUpdated += StateUpdate;

    }

    void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }

    protected virtual void OnDisable()
    {
        shootInput.Disable();
        shootInput.performed -= Shoot;
        if(_playerController != null) _playerController.OnStateUpdated -= StateUpdate;
    }

    protected virtual void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;

        muzzleFlash.Play();
        audioSource.pitch = Random.Range(0.75f, 1.55f);
        
        audioSource.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, hitLayers))
        {
            rayHitPosition = hit.point;
        }
        else
        {
            rayHitPosition = ray.GetPoint(50f);
        }

        //calculate the direction
        _shootDirection = rayHitPosition - shootPoint.position;
        _shootDirection.Normalize();

        //create a new arrow OLD: shootPoint.rotation
        bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(_shootDirection));

        //apply a force
        bullet.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
    }
}
