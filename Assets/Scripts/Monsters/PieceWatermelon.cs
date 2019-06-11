using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceWatermelon : MonoBehaviour {

    [SerializeField]
    private float speed;

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

    [SerializeField]
    private BoxCollider2D pieceWatermelonCollider;

    [SerializeField]
    private GameObject watermelon;

    [SerializeField]
    private double distanceMinBegin;

    private double distanceInitial;

    private bool chasing;

    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");      
        distanceInitial = mySpriteRenderer.transform.position.x;
        chasing = false;
    }
	
	void FixedUpdate () {
        if (distanceInitial - mySpriteRenderer.transform.position.x <= distanceMinBegin && !chasing)
            myRigidbody.velocity = new Vector2(-1 * speed, myRigidbody.velocity.y);
        else if (!chasing)
        {
            chasing = true;
            mySpriteRenderer.sortingOrder = 1;
        }
        if (chasing)
        {            
            horizontalMouvement = HandleMouvement();
            HandleAnimation(horizontalMouvement);
        }
        if (flashActive) SystemFlash();
    }

    public void Initialize(Vector3 pos)
    {
        Physics2D.IgnoreCollision(pieceWatermelonCollider, watermelon.GetComponent<BoxCollider2D>(), true);
        transform.position = new Vector3(pos.x + 12.7f, pos.y - 0.3f, 0);
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
    }

    public IEnumerator TakeDamage()
    {
        health -= 1;
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

    void OnTriggerEnter2D(Collider2D other)
    {    
        if (other.CompareTag("Sugar") && !flashActive)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(pieceWatermelonCollider, other.gameObject.GetComponent<BoxCollider2D>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(pieceWatermelonCollider, other.gameObject.GetComponent<BoxCollider2D>(), false);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
