using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 长按处理
/// </summary>
public class LongPress : MonoBehaviour {
	GameObject tipgo;

	public void longPress(){
		
	}

	public void leasePress(){
		if (tipgo != null) {
			Destroy (tipgo);
		}
	}
}
