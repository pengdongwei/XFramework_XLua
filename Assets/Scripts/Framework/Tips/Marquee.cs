using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///	<summary>
/// 文件作用: 跑马灯类型Tip
/// </summary>

public class Marquee : MonoBehaviour
{
	public Text txtTip;
	public Image backGround;

	public bool IsShowing { get; private set; }

	public float WordsPerSec = 5f;
	public float MarqueePosY = 200f;
	public float WordsPosY = 0f;

	private Transform mMoveTrans;
	private RectTransform mTxtRectTransform;
	private MarqueeMgr mMarqueeMgr;
	private float mBackGourndWid;

	public void InitMarquee(MarqueeMgr marquee)
	{
		mMarqueeMgr = marquee;
		IsShowing = false;
		mIsStarShowing = false;
		transform.localPosition = new Vector2(0, MarqueePosY);
		mBackGourndWid = backGround.rectTransform.sizeDelta.x;
		mTxtRectTransform = txtTip.rectTransform;
		mMoveTrans = txtTip.transform;
	}

	public void StartMarquee()
	{
		IsShowing = true;
		string nextContent = mMarqueeMgr.MoveToNextMarqueeContent();
		ShowTip(nextContent);
	}

	#region marqueeAnim
	private void ShowTip(string tip)
	{
		txtTip.text = tip;

		int fontSize = txtTip.fontSize;
		float txtWidSize = txtTip.preferredWidth;
		mTxtRectTransform.sizeDelta = new Vector2(txtWidSize, mTxtRectTransform.sizeDelta.y);
		float txtWid = txtWidSize + mBackGourndWid;
		mTotalTime = txtWid / (WordsPerSec * fontSize);
		mStartPosX = txtWid / 2f;
		mEndPosX = -mStartPosX;
		mMoveTrans.localPosition = new Vector3(mStartPosX, WordsPosY, 0);
		mShowTime = 0;
		mIsStarShowing = true;
	}

	protected bool mIsStarShowing;
	protected float mShowTime;
	public float mTotalTime;
	protected float mStartPosX;
	protected float mEndPosX;

	void FixedUpdate()
	{
		if (!mIsStarShowing)
			return;
		mShowTime += Time.fixedDeltaTime;
		float freq = Mathf.Clamp01(mShowTime / mTotalTime);
		if (freq >= 1)
		{
			mIsStarShowing = false;
			OnEndMarquee();
		}
		else
		{
			float curLocalPosX = Mathf.Lerp(mStartPosX, mEndPosX, freq);
			mMoveTrans.localPosition = new Vector3(curLocalPosX, WordsPosY, 0);
		}
	}

	#endregion marqueeAnim

	private void OnEndMarquee()
	{
		string nextContent = mMarqueeMgr.MoveToNextMarqueeContent();
		if (string.IsNullOrEmpty(nextContent))
		{
			OnMarqueeOver();
			return;
		}
		ShowTip(nextContent);
	}

	private void OnMarqueeOver()
	{
		IsShowing = false;
		mMarqueeMgr.DestroyMarquee();
	}
}


/// <summary>
/// 跑马灯管理
/// </summary>
public class MarqueeMgr
{
	protected bool mHasCreatedMarquee;
	protected Marquee mMarqueeSc;
	protected List<string> mContentList;

	public MarqueeMgr()
	{
		mHasCreatedMarquee = false;
		mMarqueeSc = null;
		mContentList = new List<string>();
	}

	public void ShowMarquee(string content)
	{
		if (!mHasCreatedMarquee)
		{
			mMarqueeSc = CreateMarquee(content);
			mMarqueeSc.InitMarquee(this);
			mHasCreatedMarquee = true;
		}
		mContentList.Add(content);
		if (!mMarqueeSc.IsShowing)
		{
			mMarqueeSc.StartMarquee();
		}
	}

	public string MoveToNextMarqueeContent()
	{
		if (mContentList.Count == 0)
			return string.Empty;
		string nextContent = mContentList[0];
		mContentList.RemoveAt(0);
		return nextContent;
	}

	private Marquee CreateMarquee(string content)
	{
		string prefabStr = "Marquee";
		GameObject tipObj = Resources.Load("tips/" + prefabStr) as GameObject;
		if (null == tipObj)
		{
			Debug.LogError("Load Marquee res failed!");
			return null;
		}
		tipObj = GameObject.Instantiate(tipObj) as GameObject;
		tipObj.transform.SetParent(UIManager.Intance.TipRoot.transform);
		tipObj.transform.localScale = Vector3.one;
		return tipObj.GetComponent<Marquee>();
	}

	/// <summary>
	/// 跑马灯结束（本次所有文本显示结束）
	/// </summary>
	public void DestroyMarquee()
	{
		GameObject.Destroy(mMarqueeSc);
		GameObject.Destroy(mMarqueeSc.gameObject);
		mMarqueeSc = null;
		mHasCreatedMarquee = false;
		mContentList.Clear();
	}
}