using TMPro;
using UnityEditor;
using UnityEngine;

public class Info : MonoBehaviour
{
    [SerializeField]
    private TMP_Text version;
    void Start()
    {
        version.text="ver."+Application.version;
    }
}
