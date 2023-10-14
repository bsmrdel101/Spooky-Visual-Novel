using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    Vector3 pos;
    bool movingBool;
    public int targetX, movingSpeed=1, placingPosition=1300;
    float timer=0; 
    
    void Update()
    {
        if(movingBool){
            timer += Time.deltaTime;
            if(timer > 0.01f){
                timer = 0;
                pos = this.transform.localPosition;
                if((int) pos.x > targetX){
                    pos.x -= movingSpeed;
                    if(pos.x <= targetX){
                        pos.x = targetX;
                        movingBool = false;
                    }
                }
                if((int) pos.x < targetX){
                    pos.x += movingSpeed;
                    if(pos.x >= targetX){
                        pos.x = targetX;
                        movingBool = false;
                    }
                }
                this.transform.localPosition = pos;
            }
             
        }
    }

    public void OrderToMove (bool setOnLeft,bool setOnRight,  bool moveBool, int X){
        pos = this.transform.localPosition;
        if(setOnLeft){
            pos.x = -1300; //- placingPosition;d
            this.transform.localPosition = pos;
        }
        if(setOnRight){
            pos.x = 1300 ; //placingPosition;
            this.transform.localPosition = pos;
        }
        if(moveBool){
            movingBool = true;
            targetX = X;
        }
    }
}
