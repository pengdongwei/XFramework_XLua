using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AppConst
{
	public const string DllName = "Assembly-CSharp.dll";

	public const int TimerInterval = 1;
	public const int GameFrameRate = 30;                       //游戏帧频

	public const string AppName = "WDJ";           //应用程序名称
	public const string AppPrefix = AppName + "_";             //应用程序前缀
	public const string ExtName = ".assetbundle";              //素材扩展名
	public const string AssetDirname = "StreamingAssets";      //素材目录 


	//streamingassets 下资源前缀
	public static string ui_prefix = "assetbundle/ui/";
	public static string scene_prefix = "scene/";
	public static string model_prefix = "model/";
	public static string anim_prefix = "anim/";
	public static string text_prefix = "text/";

	//平台路径
	public static string prefix_android = "Android";
	public static string prefix_ios = "IOS";
	public static string prefix_pc = "Windows";

	//DB 地址
	public static string outIp ="www.wxygame.top";
	//public static string outIp ="120.78.68.161";
	public static int redisPort = 6379;
	//资源服
	public static int httpPort = 8888;

}