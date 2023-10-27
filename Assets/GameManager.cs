using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    public bool isPaused{get;private set;}=false;
    public enum GameState{
        GameMain,
        Menu,
        GameOver
    };
    public GameState gameState{get;private set;}=GameState.GameMain;
    [SerializeField]
    private GameObject[] Suicas;
    [SerializeField]
    private GameObject Dropper;
    [SerializeField]
    private float movelimit=10f;
    [SerializeField]
    private float DropperSpeed=1f;
    [SerializeField]
    private float interval=1f;

    [SerializeField]
    private Vector2Int GameAreaStart=new Vector2Int(-1,-1);
    [SerializeField]
    private Vector2Int GameAreaEnd=new Vector2Int(1,1);
    [SerializeField]
    public int Score{get;private set;}=0;
    private bool Waiting=true;
    [SerializeField]
    private Transform[] SuicaDisplayTransform;
    private GameObject[] nextSuica;
    private GameObject[] DisplaySuica;
    [SerializeField]
    private TMP_Text Scoretext;
    [SerializeField]
    private TMP_Text Timetext;
    private float TimeOffset=0f;
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
        TimeOffset=Time.time;
        Cursor.visible=false;
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
        DisplaySuica[0]=DisplayObject(nextSuica[0],Dropper.transform);

        for(int i=1;i<len+1;i++){
            DisplaySuica[i]=DisplayObject(nextSuica[i],SuicaDisplayTransform[i-1]);
        }
    }

    public static GameObject DisplayObject(GameObject original,Transform parent){
        return DisplayObject(original,parent,new Vector3(0,0,0));
    }
    public static GameObject DisplayObject(GameObject original,Transform parent,Vector3 pos){
        GameObject instant=Instantiate(original,pos+parent.position+parent.rotation*original.transform.position,parent.rotation*original.transform.rotation ,parent);
        foreach(Component c in instant.GetComponents<Component>()){
            if(!(c is Transform)&&!(c is MeshRenderer)&&!(c is SpriteRenderer)&&!(c is SpriteShapeRenderer)&&!(c is SpriteShapeController)){
                Destroy(c);
            }
        }
        return instant;
    }

    void Drop(){
        Destroy(DisplaySuica[0]);
        Instantiate(nextSuica[0],Dropper.transform.position+Dropper.transform.rotation*nextSuica[0].transform.position,Dropper.transform.rotation*nextSuica[0].transform.rotation,this.transform);
    }

    private string getTime(){
        int time=(int)(Time.time-TimeOffset);
        int h=time/3600;
        int m=time%3600/60;
        int s=time%60;
        return String.Format("{0:D2}:{1:D2}:{2:D2}",h,m,s);
    }

    void Update()
    {

        Scoretext.text=Score.ToString();
        Timetext.text=getTime();
        if(isPaused){
            Time.timeScale=0;
        }
        else{
            Time.timeScale=1;
            MoveDropper();
        }

        if(0<maininput.Main.Menu.ReadValue<float>()){
            SceneManager.LoadScene("MainGame");
        }
    }

    void MoveDropper(){
        Vector2 pos=Dropper.transform.position;
        float input=maininput.Main.Move.ReadValue<float>();
        pos+=new Vector2(input*Time.deltaTime*DropperSpeed,0);
        if(-movelimit<=pos.x&&pos.x<=movelimit){
            Dropper.transform.position=pos;
        }
        float r=maininput.Main.Turn.ReadValue<float>()*Time.deltaTime*60;
        Dropper.transform.Rotate(0,0,-r);

        if(0<maininput.Main.Drop.ReadValue<float>() && Waiting){
            Waiting=false;
            Drop();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(interval);
        for(int i=1;i<SuicaDisplayTransform.Length+1;i++){
            Destroy(DisplaySuica[i]);
        }
        ShowNext();
        Waiting=true;
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

    public void GameOver(){
        isPaused=true;
        gameState=GameState.GameOver;
        Debug.Log("GameOver");
    }
}
