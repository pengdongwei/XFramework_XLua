using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUI : UIWndBase {

	public void OnClick(){
		UIManager.Intance.ShowWindow (WindowID.MainCity);
	}
}
