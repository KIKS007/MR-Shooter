using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetSize : MonoBehaviour 
{
	public float height = 1;
	private ImageTargetBehaviour _imageTarget;

	// Use this for initialization
	void Start () {
		_imageTarget = GetComponent<ImageTargetBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () {
		_imageTarget.SetHeight (height);
	}
}
