using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerPath : MonoBehaviour
{
    public static WalkerPath s_WalkerPath;
    
    private Transform[] m_PathPoints;

    void Awake()
    {
        s_WalkerPath = this;

        m_PathPoints = new Transform[transform.childCount];

        for (int c = 0; c < transform.childCount; ++c)
        {
            m_PathPoints[c] = transform.GetChild(c);
        }
    }

}
