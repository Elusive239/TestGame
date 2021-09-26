using System;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float speed, padding;
    public Vector2 boundsMin, boundsMax;
    private Transform self;
    public bool moving = false;
    private Vector3 target;

    public void Awake(){
        self = this.transform;
        Vector3 upperBounds = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width,  (float)Screen.height));
        Vector3 lowerBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        boundsMin = new Vector2(
            lowerBounds.x - padding,
            lowerBounds.y - padding/2
        );
        boundsMax = new Vector2(
            upperBounds.x + padding,
            upperBounds.y + padding/2
        );
    }

    public Vector3 center => Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / 2.0f, (float)Screen.height / 2.0f, -10));

    public void Update(){
        if(moving){
            // if (Input.touchCount < 1){
            //     moving = false;
            //     return;
            // }

            
            // target = new Vector3 (
            //     Mathf.Clamp(target.x, boundsMin.x, boundsMax.x), 
            //     Mathf.Clamp(target.y, boundsMin.y, boundsMax.y),                    
            //     -10f
            // );
            float dist = Vector3.Distance(center, target);
            Debug.Log("Distance from center: " + dist);
            if( dist>0.2){
                Vector3 result = Vector3.MoveTowards(center, target, speed * Time.deltaTime);
                self.position = new Vector3 (
                Mathf.Clamp(result.x, boundsMin.x, boundsMax.x), 
                Mathf.Clamp(result.y, boundsMin.y, boundsMax.y),                    
                -10f);
                float testDist = Vector3.Distance(center, target);
                if(dist == testDist){
                    moving = false;
                    return;
                }
            }else{
                self.position = target;
                moving = false;
                return;
            }
            return;
        }

        // Handle screen touches.
        if (Input.touchCount < 1)
            return;
    
            Touch touch = Input.GetTouch(0);

            // Move if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began )//|| touch.phase == TouchPhase.Stationary)
            {
                moving = true;
                target = Camera.main.ScreenToWorldPoint(touch.position);
                return;
            }
    }
}