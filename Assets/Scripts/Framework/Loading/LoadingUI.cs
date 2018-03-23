/// <summary>
/// Loading 场景
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.IO;

public class LoadingUI : UIWndBase
{
	public Text textProgress;
	public Text tips;
	private string url;
	private WWW www;
	private float progress;
	public Image loadingBar;
	//场景所占的进度比例
	public float MaxScenePercent = 0.8f;

	//延后关闭loading界面时间
	public float DelayLodingTime = 5f;
	private float oneFrameDelayNum = 0;

	public void Awake ()
	{

		loadingBar.fillAmount = 0;
		textProgress.text = (loadingBar.fillAmount * 100).ToString("0.0") + "%";

		UIManager.Intance.ClearCachExcp(WindowID.LoadingUI);
		UIManager.Intance.ClearMessageBoxs();
		url = PathTools.DataPath + "/scene/" + SceneMapManager.Instance().nextSceneName + ".unity3d";
		url = new System.Uri(url).AbsoluteUri;
		//	Debug.Log ("loading scene path = " + url);
		tips.text = SceneMapManager.Instance ().loadingTips;


		MaxScenePercent = float.Parse(Random.Range(0.2f, 0.4f).ToString("0.0"));

		oneFrameDelayNum = 0.7f / DelayLodingTime * 0.02f;
		StartCoroutine(DownLoad());
	}


	void Update()
	{
		if (MaxScenePercent != 1f) {
			HandleSceneProgress ();
		}
	}


	void HandleSceneProgress()
	{
		if (www == null) return;
		if (www.progress != progress)
		{
			progress = www.progress;
		}
		loadingBar.fillAmount = progress * MaxScenePercent;
		textProgress.text = (loadingBar.fillAmount * 100).ToString("0.0") + "%";
	}

	IEnumerator DownLoad()
	{
		UnityTools.LogMust("下载路径:" + url);

		www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			UnityTools.LogMust("下载失败 :" + www.error);
		}
		else
		{
			SceneMapManager.Instance().CurSceneBundle = www.assetBundle;
			SceneManager.LoadScene(SceneMapManager.Instance().nextSceneName);  //这里记得不要加上后缀不然会报错
			www.Dispose();
		}
	}


	public override void OnMsg(WindowMsgID msgId, object param)
	{
		if (msgId == WindowMsgID.ShowLoadingTips)
		{

		}
	}
}