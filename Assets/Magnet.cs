using System;
using System.Collections;
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

    void Start()
    {
    }

    void Update()
    {
        Rigidbody2D rig;
        if(this.TryGetComponent<Rigidbody2D>(out rig)){
            MagneticPole[] mp=GetAllPoles();
            
            for(int i=0;i<this.MagneticPoles.Length;i++){
                Vector3 F=new Vector3(0,0,0);
                for(int j=0;j<mp.Length;j++){
                    Vector3 vec=this.MagneticPoles[i].transform.position-mp[j].transform.position;
                    if(0.01f<vec.magnitude){
                        Vector3 f=vec.normalized*Time.deltaTime*(float)((this.MagneticPoles[i].m*mp[j].m)/(4*Math.PI*0.00000125663706212*vec.magnitude*vec.magnitude));
                        F+=f;
                    }
                }
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
