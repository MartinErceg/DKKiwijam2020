using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    bool Hit = false;
    private float timeRemaining = 10;

    public float maximumTime;

    public float toppleSpeed;

    private int direction;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = maximumTime;
    }

    // Update is called once per frame
    void Update()
    {   
        Topple();
    }

    void Topple()
    {
        if(Hit)
        {
            if(direction > 1 && direction < 5)
            {
                transform.Rotate(Vector3.right, -toppleSpeed * Time.deltaTime);
            }
            else if(direction > 5 && direction < 10)
            {

                transform.Rotate(Vector3.right, toppleSpeed * Time.deltaTime);
            }
        }

        if (timeRemaining > 0 && Hit)
        {
            timeRemaining -= Time.deltaTime;
        }

        if(timeRemaining <= 0)
        {
            timeRemaining = maximumTime;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            direction = Random.Range(1, 10);
            Hit = true;
            //Slow down player here
        }
    }
}
