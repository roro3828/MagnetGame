using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinkanoWa : MonoBehaviour
{
    [SerializeField]
    private Suica FirstSuica;
    private Suica[] Suicas;

    private Quaternion ang;
    void Start()
    {
        int size=1;
        Suica tmp=FirstSuica;
        while(tmp.NextSuica!=null){
            tmp=tmp.NextSuica.GetComponent<Suica>();
            size++;
        }

        tmp=FirstSuica;

        Suicas=new Suica[size];
        ang=Quaternion.Euler(0,0,-360/size);
        Vector3 pos=new Vector3(0,3);
        for(int i=0;i<size;i++){
            Suicas[i]=tmp;
            if(i<size-1){
                tmp=Suicas[i].NextSuica.GetComponent<Suica>();
            }

            GameManager.DisplayObject(Suicas[i].gameObject,this.transform,pos,Suicas[i].DisplayScale);
            pos=ang*pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
