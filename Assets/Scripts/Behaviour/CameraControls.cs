using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

	Vector3 lastMousePosition;
	Vector3 currentMousePosition;

	// Use this for initialization
	void Start ()
	{
		lastMousePosition = new Vector3();
		currentMousePosition = new Vector3();
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		currentMousePosition.z = 0;

		if(Input.GetMouseButton(2) || Input.GetMouseButton(1))
		{
			Camera.main.transform.Translate(lastMousePosition - currentMousePosition);
		}

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel") * 1.33f;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1.5f, 15f);

		lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastMousePosition.z = 0;
	}
}
