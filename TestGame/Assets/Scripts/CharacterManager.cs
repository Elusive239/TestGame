using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UIState{
    hidden, actions, confirmation
}

public class CharacterManager : MonoBehaviour
{
    GridManager manager;
    public TurnQueue<PlayableCharacter> queue;
    public List<GameObject> imgs;
    public GameObject actionGrid, confirmButtons;
    void Start(){
        manager = GridManager.manager;
        queue = new TurnQueue<PlayableCharacter>(6);
        UIStateChange(0);
    }

    public void SwapUIColors(Color c){
        foreach(GameObject image in imgs){
            image.GetComponent<Image>().color = c;
        }
    }

    void UIStateChange(UIState uIState){
        switch(uIState){
            case UIState.actions:
            confirmButtons.SetActive(false);
            actionGrid.SetActive(true);
            break;

            case UIState.confirmation:
            confirmButtons.SetActive(true);
            actionGrid.SetActive(false);
            break;

            case UIState.hidden:
            default:
            confirmButtons.SetActive(false);
            actionGrid.SetActive(false);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
