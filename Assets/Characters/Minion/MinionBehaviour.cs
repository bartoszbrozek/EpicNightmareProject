using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinionBehaviour : MonoBehaviour
{
    public Rigidbody rg;
    public Transform target;
    public float range = 70f;
    public string obstacleTag = "Obstacle";
    public float forcePower = 50f;
    public float speed = 5f;
    public int raycastMaxDistance = 5;

    public float hungerLevel = 0;
    public float eatingSpeed = .005f;
    public float minimalHungerLevelToEat = 50f;
    public float eatUntil = 10;
    public float hungerDeathLevel = 100f;

    public Text stats;

    private Vector3 dir;
    private Vector3 leftTurn;
    private Vector3 rightTurn;
    private RaycastHit lHit;
    private RaycastHit rHit;
    private bool obstacleHit = false;
    private float distanceToClosestTarget = Mathf.Infinity;
    private Transform obstacle;
    private string actualTagName = "Target";

    private bool isEating = false;
    private float creationTime;

    private string[] names = new string[] {
        "Janusz", "Helena", "Kulfonik", "Stefan", "Inka",
        "Adolf Hitler", "Miś Uszatek", "Kuba Rozpruwacz",
        "Patologiczna Gruźlica", "Krul Wszehświata", "Bartuś",
        "Jarosław Kaczyński", "Donald Tusk", "Jelito Rozpaczy",
        "Pan Tadeusz", "Stanisław Poniatowski", "Generał Jebadło",
        "Puszczalski Jarosław", "Henryk", "Belzebub", "Behemoth",
        "Katarzyna Lubnauer", "Kamila Gasiuk-Pihowicz","Tadeusz Grabarek",
        "Jerzy Meysztowicz","Witold Zembaczyński","Ryszard Petru",
        "Joanna Scheuring-Wielgus","Joanna Schmidt","Michał Stasiński",
        "Kononowicz"
    };


    void Start()
    {
        GameManager.numberBorn++;
        this.creationTime = Time.time;
        this.name = this.names[Random.Range(0, names.Length)];
        this.hungerLevel = Random.Range(1, 90);
        this.speed = Random.Range(3f, 7f);
        this.eatingSpeed = Random.Range(.002f, .01f);
        this.minimalHungerLevelToEat = Random.Range(40f, 70f);
        this.eatUntil = Random.Range(0f, 25f);
        this.hungerDeathLevel = Random.Range(80f, 150f);

        InvokeRepeating("incrementHunger", 0f, 1.5f);
        InvokeRepeating("setClosestTarget", 0f, 3f);
    }


    void FixedUpdate()
    {
        if (this.distanceToClosestTarget > 4.0f)
        {
            this.isEating = false;
            this.goToTarget();
            this.thinkAboutEating();
        }
        else
        {
            this.thinkAboutEating();

            if (this.target != null && this.target.CompareTag("Food"))
            {
                this.eat();
            }
        }

        stats.text = this.name + "\nHunger: " + this.hungerLevel;
    }

    void thinkAboutEating()
    {
        if (this.hungerLevel > this.eatUntil && this.isEating || this.hungerLevel > this.minimalHungerLevelToEat && !this.isEating)
        {
            this.actualTagName = "Food";
        }
        else
        {
            this.isEating = false;
            this.actualTagName = "Target";
        }
    }

    void goToTarget()
    {
        if (!this.target)
        {
            return;
        }

        this.isEating = false;

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
                    //Debug.Log("Turn Right");
                    dir.x += 10;
                    move(dir);
                }
                else
                {
                    //Debug.Log("Turn Left");
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
        if (!this.isEating)
        {
            this.hungerLevel += 1;
        }

        if (this.hungerLevel > this.hungerDeathLevel)
        {
            Destroy(this.gameObject);
            GameManager.numberDead++;
            Debug.Log("Oh noes! " + this.name + " is really DEAD. He lived " + (Time.time - this.creationTime) + " seconds");
        }
    }

    void eat()
    {
        this.isEating = true;
        Food Food = this.target.GetComponent<Food>();

        if (Food)
        {
            Food.foodQuantity -= this.eatingSpeed;
        }

        if (this.hungerLevel > 0)
        {
            this.hungerLevel -= (float)this.eatingSpeed * 5f;
        }
    }

    void setClosestTarget()
    {
        this.distanceToClosestTarget = Mathf.Infinity;
        GameObject[] targets = GameObject.FindGameObjectsWithTag(this.actualTagName);

        foreach (GameObject target in targets)
        {
            float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
            // Debug.Log("Target Name: " + target.name + "Dist: " + distanceToTarget);
            if (distanceToTarget < this.distanceToClosestTarget)
            {
                this.distanceToClosestTarget = distanceToTarget;
                this.target = target.transform;

                Debug.DrawLine(transform.position, this.target.position);
            }
        }

    }
}
