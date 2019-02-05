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

    [SerializeField]
    private int health;

    private bool flashActive;

    [SerializeField]
    private float flashLength;

    private float flashCounter;

    private GameObject[] lifes = new GameObject[5];


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
        if (flashActive) SystemFlash();
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
		if (!flashActive) mySpriteRenderer.color = chaseColor.Evaluate(Time.time * ChaseColorSpeed % 1f);
	}

	/// Stay Idle until the player is closer than perceptionRange then transition to the chasing state
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

    public IEnumerator TakeDamage()
    {
        health -= 1;
        Destroy(lifes[health]);
        if (!IsDead())
        {
            flashActive = true;
            flashCounter = flashLength;
        }
        else
        {
            Destroy(gameObject);
            yield return null;
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    private void SystemFlash()
    {
        Color tmp = mySpriteRenderer.color;
        if (flashCounter > flashLength * .80f)
            tmp.a = 0f;
        else if (flashCounter > flashLength * .60f)
            tmp.a = 1f;
        else if (flashCounter > flashLength * .40f)
            tmp.a = 0f;
        else if (flashCounter > flashLength * .20f)
            tmp.a = 1f;
        else if (flashCounter > 0f)
            tmp.a = 0f;
        else
        {
            tmp.a = 1f;
            flashActive = false;
        }

        mySpriteRenderer.color = tmp;
        flashCounter -= Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sugar"))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
