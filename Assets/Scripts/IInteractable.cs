using UnityEngine;

public interface IInteractable
{
    public void OnHoverIn()
    {

    }

    public void OnInteract()
    {
        Debug.Log("Interacted!");
    }

    public void OnHoverOff()
    {

    }
}
