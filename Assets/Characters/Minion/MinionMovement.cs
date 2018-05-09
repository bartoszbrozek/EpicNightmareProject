using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : MonoBehaviour
{
    public Rigidbody rg;
    public Transform target;
    public float range = 70f;
    public string obstacleTag = "Obstacle";
    public float forcePower = 50f;
    public float speed = 5f;
    public int raycastMaxDistance = 5;
    private Vector3 dir;
    private Vector3 leftTurn;
    private Vector3 rightTurn;
    private RaycastHit lHit;
    private RaycastHit rHit;
    private bool obstacleHit = false;
    private float distanceToTarget;
    private Transform obstacle;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }


    void UpdateTarget()
    {
        if (target == null)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");

            foreach (GameObject target in targets)
            {
                if (target != null)
                {
                    this.target = target.transform;
                    break;
                }
            }
        }

        this.distanceToTarget = Vector3.Distance(this.target.position, transform.position);
        Debug.Log("Distance to Target: " + this.distanceToTarget);

        leftTurn = transform.position;
        rightTurn = transform.position;
        dir = (target.position - transform.position).normalized;

        leftTurn.x -= 1.5f;
        rightTurn.x += 1.5f;

    }

    void FixedUpdate()
    {
        if (this.distanceToTarget < 4.0f)
        {
            return;
        }

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
            Debug.Log("Obstacle Hit? " + this.obstacleHit);
            if (!this.obstacleHit)
            {
                Debug.Log("Going to Target");
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
}
