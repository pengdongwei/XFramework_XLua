using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Net;

public enum ResourceFrom{
	Origin, //开发模式使用
	OriginExpData, //只热更data数据
	Bundle,
}

public enum Platform{
	Android,
	IOS,
	Windows
}

public class GameManager : MonoBehaviour
{

	public static GameManager Instance;

	[Header("资源形式：1:Origin表示Resources下读取，用于开发阶段 2:Bundle表示使用Bundle资源，解压和更新依赖次模式")]
	public ResourceFrom ResFrom;
	[Header("平台")]
	public Platform plat;
	[HideInInspector]
	public bool bPlayCg;
	[Header("解压（zip的开启必须依赖于解压）")]
	public bool bOpenExtract = false; //是否开启解压功能
	public bool bClearOldExtract = false;//是否开启重复解压
	public bool bOpenExtract_Zip = true; //是否开启Zip解压功能【上线在用就行】
	[Header("更新 + 延迟更新玩家等级（绕开国外审核）")]
	public bool bOpenUpdate = false; //是否开启解压功能
	public int delayRoleLevel= 0; //延迟更新角色等级
	[Header("恢复老版本砖石(发布)?")]
	public bool IsRecovryDiamond = false;
	[Header("引导")]
	public bool bOpenGuide = false; //是否开启引导功能
	public bool bClearGuide = false;//是否清除引导
	public int guideCount = 5; //引导的段数
	[Header("FPS")]
	public bool bOpenFPS = false; //是否开启引导功能
	[HideInInspector]
	public FPS FPSCounter;
	[Header("GM")]
	public bool bOpenGM = false; //是否开启引导功能
	[Header("剧情")]
	public bool bOpenStore = false; //是否开启剧情功能
	public bool bClearStore = false; //是否开启剧情功能
	[Header("清除黑名单")]
	public bool bClearBlack = false;

	[Header("开始调试模式")]
	public bool bOpenLog = true;
	[Header("是否是给发行的包（3天有效期）")]
	public bool IsFaXing = false;

	public CanvasScaler scaler;


	void Awake(){
		IsInitgame = false;
		float screen_rate = (float)Screen.width / (float)Screen.height;
		if (screen_rate < 0.5) { //iphonex 的比例小于 0.5
			scaler.matchWidthOrHeight = 0;
		} else {
			scaler.matchWidthOrHeight = 1;
		}
		Instance = this;
	}

	/// <summary>
	/// 初始化游戏管理器
	/// </summary>
	void Start(){
		bPlayCg = ClientSave.GetBool (PrefsInfo.IsCloseCg);
		if (!bPlayCg) {
			UnityTools.TryAddComponent<CGManager> (gameObject).PlayCGByName ("cg.mov", null, EndCg);
		} else {
			EndCg ();
		}
		//MonitorOthers ();
	}


	/// <summary>
	/// cg 播放完后处理其他逻辑
	/// </summary>
	void EndCg(){
		DontDestroyOnLoad(gameObject);  //防止销毁自己
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Application.targetFrameRate = AppConst.GameFrameRate;

		HandleSetting ();
	}

	/// <summary>
	/// 处理客户端的设置
	/// </summary>
	void HandleSetting()
	{
		if(bOpenFPS){
			FPSCounter = UnityTools.TryAddComponent<FPS> (gameObject);
		}
		if (bClearStore) {
			//StoryManager.Instance ().ClearStory ();
		}
		if (bOpenExtract) {
			EventsMgr.Instance ().AttachEvent ("EndExtract", EndExtract);
			UnityTools.TryAddComponent<ResExtract> (gameObject);
			//UnityTools.LogMust (PathTools.DataPath);
		} else {
			EndExtract (null);
		}
	}

	/// <summary>
	/// 结束解压
	/// </summary>
	void EndExtract(object msg){

		EventsMgr.Instance ().DetachEvent ("EndExtract", EndExtract);
		DicDataManager.GetInstance ().PreLoadCsvData (); //预加载全局表，为更新做准备
		if (bOpenUpdate) {
			EventsMgr.Instance ().AttachEvent ("EndUpdate", EndUpdate);
			UnityTools.TryAddComponent<ResUpdate> (gameObject);
		} else {
			EndUpdate (null);
		}
	}

	bool IsInitgame =false;
	/// <summary>
	/// 结束更新
	/// </summary>
	void EndUpdate(object msg){
		EventsMgr.Instance ().DetachEvent ("EndUpdate", EndUpdate);
		ResExtract compExtract= GetComponent<ResExtract> ();
		if (compExtract != null) {
			Destroy (compExtract);
		}
		ResUpdate compUpdate = GetComponent<ResUpdate> ();
		if (compUpdate != null) {
			Destroy (compUpdate);
		}
		if (!IsInitgame) {
			IsInitgame = true;
			InitGame();
		}
	}

	/// <summary>
	/// 结束引导
	/// </summary>
	public void StartGuide(int index){
		/*
		if(Util.IsOpenGuide()){
			if (!Util.IsGuided (index)) {
				if (index == 1) {
					Save.Instance ().DropCard (GameConfig.firstHeroId);
				}
				GuideManager guide = UnityTools.TryAddComponent<GuideManager>(UIManager.Intance.uiRoot);
				guide.Init(index);
			}
		}*/
	}

	/// <summary>
	/// 结束引导
	/// </summary>
	public void EndGuide(int guideIdx){
		/*
		GuideManager mgr = GetComponent<GuideManager> ();
		if (mgr != null) {
			Destroy (mgr);
		}
		Util.EndGuide (guideIdx);*/
	}

	/// <summary>
	/// 资源初始化结束, 等待进入游戏
	/// </summary>
	public void InitGame()
	{
		DicDataManager.GetInstance().LoadAllCsvData();
		UIManager.Intance.ShowWindow (WindowID.Login);
	}

	/// 析构函数
	/// </summary>
	void OnDestroy()
	{
		UnityTools.LogMust("~GameManager was destroyed");
	}

	public Action CallBackLevelLoaded = null;
	public void OnLevelWasLoaded(int levelId)
	{
		ResourceManager.GetInstance().UnLoadAllBundle();
		if (levelId == -1)
		{
			UIManager.Intance.ShowWindow(WindowID.MainCity);
		}
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}