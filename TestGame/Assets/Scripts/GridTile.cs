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

    #region Animation
    public void MoveDown(float rand){
        startMove = Time.time + rand;
        start = true;
    }

    void IncBorderColor(Color c){
        Color col = Color.Lerp(border.color, c,9f * Time.deltaTime);
        border.color = col;
    }

    void IncTileColor(Color a, Color b){
        Color col1 = Color.Lerp(top.color, a,18f * Time.deltaTime);
        top.color = col1;
        Color col2 = Color.Lerp(bottom.color, b,18f * Time.deltaTime);
        bottom.color = col2;
    }

    bool SameBorderColor(Color toCompare){
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
                Color temp = new Color(border.color.r, border.color.g, border.color.b, 0);
                border.color = temp;
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
    #endregion
}
