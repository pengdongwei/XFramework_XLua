using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// json 的  prefs 存储
/// </summary>
public enum PrefsInfo{
	gameversion=0, //游戏版本
	IsEndExtract11=1, //是否完成了解压过程
	IsEndGuide=2,///是否结束了引导
	guideIdx = 3,//当前引导的索引

	noticeVer=4,//公告版本
	IsInBlack=5, //是否进入了黑名单
	IsCloseCg=6, //是否开启cg
	IsRecoryDiamond=7,//是否已经恢复了砖石
	IsCloseMusic=8,
	lastTimeZhongsheng=9, //上次领取终身卡的时间
	serverTime=10, //服务器当前时间
	ZhongshengCount=11,//终身卡的数量
	HasSpeedCard =12, //是否拥有加速卡
}