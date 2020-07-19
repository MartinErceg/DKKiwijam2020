using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    public ParticleSystem rockParticles;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "Player")
        {
            rockParticles = Instantiate(rockParticles, transform.position, transform.rotation);
            Destroy(gameObject);
            //Slow down player here
        }
    }
}
