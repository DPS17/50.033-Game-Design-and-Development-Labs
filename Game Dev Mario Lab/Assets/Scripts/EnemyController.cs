using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EnemyController : MonoBehaviour
{
	public  GameConstants gameConstants;
	private  int moveRight;
	private  float originalX;
	private  Vector2 velocity;
	private  Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
	private bool playerDead = false;
    private bool flipping = false;

	void  Start()
	{
		enemyBody  =  GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();

        // subscribe to player event
        GameManager.OnPlayerDeath  +=  EnemyRejoice;
		
		// get the starting position
		originalX  =  transform.position.x;
	
		// randomise initial direction
		moveRight  =  Random.Range(0, 2) ==  0  ?  -1  :  1;
		
		// compute initial velocity
		ComputeVelocity();
	}
	
	void  ComputeVelocity()
	{
			velocity  =  new  Vector2((moveRight) *  gameConstants.maxOffset  /  gameConstants.enemyPatroltime, 0);
	}
  
	void  MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position  +  velocity  *  Time.fixedDeltaTime);
	}

	void  Update()
	{
        Debug.Log(flipping);
        Debug.Log(playerDead);

        if (playerDead){
            Debug.Log(flipping);
            if (!flipping){
                // Don't move, just flip
                StartCoroutine(flipEnemy());
            }
        }
        else{
            if (Mathf.Abs(enemyBody.position.x  -  originalX) <  gameConstants.maxOffset)
            {// move goomba
                MoveEnemy();
            }
            else
            {
                // change direction
                moveRight  *=  -1;
                ComputeVelocity();
                MoveEnemy();
            }
        }
	}

    // animation when player is dead
    void  EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        // ...
        playerDead = true;
        StartCoroutine(flipEnemy());
    }

    private IEnumerator flipEnemy()
    {   
        flipping = true;
        enemySprite.flipX = !enemySprite.flipX;
        yield return new WaitForSeconds(0.5f);
        flipping = false;
    }

    void  OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
                CentralManager.centralManagerInstance.killEnemy();
			}
			else{
				// hurt player
			    CentralManager.centralManagerInstance.damagePlayer();
			}
		}
	}

    void  KillSelf(){
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
	}
}