using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathLine
{
    public Vector3 Left;
    public Vector3 Right;

    public Vector3 GetLerped(float f) { return Vector3.Lerp(Left, Right, f); }
}

public class WalkerPath : MonoBehaviour
{
    private static WalkerPath s_Instance;
    
    public PathLine[] PathLines = new PathLine[0];

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

        PathLines = new PathLine[transform.childCount / 2];

        for (int c = 0; c < transform.childCount; ++c)
        {
            if (c % 2 == 0)
            {
                PathLines[c/2].Left = transform.GetChild(c).transform.position;
            }
            else
            {
                PathLines[c/2].Right = transform.GetChild(c).transform.position;
            }
        }
    }

}
