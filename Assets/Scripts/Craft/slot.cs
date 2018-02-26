using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class slot : MonoBehaviour , IDropHandler{ 
	public GameObject item { 
		get { 
			if (transform.childCount > 0) {
				return transform.GetChild(0).gameObject; 
			} 
			return null; 
		} 
	} 

	#region IdropHandler implementation 
	public void OnDrop(PointerEventData eventData) {
		GameObject libre = item;
		if (!libre) { 
			dragHandler.item.transform.SetParent (transform); 
		} else {
			libre.transform.SetParent (dragHandler.startParent);
			libre.transform.position= libre.transform.parent.position;
			dragHandler.item.transform.SetParent (transform);
		}
	} #endregion 

}﻿ 