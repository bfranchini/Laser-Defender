using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
	public GameObject Laser;
	public float LaserSpeed = 15f;
	public float FiringRate = 1f; 
	public float health = 100f;
	public int scoreValue = 150;	
	public AudioClip LaserSfx;
	public AudioClip ExplodeSfx;
	private ScoreKeeper scoreKeeper;
	private AudioSource audioSource;
	
	void Start()
	{
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update() 
	{	
		var probability = FiringRate * Time.deltaTime;	
		
		if(Random.value < probability)
		{
			fire();
		}
	}
	
	public void OnTriggerEnter2D(Collider2D coll)
	{
		var projectile = coll.gameObject.GetComponent<Projectile>();																
		health -= projectile.GetDamage();
		
		if(health <= 0)
		{ 
			scoreKeeper.Score(scoreValue);
			AudioSource.PlayClipAtPoint(ExplodeSfx, Camera.main.transform.position);
			Destroy(gameObject);
		}
			
		projectile.Hit();		
	}
	
	private void fire()
	{		
		var laser = Instantiate(Laser, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -LaserSpeed, 0);		
		AudioSource.PlayClipAtPoint(LaserSfx, Camera.main.transform.position);
	}		
}
