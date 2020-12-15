using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AVProVideoSettings", menuName = "AVPro Player/AVProVideoSettings")]
public class Video : ScriptableObject
{
    [SerializeField] public bool streaming = false;
    [SerializeField] public string path = null;
    [SerializeField] public string vName = null;
}
