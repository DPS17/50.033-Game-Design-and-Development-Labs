using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{   
    void Update(){
        Vector3 currPosition = transform.position;
        Vector3 axis = new Vector3(0,1,0);
        transform.RotateAround(currPosition,axis, 5);
    }
    void  OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            // update UI
            GetComponent<AudioSource>().Play();
            CentralManager.centralManagerInstance.increaseScore();
            CentralManager.centralManagerInstance.collectCoin();
            StartCoroutine(ScaleOverTime(0.1f));
        }
    }

    IEnumerator ScaleOverTime(float time){
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = new Vector3(1.5f,1.5f,1.5f);
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
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
