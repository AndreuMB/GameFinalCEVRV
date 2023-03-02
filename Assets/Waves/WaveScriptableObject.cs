using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveScriptableObject : ScriptableObject
{

    public int waveID;
    public int numEnemies;

    public AnimationCurve curve;

}
