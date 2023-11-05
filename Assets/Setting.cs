using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Setting : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}
