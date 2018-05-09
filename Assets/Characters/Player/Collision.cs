using UnityEngine;

public class Collision : MonoBehaviour {

	void OnCollisionEnter ()
    {
        switch (GetComponent<Collider>().tag)
        {
            case "Obstacle":
                Debug.Log("Obstacle HIT");
                break;
        }
    }
}
