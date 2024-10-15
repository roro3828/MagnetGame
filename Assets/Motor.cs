using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : Magnet
{
    [SerializeField]
    private float cycle=20;
    [SerializeField]
    private float modifier=-1;
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i=0;i<MagneticPoles.Length;i++){
            MagneticPoles[i].m*=gameManager.config.MagneticAMP;
        }
        StartCoroutine(Switch());
    }

    IEnumerator Switch(){
        while(true){
            yield return new WaitForSeconds(cycle);
            for(int i=0;i<this.MagneticPoles.Length;i++){
                this.MagneticPoles[i].m*=modifier;
            }
        }
    }
}
