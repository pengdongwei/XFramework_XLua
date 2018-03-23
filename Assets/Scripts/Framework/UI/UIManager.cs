/// <summary>
/// 窗口管理器
/// 支持功能： 动态改变缓存大小＋链状显示窗口＋动态更新alpha值 + 显示窗口，并自动关闭窗口 + 窗口msgbox + 动态配置bar + 预制窗口动画
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject root;
	[HideInInspector]
	public GameObject uiRoot;
	[HideInInspector]
	private GameObject wndRoot;
	[HideInInspector]
	private GameObject msgBoxRoot;
	[HideInInspector]
	public GameObject TipRoot;

	[HideInInspector]
	public GameObject EffectRoot;
	[HideInInspector]
	public UIMessageBox msgBox;

	private static UIManager instance;

	/// <summary>
	/// 缓存的窗口列表 
	/// </summary>
	private const int MAX_CACH_WND =6;
	//窗口 id  匹配 ui 预制体
	private Dictionary<WindowID, string> wndResDict = new Dictionary<WindowID, string>(); 

	private Dictionary<WindowID, UIWndBase> cacheWndDict = new Dictionary<WindowID, UIWndBase>(); 
	/// <summary>
	/// 窗口的返回队列 , 只有追加型的窗口会进入栈，一次只会存在一个栈
	/// </summary>
	private Stack<UIWndBase> backStack = new Stack<UIWndBase> ();
	/// <summary>
	/// 当前显示的窗口 
	/// </summary>
	private UIWndBase curShowWnd;
	public UIWndBase CurShowWnd{
		get{ 
			return curShowWnd;		
		}
	}

	public static UIManager Intance{
		get{ 
			if (instance == null) {
				GameObject obj = new GameObject ("UIRoot");

				instance = obj.AddComponent<UIManager> ();
			}
			return instance;
		}
	}

	void Awake(){
		instance = this;
		wndRoot = new GameObject ("WndRoot");
		msgBoxRoot = new GameObject("MsgBoxRoot");
		TipRoot = new GameObject("TipRoot");
		EffectRoot = new GameObject("EffectRoot");

		UnityTools.AddChildToTarget (root.transform, wndRoot.transform);
		UnityTools.AddChildToTarget (root.transform, msgBoxRoot.transform);
		UnityTools.AddChildToTarget (root.transform, EffectRoot.transform);
		UnityTools.AddChildToTarget (root.transform, TipRoot.transform);;
		//初始化窗口预制体资源
		foreach (WindowID id in Enum.GetValues(typeof(WindowID))) {
			//	Debug.Log (id);
			wndResDict.Add(id, AppConst.ui_prefix + id.ToString());
		}
	}
		
	/// <summary>
	/// 向指定窗口发送消息
	/// </summary>
	public void SendMsg(WindowID wndId, WindowMsgID msgId, object param){
		if (cacheWndDict.ContainsKey (wndId)) {
			UIWndBase wnd = cacheWndDict [wndId];
			wnd.OnMsg (msgId, param);
		} else {
			Debug.Log ("窗口不在缓存中, 事件更新失败");
		}
	}

	/// <summary>
	/// 消息窗口的显示，目前支持3种类型
	/// </summary>
	public void ShowMessageBox(string msg, MessageBoxType type, MsgBoxCallBack callback=null){
			string wndPath = wndResDict [WindowID.Msgbox];
			GameObject prefab =	Resources.Load<GameObject> ("UIPrefab/Msgbox");
			if (prefab != null) {
				GameObject clone = (GameObject)GameObject.Instantiate (prefab);
				UnityTools.AddChildToTarget (msgBoxRoot.transform, clone.transform);
				UIMessageBox msgBox = clone.GetComponent<UIMessageBox> ();
			}	
	}

	/// <summary>
	/// 窗口显示，队列型加载
	/// </summary>
	public void ShowWindow(WindowID windowId, bool bAppend = false){
		if (wndResDict.ContainsKey (windowId)) {
			if (cacheWndDict.ContainsKey (windowId)) {
				UIWndBase wnd = cacheWndDict [windowId];
				if (wnd.status == WindowStatus.Inactive) {
					if (bAppend) {
						if (backStack.Count == 0) {
							backStack.Push (curShowWnd);
						}
						backStack.Push (wnd);
						AdjustAlpha ();
					} else {
						HideAllCach ();
						BreakBackStack ();
						curShowWnd.Close ();
					}
					wnd.Show ();
					curShowWnd = wnd;
				} else if (wnd.status == WindowStatus.Gray) {
					curShowWnd.Close ();
					curShowWnd = wnd;
					backStack.Pop ();
					AdjustAlpha ();
				} else if (wnd.status == WindowStatus.Active) {
					Debug.Log ("当前窗口已经是显示窗口");
				}
				wnd.Refresh ();
			} else {
				RealShow(windowId, bAppend);
			}	
		} else {
			Debug.Log (windowId.ToString()+" 不存在资源");
		}
	}

	private void RealShow(WindowID windowId, bool bAppend){
		string wndPath = wndResDict [windowId];
		GameObject prefab = null;
		if (GameManager.Instance.ResFrom!= ResourceFrom.Bundle) {
			prefab = ResourceManager.LoadResource ("UIPrefab/"+windowId.ToString()) as GameObject;
		} else {
			prefab = ResourceManager.GetInstance().LoadAsset(wndPath, windowId.ToString());
		}

		if (prefab != null) {
			GameObject clone = (GameObject)GameObject.Instantiate (prefab);
			UnityTools.AddChildToTarget (wndRoot.transform, clone.transform);
			UIWndBase wnd = clone.GetComponent<UIWndBase> ();
			wnd.status=WindowStatus.Active;
			if (curShowWnd != null) {
				wnd.preWndID = curShowWnd.wndID;			
			}
			wnd.wndID = windowId;
			if (bAppend) {
				if(backStack.Count==0){
					backStack.Push (curShowWnd);
				}
				curShowWnd = wnd;
				backStack.Push (curShowWnd);
				AdjustAlpha ();
			} else {
				curShowWnd = wnd;
				HideAllCach ();
				BreakBackStack ();
			}
			cacheWndDict.Add (windowId, curShowWnd);
			curShowWnd.AdjustAlpha (1.0f);
			CheckCach ();
			wnd.Refresh ();
		}
	} 

	/// <summary>
	/// 调整窗口alpha值 
	/// </summary>
	private void AdjustAlpha(){
		float tinyAlpha = (float)((float)1/ (float)backStack.Count);
		int index = 0;
		foreach(UIWndBase wnd in backStack){
			float alpha =(float) ((backStack.Count-index) *  (float)tinyAlpha);
			alpha = float.Parse(Mathf.Clamp (alpha, 0.2f, 1f).ToString ("0.0"));
			wnd.AdjustAlpha (alpha);
			index++;
		}
	}

	/// <summary>
	/// 检查缓存区~如果超过最大上限值，则进行清理，如果链存在不进行清理，一般情况下，链的长度不会超过缓存大小
	/// </summary>
	private void CheckCach(){
		if (cacheWndDict.Count < MAX_CACH_WND) {
			return;
		} else {
			int minDepth = MAX_CACH_WND;
			UIWndBase wnd = null;
			foreach(KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict){
				if (pair.Value.Depth < minDepth && !backStack.Contains(pair.Value)) {
					minDepth = pair.Value.Depth;
					wnd = pair.Value;
				}
			}
			if (wnd!=null) {
				Destroy (wnd.gameObject);
				cacheWndDict.Remove (wnd.wndID);
			}
		}
	}

	public bool Exist(WindowID id){
		foreach(var v in cacheWndDict){
			if (id == v.Value.wndID) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 因此其他缓存窗口
	/// </summary>
	private void HideAllCach(){
		foreach(KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict){ 
			if (pair.Value == curShowWnd) {
				pair.Value.Show ();
			} else {
				pair.Value.Close ();
			}
		}
	}

	/// <summary>
	/// 处理断链 
	/// </summary>
	private void BreakBackStack(){
		backStack.Clear ();
	}

	/// <summary>
	/// 清除缓存窗口
	/// </summary>
	public void ClearCachExcp(WindowID windowID)
	{
		List<WindowID> deleteList = new List<WindowID>();
		foreach (KeyValuePair<WindowID, UIWndBase> pair in cacheWndDict)
		{
			if (windowID != pair.Key)
			{
				Destroy(pair.Value.gameObject);
				deleteList.Add(pair.Key);
			}
		}

		foreach (WindowID wnd in deleteList)
		{
			cacheWndDict.Remove(wnd);
		}
		deleteList.Clear();
	}

	public void ClearMessageBoxs(){

	}
}