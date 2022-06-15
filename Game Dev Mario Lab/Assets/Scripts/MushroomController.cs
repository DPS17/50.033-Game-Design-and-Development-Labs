using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{  
    private Rigidbody2D mushroomBody;

    private int moveRight = -1;

    private Vector2 velocity;

    private bool collected = false;
    // private bool enlarge = false;
    // private Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);
    // private float enlargeSpeed = 0.1f;
    private SpriteRenderer mushroomSprite;



    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomSprite = GetComponent<SpriteRenderer>();
        mushroomBody.AddForce(Vector2.up  *  5, ForceMode2D.Impulse);
        moveRight = Random.Range(0,2)*2-1;
        Debug.Log(moveRight);
        ComputeVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        MoveMushroom();
        if (collected){
            StartCoroutine(ScaleOverTime(0.2f));
        }
    }

    void ComputeVelocity(){
      velocity = new Vector2(moveRight*5.0f, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {   
            velocity = Vector2.zero;
            collected = true;
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

    // void  OnBecameInvisible(){
	//     Destroy(gameObject);	
    // }

    IEnumerator ScaleOverTime(float time){
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(0.3f,0.3f,0.3f);
        float enlargeTime = 0.0f;
        float shrinkTime = 0.0f;

        do {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, enlargeTime/time);
            enlargeTime += Time.deltaTime;
            yield return null;
        } while (enlargeTime <= time);
        do {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, shrinkTime/time);
            shrinkTime += Time.deltaTime;
            yield return null;
        } while (shrinkTime <= time);
        // Destroy(gameObject);
        mushroomSprite.enabled = false;
    }
}
