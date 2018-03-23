using UnityEngine;
using System;

/// <summary>
/// 文件作用:
/// 实现功能： 带计时显示的tip
/// 版本:v1.0
/// 作者:zhangshuiqing
/// </summary>
public class TimeTipBase : TipBase {

	#region 计时显示下一个
	//显示下一条的时间
	protected float mShowNextInterval;
	protected float mStartTime;
	protected bool mShowNextCounterStart;
	protected Action mShowNextAction;
	#endregion 计时显示下一个

	public override void OnInit(Action showNextAction = null, float showInterval = 0)
	{
		mShowNextAction = showNextAction;
		mShowNextInterval = showInterval;
		mShowNextCounterStart = true;
		mStartTime = Time.realtimeSinceStartup;
	}

	public override void ShowTip(string tip, Callback onTweenOver)
	{
	}



	#region 计时显示下一个
	void Update()
	{
		if (!enabled || mShowNextAction == null || !mShowNextCounterStart)
			return;
		if (Time.realtimeSinceStartup - mStartTime > mShowNextInterval)
		{
			mShowNextCounterStart = false;
			if (null != mShowNextAction)
				mShowNextAction();
		}
	}
	#endregion 计时显示下一个
}