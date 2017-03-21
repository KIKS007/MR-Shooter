using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shooting : MonoBehaviour 
{
	[Header ("FOV")]
	public float heightFinalValue;
	public float heightDuration;
	public float heightResetDuration;

	private float _initialHeight;

	[Header ("Trigger Laser")]
	public GameObject triggerLaser;

	[Header ("Graphic Lasers")]
	public LineRenderer[] lasers;
	public LayerMask groundLayer;
	public float finalWidth;
	public float graphicLaserDuration = 0.5f;

	private GameObject[] _mainCameras = new GameObject[2];
	private RaycastHit _hitInfo;
	private bool _setup = false;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (WaitForVuforia ());
	}

	IEnumerator WaitForVuforia ()
	{
		int child = transform.parent.childCount;

		yield return new WaitWhile (()=> child == transform.parent.childCount);

		_mainCameras = GameObject.FindGameObjectsWithTag ("MainCamera");
		_initialHeight = _mainCameras [0].transform.localPosition.y;

		for (int i = 0; i < lasers.Length; i++)
			lasers [i].gameObject.SetActive (true);

		_setup = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!_setup)
			return;
		
		if(Input.GetMouseButton (0))
		{
			SetLasers ();
			triggerLaser.SetActive (true);

			DOTween.Kill ("ResetHeight");
			DOTween.Kill ("ResetGraphicLaser");


			/*if (_mainCameras [0].transform.localPosition.y != heightFinalValue && !DOTween.IsTweening ("NewHeight"))
			{
				_mainCameras [0].transform.DOLocalMoveY (heightFinalValue, heightDuration).SetEase (Ease.OutQuad).SetId ("NewHeight");
				_mainCameras [1].transform.DOLocalMoveY (heightFinalValue, heightDuration).SetEase (Ease.OutQuad).SetId ("NewHeight");
			}*/

			if(lasers [0].startWidth != finalWidth && !DOTween.IsTweening ("GraphicLaser"))
			{
				DOTween.To (()=> lasers[0].startWidth, x=> lasers[0].startWidth = x, finalWidth, graphicLaserDuration).SetEase (Ease.OutQuad).SetId ("GraphicLaser");
				DOTween.To (()=> lasers[1].startWidth, x=> lasers[1].startWidth = x, finalWidth, graphicLaserDuration).SetEase (Ease.OutQuad).SetId ("GraphicLaser");
			}
		}
		else
		{
			triggerLaser.SetActive (false);

			DOTween.Kill ("NewHeight");
			DOTween.Kill ("GraphicLaser");

			/*if (_mainCameras [0].transform.localPosition.y != _initialHeight && !DOTween.IsTweening ("ResetFOV"))
			{
				_mainCameras [0].transform.DOLocalMoveY (_initialHeight, heightResetDuration).SetEase (Ease.OutQuad).SetId ("NewHeight");
				_mainCameras [1].transform.DOLocalMoveY (_initialHeight, heightResetDuration).SetEase (Ease.OutQuad).SetId ("NewHeight");
			}*/

			if(lasers [0].startWidth != 0 && !DOTween.IsTweening ("ResetGraphicLaser"))
			{
				DOTween.To (()=> lasers[0].startWidth, x=> lasers[0].startWidth = x, 0, graphicLaserDuration).SetEase (Ease.OutQuad).SetId ("ResetGraphicLaser");
				DOTween.To (()=> lasers[1].startWidth, x=> lasers[1].startWidth = x, 0, graphicLaserDuration).SetEase (Ease.OutQuad).SetId ("ResetGraphicLaser");
			}
		}
	}

	void SetLasers ()
	{
		if (!Physics.Raycast (_mainCameras [0].transform.position, _mainCameras [0].transform.forward, out _hitInfo, Mathf.Infinity, groundLayer, QueryTriggerInteraction.Ignore))
			return;

		for(int i = 0; i < lasers.Length; i++)
		{
			lasers [i].SetPosition (0, lasers [i].transform.position);
			lasers [i].SetPosition (1, _hitInfo.point);
		}
	}
}
