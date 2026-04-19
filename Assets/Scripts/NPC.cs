using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    protected virtual void Move()
    {
        Debug.Log("I am moving");
    }

    void Interact()
    {
        Debug.Log("I am interacting");
    }

    protected virtual void Damage()
    {
        Debug.Log("Oh oh");
    }

    protected virtual void Damage(float damageValue)
    {
        
    }

    protected virtual float Damage(float multiplier, string name)
    {
        float x = 0f;
        return x;
    }
}
