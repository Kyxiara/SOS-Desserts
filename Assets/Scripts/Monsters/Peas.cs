using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peas : MonoBehaviour {

    [SerializeField]
    private GameObject peaPrefab;

    // Use this for initialization
    void Start () {
        StartCoroutine(instanciatePea());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator instanciatePea()
    {
        GameObject pea = Instantiate(peaPrefab, new Vector3(transform.position.x + 0.558f, transform.position.y - 0.611f,transform.position.z), Quaternion.identity);
        pea.GetComponent<Pea>().Initialize(Vector2.left);
        yield return new WaitForSeconds(3f);
        pea = Instantiate(peaPrefab, new Vector3(transform.position.x + 0.558f, transform.position.y - 0.611f, transform.position.z), Quaternion.identity);
        pea.GetComponent<Pea>().Initialize(Vector2.right);
        yield return new WaitForSeconds(3f);
        StartCoroutine(instanciatePea());
    }
}
