using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM{
	/*
* =============个人=============
add dimond = 增加砖石
add coin =   增加金币
add love =   增加魅力值
add lv =    （全部卡片）等级 
=============任务=============
add task id=    添加任务
finish task id =     完成任务
clear task 清除任务
=============关卡=============
pass all level  通关所有地图
reset all level 重置所有地图
pass map id=
=============图鉴=============
unlock all monster  解锁所有怪物
unlock all hero     解锁所有英雄
=============军队=============
unlock hero id =      添加一个英雄到阵型中
unlock monster id =      添加一个英雄到阵型中
=============抽奖=============
draw id =           指定抽奖
=============背包=============
add item id,count= 11;11     添加一个道具
clear bag  清除背包
=============背包=============
refresh id =  //立即刷新商品
remove id =   //清除一次性商品，重新可以购买
add taskitem id = //增加一个任务道具
=============事件=============
show all event
=============活动=============
login day=2 //两天前登陆的
*/
	public static void Parse(string line){
		if (line.StartsWith ("add_")) {
			HandleUserInfo (line);
		} else if(line.Contains("task_")){
			HandleTask (line);
		}else if(line.Contains("card_")){
			HandleCard (line);
		}else if(line.Contains("level_")){
			HandleLevel (line);
		}else if(line.Contains("item_")){
			HandleBag (line);
		}else if(line.Contains("shop_")){
			HandleShop (line);
		}
		else if (line.StartsWith ("show all event")) {
			EventsMgr.Instance ().ToString ();
		}
		else {
			CommonTip.Instance ().ShowTip ("<color=red>gm 执行错误</color>", TipType.PopTip);
		}
	}




	/// <summary>
	/// 处理任务
	/// </summary>
	static void HandleTask(string line){
		
	}

	/// <summary>
	/// 个人信息
	/// </summary>
	static void HandleUserInfo(string line){
		
	}

	/// <summary>
	/// 卡片
	/// </summary>
	static void HandleCard(string line){
		
	}

	static void HandleBag(string line){
		
	}

	static void HandleShop(string line){

	}

	static void HandleLevel(string line){

	}

	public static string GetGMDesc(){
		string other =  string.Format ("=============通用=============\n\n清除所有{0}\n显示当前事件{1}\n", "clear all", "show all event");
		string taskDesc = string.Format ("=============任务=============\n\n激活{0}\n完成{1}\n清除{2}\n激活所有任务{3}\n", "task_add id = ", "task_finish id = ", "task_clear all", "task_add all");
		string userDesc = string.Format ("=============个人信息=============\n\n金币{0}\n元宝{1}\n魅力{2}\n等级{3}\n创建游戏时间(几天前)：{4}\n模拟充值：{5}\n竞技币：{6}\n", "add_dimond = ", "add_coin = ", 
			"add_love = ", "add_lv = ", "add_create=", "add_money=", "add_jj=");
		string cardDesc = string.Format ("=============卡片=============\n\n解锁图鉴{0}\n获得新英雄{1}\n", "card_unlock all", "card_unlock id = ");
		string bagDesc = string.Format ("=============背包=============\n\n掉落物品{0}\n掉落所有物品{1}\n清除背包{2}\n", "item_add id,count = xx,xx", "item_unlock all ", "item_clear all");
		string shopDesc = string.Format ("=============商店信息=============\n\n获取新物品{0}\n调整卖的时间，多少天前{1}\n删除物品{2}\n清除商店{3}\n", "shop_add id =", "shop_daysago = ", "shop_remove id = ", "shop_clear all");
		string levelDesc = string.Format ("=============地图=============\n\n通关所有{0}\n重制关卡{1}\n通过某章节{2}\n", "level_pass all", "level_reset all", "level_pass map id = ");

		return other + userDesc + taskDesc + cardDesc + bagDesc+shopDesc + levelDesc;
	}

}