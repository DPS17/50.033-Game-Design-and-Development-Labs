using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBrick : MonoBehaviour
{   
    private bool broken = false;
    public GameObject debris; // the spawned mushroom prefab
    public GameObject coin;
    private  AudioSource breakAudio;
    private SpriteRenderer brickSprite;


    // Start is called before the first frame update
    void Start()
    {
        breakAudio  =  GetComponent<AudioSource>();
        brickSprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void  OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Player") &&  !broken){
            breakAudio.Play();
            brickSprite.enabled = false;
            broken  =  true;
            // assume we have 5 debris per box
            for (int x =  0; x<5; x++){
                Instantiate(debris, transform.position, Quaternion.identity);
            }
            Vector3 coinPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Instantiate(coin, coinPosition, Quaternion.identity);
            // gameObject.transform.parent.GetComponent<SpriteRenderer>().enabled  =  false;
            // gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled  =  false;
            // GetComponent<EdgeCollider2D>().enabled  =  false;
            Destroy(gameObject, 1f);
        }
    }

}
