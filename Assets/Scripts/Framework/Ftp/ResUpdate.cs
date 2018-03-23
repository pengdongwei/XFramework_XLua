using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Net;

/// <summary>
/// 更新
/// </summary>
public class ResUpdate : MonoBehaviour {
	private List<string> downloadFiles = new List<string> ();

	void Awake(){
		UIManager.Intance.ShowWindow (WindowID.AssetCheckUI);
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			Debug.LogError ("没有网络也可以玩耍....");
			OnUpdateFailed (string.Empty);
		}else{
			StartCoroutine(OnUpdateResource());  
		}
	}

	string url;
	IEnumerator OnUpdateResource(){
		AssetCheckData data = new AssetCheckData();
		string dataPath = PathTools.DataPath;  //数据目录
		url = PathTools.GetWebURL();
		string random = DateTime.Now.ToString("yyyymmddhhmmss");
		string listUrl = url + "files.txt?v=" + random;
		WWW www = new WWW(listUrl); 
		yield return www;
		if (www.error != null)
		{
			OnUpdateFailed(string.Empty);
			yield break;
		}else{
			if (!Directory.Exists(dataPath))
			{
				Directory.CreateDirectory(dataPath);
			}

			File.WriteAllBytes(dataPath + "files.txt", www.bytes);

			string filesText = www.text;
			UpdateInfo info = GetUpdateInfo(filesText);
			if (info.totalSize > 0)
			{
				CachUpdateInfo = info;
				CachCheckData = data;
				StartCoroutine(GoToUpdateResource(CachUpdateInfo, CachCheckData));
				/*
				float size = (float)info.totalSize / 1024;
				UIManager.Intance.ShowMessageBox(string.Format("更新大小: {0} kb", size.ToString("0.0")), MessageBoxType.OK_Cancle, OnSureUpdateClick);
				yield break;*/
			}else{
				EventsMgr.Instance ().TriigerEvent ("EndUpdate", null);
			}
		}
	}

	private UpdateInfo CachUpdateInfo;
	private AssetCheckData CachCheckData;
	void OnSureUpdateClick(MessageBoxEvent evt)
	{
		if (evt == MessageBoxEvent.Ok)
		{
			StartCoroutine(GoToUpdateResource(CachUpdateInfo, CachCheckData));
		}
		else
		{
			Application.Quit();
		}
	}

	IEnumerator GoToUpdateResource(UpdateInfo info,AssetCheckData data)
	{
		float size = (float)info.totalSize / 1024;
		if (size < 0.1) size = 0.1f;
		data.msg = "更新大小:" + size.ToString("0.0") + "kb";
		int downLoadIdx = 0;
		foreach (KeyValuePair<string, string> pair in info.dict)
		{
			downLoadIdx++;
			data.value = (float)(downLoadIdx - 1) / (float)info.dict.Count;
			data.msg = "更新文件:" + downLoadIdx + "/" + info.dict.Count + ";总大小：" + size.ToString("0.0") + "kb";
			//data.msg = "更新大小:" + size.ToString("0.0") + "M" ;
			UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
			BeginDownload(pair.Value, pair.Key);
			while (!(IsDownOK(pair.Key))) { yield return new WaitForEndOfFrame(); }
		}
		yield return new WaitForEndOfFrame(); 
		data.msg = "更新完成, 准备进入游戏";
		data.value = 0f;
		Debug.Log("更新完成!!");
		UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		EventsMgr.Instance ().TriigerEvent ("EndUpdate", null);
	}

	/// <summary>
	/// 是否下载完成
	/// </summary>
	bool IsDownOK(string file)
	{
		return downloadFiles.Contains(file);
	}

	/// <summary>
	/// 线程下载
	/// </summary>
	void BeginDownload(string url, string file)
	{     //线程下载
		object[] param = new object[2] { url, file };

		ThreadEvent ev = new ThreadEvent();
		ev.Key = NotiConst.UPDATE_DOWNLOAD;
		ev.evParams.AddRange(param);
		ThreadManager.GetInstance().AddEvent(ev, OnThreadCompleted);   //线程下载
	}

	/// <summary>
	/// 线程完成
	/// </summary>
	/// <param name="data"></param>
	void OnThreadCompleted(NotiData data)
	{
		switch (data.evName)
		{
		case NotiConst.UPDATE_EXTRACT:  //解压一个完成
			//
			break;
		case NotiConst.UPDATE_DOWNLOAD: //下载一个完成
			downloadFiles.Add(data.evParam.ToString());
			break;
		}
	}

	void OnUpdateFailed(string file)
	{
		UnityTools.LogMust("更新失败!>" + file);
		AssetCheckData data = new AssetCheckData();
		data.msg = "下次记得在有网络下的环境玩耍约!";
		data.value = 0f;
		UIManager.Intance.SendMsg(WindowID.AssetCheckUI, WindowMsgID.ShowLoadingTips, data);
		EventsMgr.Instance ().TriigerEvent ("EndUpdate", null);
	}

	public UpdateInfo GetUpdateInfo(string content)
	{
		UpdateInfo info = new UpdateInfo();
		string[] files = content.Split('\n');
		string random = DateTime.Now.ToString("yyyymmddhhmmss");
		for (int i = 0; i < files.Length; i++)
		{
			//优先判断是否存在于datapath，如果存在则读取，不存在尝试读取streamingassts目录
			if (string.IsNullOrEmpty(files[i])) continue;
			string[] keyValue = files[i].Split('|');
			string f = keyValue[0];
			string dataLocalFile = (PathTools.DataPath + f).Trim();
			string localfile = dataLocalFile;
			string path = Path.GetDirectoryName(localfile);
			if (!Directory.Exists(path))
			{

				string contentLocalFile = (PathTools.AppContentPath() + f).Trim();
				string contentpath = Path.GetDirectoryName(contentLocalFile);
				if (!Directory.Exists(contentpath))
				{
					Directory.CreateDirectory(path);
				}
				else
				{
					localfile = contentLocalFile;
				}
			}
			string fileUrl = url + f + "?v=" + random;
			bool canUpdate = !File.Exists(localfile);
			//本地存在文件
			if (!canUpdate)
			{
				string remoteMd5 = keyValue[1].Trim();
				string localMd5 = MD5Tools.md5file(localfile);
				canUpdate = !remoteMd5.Equals(localMd5);
				if (canUpdate)
				{
					File.Delete(localfile);
				}
			}
			//本地缺少文件
			if (canUpdate)
			{
				if (fileUrl.Contains(AppConst.DllName) && Application.platform == RuntimePlatform.Android)
				{
					info.bRestart = true;
				}
				if(!fileUrl.Contains("manifest"))  info.totalSize += GetHttpFileSize(fileUrl);
				UnityTools.LogMust("需要更新：" + fileUrl);
				info.dict[dataLocalFile] = fileUrl;
			}
		}
		return info;
	}

	long GetHttpFileSize(string url){
		return 2000;
	}
}

public class UpdateInfo
{
	public long totalSize;
	public bool bRestart;
	//local path, remotepath
	public Dictionary<string, string> dict = new Dictionary<string, string>();
}