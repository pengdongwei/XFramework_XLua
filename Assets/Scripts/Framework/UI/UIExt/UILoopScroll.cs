using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CommonComponent
{
	//先不考虑滚动方向
	public abstract class UILoopScroll : MonoBehaviour
	{
		enum ScrollDir
		{
			Horizontal,
			Vertical
		}
		[SerializeField]
		ScrollDir mDirection = ScrollDir.Horizontal;

		[SerializeField]
		private Vector2 mPage;  //以垂直为例，mPage.y为竖直拖动时，每次移动的item数量.
		[SerializeField, Range(5, 45)]
		private int mBufferSize;
		private ScrollRect mScrolRect;
		private RectTransform mContentRect;
		[SerializeField]
		private RectTransform mElement; //要存放的item对象

		List<RectTransform> mInitiateElems = new List<RectTransform>();
		public IList mElemData;    //暂时存放int数据, todo

		public Vector2 ElemRect { get { return mElement != null ? mElement.sizeDelta : new Vector2(100f, 100f); } }
		public float CellScale { get { return mDirection == ScrollDir.Horizontal ? ElemRect.x : ElemRect.y; } }
		float mPreviosPos = 0;
		public float DirectionPos
		{
			get { return mDirection == ScrollDir.Horizontal ? mContentRect.anchoredPosition.x : mContentRect.anchoredPosition.y; }
			set
			{
				Vector2 temp = mContentRect.anchoredPosition;
				if (mDirection == ScrollDir.Horizontal)
				{
					temp.x = value;
				}
				else
				{ temp.y = value; }
				mContentRect.anchoredPosition = temp;
			}
		}
		int mCurrentFirstIndex;   //当前显示的首行或首列的index
		Vector2 mInstantiateSize = Vector2.zero;
		public Vector2 InstantiateSize
		{
			get
			{
				if (mInstantiateSize == Vector2.zero)
				{
					float rows = 0f, cols = 0f;
					if (mDirection == ScrollDir.Horizontal)
					{
						rows = mPage.x;
						cols = mPage.y + mBufferSize;
					}
					else
					{
						rows = mPage.x + mBufferSize;
						cols = mPage.y;
					}
					mInstantiateSize = new Vector2(rows, cols);
				}
				return mInstantiateSize;
			}
		}
		public float MaxPrevPos
		{
			get
			{
				float result;
				Vector2 max = GetRectByCount(mElemData.Count);
				if (mDirection == ScrollDir.Horizontal)
				{
					result = max.y - mPage.y;
				}
				else
				{
					result = max.x - mPage.x;
				}

				return result * CellScale;
			}
		}
		public int PageCount { get { return (int)mPage.x * (int)mPage.y; } }

		public int PageScale { get { return mDirection == ScrollDir.Horizontal ? (int)mPage.x : (int)mPage.y; } }   //每次翻页的大小。
		public int InstantiateCount { get { return (int)InstantiateSize.x * (int)InstantiateSize.y; } }

		const float MIN_MOVE_VELOCITY = 0.01f;

		void Awake()
		{
			mScrolRect = GetComponentInParent<ScrollRect>();
			mScrolRect.horizontal = mDirection == ScrollDir.Horizontal;
			mScrolRect.vertical = mDirection == ScrollDir.Vertical;
			mContentRect = GetComponent<RectTransform>();
			mElement.gameObject.SetActive(false);
			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		public void InitData(IList data)
		{
			if (null == data)
				return;
			if (null != mElemData)
				mElemData = null;
			mElemData = data;
			//设置content区域大小
			if (mElemData.Count > PageCount)
			{
				SetContentBound(GetRectByCount(mElemData.Count));
			}
			else
			{
				SetContentBound(mPage);
			}
			//创建或销毁物品
			if (mElemData.Count > InstantiateCount)
			{
				while (mInitiateElems.Count < InstantiateCount)
				{
					CreateItem(mInitiateElems.Count);
				}
			}
			else
			{
				while (mInitiateElems.Count > mElemData.Count)
				{
					RemoveItem(mInitiateElems.Count - 1);
				}
				while (mInitiateElems.Count < mElemData.Count)
				{
					CreateItem(mInitiateElems.Count);
				}
			}
		}

		//老版刷新
		//public virtual void Refresh()
		//{
		//    DirectionPos = 0f;
		//    int index = mCurrentFirstIndex * PageScale;
		//    foreach (var item in mInitiateElems)
		//    {
		//        UpdateItemData(index, item.gameObject);
		//        ++index;
		//    }
		//}

		/// <summary>
		/// 刷新，会返回顶端
		/// </summary>
		public virtual void Refresh()
		{
			if (mInitiateElems.Count == 0)
				return;
			int diffIdx = (mCurrentFirstIndex * PageScale) % mInitiateElems.Count;
			List<RectTransform> range = mInitiateElems.GetRange(mInitiateElems.Count - diffIdx, diffIdx);
			mInitiateElems.RemoveRange(mInitiateElems.Count - diffIdx, diffIdx);
			mInitiateElems.InsertRange(0, range);
			for (int i = 0; i < mInitiateElems.Count; ++i)
			{
				mInitiateElems[i].anchoredPosition = GetPosByIndex(i);
				UpdateItemData(i, mInitiateElems[i].gameObject);
			}
			DirectionPos = 0f;
			mPreviosPos = 0;
			int index = 0;
			mCurrentFirstIndex = 0;
		}

		void CreateItem(int index)
		{
			RectTransform item = GameObject.Instantiate(mElement);
			item.SetParent(transform, false);
			item.anchorMax = Vector2.up;
			item.anchorMin = Vector2.up;
			item.pivot = Vector2.up;
			item.name = "item_" + index;
			item.anchoredPosition = mDirection == ScrollDir.Horizontal ?
				new Vector2((index / InstantiateSize.x) * ElemRect.x, -Mathf.Floor(index % InstantiateSize.x) * ElemRect.y)
				: new Vector2((index % InstantiateSize.y) * ElemRect.x, -Mathf.Floor(index / InstantiateSize.y) * ElemRect.y);
			mInitiateElems.Add(item);
			item.gameObject.SetActive(true);
			UpdateItemData(index, item.gameObject);
		}

		void RemoveItem(int index)
		{
			RectTransform item = mInitiateElems[index];
			mInitiateElems.Remove(item);
			GameObject.Destroy(item.gameObject);
		}

		public void Clear(){
			for(int index=0; index<mInitiateElems.Count; index++){
				RectTransform item = mInitiateElems[index];
				mInitiateElems.Remove(item);
				GameObject.Destroy(item.gameObject);
			}
		}

		Vector2 GetRectByCount(int dataCount)
		{
			return mDirection == ScrollDir.Horizontal ? new Vector2(mPage.x, Mathf.CeilToInt(dataCount / mPage.x)) :
				new Vector2(Mathf.CeilToInt(dataCount / mPage.y), mPage.y);
		}

		void SetContentBound(Vector2 vec)
		{
			if (mContentRect == null)
				mContentRect = GetComponent<RectTransform>();
			mContentRect.sizeDelta = new Vector2(vec.y * ElemRect.x, vec.x * ElemRect.y);
		}

		float dir { get { return mDirection == ScrollDir.Horizontal ? 1f : -1f; } }

		// Update is called once per frame
		void Update()
		{
			if (mElemData == null)
				return;
			float velocity = GetScrollingVelocity();
			if (velocity > 0f)
			{
				//往后
				if (dir * DirectionPos - mPreviosPos < -CellScale)
				{
					if (mPreviosPos <= -MaxPrevPos)
						return;
					mPreviosPos -= CellScale;
					List<RectTransform> range = mInitiateElems.GetRange(0, PageScale);
					mInitiateElems.RemoveRange(0, PageScale);
					mInitiateElems.AddRange(range);
					for (int i = 0; i < range.Count; ++i)
					{
						MoveItemToIndex(mCurrentFirstIndex * PageScale + mInitiateElems.Count + i, range[i]);
					}
					++mCurrentFirstIndex;
				}
				return;
			}
			if (velocity < 0f)
			{
				//往前
				if (dir * DirectionPos - mPreviosPos > -CellScale)
				{
					if (Mathf.RoundToInt(mPreviosPos) >= 0)
						return;
					mPreviosPos += CellScale;
					--mCurrentFirstIndex;
					if (mCurrentFirstIndex < 0)
						return;
					List<RectTransform> range = mInitiateElems.GetRange(mInitiateElems.Count - PageScale, PageScale);
					mInitiateElems.RemoveRange(mInitiateElems.Count - PageScale, PageScale);
					mInitiateElems.InsertRange(0, range);
					for (int i = 0; i < range.Count; ++i)
					{
						MoveItemToIndex(mCurrentFirstIndex * PageScale + i, range[i]);
					}
				}
				return;
			}
		}

		private float GetScrollingVelocity()
		{
			float veloc = mDirection == ScrollDir.Horizontal ? mScrolRect.velocity.x : mScrolRect.velocity.y;
			return veloc;
		}

		void MoveItemToIndex(int index, RectTransform item)
		{
			item.anchoredPosition = GetPosByIndex(index);
			UpdateItemData(index, item.gameObject);
		}

		Vector2 GetPosByIndex(int index)
		{
			float x, y;
			if (mDirection == ScrollDir.Horizontal)
			{
				x = index % mPage.x;
				y = Mathf.FloorToInt(index / mPage.x);
			}
			else
			{
				x = Mathf.FloorToInt(index / mPage.y);
				y = index % mPage.y;
			}
			return new Vector2(y * ElemRect.x, -x * ElemRect.y);
		}

		void UpdateItemData(int index, GameObject gobj)
		{
			gobj.SetActive(index < mElemData.Count);
			if (gobj.activeSelf)
			{
				SetItem(gobj, mElemData[index]);
				SetItemIndex(gobj, mElemData[index], index);
			}
		}

		/// <summary>
		/// 设置或更新item的数据和显示
		/// </summary>
		/// <param name="gobj"></param>
		/// <param name="data"></param>
		protected virtual void SetItem(GameObject gobj, object data){}

		protected virtual void SetItemIndex(GameObject gobj, object data, int index){}
	}
}