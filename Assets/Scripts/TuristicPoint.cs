using UnityEngine;

public class TuristicPoint : MonoBehaviour
{
    public bool IsDestination { get; private set; }

    public void OnDisable()
    {
        IsDestination = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        IsDestination = true;
    }
}