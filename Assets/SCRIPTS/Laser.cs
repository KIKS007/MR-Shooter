using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour 
{
	private EnemyManager _enemyManager;

	void Start ()
	{
		_enemyManager = FindObjectOfType<EnemyManager> ();
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.tag == "Enemy")
		{
			Destroy (collider.gameObject);
			_enemyManager.EnemyDead ();
		}
	}
}
