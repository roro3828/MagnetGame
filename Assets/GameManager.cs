using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Suicas;

    [SerializeField]
    private Vector2Int GameAreaStart=new Vector2Int(-1,-1);
    [SerializeField]
    private Vector2Int GameAreaEnd=new Vector2Int(1,1);
    [SerializeField]
    private int Score=0;
    public int addScore(int score){
        this.Score+=score;
        return this.Score;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loop());
    }

    IEnumerator loop(){
        while(true){
            Instantiate(Suicas[UnityEngine.Random.Range(0,Suicas.Length)],this.transform.position,this.transform.rotation,this.transform);
            yield return new WaitForSeconds(0.8f);
        }
    }

    public bool isInArea(Vector2 pos){
        bool x;
        bool y;
        if(GameAreaStart.x<GameAreaEnd.x){
            x=GameAreaStart.x<=pos.x&&pos.x<=GameAreaEnd.x;
        }
        else{
            x=GameAreaEnd.x<=pos.x&&pos.x<=GameAreaStart.x;
        }

        if(GameAreaStart.y<GameAreaEnd.y){
            y=GameAreaStart.y<=pos.y&&pos.y<=GameAreaEnd.y;
        }
        else{
            y=GameAreaEnd.y<=pos.y&&pos.y<=GameAreaStart.y;
        }
        return x&&y;
    }
}
