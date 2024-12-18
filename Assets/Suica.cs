using System;
using UnityEngine;

public class Suica : MonoBehaviour
{
    [SerializeField]
    public int size=1;
    public int getSize(){
        return size;
    }
    [field:SerializeField]
    public GameObject NextSuica{get;private set;}
    [SerializeField]
    private int score=1;
    private GameManager gameManager;
    [field:SerializeField]
    public Guid guid{get;private set;}
    private bool ripe=false;

    [SerializeField]
    private AudioClip MergeSE;

    [field:SerializeField]
    public float DisplayScale{get;private set;}=1f;

    void Awake()
    {
        guid=Guid.NewGuid();
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(gameManager.isPaused){
            return;
        }
        if(!gameManager.isInArea(this.transform.position)){
            gameManager.GameOver();
        }
    }
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Suica otherSuica;
        if(collisionInfo.gameObject.TryGetComponent<Suica>(out otherSuica)){
            //ぶつかったスイカと同じ大きさかつ両方とも処理中でない場合
            if(otherSuica.getSize()==this.size&&!(this.ripe||otherSuica.ripe)){
                //ぶつかったスイカよりuidが大きい場合
                if(0<otherSuica.guid.CompareTo(this.guid)){
                    this.ripe=true;
                    otherSuica.ripe=true;
                    GenNextSuica((otherSuica.transform.position-this.transform.position)/2+this.transform.position);
                    PlaySE((otherSuica.transform.position-this.transform.position)/2+this.transform.position);
                    gameManager.addScore(this.score);
                    Destroy(otherSuica.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    GameObject GenNextSuica(Vector3 pos){
        if(this.NextSuica!=null){
            GameObject nextSuica=Instantiate(this.NextSuica,pos+this.transform.rotation*this.NextSuica.transform.position,this.transform.rotation*this.NextSuica.transform.rotation,this.transform.parent);
            return nextSuica;
        }
        return null;
    }

    public void DestroySelf(){
        Destroy(this.gameObject);
    }
    void PlaySE(Vector3 pos){
        if(MergeSE!=null){
            GameObject audio=new GameObject("SuicaAudioSource",typeof(AudioSource));
            audio.transform.position=pos;
            audio.GetComponent<AudioSource>().PlayOneShot(MergeSE);
            Destroy(audio,2f);
        }
    }
}
