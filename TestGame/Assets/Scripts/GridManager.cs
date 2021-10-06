using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Vector2Int size;
    public Vector2 padding, isoValue;
    private Vector2 tileScale;
    public GameObject prefab;
    public Transform center;

    public static GridManager manager;
    public List<PlayableCharacter> enemChars, playerChars;
    
    public GridTile[,] board;

    public Color border => new Color(0, 198f/255f,1,1);
    public Color topColor, bottomColor;
    Color selected => new Color(1f, 34f/255f,139f/255f,34f/255f);
    Color blank => new Color(0, 0,0 ,0);



    void InitGrid(){
        Transform tileTrans = prefab.GetComponent<Transform>();
        tileScale = new Vector2(tileTrans.localScale.x, tileTrans.localScale.y);
        playerChars = new List<PlayableCharacter>();
        enemChars = new List<PlayableCharacter>();
        if(board == null){
            board = new GridTile[size.x,size.y];
            Quaternion rotation = new Quaternion(0,0,0,0);
            for(int x = size.x; x > 0 ; x--){
                for(int y = size.y; y > 0 ; y--){
                    x--; y--;
                    Vector3 actPos = new Vector3(
                        (x-y)*isoValue.x*tileScale.x + padding.x , (x+y)*isoValue.y*tileScale.y + padding.y, 0
                    );
                    Vector3 tempPos = new Vector3(actPos.x, actPos.y+3.3f*tileScale.y, actPos.z);
                    GameObject tile = Instantiate(prefab, tempPos, rotation, center);
                    board[x,y] = tile.GetComponent<GridTile>();
                    board[x,y].InitTile( new Vector2(x,y), actPos );
                    x++; y++;
                }
            }
            StartCoroutine( GridAnim() );
        }
    }

    IEnumerator GridAnim(){
        List<GridTile> allTiles = new List<GridTile>();

        //print("Collecting tiles...");
        for(int x = size.x; x > 0 ; x--){
            for(int y = size.y; y > 0 ; y--){
                allTiles.Add(board[x-1,y-1]);
            }
        }
        yield return new WaitForSeconds(3f);

        //print("Randomly ordering tiles...");
        var rng = new System.Random();
        int n = allTiles.Count;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            GridTile value = allTiles[k];
            allTiles[k] = allTiles[n];
            allTiles[n] = value;
        }

        //print("Putting tiles in place...");
        float delay = 0f, lastDelay = 0f;
        for(int i = 0; i < allTiles.Count; i++){
            delay = ((float)(rng.Next(100) /100.0) * 2.25f) + 1;
            //print("delay: "+delay);
            if(lastDelay - delay <= 0.5f)
                allTiles[i].MoveDown(((float)(rng.Next(100) /100.0) * 2.25f) + 1);
            else 
                allTiles[i].MoveDown(delay);
            lastDelay = delay;
        }

        //print("Waiting...");
        yield return new WaitForSeconds(3.7f);
        //print("Coloring...");
        for(int x = size.x; x > 0 ; x--){
            for(int y = size.y; y > 0 ; y--){
                yield return 0.09f;
                board[x-1,y-1].startTileColor = true;
            }
        }
        // yield return new WaitForSeconds(1.2f);
        // playerChars[0].transform.position = board[0,0].charPosition;
    }
    
    void Awake(){
        if(manager == null)
            manager = this;
        InitGrid();
        CameraController.canMove = true;
    }

    //NOT DONE
    //Gonna need to do somethin like A*
    public Vector3[] GetPath(Vector3 start, Vector3 target){
        Vector3[] path = new Vector3[] {
            start, GetTile(target).charPosition
        };
        if(Vector3.Distance(path[0], path[1]) <= 0.1f) return null;

        return path;
    }

    public PlayableCharacter GetCharacter(Vector3 target){
        if(playerChars.Count < 1)
            return null;
        else if(playerChars.Count == 1)
            if(Vector3.Distance(target, playerChars[0].position) < 0.2f)
                return playerChars[0];
        else for(int x = 0; x < playerChars.Count; x++){
            if(Vector3.Distance(target, playerChars[x].position) < 0.2f)
                return playerChars[x];
        }
        return null;
    }

    public GridTile GetTile(Vector3 target){
        for(int x = size.x; x > 0 ; x--){
            for(int y = size.y; y > 0 ; y--){
                if(Vector3.Distance(target, board[x-1,y-1].charPosition) < 0.1f)
                    return board[x-1,y-1];
            }
        }
        return null;
    }

    void Update(){
        
    }
}
