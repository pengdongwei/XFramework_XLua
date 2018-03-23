using UnityEngine;
using System.Collections;
/// <summary>
/// 文件作用:
/// 实现功能： 此文件的作用只是在新切入的场景中加载loadingUI 不做其他任何事情
/// 版本:v1.0
/// 作者:pengdongwei
/// </summary>
public class LoadingHelper : MonoBehaviour {
	void Awake(){
		UIManager.Intance.ShowWindow (WindowID.LoadingUI);
	}
}