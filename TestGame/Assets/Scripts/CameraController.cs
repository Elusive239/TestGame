using System;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float speed, padding;
    public Vector2 boundsMin, boundsMax;
    private Transform self;
    private Vector2 centerOfScreen;
    private Vector3 posToMoveTo;
    private bool canMove = false;

    public void Awake(){
        self = this.transform;
        centerOfScreen = new Vector2((float)Screen.width / 2.0f, (float)Screen.height / 2.0f);
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

    public void Update(){
        if(canMove){
            posToMoveTo = Vector3.MoveTowards(self.position, posToMoveTo, speed*Time.deltaTime); 
            
            if(Vector3.Distance(self.position, posToMoveTo) < 0.01f){
                canMove = false;
                self.position = posToMoveTo;
                return;
            }

            posToMoveTo = new Vector3 (
                Mathf.Clamp(posToMoveTo.x, boundsMin.x, boundsMax.x), 
                Mathf.Clamp(posToMoveTo.y, boundsMin.y, boundsMax.y),                    
                self.position.z 
            );
            self.position = posToMoveTo;
        }

        // Handle screen touches.
        if (Input.touchCount < 1)
            return;
    
            Touch touch = Input.GetTouch(0);
            if (Input.touchCount >= 2){
                Touch touch2 = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    
                }
                return;
            }

            // Move if the screen has the finger moving.
            if (touch.phase == TouchPhase.Began )//|| touch.phase == TouchPhase.Stationary)
            {
                canMove = true;
                posToMoveTo = Camera.main.ScreenToWorldPoint(touch.position);
                return;
            }
            else if (touch.phase == TouchPhase.Ended) canMove = false;
    }
}