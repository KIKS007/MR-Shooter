using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour 
{
	[Header ("Enemies")]
	public Transform enemiesParent;
	public GameObject[] enemiesPrefabs = new GameObject[0];
	public Transform[] enemiesSpawns = new Transform[0];
	public Transform enemiesTarget;

	[Header ("Infos")]
	public int enemiesCount = 0;

	[Header ("Settings")]
	public int maxEnemiesCount = 0;
	public float addUpDuration = 5f;
	public Vector2 randomDurationBetweenSpawns;
	public Vector2 enemiesSpeed;
	public float speedMin;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (Spawner ());
		StartCoroutine (AddUp ());
	}

	IEnumerator AddUp ()
	{
		maxEnemiesCount++;

		yield return new WaitForSeconds (addUpDuration);

		StartCoroutine (AddUp ());
	}

	IEnumerator Spawner ()
	{
		if (enemiesCount < maxEnemiesCount)
			SpawnEnemy ();

		yield return new WaitWhile (()=> enemiesCount == maxEnemiesCount);

		yield return new WaitForSeconds (Random.Range (randomDurationBetweenSpawns.x, randomDurationBetweenSpawns.y));

		StartCoroutine (Spawner ());
	}

	void SpawnEnemy ()
	{
		enemiesCount++;

		GameObject agent = Instantiate (enemiesPrefabs [Random.Range (0, enemiesPrefabs.Length)], enemiesSpawns [Random.Range (0, enemiesSpawns.Length)].position, Quaternion.identity, enemiesParent) as GameObject;

		StartCoroutine (WaitTillAgentEnabled (agent));
	}

	IEnumerator WaitTillAgentEnabled (GameObject agent)
	{
		yield return new WaitUntil (()=> agent.activeSelf == true);

		agent.GetComponent<NavMeshAgent> ().SetDestination (enemiesTarget.position);
		agent.GetComponent<NavMeshAgent> ().speed = speedMin + maxEnemiesCount + Random.Range (enemiesSpeed.x, enemiesSpeed.y);
	}

	public void EnemyDead ()
	{
		enemiesCount--;
	}
}
