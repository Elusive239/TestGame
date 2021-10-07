using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction{
    player, ai
}

public class PlayableCharacter : MonoBehaviour, IComparable<PlayableCharacter>{
    public Faction faction;
    Vector2 target;
    bool moving = false;
    public Vector3 position => transform.position;
    const float speed = 6f;
    Vector3[] currentPath;
    int pathIndex = 0;
    public Color uiColor;
    
    void Start(){
        CharacterManager.instance.queue.Push(this);
    }
    public void MoveToPos(Vector3 targetTile){
        currentPath = GridManager.manager.GetPath(transform.position, targetTile);
        
        foreach(Vector3 pos in currentPath){
            if(pos == new Vector3(-1500, -1500, 0)){
                return;
            }
        }

        pathIndex = 0;
        moving = true;
    }   

    void Update(){
        if(moving){
            if(currentPath == null){
                moving = false;
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }

            target = currentPath[pathIndex];
            transform.position = Vector3.MoveTowards(position, target, speed * Time.deltaTime);
            if(Vector3.Distance(position, target) < 0.1f){
                if(currentPath.Length -1 == pathIndex){
                    moving = false;
                    GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }

                transform.position = target;
                pathIndex++;
            }
        }
    }

    public int CompareTo(PlayableCharacter other){
        return faction == Faction.player && other.faction != Faction.player ? 1 : -1 ;
    }
}
