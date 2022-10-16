using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{   
    Vector3 targetPos;
    float moveSpeed;
    float flipSpeed;
    float baseScale;
    bool isFlipping;
    bool posInit;

    // Start is called before the first frame update
    void Start()
    {      
        moveSpeed = 0.05f;
        flipSpeed = 0.01f;
        baseScale = 0.2f;
        isFlipping = true;
        if (!posInit) {
            SetTargetPos(transform.position);
        }
        //Debug.Log(targetPos);
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed);
        if (isFlipping) {
            transform.localScale = new Vector3(transform.localScale.x-(flipSpeed*baseScale),baseScale,baseScale);
            if (transform.localScale.x <= 0) {
                isFlipping = false;
                if (tag == "FaceDown") {
                    tag = "FaceUp";
                } else {
                    tag = "FaceDown";
                }
            }
        } else if (transform.localScale.x < baseScale) {
            transform.localScale = new Vector3(transform.localScale.x+(flipSpeed*baseScale),baseScale,baseScale);
        }
    }

    void SetTargetPos(Vector3 pos) {
        targetPos = pos;
        posInit = true;
    }
}
