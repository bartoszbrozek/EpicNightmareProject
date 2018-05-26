using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject spawnObject;

    public float spawnInterval = 20.0f;

    public float rangeMinX = 0;
    public float rangeMaxX = 50;
    public float rangeMinY = 0;
    public float rangeMaxY = 50;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("spawn", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawn()
    {
        float x = this.transform.position.x - Random.Range(rangeMinX, rangeMaxX);
        float y = this.transform.position.y - Random.Range(rangeMinY, rangeMaxY);
        float z = 1.5f;

        //Instantiate(this.spawnObject, new Vector3(x, z, y), Quaternion.identity);
        Instantiate(this.spawnObject, transform.position + (transform.right * Random.Range(rangeMinX, rangeMaxX)), transform.rotation);
    }
}
