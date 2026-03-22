using UnityEngine;
using DG.Tweening;

public class ChestInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private Animator anim;

    private int isOpenHash;
    private Tween _loopTween;
    private Tween _collectTween;

    [SerializeField] private PlayerAnimator _playerAnimator;


    void Start()
    {
        if (!anim) return;

        isOpenHash = Animator.StringToHash("IsOpen");

        //to make the chests keep growing and shrinking
        //transform.DOScale(1.1f, 1.2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    public void OnHoverIn()
    {
        Debug.Log("Interactor in!");
        anim?.SetBool(isOpenHash, true);

        // TODO - Show UI
        Toast.Instance.ShowToast("Press \"E\" to Interact");
    }

    public void OnInteract()
    {
        _playerAnimator.OnOpenedChest();

        Debug.Log($"Interacted with {gameObject.name}");

        Toast.Instance.HideToast();

        _collectTween = transform.DOScale(0, .5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            transform.DOKill();
            Destroy(gameObject);
        });

    }

    void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }

    public void OnHoverOff()
    {
        Debug.Log("Interactor out!");
        anim?.SetBool(isOpenHash, false);

        Toast.Instance.HideToast();
    }
}
