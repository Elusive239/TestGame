using System;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float speed;
    private Transform self;
    private Vector2 dimensions;

    public void Awake(){
        self = this.transform;
        dimensions = new Vector2((float)Screen.width / 2.0f, (float)Screen.height / 2.0f);
    }

    public void Update(){
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - dimensions.x) / dimensions.x;
                pos.y = (pos.y - dimensions.y) / dimensions.y;
            }

            if (Input.touchCount >= 2){
                touch = Input.GetTouch(1);

                if (touch.phase == TouchPhase.Began)
                {
                    
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    
                }
            }
        }
    }
}