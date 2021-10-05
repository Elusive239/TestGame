using System;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public float speed, padding;
    public Vector2 boundsMin, boundsMax, cameraZoom;
    private Transform self;
    public bool zooming = false;
    public static bool canMove = false;
    private Vector3 target;

    public PlayableCharacter character;

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
        if (Input.touchCount < 1)// || !canMove)
            return;        
        
        // if(!Input.GetMouseButton(0)) return;
        // Vector3 tOnePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Touch touchOne = Input.GetTouch(0);
        Vector2 tOnePos = Camera.main.ScreenToWorldPoint(touchOne.position);

        if(character == null){
            character = GridManager.manager.GetCharacter(tOnePos);
            Debug.Log(character);
            if(character != null)
                character.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        } else{
            Debug.Log(character);
            if(Input.GetTouch(0).phase == TouchPhase.Began){
                character.MoveToPos(tOnePos);
                character = null;
            }
            else return;
        }
        
        
        if (Input.touchCount >= 2){
            Touch touchTwo = Input.GetTouch(1);
            Vector2 touchOnePos = tOnePos - touchOne.deltaPosition, touchTwoPos = touchTwo.position - touchTwo.deltaPosition;
            float magnitude = (touchOnePos - touchTwoPos).magnitude;
            magnitude = magnitude - (tOnePos - touchTwo.position).magnitude;
            Zoom(magnitude);
            zooming = true;
        } 

        if (Input.touchCount == 1){
            if(touchOne.phase == TouchPhase.Began  ){
                zooming = false;
                target = tOnePos;
            }
            else if(touchOne.phase == TouchPhase.Moved  ){
                
                if(zooming) return;
                Vector3 direction = target - new Vector3(tOnePos.x, tOnePos.y, 0);
                direction = new Vector3(
                    Mathf.Clamp(self.position.x + direction.x, boundsMin.x, boundsMax.x), 
                    Mathf.Clamp(self.position.y + direction.y, boundsMin.y, boundsMax.y),  
                    0
                );
                self.position = direction;
            }
        }
    }

    public void Zoom(float increment){
        //x is min, y is max
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment*0.001f, cameraZoom.x, cameraZoom.y);
    }

    //for cut scenes and focusing on characters? too good to remove lol
    public bool MoveTowardsTarget(Vector3 target){
        if(canMove) return false;

        float dist = Vector3.Distance(center, target);
        Debug.Log("Distance from center: " + dist);
        Vector3 result = Vector3.MoveTowards(center, target, speed * Time.deltaTime);
        self.position = new Vector3 (
            Mathf.Clamp(result.x, boundsMin.x, boundsMax.x), 
            Mathf.Clamp(result.y, boundsMin.y, boundsMax.y),                    
            -10f);
        float tOnePosDist = Vector3.Distance(center, target);
        if(dist == tOnePosDist){
            canMove = false;
            return true;
        }
        return false;
    }
}

//Old input update code
// public void Update(){
//         if(MoveTowardsTarget()){
//             if(Input.touchCount == 1){
//                 Touch t = Input.GetTouch(0);
//                 if(t.phase == TouchPhase.Began)
//                 target = Camera.main.ScreenToWorldPoint(t.position);
//             }
//             return;
//         }

//         // Handle screen touches.
//         if (Input.touchCount < 1)
//             return;
    
//             Touch touch = Input.GetTouch(0);

//             // Move if the screen has the finger canMove.
//             if (touch.phase == TouchPhase.Began )//|| touch.phase == TouchPhase.Stationary)
//             {
//                 canMove = true;
//                 target = Camera.main.ScreenToWorldPoint(touch.position);
//                 return;
//             }
//     }