using System.Collections.Generic;
using UnityEngine;

// ExtensionMethods-Helper to allow for syntax vector3.xz();

public static class VectorExtensions
{
	///////////////////////////////////////////////////////////////////////////

	public static bool AlmostEquals(this float f1, float f2, float epsilon = 0.001f)
	{
		return Mathf.Abs(f1 - f2) < epsilon;
    }

	///////////////////////////////////////////////////////////////////////////

	public static bool AlmostEquals(this Vector2 v1, Vector2 v2, float epsilon = 0.001f)
	{
		return v1.x.AlmostEquals(v2.x, epsilon) && v1.y.AlmostEquals(v2.y, epsilon);
    }

	///////////////////////////////////////////////////////////////////////////
	
	public static void SnapTo(this float f1, float f2, float epsilon = 0.001f)
	{
		if (f1.AlmostEquals(f2))
		{
			f1 = f2;
		}
	}
	
	///////////////////////////////////////////////////////////////////////////

	public static float DistanceSquare(this Vector2 v1, Vector2 v2)
	{
		return Vector2.SqrMagnitude(v1 - v2);
    }

	///////////////////////////////////////////////////////////////////////////

	public static Vector2 xz(this Vector3 vec3D)
	{
		return new Vector2(vec3D.x, vec3D.z);
	}
	///////////////////////////////////////////////////////////////////////////

	public static Vector2 Abs(this Vector2 vec2)
	{
		return new Vector2(Mathf.Abs(vec2.x), Mathf.Abs(vec2.y));
	}

	///////////////////////////////////////////////////////////////////////////

