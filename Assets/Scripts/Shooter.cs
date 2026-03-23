using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float shootForce;

    [SerializeField] private Transform aimTrack;

    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private Camera cam;

    private AudioSource audioSource;

    private Vector3 rayHitPosition;

    private GameObject bullet;

    private Vector3 _shootDirection;
    private PlayerState _currentState;

    private PlayerController _playerController;

    
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;


        //_playerController.OnStateUpdated += (state) => _currentState = state;
        _playerController.OnStateUpdated += StateUpdate;

    }

    void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }



    private void OnDisable()
    {
        shootInput.Disable();
        shootInput.performed -= Shoot;
        _playerController.OnStateUpdated -= StateUpdate;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;

        muzzleFlash.Play();
        audioSource.pitch = Random.Range(0.75f, 1.25f);
        
        audioSource.Play();

        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
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

//CHALLENGE FROM INDI - Make direction of arrow go with the velocity direction,
// so if its going downwards it should be aiming downwards-might not need to do if using guns instead
// ALSO - Destroy Arrows/Bullets after a while, so they don't go on forever
// ALSO - perhaps add a crosshair(of course)
