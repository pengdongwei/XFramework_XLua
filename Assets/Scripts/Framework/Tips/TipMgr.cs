using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 文件作用:
/// 实现功能： tip管理类
/// 版本:v1.0
/// 作者:zhangshuiqing
/// </summary>
public class TipMgr {

	public TipType MgrTipTyp { get;  set; }
	private List<string> mTipDataList;
	private readonly int MaxTipCacheSize = 1;   //每次最大可(同时)弹的个数
	private readonly float TipShowNextTime = 0.8f;
	private int mCurTipTipCount = 0;

	public TipMgr(TipType tipType)
	{
		MgrTipTyp = tipType;
		mTipDataList = new List<string>();
	}

	public void StartShowTip(string content, Vector3 startpoint = default(Vector3))
	{
		if (mCurTipTipCount >= MaxTipCacheSize)
		{
			mTipDataList.Add(content);
		}
		else
		{
			RealShowTipObj(content, startpoint);
		}
	}

	private void RealShowTipObj(string content, Vector3 startpoint = default(Vector3))
	{
		++mCurTipTipCount;
		if (MgrTipTyp == TipType.Marquee)
		{
			//CreateMarquee(content);
		}
		else if (MgrTipTyp == TipType.PopTip)
		{
			CreatePopup(content, startpoint);
		}
		else if (MgrTipTyp == TipType.FightTip)
		{
			CreateFightPopup(content, startpoint);
		}
		else
		{
			Debug.LogError("no such common-tip type!!");
		}
	}

	private void CreateFightPopup(string content, Vector3 startpoint = default(Vector3))
	{
		string prefabStr = "FightTip";
		GameObject tipObj = Resources.Load("tips/" + prefabStr) as GameObject;
		if (null == tipObj)
			return;
		tipObj = GameObject.Instantiate(tipObj) as GameObject;
		UnityTools.AddChildToTarget (UIManager.Intance.TipRoot.transform, tipObj.transform);
		PopTip pop = tipObj.GetComponent<PopTip>();
		pop.m_startPoint = startpoint;
		pop.OnInit(ShowNextTip, TipShowNextTime);
		pop.ShowTip(content, OnTipOver);
	}

	#region create-tip
	private void CreatePopup(string content, Vector3 startpoint = default(Vector3))
	{
		string prefabStr = "PopTip";
		GameObject tipObj = Resources.Load("tips/" + prefabStr) as GameObject;
		if (null == tipObj)
			return;
		tipObj = GameObject.Instantiate(tipObj) as GameObject;
		UnityTools.AddChildToTarget (UIManager.Intance.TipRoot.transform, tipObj.transform);
		PopTip pop = tipObj.GetComponent<PopTip>();
		pop.m_startPoint = startpoint+new Vector3(0, 200, 0);
		pop.flyHigh = 250;
		pop.OnInit(ShowNextTip, TipShowNextTime);
		pop.ShowTip(content, OnTipOver);
	}
	#endregion create-tip

	private void ShowNextTip()
	{
		if (mCurTipTipCount > MaxTipCacheSize)
			return;
		if (mTipDataList.Count == 0)
			return;
		string nextPopStr = mTipDataList[0];
		mTipDataList.RemoveAt(0);
		RealShowTipObj(nextPopStr);
	}

	private void OnTipOver(GameObject gobj)
	{
		--mCurTipTipCount;
		mCurTipTipCount = mCurTipTipCount < 0 ? 0 : mCurTipTipCount;
		GameObject.DestroyImmediate(gobj);
		ShowNextTip();
	}

	public void ClearTips()
	{
		mTipDataList.Clear();
	}
}