using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peas : MonoBehaviour {

    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private GameObject peaPrefab;

    [SerializeField]
    private BoxCollider2D peasCollider;

    [SerializeField]
    private BoxCollider2D peasTrigger;

    [SerializeField]
    private int health;

    private bool flashActive;

    [SerializeField]
    private float flashLength;

    private float flashCounter;

    // Use this for initialization
    void Start () {
        Physics2D.IgnoreCollision(peasCollider, peasTrigger, true);
        StartCoroutine(instanciatePea());
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (flashActive) SystemFlash();
    }

    IEnumerator instanciatePea()
    {
        Vector2[] direction = { Vector2.left, Vector2.right };
        GameObject pea = Instantiate(peaPrefab, new Vector3(transform.position.x + 0.558f, transform.position.y - 0.611f,transform.position.z), Quaternion.identity);
        pea.GetComponent<Pea>().Initialize(direction[Random.Range(0, 2)]);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(instanciatePea());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sugar"))
        {
            StartCoroutine(TakeDamage());
        }
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(peasCollider, other.gameObject.GetComponent<BoxCollider2D>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Monster")
        {
            Physics2D.IgnoreCollision(peasCollider, other.gameObject.GetComponent<BoxCollider2D>(), false);
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
}
