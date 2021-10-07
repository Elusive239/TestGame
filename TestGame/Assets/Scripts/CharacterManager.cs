using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UIState{
    hidden, actions, confirmation, enemyTurn
}

public class CharacterManager : MonoBehaviour
{
    public static GridManager manager;
    public static CharacterManager instance;
    public static CameraController controller;

    public TurnQueue<PlayableCharacter> queue;
    public PlayableCharacter currentCharacter;

    public List<GameObject> imgs;
    public Transform actionGrid, confirmButtons;
    Transform[] actions, confirms; 
    public UIState uIState;
    public float shiftAmount;
    private Vector3 maxScale, hidingScale;

    bool stopCamera = false;
    void Awake(){
        instance = this;
        queue = new TurnQueue<PlayableCharacter>(6);
        
        maxScale = new Vector3(1, 1, 1);
        hidingScale = new Vector3(0,0,1);
        actions = actionGrid.GetComponentsInChildren<Transform>();
        confirms = confirmButtons.GetComponentsInChildren<Transform>();
        //UIStateChange(0);
    }

    public void SwapUIColors(){
        foreach(GameObject image in imgs){
            image.GetComponent<Image>().color = currentCharacter.uiColor;
        }
    }

    Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) => new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
    

    bool UIStateShift(UIState uIState, float timeShift){
        Vector3 shift = new Vector3(shiftAmount*timeShift, shiftAmount*timeShift, 0);
        float actionDist = 0, confirDist = 0;
        switch(uIState){
            case UIState.actions:
            foreach(Transform t in confirms){
                t.localScale = Clamp(t.localScale - shift, hidingScale, maxScale);
            }

            foreach(Transform t in actions){
                t.localScale = Clamp(t.localScale + shift, hidingScale, maxScale);
            }
            
            actionDist = Vector3.Distance(actions[0].localScale, maxScale);
            confirDist = Vector3.Distance(confirms[0].localScale, hidingScale);
            if(actionDist == 0 && confirDist == 0) return true;
            return false;

            case UIState.confirmation:
            foreach(Transform t in confirms){
                t.localScale = Clamp(t.localScale + shift, hidingScale, maxScale);
            }

            foreach(Transform t in actions){
                t.localScale = Clamp(t.localScale - shift, hidingScale, maxScale);
            }
            
            actionDist = Vector3.Distance(actions[0].localScale, hidingScale);
            confirDist = Vector3.Distance(confirms[0].localScale, maxScale);
            if(actionDist == 0 && confirDist == 0) return true;
            break;

            case UIState.enemyTurn:
            confirmButtons.localScale = hidingScale;
            actionGrid.localScale = hidingScale;
            break;

            case UIState.hidden:
            default:
            foreach(Transform t in confirms){
                t.localScale = Clamp(t.localScale - shift, hidingScale, maxScale);
            }

            foreach(Transform t in actions){
                t.localScale = Clamp(t.localScale - shift, hidingScale, maxScale);
            }
            
            actionDist = Vector3.Distance(actions[0].localScale, hidingScale);
            confirDist = Vector3.Distance(confirms[0].localScale, hidingScale);
            if(actionDist == 0 && confirDist == 0) return true;
            return true;
        }
        return false;
    }

    public PlayableCharacter GetCharacter(Vector3 target){
        if(queue.Count < 1)
            return null;
        else if(queue.Count == 1)
            if(Vector3.Distance(target, queue[0].position) < 0.2f)
                return queue[0];
        else for(int x = 0; x < queue.Count; x++){
            if(Vector3.Distance(target, queue[x].position) < 0.2f)
                return queue[x];
        }
        return null;
    }

    // Update is called once per frame
    void FixedUpdate(){
        UIStateShift(uIState, Time.deltaTime);
        if(currentCharacter == null){// add or character is out of actions
            if(queue.currentItemTurn == null) {
                queue.NextTurn();
                return;
            }
            currentCharacter = queue.currentItemTurn;
            stopCamera = true;
            SwapUIColors();
            uIState = UIState.actions;
        }

        if(stopCamera)
            stopCamera = !controller.MoveTowardsTarget(currentCharacter.position);


    }
}
