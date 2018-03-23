/// <summary>
/// On play cg finish delegate.
/// 功能:cg 播放
/// 创建人：彭东维
/// 创建日期：2014-12-29
/// </summary>
using UnityEngine;
using System.Collections;

public delegate void OnPlayCgFinishDelegate();

public class CGManager : MonoBehaviour {
	bool bPlayFinishCg = false;
	bool bPlayFinishTitle = false;
	string cgName;
	OnPlayCgFinishDelegate playcgDel;

	//隐藏播放控制，点击无效，适合播放工作室名
	public void PlayCGByName(string titleName, string cgName, OnPlayCgFinishDelegate del){
		this.playcgDel = del;
		StartCoroutine(PlayCG(titleName));
		#if UNITY_ANDROID || UNITY_IPHONE
		StartCoroutine(PlayCG(titleName));
		/*
		if(titleName.EndsWith("mp4")){
			this.cgName = cgName;
			bPlayFinishTitle = false;
			Handheld.PlayFullScreenMovie (titleName, Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);
			bPlayFinishTitle = true;
		}
		else{
			Debug.LogError("cg audio is not mp4");
		}
         * */
		#else
		OnFinishPlayHandle();
		#endif
	}


	IEnumerator PlayCG(string titleName)
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		Handheld.PlayFullScreenMovie(titleName, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
		#endif
		yield return null;
		OnFinishPlayHandle();
	}


	void OnApplicaionPause(bool pause){
		#if UNITY_ANDROID || UNITY_IOS
		//隐藏播放控制，点击取消播放，适合播放cg
		if (!pause && bPlayFinishCg) {
			OnFinishPlayHandle();
		}
		if (!pause && bPlayFinishTitle) {
			//不存在cg直接结束播放
			if(string.IsNullOrEmpty(this.cgName)){
				OnFinishPlayHandle();
			}else{
				bPlayFinishCg = false;
				Debug.LogError("cg 播放中");
				Handheld.PlayFullScreenMovie (this.cgName, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
				bPlayFinishCg = true;
			}
		}
		#endif
	}

	void OnFinishPlayHandle(){
		if(playcgDel!=null){
			playcgDel();
		}	
	}
}