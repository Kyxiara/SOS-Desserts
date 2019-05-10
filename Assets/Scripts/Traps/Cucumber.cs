using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : MonoBehaviour {

    [SerializeField]
    private GameObject pieceCucumberPrefab;

    // Use this for initialization
    void Start () {
        StartCoroutine(instanciatePieceCucumber());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator instanciatePieceCucumber()
    {
        Instantiate(pieceCucumberPrefab, new Vector3(transform.position.x + 0.082f, transform.position.y - 0.942f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(instanciatePieceCucumber());
    }
}
