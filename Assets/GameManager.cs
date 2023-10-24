using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private Transform[] SuicaDisplayTransform;
    private GameObject[] nextSuica;
    private GameObject[] DisplaySuica;
    [SerializeField]
    private TMP_Text textMesh;
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

        int len=SuicaDisplayTransform.Length;
        nextSuica=new GameObject[len+1];
        DisplaySuica=new GameObject[len+1];

        for(int i=1;i<len+1;i++){
            nextSuica[i]=Suicas[0];
        }

        ShowNext();
    }
    void ShowNext(){
        int len=SuicaDisplayTransform.Length;

        for(int i=0;i<len;i++){
            nextSuica[i]=nextSuica[i+1];
        }

        nextSuica[len]=Suicas[UnityEngine.Random.Range(0,Suicas.Length)];
        DisplaySuica[0]=Instantiate(nextSuica[0],Dropper.transform);

        for(int i=1;i<len+1;i++){
            DisplaySuica[i]=Instantiate(nextSuica[i],SuicaDisplayTransform[i-1]);
        }
        for(int i=0;i<len+1;i++){
            foreach(Component c in DisplaySuica[i].GetComponents<Component>()){
                if(!(c is Transform)&&!(c is MeshRenderer)&&!(c is SpriteRenderer)){
                    Destroy(c);
                }
            }
        }
    }

    void Drop(){
        Destroy(DisplaySuica[0]);
        Instantiate(nextSuica[0],Dropper.transform.position,Dropper.transform.rotation,this.transform);
    }

    void Update()
    {
        Vector2 pos=Dropper.transform.position;
        float input=maininput.Main.Move.ReadValue<float>();
        pos+=new Vector2(input*Time.deltaTime*DropperSpeed,0);
        if(-movelimit<=pos.x&&pos.x<=movelimit){
            Dropper.transform.position=pos;
        }
        float r=maininput.Main.Turn.ReadValue<float>()*Time.deltaTime*60;
        Dropper.transform.Rotate(0,0,-r);

        if(0<maininput.Main.Drop.ReadValue<float>() && interval){
            interval=false;
            Drop();
            StartCoroutine(Wait());
        }

        textMesh.text=Score.ToString();
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(0.7f);
        for(int i=1;i<SuicaDisplayTransform.Length+1;i++){
            Destroy(DisplaySuica[i]);
        }
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
