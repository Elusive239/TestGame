using System;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float speed, padding;
    public Vector2 boundsMin, boundsMax;
    private Transform self;
    private Vector2 centerOfScreen;

    public void Awake(){
        self = this.transform;
        centerOfScreen = new Vector2((float)Screen.width / 2.0f, (float)Screen.height / 2.0f);
        Vector3 upperBounds = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width,  (float)Screen.height));
        Vector3 lowerBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        boundsMin = new Vector2(
            lowerBounds.x - padding,
            lowerBounds.y - padding
        );
        boundsMax = new Vector2(
            upperBounds.x + padding,
            upperBounds.y + padding
        );
    }

    public void Update(){
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
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
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                pos = Vector2.MoveTowards(self.position, pos, speed*Time.deltaTime);
                                
                pos = new Vector2 (
                    Mathf.Clamp(pos.x, boundsMin.x, boundsMax.x), 
                    Mathf.Clamp(pos.y, boundsMin.y, boundsMax.y) 
                );
                self.position = pos;
            }
        }
    }
}