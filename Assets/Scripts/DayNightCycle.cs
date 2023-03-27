using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    float diaElMinuto = 1;
    [SerializeField]
    Vector2 velocidadNubes = new Vector2(1,1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotarSol();
        MovimientoNubes();
    }

    void RotarSol()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * 6 / diaElMinuto);
    }
    void MovimientoNubes()
    {
        RenderSettings.skybox.SetVector("_Direccion", (Vector2)RenderSettings.skybox.GetVector("_Direccion" + Time.deltaTime * velocidadNubes));
    }
}
