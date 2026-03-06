using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator anim;

    Vector3 _playerVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("IsGrounded", playerController.IsGrounded());

        _playerVelocity = playerController.GetPlayerVelocity();
        _playerVelocity.y = 0;

        anim.SetFloat("Velocity", playerController.GetPlayerVelocity().sqrMagnitude);
        
    }

    private void OnEnable()
    {
        playerController.OnJumpEvent += OnJump;
    }

    private void OnDisable()
    {
        playerController.OnJumpEvent -= OnJump;
    }

    private void OnJump()
    {
        anim.SetTrigger("Jump");
    }

    public void OnOpenedChest()
    {
        anim.SetTrigger("JustOpenedChest");
    }
}

