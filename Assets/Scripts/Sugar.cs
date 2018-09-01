using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sugar : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;
    private Vector2 direction;

	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        myRigidbody.velocity = direction * speed;
	}

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }
}
