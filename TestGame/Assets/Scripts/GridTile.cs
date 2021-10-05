using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Transform center;
    public Vector2 coordinates;
    public Vector3 target;
    public Vector3 position => transform.position;
    public Vector3 charPosition => center.position;
    public bool isWalkable = true;
    public GameObject currentOccupier;
    public SpriteRenderer border, top, bottom;
    
    
    float startMove;
    bool start = false;
    public bool startHighlightColor = false, startTileColor = false;

    public void InitTile(Vector2 _coordinates, Vector3 _target){
        coordinates = _coordinates;
        target = _target;
    }

    public void MoveDown(float rand){
        startMove = Time.time + rand;
        start = true;
    }

    public void IncBorderColor(Color c){
        Color col = Color.Lerp(border.color, c,11f * Time.deltaTime);
        border.color = col;
    }

    public void IncTileColor(Color a, Color b){
        Color col1 = Color.Lerp(top.color, a,11f * Time.deltaTime);
        top.color = col1;
        Color col2 = Color.Lerp(bottom.color, b,11f * Time.deltaTime);
        bottom.color = col2;
    }

    public bool SameBorderColor(Color toCompare){
        bool toReturn = border.color == toCompare;
        //print(toReturn);
        return toReturn;
    }

    public bool SameTileColor(Color a, Color b){
        bool toReturn = top.color == a && bottom.color == b;
        //print(toReturn);
        return toReturn;
    }

    public void Update(){
        //print(startHighlightColor);
        if(!start){

            if(startTileColor){
                if(!SameTileColor(GridManager.manager.topColor, GridManager.manager.bottomColor)){
                    IncTileColor(GridManager.manager.topColor, GridManager.manager.bottomColor);
                }else{
                startHighlightColor = true;
                startTileColor = false;
                }
            }


            if(!startHighlightColor ) return;

            if(!SameBorderColor(GridManager.manager.border)){
                IncBorderColor(GridManager.manager.border);
            }else{
                startHighlightColor = false;
            }
        }

        if(Time.time <= startMove)
            return;

        transform.position = Vector3.MoveTowards(position, target, 9.9f * Time.deltaTime);
        if(Vector3.Distance(position, target) < 0.001f){
            transform.position = target;
            start = false;
        }
    }
}
