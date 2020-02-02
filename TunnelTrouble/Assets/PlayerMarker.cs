using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarker : MonoBehaviour
{
	public PlayerController pc;
	public GameObject ring;

	private Material mat;
	private PlayerSlot slot;

    void Start()
    {
		mat = ring.GetComponent<Renderer>().material;
		slot = pc.Slot;
		mat.SetColor("_EmissionColor", PlayerController.GetPlayerColor(slot));
    }
}
