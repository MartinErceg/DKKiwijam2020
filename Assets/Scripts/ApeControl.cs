using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApeControl : MonoBehaviour
{
    public float acceleration;
    public float minSpeed;
    public float maxSpeed;
   
    public float sideSpeed;

    public float gravity;
    public float maxGravity = 4;

    public float jumpImpulse;
    public float slamImpulse;
    
    public float launchAngle;

    public float charHeight;

    private float speed;
    private float velocityY;
    private float velocityX;
    private bool falling;
    private bool jumping;

    private Vector3 momentumVec;
    private Vector3 lastFrameMomentum;

    private const float stepHeight = 0.2f;


    public Animator anim;
    private AudioSource AudioS;

    private void Start()
    {
        anim = GetComponent<Animator>();
        AudioS = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(speed<maxSpeed)
        {
            speed += acceleration * Time.deltaTime;            
        }

        //Debug.Log("GROUND DIF: "+CheckGround());       
        float groundDif = CheckGround();

        // On Ground
        if(!falling)
        {
            // Ground Below
            if(groundDif < 0)
            {
                if(-groundDif < stepHeight)
                {
                    //print("*******************************  SNAP - GROUND BELOW");
                    Vector3 snapPos = new Vector3(transform.position.x, transform.position.y + groundDif, transform.position.z);
                    transform.position = snapPos;
                }
                else if(-groundDif < stepHeight - (momentumVec.y * speed))
                {                                      
                    Vector2 from = new Vector2(transform.position.z + 1, transform.position.y);
                    Vector2 to = new Vector2(lastFrameMomentum.z, lastFrameMomentum.y);
                    
                    float slopeAngle = Vector2.Angle(from, to);                    
                    if (slopeAngle < launchAngle)
                    {
                        //print("************************************************************************* Falling Speed Keep on Slope");
                        Vector3 snapPos = new Vector3(transform.position.x, transform.position.y + groundDif, transform.position.z);
                        transform.position = snapPos;
                    }
                    else // LAUNCH
                    {
                        //print("---------------------------------------------------------------------------------------------------------------------------Launch");
                        velocityY = lastFrameMomentum.y * speed;
                        falling = true;
                    }                    
                }
                else
                {
                    //print("stepheight: " + stepHeight + " StepHeight+Mom " + (stepHeight - (momentumVec.y * speed)) + " GroundDif: "+ (-groundDif) );
                    //print("****************** ______________________________________________________________FALLING");
                    //Debug.Log("___________________________________last frame Momentum: "+ lastFrameMomentum.y + " speed: "+(lastFrameMomentum.y * speed));                    

                    velocityY = lastFrameMomentum.y * speed;
                    falling = true;
                    
                }
            } // Ground Above
            else if(groundDif > 0)
            {
                //print("```````````````````````````````````````````````````` SNAP - GROUND ABOVE");
                if(groundDif < stepHeight)
                {
                    Vector2 from = new Vector2(transform.position.z + 1, transform.position.y);
                    Vector2 to = new Vector2(lastFrameMomentum.z, lastFrameMomentum.y);

                    float slopeAngle = Vector2.Angle(from, to);                    

                    if (slopeAngle < launchAngle)
                    {
                        //print("-------------------------------------------------------------------------************** ------------- Snapping Ground");
                        Vector3 snapPos = new Vector3(transform.position.x, transform.position.y + groundDif, transform.position.z);
                        transform.position = snapPos;
                    }
                    else // LAUNCH
                    {
                        //print("------------------------------------------------------------------------------------------------------ launch");
                        velocityY = lastFrameMomentum.y * speed;
                        falling = true;                                                
                    }                    
                }
                else if(groundDif < charHeight)
                {
                    //print("*************** Climbing");
                    
                    if(speed > minSpeed)
                        speed -= acceleration;

                    //float climbHeight = ClimbCheck();

                    /*if(climbHeight != -1)
                    {
                        Vector3 snapPos = new Vector3(transform.position.x, climbHeight, transform.position.z);
                        transform.position = snapPos;
                    }*/
                    //else
                    //{
                        Vector3 snapPos = new Vector3(transform.position.x, transform.position.y + groundDif, transform.position.z);
                        transform.position = snapPos;
                    //}                    
                }
            }


            // JUMP
            if(Input.GetKeyDown("space"))
            {
                falling = true;
                velocityY = jumpImpulse;
                print("jumping");
            }

        }
        else if(falling)
        {
            velocityY -= gravity * Time.deltaTime;

            // Check the final position with the new velocity to make sure we dont fall thru anything
            if (CheckFall() > Mathf.NegativeInfinity)
            {
                //Debug.Log("========================================================================================= LANDING - groundDif: " + groundDif);
                Vector3 snapPos = new Vector3(transform.position.x, transform.position.y + groundDif, transform.position.z);
                transform.position = snapPos;

                velocityY = 0;

                falling = false;
            }
            else
            {
                //print("KeepFalling: " + velocityY);
            }

            // Slam
            if(Input.GetKeyDown("down"))
            {
                velocityY += slamImpulse;
                anim.SetBool("Slam", true);
                AudioS.Stop();
            }
        }

        // Horizontal 
        velocityX = 0;

        if(Input.GetKey("left"))
        {
            velocityX = -sideSpeed;
            anim.SetBool("Left", true);
        }
        else if(Input.GetKey("right"))
        {
            velocityX = sideSpeed;
            anim.SetBool("Right", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Left", true);
            anim.SetBool("Jump", true);
            AudioS.Stop();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Right", true);
            anim.SetBool("Jump", true);
            AudioS.Stop();
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("Jump", true);
            AudioS.Stop();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("Slam", true);
            AudioS.Stop();
        }
        else
        {
            if (!AudioS.isPlaying)
            {
                AudioS.Play();
            }
            anim.SetBool("Idle", true);
            anim.SetBool("Slam", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
            anim.SetBool("Jump", false);
        }

        // Final positioning

        Vector3 newPos = new Vector3(transform.position.x + velocityX, transform.position.y + velocityY, transform.position.z + speed);
        transform.position = newPos;

        // Momentum Vector
        Debug.DrawLine(transform.position, (transform.position + (momentumVec + lastFrameMomentum) ), Color.green);        
        //Debug.Log("Falling Momentum: " + (momentumVec + lastFrameMomentum).y + "momentumY: " + momentumVec.y);        
        lastFrameMomentum = momentumVec;
    }

    // Check Ground
    float CheckGround()
    {
        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y+charHeight, transform.position.z);

        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(rayStart, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity, 1))
        {
            //Debug.DrawRay(rayStart, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            //Debug.Log("position y: "+ transform.position.y + " hit Y: "+hit.point.y);
           
            momentumVec = hit.normal;
            momentumVec = Quaternion.AngleAxis(-90, Vector3.left) * momentumVec;
          
            return hit.point.y - transform.position.y;
        }
        
        return Mathf.NegativeInfinity;
    }

    float CheckFall()
    {
        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + charHeight, transform.position.z);       
        
        if(Physics.Raycast(rayStart, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Abs((transform.position.y+velocityY*2) - (transform.position.y + charHeight)) , 1))
        {
            //Debug.DrawRay(rayStart, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            //Debug.Log("position y: "+ transform.position.y + " hit Y: "+hit.point.y);
            return hit.point.y;
        }

        Vector3 rayEnd = new Vector3(rayStart.x, rayStart.y - Mathf.Abs((transform.position.y + velocityY) - (transform.position.y + charHeight)), rayStart.z);

        //Debug.DrawRay(rayStart, transform.TransformDirection(Vector3.down), Color.red);
        Debug.DrawLine(rayStart, rayEnd, Color.red);

        return Mathf.NegativeInfinity;
    }


    float ClimbCheck()
    {
        Vector3 rayStart = new Vector3(transform.position.x, transform.position.y + charHeight, transform.position.z);

        if (Physics.Raycast(rayStart, transform.TransformDirection(Vector3.down), out RaycastHit hit, Mathf.Infinity, 1))
        {
            Debug.DrawRay(rayStart, transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
            //Debug.Log("position y: " + transform.position.y + " hit Y: " + hit.point.y);
            return hit.point.y;
        }

        return -1;
    }

}
