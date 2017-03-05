using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MenuButton : MonoBehaviour {

	[SerializeField]
	public UnityEngine.Events.UnityEvent ClickEvent;


	void OnMouseDown()
	{
		if (ClickEvent != null)
		{
			ClickEvent.Invoke();
		}
	}
}
