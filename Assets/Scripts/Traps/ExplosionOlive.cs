using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionOlive : MonoBehaviour {

    private ParticleSystem ps;
    private bool playerTouched;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();
        playerTouched = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other == GameObject.Find("Player") && !playerTouched)
        {
            playerTouched = true;
            StartCoroutine(other.GetComponent<Player>().TakeDamage());
        }
    }
}
