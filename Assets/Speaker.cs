using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Magnet
{

    [SerializeField]
    private float scale=1.0f;

    AudioSource bgm;

    float[] data;
    int sampleStep;
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        for(int i=0;i<MagneticPoles.Length;i++){
            MagneticPoles[i].m*=gameManager.config.MagneticAMP;
        }

        bgm=GameObject.Find("BGM").GetComponent<AudioSource>();
        data=new float[bgm.clip.samples*bgm.clip.channels];
        bgm.clip.GetData(data,0);

        sampleStep=(int)(bgm.clip.frequency/(Mathf.Max(60f,1f/Time.fixedDeltaTime)));
    }

    private void FixedUpdate() {
        if (bgm.isPlaying){
            int startIndex=bgm.timeSamples;
            int endIndex=Math.Min(bgm.timeSamples+sampleStep,data.Length);
            float level = DetectVolumeLevel(data, startIndex, endIndex);

            for(int i=0;i<this.MagneticPoles.Length;i++){
                this.MagneticPoles[i].m=level*scale;
            }
        }
    }
    float DetectVolumeLevel(float[] data, int start, int end)
    {
        float max=0f;
        float min=0f;

        for(int i=start;i<end;i++){
            if (max < data[i]) max=data[i];
            if (min > data[i]) min=data[i];
        }

        return max-min;
    }

}
