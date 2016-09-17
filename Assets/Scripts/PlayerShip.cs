using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour 
{
	public float PlayerSpeed;
	public float ScreenXmin; 
	public float ScreenXmax;		
	public float LaserSpeed = 15f;		
	public float FiringRate = 0.2f;			
	public float Health = 250f;
	public AudioClip LaserSfx;
	public GameObject Laser;	
		
	void Start()
	{
		//find the left and right boundaries of the play space for the main camera
		var distance = transform.position.z - Camera.main.transform.position.z;
		
		var leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		var righEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		
		var halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
		
		ScreenXmin = leftEdge.x + halfWidth;
		ScreenXmax = righEdge.x - halfWidth;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		moveShip();		
		
		if(Input.GetKeyDown(KeyCode.Space))	
			//using 0.0 for the second parameter can cause bugs	
			InvokeRepeating("fire", 0.000001f, FiringRate);
			
		if(Input.GetKeyUp(KeyCode.Space))
			CancelInvoke("fire");
	}
	
	private void fire()
	{
		var laser = Instantiate(Laser, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, LaserSpeed, 0);		
		
		AudioSource.PlayClipAtPoint(LaserSfx, transform.position);		
	}
	
	private void moveShip()
	{
		if(Input.GetKey(KeyCode.LeftArrow))
		{	
			transform.position += Vector3.left * PlayerSpeed * Time.deltaTime;		
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{			
			transform.position += Vector3.right * PlayerSpeed * Time.deltaTime;
		}	
		
		float newX = Mathf.Clamp (transform.position.x, ScreenXmin, ScreenXmax);
		
		//restrict the player to the game space
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	public void OnTriggerEnter2D(Collider2D coll)
	{						
		print ("Player Hit!");
		var projectile = coll.gameObject.GetComponent<Projectile>();															
		Health -= projectile.GetDamage();
		
		if(Health <= 0)
			Die ();
	}
	
	private void Die()
	{
		var levelManager = GameObject.Find ("LevelManager").GetComponent<LevelManager>();
		levelManager.LoadLevel("Win Screen");
		Destroy(gameObject);					
	}
}
