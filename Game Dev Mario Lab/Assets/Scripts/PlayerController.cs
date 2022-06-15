using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D marioBody;
    private BoxCollider2D marioCollider;

    public float maxSpeed = 10;
    public float upSpeed = 10;
    private bool onGroundState = true;

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // public Transform enemyLocation;
    // public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;

    private GameObject restartButton;

    private  Animator marioAnimator;
    private  AudioSource marioAudio;
    private ParticleSystem dustCloud;

    public AudioClip marioJump;
    public AudioClip marioDie;

        
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioCollider = GetComponent<BoxCollider2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator  =  GetComponent<Animator>();
        marioAudio  =  GetComponent<AudioSource>();
        dustCloud = GetComponentInChildren<ParticleSystem>();
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence;
        restartButton = GameObject.Find("restart");
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
            if (Mathf.Abs(marioBody.velocity.x) >  0.05){
                marioAnimator.SetTrigger("onSkid");
            } 
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) >  0.05){
                marioAnimator.SetTrigger("onSkid");
            } 
        }

        if (Input.GetKeyDown("z")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }

        // // when jumping, and Gomba is near Mario and we haven't registered our score
        // if (!onGroundState)
        // // && countScoreState)
        // {
        //     if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //     {
        //         countScoreState = false;
        //         score++;
        //         // Debug.Log(score);
        //     }
        // }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

    }

    void  FixedUpdate()
    {
        // dynamic rigidbody
      float moveHorizontal = Input.GetAxis("Horizontal");
      if (Mathf.Abs(moveHorizontal) > 0){
          Vector2 movement = new Vector2(moveHorizontal, 0);
          if (marioBody.velocity.magnitude < maxSpeed)
                  marioBody.AddForce(movement * speed);
      }
      if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
          // stop
          marioBody.velocity = Vector2.zero;
      }

      if (Input.GetKeyDown("space") && onGroundState)
      {
        marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        onGroundState = false;
        marioAnimator.SetBool("onGround", onGroundState);

        // countScoreState = true; //check if Gomba is underneath
      }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
         if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacle") || col.gameObject.CompareTag("GroundObstacle"))
        {   
            if (!onGroundState){
                dustCloud.Play();
            }
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            // Time.timeScale = 0.0f;
            restartButton.SetActive(true);
        }
    }

    void  PlayJumpSound(){
        // if (!marioAudio.isPlaying){
        marioAudio.PlayOneShot(marioJump);

        // }
    }

    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        // ...
        marioAudio.PlayOneShot(marioDie);
        marioBody.AddForce(Vector2.up  *  50, ForceMode2D.Impulse);
        marioBody.gravityScale = 4;
        marioCollider.enabled = false;
        onGroundState = false;
        marioAnimator.SetBool("onGround", onGroundState);
        

    }
}
