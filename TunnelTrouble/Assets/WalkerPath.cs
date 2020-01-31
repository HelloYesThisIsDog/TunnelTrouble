using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerPath : MonoBehaviour
{
    private static WalkerPath s_Instance;
    
    public Transform[] PathPoints;

    public static WalkerPath Get()
    {
        if (s_Instance)
        {
            return s_Instance;
        }
        else
        {
            return GameObject.FindObjectOfType<WalkerPath>();
        }
    }

    void Awake()
    {
        s_Instance = this;

        PathPoints = new Transform[transform.childCount];

        for (int c = 0; c < transform.childCount; ++c)
        {
            PathPoints[c] = transform.GetChild(c);
        }
    }

}
