using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

	#region Private properties

	[SerializeField]
	private float _rotationSpeed;

	[SerializeField]
	private Vector3 _rotationDirection;

	#endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);

	}
}
