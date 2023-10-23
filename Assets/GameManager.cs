using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Suicas;
    [SerializeField]
    private GameObject Dropper;
    [SerializeField]
    private float movelimit=10f;
    [SerializeField]
    private float DropperSpeed=1f;

    [SerializeField]
    private Vector2Int GameAreaStart=new Vector2Int(-1,-1);
    [SerializeField]
    private Vector2Int GameAreaEnd=new Vector2Int(1,1);
    [SerializeField]
    private int Score=0;
    private bool interval=true;
    private GameObject nextSuica;
    private GameObject DisplaySuica;
    public int addScore(int score){
        this.Score+=score;
        return this.Score;
    }

    private MainInput maininput;
    void OnDisable()
    {
        maininput.Disable();
    }
    void Start()
    {
        maininput=new MainInput();
        maininput.Enable();
        ShowNext();
    }
    void ShowNext(){

        nextSuica=Suicas[UnityEngine.Random.Range(0,Suicas.Length)];
        DisplaySuica=Instantiate(nextSuica,Dropper.transform.position,Dropper.transform.rotation,Dropper.transform);
        foreach(Component c in DisplaySuica.GetComponents<Component>()){
            if(!(c is Transform)&&!(c is MeshRenderer)&&!(c is SpriteRenderer)){
                Destroy(c);
            }
        }
    }

    void Update()
    {
        Vector2 pos=Dropper.transform.position;
        float input=maininput.Main.Move.ReadValue<float>();
        pos+=new Vector2(input*Time.deltaTime*DropperSpeed,0);
        if(-movelimit<=pos.x&&pos.x<=movelimit){
            Dropper.transform.position=pos;
        }

        if(0<maininput.Main.Drop.ReadValue<float>() && interval){
            interval=false;
            Destroy(DisplaySuica);
            Instantiate(nextSuica,Dropper.transform.position,Dropper.transform.rotation,this.transform);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(0.7f);
        ShowNext();
        interval=true;
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
