using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : MonoBehaviour {

    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private GameObject pieceWatermelonPrefab;

    [SerializeField]
    private BoxCollider2D watermelonCollider;

    [SerializeField]
    private BoxCollider2D watermelonTrigger;

    [SerializeField]
    private int health;

    private bool flashActive;

    [SerializeField]
    private float flashLength;

    private float flashCounter;

    private GameObject player;

    [SerializeField]
    private double distanceActivation;

    private bool canInstanciate;

    void Start () {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        canInstanciate = true;
        Physics2D.IgnoreCollision(watermelonCollider, pieceWatermelonPrefab.gameObject.GetComponent<BoxCollider2D>(), true);
    }

	void Update () {
        if (flashActive) SystemFlash();
        if (Vector2.Distance(player.transform.position, transform.position + new Vector3(12.7f, 0, 0)) < distanceActivation && canInstanciate)
        {
            canInstanciate = false;
            StartCoroutine(instanciatePieceWatermelon());
        }
    }

    IEnumerator instanciatePieceWatermelon()
    {
        GameObject pieceWatermelon = Instantiate(pieceWatermelonPrefab, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(watermelonCollider, pieceWatermelon.GetComponent<BoxCollider2D>(), true);
        pieceWatermelon.GetComponent<PieceWatermelon>().Initialize(transform.position);
        yield return new WaitForSeconds(4f);
        canInstanciate = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sugar") && !flashActive)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(watermelonCollider, other.gameObject.GetComponent<BoxCollider2D>(), true);
        }
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
        Color outside = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
        Color inside = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
        if (flashCounter > flashLength * .80f)
        {
            outside.a = 0f;
            inside.a = 0f;
        }
        else if (flashCounter > flashLength * .60f)
        {
            outside.a = 1f;
            inside.a = 1f;
        }
        else if (flashCounter > flashLength * .40f)
        {
            outside.a = 0f;
            inside.a = 0f;
        }
        else if (flashCounter > flashLength * .20f)
        {
            outside.a = 1f;
            inside.a = 1f;
        }
        else if (flashCounter > 0f)
        {
            outside.a = 0f;
            inside.a = 0f;
        }
        else
        {
            outside.a = 1f;
            inside.a = 1f;
            flashActive = false;
        }

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = outside;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = inside;
        flashCounter -= Time.deltaTime;
    }
}
