using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 文件作用:
/// 实现功能：
/// 版本:v1.0
/// 作者:崔健
/// </summary>
public class FightTip : MonoBehaviour
{
	//  public Text Text_FightDes;
	public Text Text_FightNum;
	uint m_FightNum;
	uint m_CachNum;
	public float m_ScaleDuration = 1.5f;
	public float m_ScaleValue = 2f;
	/// <summary>
	/// 字体颜色
	/// </summary>
	private int colorType = 1;

	void Awake()
	{

	}

	void Update()
	{
		Text_FightNum.text = "战斗力:" + m_FightNum;
	}

	public void Init(uint fight)
	{
		transform.gameObject.SetActive (true);
		if (m_FightNum<fight)
		{
			colorType = 1;
		}else
		{
			colorType = 8;
		}
		DOTween.To(ChangeNum, m_FightNum, fight, 1f).OnComplete(DoValueOnComplete);
		Text_FightNum.transform.DOScale(m_ScaleValue, m_ScaleDuration);
	}

	public void ChangeFight(uint oldFight, uint newFight){
		m_FightNum = oldFight;
		m_CachNum = newFight;
		Init (newFight);
	}

	void ChangeNum(float f)
	{
		m_FightNum = (uint)f;
	}

	void DoValueOnComplete()
	{
		// Destroy(transform.gameObject,0.5f);
		m_FightNum = m_CachNum;
		transform.gameObject.SetActive (false);
	}
}