using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTrolley : MonoBehaviour
{

	private Tool[]			m_AllTools	= new Tool[0];
	private TrolleySlot[]	m_AllSlots	= new TrolleySlot[0];
	
	public int TrolleySlotCount		= 6;
	public Vector2 TrolleyExtentsOS	= new Vector2(4.0f, 2.0f);
	public float ToolPickupDistance	= 1.0f;

	struct TrolleySlot
	{
		public Tool		OccupiedWith;
		public Vector2	PositionOS;
	}

	private static ToolTrolley s_Instance;

	///////////////////////////////////////////////////////////////////////////

	public static ToolTrolley Get()
	{
		if (!s_Instance)
		{
			s_Instance = GameObject.FindObjectOfType<ToolTrolley>();
			s_Instance.ReInit();
		}

		return s_Instance;
	}

	///////////////////////////////////////////////////////////////////////////

	public Tool GetNearestTool(Vector2 referencePos, bool forceWithinRange, Vector2? requireDirectionTowards)
	{
		Tool bestTool = null;
		float bestDist = float.MaxValue;

		for (int c = 0; c < m_AllSlots.Length; ++c)
		{
			TrolleySlot curSlot = m_AllSlots[c];

			if (!curSlot.OccupiedWith)
			{
				continue;
			}

			Vector2 slotPos = GetSlotPositionWS(curSlot.PositionOS).xz();

			float dist = (referencePos - slotPos).magnitude;

			if (forceWithinRange && dist > ToolPickupDistance)
			{
				continue;
			}

			if (dist > bestDist)
			{
				continue;
			}

			if (requireDirectionTowards.HasValue)
			{
				float dot = Vector2.Dot(slotPos - referencePos, requireDirectionTowards.Value);
				if (dot < 0)
				{
					continue;
				}
			}

			bestTool = curSlot.OccupiedWith;
			bestDist = dist;
		}

		return bestTool;
	}

	///////////////////////////////////////////////////////////////////////////

	void ReInit()
	{
		// 1) Get Tools
		Tool[] m_AllTools = GameObject.FindObjectsOfType<Tool>();

		// 2) Calculate Slot Positions
		if (TrolleySlotCount % 2 != 0)
		{
			TrolleySlotCount += 1;
		}

		BoxCollider boxCollider = GetComponent<BoxCollider>();
		if (!boxCollider)
		{
			boxCollider = GetComponentInChildren<BoxCollider>();

			if (!boxCollider)
			{
				Debug.LogWarning("ToolTrolley does not have a Box collider on self or child");
				return;
			}
		}
		
		Vector2 trolleyMin = Vector2.zero - TrolleyExtentsOS;

		m_AllSlots = new TrolleySlot[TrolleySlotCount];

		int slotCountX = TrolleySlotCount / 2;
		int slotCountY = 2;

		float cellSizeX = TrolleyExtentsOS.x * 2.0f / slotCountX;
		float cellSizeY = TrolleyExtentsOS.y * 2.0f / slotCountY;

		for (int xI = 0; xI < slotCountX; ++xI)
		{
			float cellMinX = trolleyMin.x + xI * cellSizeX;

			for (int yI = 0; yI < slotCountY; ++yI)
			{
				float cellMinY = trolleyMin.y + yI * cellSizeY;

				int slotIndex = yI * slotCountX + xI;

				m_AllSlots[slotIndex].PositionOS = new Vector2(cellMinX + cellSizeX * 0.5f, cellMinY + cellSizeY * 0.5f);
			}
		}

		// 3) Fill Slots
		for (int s = 0; s < m_AllSlots.Length; ++s)
		{
			if (s >= m_AllTools.Length)
			{
				m_AllSlots[s].OccupiedWith = null;
				continue;
			}

			m_AllSlots[s].OccupiedWith = m_AllTools[s];
		}
	}

	///////////////////////////////////////////////////////////////////////////

	public int? GetOccupiedSlotIfAny(Tool tool)
	{
		for (int i = 0; i < m_AllSlots.Length; ++i)
		{
			if (m_AllSlots[i].OccupiedWith == tool)
			{
				return i;
			}
		}

		return null;
	}

	///////////////////////////////////////////////////////////////////////////

	private void OnDrawGizmosSelected()
	{
		ToolTrolley.Get();

		for (int i = 0; i < TrolleySlotCount; ++i)
		{
			Vector2 slotPosOS = m_AllSlots[i].PositionOS;
			Vector3 positionWS = GetSlotPositionWS(slotPosOS);

			Gizmos.DrawWireSphere(positionWS, ToolPickupDistance);
		}
	}

	///////////////////////////////////////////////////////////////////////////

	Vector3 GetSlotPositionWS(Vector2 posOS)
	{
		Vector3 posWS = transform.TransformPoint(posOS.To3D(0.0f));

		return posWS;
	}

	///////////////////////////////////////////////////////////////////////////
}
