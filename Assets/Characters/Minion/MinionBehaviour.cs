using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaviour : MonoBehaviour
{
    public Rigidbody rg;
    public Transform target;
    public float range = 70f;
    public string obstacleTag = "Obstacle";
    public float forcePower = 50f;
    public float speed = 5f;
    public int raycastMaxDistance = 5;

    public int hungerLevel = 0;

    private Vector3 dir;
    private Vector3 leftTurn;
    private Vector3 rightTurn;
    private RaycastHit lHit;
    private RaycastHit rHit;
    private bool obstacleHit = false;
    private float distanceToClosestTarget = Mathf.Infinity;
    private Transform obstacle;
    private string actualTagName = "Target";

    void Start()
    {
        this.hungerLevel = Random.Range(0, 100);
        InvokeRepeating("incrementHunger", 0f, 1.5f);
        InvokeRepeating("setClosestTarget", 0f, 3f);
    }


    void FixedUpdate()
    {
        if (this.distanceToClosestTarget > 4.0f)
        {
            this.goToTarget();
        }
        else
        {
            if (this.target.CompareTag("Food"))
            {
                this.eat();
            }
           
        }
    }

    void goToTarget()
    {
        if (!this.target)
        {
            return;
        }

        leftTurn = transform.position;
        rightTurn = transform.position;
        dir = (target.position - transform.position).normalized;

        leftTurn.x -= 1.5f;
        rightTurn.x += 1.5f;

        this.obstacleHit = false;
        if (Physics.Raycast(leftTurn, transform.forward, out lHit, raycastMaxDistance))
        {
            if (lHit.transform != transform && lHit.transform.tag == "Obstacle")
            {
                this.obstacleHit = true;
                Debug.DrawLine(transform.position, lHit.point, Color.red);
            }
        }

        if (Physics.Raycast(rightTurn, transform.forward, out rHit, raycastMaxDistance))
        {
            if (rHit.transform != transform && rHit.transform.tag == "Obstacle")
            {
                this.obstacleHit = true;
                Debug.DrawLine(transform.position, rHit.point, Color.red);
            }
        }

        if (target != null)
        {
            // Debug.Log("Obstacle Hit? " + this.obstacleHit);
            if (!this.obstacleHit)
            {
                // Debug.Log("Going to Target");
                rotateToTarget();
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else
            {
                if (lHit.distance > rHit.distance)
                {
                    Debug.Log("Turn Right");
                    dir.x += 10;
                    move(dir);
                }
                else
                {
                    Debug.Log("Turn Left");
                    dir.x -= 10;
                    move(dir);
                }
            }
        }
    }

    void rotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    void move(Vector3 dir)
    {
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (this.obstacle != null)
        {
            Gizmos.DrawRay(transform.position, this.obstacle.position - transform.position);
            Gizmos.DrawRay(transform.position, transform.position);
        }

        Gizmos.DrawWireSphere(transform.position, range);
    }

    void incrementHunger()
    {
        this.hungerLevel += 1;

        // Set target based on hunger level
        if (this.hungerLevel <= 50)
        {
            this.actualTagName = "Target";
        }
        else
        {
            this.actualTagName = "Food";

            if (this.hungerLevel > 75 && this.speed < 10.0f)
            {
                this.speed += .1f;
            }
        }
    }

    void eat()
    {
        Food Food = this.target.GetComponent<Food>();
        Food.foodQuantity -= .001f;
    }

    void setClosestTarget()
    {
        this.distanceToClosestTarget = Mathf.Infinity;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(this.actualTagName);

        foreach (GameObject target in targets)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            Debug.Log("Target Name: " + target.name + "Dist: " + distanceToTarget);
            if (distanceToTarget < this.distanceToClosestTarget)
            {
                this.distanceToClosestTarget = distanceToTarget;
                this.target = target.transform;

                Debug.DrawLine(transform.position, this.target.position);
            }
        }

    }
}
