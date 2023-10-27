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
    [System.NonSerialized]
    public bool ripe=false;
    private GameObject OtherObject=null;
    private GameManager gameManager;

    public bool isOtherObjectNull(){
        return this.OtherObject==null;
    }
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();

        ripe=false;
        OtherObject=null;
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
            if(otherSuica.getSize()==this.size){
                if((this.isOtherObjectNull()&&otherSuica.isOtherObjectNull())&&!(this.ripe||otherSuica.ripe)){
                    this.OtherObject=collisionInfo.gameObject;
                    this.ripe=true;
                    otherSuica.ripe=true;
                    GenNextSuica((this.OtherObject.transform.position-this.transform.position)/2+this.transform.position);
                    gameManager.addScore(this.score);
                    Destroy(this.OtherObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void GenNextSuica(Vector3 pos){
        if(this.NextSuica!=null){
            GameObject nextSuica=Instantiate(this.NextSuica,pos+this.transform.rotation*this.NextSuica.transform.position,this.transform.rotation*this.NextSuica.transform.rotation,this.transform.parent);
        }
    }
}
