using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AssetCheckData{
	public string msg;
	public float value;
}

public class AssetCheckUI : UIWndBase {
	public Text tips;
	public Image loadingBar;
	private string msg = "";

	public override void OnMsg (WindowMsgID msgId, object param)
	{
		if(msgId == WindowMsgID.ShowLoadingTips){
			AssetCheckData data =  (AssetCheckData)param;
			if(data!=null){
				loadingBar.fillAmount = data.value;
				if (loadingBar.fillAmount == 0) {
					tips.text = data.msg;
					loadingBar.transform.parent.gameObject.SetActive (false);
				} else {
					loadingBar.transform.parent.gameObject.SetActive (true);
					tips.text = data.msg + "<color=#DFCD9FFF>(" + (loadingBar.fillAmount * 100).ToString("0.0") + "%)</color>";
				}
			}
		}
	}
}