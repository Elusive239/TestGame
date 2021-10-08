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
    public Transform actionGrid, confirmButtons, canvas;
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
        foreach(Transform t in actions){
            t.SetParent(canvas);
        }
        confirms = confirmButtons.GetComponentsInChildren<Transform>();
        foreach(Transform t in confirms){
            t.SetParent(canvas);
        }
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
        Vector3 confirmShift = shift, actionShift = shift, cScale = maxScale, aScale = maxScale;

        switch(uIState){
            case UIState.actions:
                confirmShift = -confirmShift;
                cScale = hidingScale;
                break;
            case UIState.confirmation:
                actionShift = -actionShift;
                aScale = hidingScale;
                break;
            case UIState.enemyTurn:
            case UIState.hidden:
            default:
                confirmShift = -confirmShift;
                actionShift = -actionShift;
                cScale = hidingScale;
                aScale = hidingScale;
            break;
        }

        foreach(Transform t in confirms){
            t.localScale = Clamp(t.localScale + confirmShift, hidingScale, maxScale);
        }

        foreach(Transform t in actions){
            t.localScale = Clamp(t.localScale + actionShift, hidingScale, maxScale);
        }
            
        actionDist = Vector3.Distance(actions[0].localScale, aScale);
        confirDist = Vector3.Distance(confirms[0].localScale, cScale);
        if(actionDist == 0 && confirDist == 0) return true;
        else return false;
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

    public void TestHidden() => uIState = UIState.hidden;
    public void TestAction() => uIState = UIState.actions;
    public void TestConfirm() => uIState = UIState.confirmation;

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
