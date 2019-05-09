using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Olive : MonoBehaviour {

    private Rigidbody2D myRigidbody;
    private bool explode;
    private float explodeCounter;

    [SerializeField]
    private float explodeLength;

    [SerializeField]
    private float sizeGrow;

    [SerializeField]
    private GameObject particlesExplosion;

    private GameObject player;

    [SerializeField]
    private float distanceStart;

    // Use this for initialization
    void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        explode = false;
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (explode) Explode();
        if (Vector2.Distance(player.transform.position, transform.position) < distanceStart && !explode)
        {
            explode = true;
            explodeCounter = explodeLength;
        }
    }

    private void Explode()
    {
        if (explodeCounter > explodeLength * .80f)
            myRigidbody.transform.localScale += new Vector3(sizeGrow, sizeGrow, 0);
        else if (explodeCounter > explodeLength * .65f)
            myRigidbody.transform.localScale -= new Vector3(sizeGrow, sizeGrow, 0);
        else if (explodeCounter > explodeLength * .45)
            myRigidbody.transform.localScale += new Vector3(sizeGrow, sizeGrow, 0);
        else if (explodeCounter > explodeLength * .30f)
            myRigidbody.transform.localScale -= new Vector3(sizeGrow, sizeGrow, 0);
        else if (explodeCounter > 0f)
            myRigidbody.transform.localScale += new Vector3(sizeGrow, sizeGrow, 0);
        else
        {
            GameObject particles = Instantiate(particlesExplosion, transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
        explodeCounter -= Time.deltaTime;
    }
}
