using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class dragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public static GameObject item;    // i changed itembeigdraged to item.


	public static Transform startParent;
	Vector3 startPosition;
	bool start = true;

	public void OnBeginDrag(PointerEventData eventData)
	{
		item = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;

		GetComponent<CanvasGroup>().blocksRaycasts = false;
		item.GetComponent<LayoutElement>().ignoreLayout = true;
		item.transform.SetParent(item.transform.parent);
	}


	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		item = null;
		transform.position = transform.parent.position;

		GetComponent<CanvasGroup>().blocksRaycasts = true;
		GetComponent<LayoutElement>().ignoreLayout = false;


	}

}