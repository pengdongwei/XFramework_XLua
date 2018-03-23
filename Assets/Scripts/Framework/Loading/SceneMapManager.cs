using UnityEngine;
using System.Collections;
///	<summary>
/// 文件作用: wdj场景管理
/// 实现功能:
/// 版本: v1.0
/// 作者: zhangshuiqing
/// </summary>

public class SceneMapManager : ISingleton<SceneMapManager>
{

	private string mNextSceneName;
	public string nextSceneName { get { return mNextSceneName; } }

	public bool IsInLoadingMapSceneBundle { get; set; }
	public bool IsInLoadingScene { get; set; }

	public string loadingTips;

	/// <summary>
	/// 当前加载场景的AssetBundle，加载新场景前先进行卸载，避免资源重复
	/// </summary>
	public AssetBundle CurSceneBundle = null;

	public void LoadSceneMap(string sceneName)
	{
		mNextSceneName = sceneName;
		LoadScene();
	}

	private void LoadScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
	}
}