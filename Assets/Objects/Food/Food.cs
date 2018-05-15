using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    public float foodQuantity = 100.0f;

	void Start () {
        foodQuantity = Random.Range(1.0f, 100.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
