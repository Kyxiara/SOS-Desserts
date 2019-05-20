using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites = new Sprite[4];

    [SerializeField]
    private float groundBoundary;

    [SerializeField]
    private int triggerDistance;

    [SerializeField]
    private float speed;

    private float initialPosition;

    private GameObject player;

    private SpriteRenderer mySpriteRenderer;

    private bool attack;
    private bool pullUP;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        attack = false;
        pullUP = false;
        initialPosition = mySpriteRenderer.transform.position.y;
        StartCoroutine(Idle());
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (attack)
        {
            mySpriteRenderer.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * speed;
            if (mySpriteRenderer.transform.position.y <= groundBoundary)
            {
                attack = false;
                pullUP = true;
            } 
        }
        if (pullUP)
        {
            mySpriteRenderer.transform.position += new Vector3(0, 1, 0) * Time.deltaTime * speed;
            if (mySpriteRenderer.transform.position.y >= initialPosition)
            {
                pullUP = false;
                mySpriteRenderer.sprite = sprites[0];
                StartCoroutine(Idle());
            }
        }
	}

    private IEnumerator Idle()
    {
        while (Mathf.Abs(player.transform.position.x - transform.position.x) > triggerDistance)
        {
            int rand = Random.Range(0, 3);
            mySpriteRenderer.sprite = sprites[rand];
            yield return new WaitForSeconds(1f);
        }
        attack = true;
        mySpriteRenderer.sprite = sprites[3];
    }
}
