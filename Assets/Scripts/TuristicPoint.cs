using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuristicPoint : MonoBehaviour
{
    public bool IsDestination { get; private set;} = false;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        IsDestination = true;
    }

    public void OnDisable()
    {
        IsDestination = false;
    }
}
