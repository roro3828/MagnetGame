using System;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [System.Serializable]
    public class MagneticPole{
        public Transform transform;

        public float m=0.01f;
    }

    [SerializeField]
    private MagneticPole[] MagneticPoles;
    private GameObject[] Magnets;
    public Vector3 getPos(){
        return this.gameObject.transform.position;
    }
    private GameManager gameManager;

    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i=0;i<MagneticPoles.Length;i++){
            MagneticPoles[i].m*=gameManager.config.MagneticAMP;
        }

    }

    void Update()
    {
        if(gameManager.isPaused){
            return;
        }

        Rigidbody2D rig;
        if(this.TryGetComponent<Rigidbody2D>(out rig)){
            MagneticPole[] mp=GetAllPoles();
            
            for(int i=0;i<this.MagneticPoles.Length;i++){
                Vector3 F=new Vector3(0,0,0);
                for(int j=0;j<mp.Length;j++){
                    Vector3 vec=this.MagneticPoles[i].transform.position-mp[j].transform.position;
                    if(0.01f<vec.magnitude){
                        Vector3 f=vec.normalized*Time.deltaTime*((this.MagneticPoles[i].m*mp[j].m)/(vec.magnitude*vec.magnitude));
                        F+=f;
                    }
                }
                F=F/(float)(16*Math.PI*Math.PI*0.0000001);
                rig.AddForceAtPosition(F,this.MagneticPoles[i].transform.position);
            }
        }
    }

    private MagneticPole[] GetAllPoles(){
        Magnet[] ms=transform.root.GetComponentsInChildren<Magnet>();
        List<MagneticPole> poles=new List<MagneticPole>();
        for(int i=0;i<ms.Length;i++){

            if(ms[i]!=this){
                for(int j=0;j<ms[i].MagneticPoles.Length;j++){
                    poles.Add(ms[i].MagneticPoles[j]);
                }
            }

        }

        return poles.ToArray();
    }
}
