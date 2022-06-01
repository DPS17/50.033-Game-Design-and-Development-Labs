using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{  
    private Rigidbody2D mushroomBody;

    private int moveRight = -1;

    private Vector2 velocity;



    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomBody.AddForce(Vector2.up  *  5, ForceMode2D.Impulse);
        moveRight = Random.Range(0,2)*2-1;
        Debug.Log(moveRight);
        ComputeVelocity();

    }

    // Update is called once per frame
    void Update()
    {
        MoveMushroom();
    }

    void ComputeVelocity(){
      velocity = new Vector2(moveRight*5.0f, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {   
            velocity = Vector2.zero;
            // Debug.Log("stopping");
        };

        if (col.gameObject.CompareTag("GroundObstacle"))
        {
            moveRight *= -1;
            ComputeVelocity();
            // Debug.Log("change direction");
        };
    }

    void MoveMushroom(){
        mushroomBody.MovePosition(mushroomBody.position + velocity * Time.fixedDeltaTime);
    }

    void  OnBecameInvisible(){
	    Destroy(gameObject);	
    }
}
