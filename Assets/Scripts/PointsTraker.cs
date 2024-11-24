using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsTraker : MonoBehaviour
{
    
    public static PointsTraker Instance;
    
    public float points;

    public float time;
    
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
