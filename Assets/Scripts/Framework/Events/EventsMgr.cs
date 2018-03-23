/// <summary>
/// 全局的事件系统
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class EventsMgr: ISingleton<EventsMgr> {

	//公共委托事件
	public delegate void CommonEvent(object data);

	//事件列表
	public Dictionary<string, CommonEvent> m_dicEvents = new Dictionary<string, CommonEvent>();

	/// <summary>
	/// 事件触发
	/// </summary>
	public void TriigerEvent(string strEventKey, object param)
	{
		if (m_dicEvents.ContainsKey(strEventKey))
		{
			m_dicEvents[strEventKey](param);
		}

	}

	/// <summary>
	/// 事件绑定
	/// </summary>
	public void AttachEvent(string key, CommonEvent attachEvent)
	{
		if (m_dicEvents.ContainsKey(key))
		{
			m_dicEvents[key] += attachEvent;
		}
		else
		{
			m_dicEvents.Add(key, attachEvent);
		}
	}


	/// <summary>
	/// 去除事件绑定
	/// </summary>
	public void DetachEvent(string strEventKey, CommonEvent attachEvent)
	{
		if (m_dicEvents.ContainsKey(strEventKey))
		{
			System.Delegate[] lst = m_dicEvents[strEventKey].GetInvocationList();
			foreach (System.Delegate d in lst)
			{
				if(attachEvent == d)
				{
					m_dicEvents[strEventKey] -= attachEvent;
				}
			}
			//当前key没有监听对象时，移除当前key.
			if (m_dicEvents[strEventKey] == null)
			{
				m_dicEvents.Remove(strEventKey);
			}
		}
		else
		{
			Debug.LogWarning("没有这个定义的事件:" + strEventKey);
		}
	}

	public override string ToString ()
	{
		string value = null;
		foreach(var v in m_dicEvents){
			value+=v.Key+ " ; ";
		}
		string log =  string.Format ("存在的event [EventsMgr] {0}", value);
		return log;
	}

}