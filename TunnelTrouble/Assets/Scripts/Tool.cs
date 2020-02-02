using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Hammer,
    Drill,
    Mop,
    Megaphone,
}

public class Tool : MonoBehaviour
{
    public ToolType     _ToolType;
    public AudioClip[]  RepairSound;
    public float        RepairSoundVolume  = 1.0f;
    
    bool IsOnTrolley()
    {
        int? slot = ToolTrolley.Get().GetOccupiedSlotIfAny(this);

        return slot.HasValue;
    }
}
