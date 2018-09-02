using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leek : MonoBehaviour {

	[SerializeField]
	private float speed;
	
	[SerializeField]
	private float perceptionRange;

	private bool chasing = false;  // is the leek chasing the player ?
	[SerializeField] 
	private Gradient chaseColor;
	private const float ChaseColorSpeed = 0.5f;
	
	private float horizontalMouvement;
	
	private Rigidbody2D myRigidbody;
	private SpriteRenderer mySpriteRenderer;
	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player");

		StartCoroutine(Idle());
	}

	private void Update()
	{
		if(chasing)
			HandleAnimation(horizontalMouvement);
	}

	void FixedUpdate()
	{
		if (chasing)
			horizontalMouvement = HandleMouvement();
	}

	private float HandleMouvement()
	{
		float horizontal = 1f;
		if (player.transform.position.x < transform.position.x)
			horizontal = -1f;
		
		myRigidbody.velocity = new Vector2(horizontal * speed, myRigidbody.velocity.y);
		return horizontal;
	}

	private void HandleAnimation(float horizontal)
	{
		mySpriteRenderer.flipX = horizontal > 0f;
		mySpriteRenderer.color = chaseColor.Evaluate(Time.time * ChaseColorSpeed % 1f);
	}

	/// <summary>
	/// Stay Idle until the player is closer than perceptionRange then transition to the chasing state
	/// </summary>
	/// <returns></returns>
	private IEnumerator Idle()
	{
		while (Vector2.Distance(player.transform.position, transform.position) > perceptionRange)
		{
			if (Random.Range(0f, 1f) > 0.7f)
			{
				mySpriteRenderer.flipX = !mySpriteRenderer.flipX; 
				myRigidbody.AddForce(new Vector2(0f, 150f));
			}
			yield return new WaitForSeconds(1f);
		}
		chasing = true;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject == player)
			StartCoroutine(player.GetComponent<Player>().TakeDamage());
	}
}
