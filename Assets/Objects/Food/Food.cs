using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour {

    public float foodQuantity = 100.0f;

    public Text stats;

    void Start () {
        foodQuantity = Random.Range(10.0f, 150.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (foodQuantity <= 0)
        {
            Destroy(this.gameObject);
        }

        stats.text = "Food Quantity: " + foodQuantity;

    }
}
