using System;
using UnityEngine;
using DG.Tweening;
///	<summary>
/// 文件作用: 弹出类型Tip
/// 实现功能:
/// 版本: v1.0
/// 作者: zhangshuiqing
/// </summary>

public class PopTip : TimeTipBase {

	/// <summary>
	/// 位移的目标位置y坐标
	/// </summary>
	float y;
	[HideInInspector]
	public Vector3 m_startPoint = new Vector3(0, -200, 0);

	RectTransform rt;
	[HideInInspector]
	public float flyHigh = 100f;

	public override void OnInit(Action showNextAction = null, float showInterval = 0f)
	{

		rt = GetComponent<RectTransform> ();
		base.OnInit(showNextAction, showInterval);
		transform.localScale = Vector3.one;
		rt.anchoredPosition3D = m_startPoint;
		y = rt.anchoredPosition3D.y + flyHigh;
	}

	public override void ShowTip(string tip, Callback onTweenOver)
	{
		mOnTweenOver = onTweenOver;
		txtTip.text = tip;
		backGround.rectTransform.sizeDelta = new Vector2(txtTip.fontSize * (Util.CalculateStringLength(tip) + 2), backGround.rectTransform.sizeDelta.y);

		rt.DOLocalMoveY (y, flyHigh/200).SetEase (Ease.OutCirc).OnComplete(OnTweenOver);
	}

}