	public static Vector2 GetSwapped(this Vector2 vecOld, bool doSwap)
	{
		return doSwap ? new Vector2(vecOld.y, vecOld.x) : vecOld;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Vector3 To3D(this Vector2 vec2D, float y)
    {
		return new Vector3(vec2D.x, y, vec2D.y);
	}

	///////////////////////////////////////////////////////////////////////////

	public static string AddBrackets(this string str)
	{
		return "[" + str + "]";
	}

	public static Vector2 GetExtents(this Rect rect)
	{
		return rect.size * 0.5f;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Rect GetRotatedAABB(this Rect aabb, float rotZ)
	{
		float sin = Mathf.Sin(rotZ * Mathf.Deg2Rad);
		float cos = Mathf.Cos(rotZ * Mathf.Deg2Rad);

		Vector2 p1 = aabb.min.GetRotated(sin, cos);
		Vector2 p2 = aabb.max.GetRotated(sin, cos);

		float minX, maxX, minY, maxY;

		if (p1.x < p2.x)	{	minX = p1.x;	maxX = p2.x; }
		else				{	minX = p2.x;	maxX = p1.x; }
		if (p1.y < p2.y)	{	minY = p1.y;	maxY = p2.y; }
		else				{	minY = p2.y;	maxY = p1.y; }

		Rect outRect = new Rect(minX, minY, maxX - minX, maxY - minY);
		return outRect;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Vector2 GetRotated(this Vector2 vec2, float sinAlpha, float cosAlpha)
	{
		return new Vector2(cosAlpha * vec2.x - sinAlpha * vec2.y, sinAlpha * vec2.x + cosAlpha * vec2.y);
	}

	public static Vector2 GetRotated(this Vector2 vec2, float alpha)
	{
		float sinAlpha = Mathf.Sin(alpha * Mathf.Deg2Rad);
		float cosAlpha = Mathf.Cos(alpha * Mathf.Deg2Rad);
		return new Vector2(cosAlpha * vec2.x - sinAlpha * vec2.y, sinAlpha * vec2.x + cosAlpha * vec2.y);
	}

	///////////////////////////////////////////////////////////////////////////

	public static string GetDebugInfo(this Vector3 vec3D)
	{
		return "(" + (int)vec3D.x + "|" + (int)vec3D.y + "|" + (int)vec3D.z + ")";
	}

	///////////////////////////////////////////////////////////////////////////

	public static string GetDebugInfo(this Vector2 vec2D)
	{
		return "(" + (int)vec2D.x + "|" + (int)vec2D.y + ")";
	}

	///////////////////////////////////////////////////////////////////////////

	public static GameObject FindImmediateChildOfName(this Transform parent, string name)
	{
		for (int i = 0; i < parent.childCount; ++i)
		{
			if (parent.GetChild(i).name == name)
			{
				return parent.GetChild(i).gameObject;
			}
		}

		return null;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Rect GetUnion(this Rect rect, Rect otherRect)
	{
		Rect outRect = new Rect();
		outRect.xMin = Mathf.Min(rect.xMin, otherRect.xMin);
		outRect.yMin = Mathf.Min(rect.yMin, otherRect.yMin);
		outRect.xMax = Mathf.Max(rect.xMax, otherRect.xMax);
		outRect.yMax = Mathf.Max(rect.yMax, otherRect.yMax);

		return outRect;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Vector2 GetPointWS(this EdgeCollider2D collider, int index)
	{
		Vector2 colliderObjectPosWS = collider.GetPosition2D();
		Vector2 pointOS				= collider.points[index];

		return colliderObjectPosWS + pointOS;
	}

	///////////////////////////////////////////////////////////////////////////

	public static Rect GetUnionSafe(this Rect rect, Rect otherRect)
	{
		if (rect.IsEmpty())
		{
			return otherRect;
		}

		if (otherRect.IsEmpty())
		{
			return rect;
		}

		return rect.GetUnion(otherRect);
	}

	///////////////////////////////////////////////////////////////////////////

	public static Color GetWithModifiedAlpha(this Color col, float a)
	{
		return new Color(col.r, col.g, col.b, a);
	}

	///////////////////////////////////////////////////////////////////////////

	public static Bounds ToBounds(this Rect rect)
	{
		return new Bounds(rect.min.To3D(0.0f), rect.max.To3D(0.0f));
	}

	///////////////////////////////////////////////////////////////////////////

	public static bool IsEmpty(this Rect rect)
	{
		return ((rect.width <= 0) || (rect.height <= 0));
	}

	///////////////////////////////////////////////////////////////////////////

	public static bool IsNullOrEmpty(this string value)
	{
		return string.IsNullOrEmpty(value);
	}

	///////////////////////////////////////////////////////////////////////////
	
	public static bool Equals_IgnoreCase(this string value, string other)
	{
		return value.Equals(other, System.StringComparison.InvariantCultureIgnoreCase);
	}

	///////////////////////////////////////////////////////////////////////////

	public static bool IsAsciiLetter(this char ch)
	{
		return ((ch >= 'A' && ch <='Z') || (ch >= 'a' && ch <='z'));
	}

	///////////////////////////////////////////////////////////////////////////

	public static V MakeSureKeyExists_UseNew<K,V>(this Dictionary<K,V> dictionary, K key) where V : class, new()
	{
		V value;

		bool foundKey = dictionary.TryGetValue(key, out value);

		if (foundKey)
		{
			return value;
		}

		V newV = new V();

		dictionary.Add(key, newV);

		return newV;
	}

	///////////////////////////////////////////////////////////////////////////

	public static void SwapAndPop_Unsafe<T>(this List<T> list, ref int indexToRemoveAndDecrement)
	{
		list[indexToRemoveAndDecrement] = list [list.Count - 1];
		list.RemoveAt(list.Count - 1);
		--indexToRemoveAndDecrement;
	}

	///////////////////////////////////////////////////////////////////////////

	public static T Back<T>(this T[] arr)
	{
		return arr[arr.Length -1];
	}

	///////////////////////////////////////////////////////////////////////////

	public static T Back<T>(this List<T> lst)
	{
		return lst[lst.Count - 1];
	}

	///////////////////////////////////////////////////////////////////////////

	public static bool ReverseDictionary<K,V>(this Dictionary<K,V> originalDic, out Dictionary<V,K> outReversedDic)
	{
		outReversedDic = new Dictionary<V, K>();
		bool success = true;

		foreach (var origPair in originalDic)
		{
			if (outReversedDic.ContainsKey(origPair.Value))
			{
				Debug.Assert(false, "Cannot reverse Dictionary. Value [" + origPair.Value + "] is set for multiple keys" +
					" [" + outReversedDic[origPair.Value] + "] and [" + origPair.Key + "] ." );
				success = false;
				continue;
			}

			outReversedDic[origPair.Value] = origPair.Key;
		}

		return success;
	}

	public static void SetPositionX(this Transform t, float posX)       { Vector3 pos = t.position; pos.x = posX; t.position = pos; }
    public static void SetPositionY(this Transform t, float posY)       { Vector3 pos = t.position; pos.y = posY; t.position = pos; }
    public static void SetPositionZ(this Transform t, float posZ)       { Vector3 pos = t.position; pos.z = posZ; t.position = pos; }
    public static void SetPositionXY(this Transform t, Vector2 posXY)   { Vector3 pos = posXY.To3D(t.position.z); t.position = pos; }

	public static Vector2 GetPosition2D(this Transform t)					{ return t.position.xz(); }
	public static Vector2 GetPosition2D<T>(this T t) where T : Behaviour	{ return t.transform.position.xz(); }
	public static Vector2 GetPosition2D(this GameObject g)					{ return g.transform.position.xz(); }

	///////////////////////////////////////////////////////////////////////////

}