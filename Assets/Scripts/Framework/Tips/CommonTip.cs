using UnityEngine;

///	<summary>
/// 文件作用: 通用信息提示
/// 实现功能:
/// 版本: v1.0
/// 作者: zhangshuiqing
/// </summary>

///	<summary>
///	Tip提示类型
/// </summary>
public enum TipType
{
	Marquee = 1,    //跑马灯
	PopTip,     //弹出Tip
	FightTip,
}
	
/// <summary>
/// CommonTip挂在UIRoot下面
/// </summary>
public class CommonTip : ISingleton<CommonTip>
{
	private readonly TipMgr mPopupTipMgr;
	private readonly MarqueeMgr mMarqueeTipMgr;

	public CommonTip()
	{
		mPopupTipMgr = new TipMgr(TipType.PopTip);
		mMarqueeTipMgr = new MarqueeMgr();
	}

	/// <summary>
	/// 公用显示Tip接口
	/// </summary>
	/// <param name="content">文本内容</param>
	/// <param name="type">提示类型</param>
	public void ShowTip(string content, TipType type = TipType.PopTip, Vector3 startPoint= default(Vector3))
	{
		switch (type)
		{
		case TipType.Marquee:
			mMarqueeTipMgr.ShowMarquee(content);
			break;
		case TipType.PopTip:
			mPopupTipMgr.MgrTipTyp = TipType.PopTip;
			mPopupTipMgr.StartShowTip(content, startPoint);
			break;
		case TipType.FightTip:
			mPopupTipMgr.MgrTipTyp = type;
			mPopupTipMgr.StartShowTip(content, startPoint);
			break;
		}
	}

	/// <summary>
	/// 清理缓存中的提示内容。未来的及显示的不再显示
	/// </summary>
	public void ClearTips()
	{
		mPopupTipMgr.ClearTips();
	}

	/// <summary>
	/// 战斗力发生改变
	/// </summary>
	public void FightTip(uint value)
	{
		string prefabStr = "FightTip";
		GameObject tipObj = Resources.Load("tips/" + prefabStr) as GameObject;
		if (null == tipObj)
			return;
		tipObj = GameObject.Instantiate(tipObj) as GameObject;
		tipObj.transform.SetParent(UIManager.Intance.TipRoot.transform);
		tipObj.transform.localPosition = new Vector3(0, -142, 0);//Vector3.zero;
		tipObj.transform.localScale = Vector3.one;
		FightTip pop = tipObj.GetComponent<FightTip>();
		pop.Init(value);
	}
}