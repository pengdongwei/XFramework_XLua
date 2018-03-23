using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTools{
	public static void AddChildToTarget(Transform target, Transform child){
		child.SetParent (target);
		Reset (child.gameObject);
		ChangeChileLayer (child, target.gameObject.layer);
	}
	public static void ChangeChileLayer(Transform t, int layer){
		t.gameObject.layer = layer;
		for (int i = 0; i > t.childCount; ++i) {
			Transform child = t.GetChild (i);
			child.gameObject.layer = layer;
			ChangeChileLayer (child, layer);
		}
	}
		
	//归一化
	public static void Reset(GameObject go)
	{
		go.transform.localPosition = Vector3.zero;
		go.transform.localEulerAngles = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	public static void ClearChild(Transform go)
	{
		if (go == null) return;
		for (int i = go.childCount - 1; i >= 0; i--)
		{
			//此处需使用DestroyImmediate，使用Destroy可能会留下缓存
			GameObject.DestroyImmediate(go.GetChild(i).gameObject);
		}
	}


	/// <summary>
	/// 搜索子物体组件-GameObject版
	/// </summary>
	public static T Get<T>(GameObject go, string subnode) where T : Component
	{
		if (go != null)
		{
			Transform sub = go.transform.Find(subnode);
			if (sub != null) return sub.GetComponent<T>();
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Transform版
	/// </summary>
	public static T Get<T>(Transform go, string subnode) where T : Component
	{
		if (go != null)
		{
			Transform sub = go.Find(subnode);
			if (sub != null) return sub.GetComponent<T>();
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Component版
	/// </summary>
	public static T Get<T>(Component go, string subnode) where T : Component
	{
		return go.transform.Find(subnode).GetComponent<T>();
	}

	//添加管理器
	public static T AddManager<T>() where T : UnityEngine.Component
	{
		T com = GameManager.Instance.gameObject.GetComponent<T>();
		if (com == null)
		{
			com = GameManager.Instance.gameObject.AddComponent<T>();
		}
		return com;
	}

	/// <summary>
	/// 添加脚本
	/// </summary>
	public static T TryAddComponent<T>(GameObject go) where T : Component
	{
		if (go != null)
		{
			T t = go.GetComponent<T>();
			if (t == null)
			{
				t = go.AddComponent<T>();
			}
			return t;
		}
		return null;
	}

	public static void TryRemove<T>(GameObject go) where T:Component{
		if (go != null) {
			T t = go.GetComponent<T> ();
			if (t != null) {
				GameObject.Destroy (t);
			}
		}
	}


	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T>(GameObject go) where T : Component
	{
		if (go != null)
		{
			T[] ts = go.GetComponents<T>();
			for (int i = 0; i < ts.Length; i++)
			{
				if (ts[i] != null) GameObject.Destroy(ts[i]);
			}
			return go.gameObject.AddComponent<T>();
		}
		return null;
	}

	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T>(Transform go) where T : Component
	{
		return Add<T>(go.gameObject);
	}

	/// <summary>
	/// 递归遍历子节点
	/// </summary>
	public static Transform ChildRecursive(Transform trans, string subnode)
	{
		for (int i = 0; i < trans.childCount; i++)
		{
			Transform childTrans = trans.GetChild(i);
			if (!childTrans.name.Equals(subnode))
			{
				var child = ChildRecursive(childTrans, subnode);
				if (child != null)
				{
					return child;
				}
			}
			else
			{
				return childTrans;
			}
		}
		return null;
	}


	public static void LogMust(string str)
	{
		Debug.Log(str);
	}

	public static void Log(string str)
	{
		if(GameManager.Instance.bOpenLog)
			Debug.Log(str);
	}



}
