using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pea : MonoBehaviour {

    private float speed;
    private Rigidbody2D myRigidbody;
    private Vector2 direction;

    [SerializeField]
    private BoxCollider2D peaCollider;

    [SerializeField]
    private BoxCollider2D peaTrigger;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        speed = 5f;
        Physics2D.IgnoreCollision(peaCollider, peaTrigger, true);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(direction.x * speed, myRigidbody.velocity.y);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
        if (direction == Vector2.left)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Obstacle" || other.gameObject.name == "Background")
        {
            Physics2D.IgnoreCollision(peaCollider, other.gameObject.GetComponent<BoxCollider2D>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Obstacle" || other.gameObject.name == "Background")
        {
            Physics2D.IgnoreCollision(peaCollider, other.gameObject.GetComponent<BoxCollider2D>(), false);
        }
    }
}
