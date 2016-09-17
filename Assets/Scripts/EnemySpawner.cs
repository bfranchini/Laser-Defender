using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject enemyPrefab;
	public float spawnerWidth = 10f;
	public float spawnerHeight = 5f;
	public float formationSpeed = 5f;
	public float SpawnDelay = 0.5f;
	private bool moveRight = true;
	
	private PlayerShip player; //used to get x min and max values for play space
	
	// Use this for initialization
	void Start () 
	{			
		//find the instance of the player ship so we can get the play space's min and max x values
		player = FindObjectOfType<PlayerShip>();		
		
		SpawnEnemyFormation();									
	}
	
	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(spawnerWidth, spawnerHeight));
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move formation left or right
		if(moveRight)			
			transform.position += Vector3.right * formationSpeed * Time.deltaTime;
		else			
			transform.position += Vector3.left * formationSpeed * Time.deltaTime;
		
		//prevent ships from moving past edge of screen
		var formationRightEdge = transform.position.x + (spawnerWidth / 2);
		var formationLeftEdge = transform.position.x - (spawnerWidth / 2);
		
		if(formationRightEdge >= player.ScreenXmax)		
			moveRight = false;									
		else if(formationLeftEdge <= player.ScreenXmin)		
			moveRight = true;				
		
		//prevent ship formation from moving past play space
		float newX = Mathf.Clamp(transform.position.x, player.ScreenXmin, player.ScreenXmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		
		if(AllMembersDead())
		{
			SpawnUntilFull();
		}
	}
	
	private bool AllMembersDead()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount > 0)
				return false;
		}
		
		return true;
	}
	
	private Transform NextFreePosition()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount == 0)
				return childPositionGameObject;			
		}
		
		return null;
	}
	
	private void SpawnEnemyFormation()
	{
		//the children are the position objects within the EnemyFormation object
		foreach(Transform child in transform)
		{
			var enemy = Instantiate(enemyPrefab, child.position, Quaternion.identity) as GameObject;
			
			//set parent to EnemyFormation object
			enemy.transform.parent = child;				
		}		
	}
	
	private void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		
		if(freePosition != null)
		{
			var enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
		
			//set parent to EnemyFormation object
			enemy.transform.parent = freePosition;				
		}
		
		if(NextFreePosition())
			Invoke("SpawnUntilFull", SpawnDelay);
	}
}
