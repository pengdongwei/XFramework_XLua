using System;
using UnityEngine;
using UnityEngine.UI;
///	<summary>
/// 文件作用: Tip基类，用于扩展
/// 实现功能:
/// 版本: v1.0
/// 作者: zhangshuiqing
/// </summary>

public abstract class TipBase : MonoBehaviour {

	public Text txtTip;
	public Image backGround;
	public delegate void Callback(GameObject gobj);
	public Callback mOnTweenOver;

	void Awake()
	{
		OnInit();
	}

	protected void OnTweenOver()
	{
		if (null != mOnTweenOver)
			mOnTweenOver(gameObject);
	}

	public virtual void OnInit(Action showNextAction = null, float showInterval = 0f)
	{
	}
	public abstract void ShowTip(string tip, Callback onTweenOver);
}