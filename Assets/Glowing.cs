using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Glowing : MonoBehaviour
{
    public float interval = 0.05f, power = 0.005f;
    float timer = 0;
    Vector4 v4Color = new Vector4 (0,0,0,0);
    public Image myImage;
    public bool glowUP, glowDown;
    void Update()
    {
        if(glowUP || glowDown){
            timer += Time.deltaTime;
            if(timer > interval){
                timer = 0;
                v4Color = (Vector4) myImage.color;
                if(glowUP) {v4Color.w += power;}
                if(glowDown) {v4Color.w -= power;}
                if(v4Color.w >= 0.20f){
                    glowUP = false; glowDown = true;
                }
                if(v4Color.w <= 0f){
                    v4Color.w = 0;
                    glowUP = true; glowDown = false;
                }
                myImage.color = v4Color;
            }
        }
    }
}
