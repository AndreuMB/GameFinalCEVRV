using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    

    [Header("Configuración")]
    [SerializeField]
    float sensibilidad;

    float rotacionX, rotacionY;

    float movimientoX, movimientoZ;

    [Header("Stats")]
    [SerializeField]
    float velocidad;
    [SerializeField]
    int vida;
    [SerializeField]
    float altura;




    // Start is called before the first frame update
    void Start()
    {
        vida = 100;
        altura = transform.localScale.y;
    }

    void FixedUpdate(){
        PlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotation();

        if(Input.GetKey(KeyCode.K)){
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y+0.25f, transform.localScale.z);
        }


    }

    void PlayerMovement()
    {
        movimientoX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;
        movimientoZ = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;

        //transform.localPosition += new Vector3(movimientoX, 0, movimientoZ); Normalizar
        Vector3 aux = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 ver = aux * movimientoZ;
        Vector3 hor = transform.right * movimientoX;
        transform.localPosition += hor * velocidad * Time.deltaTime;
        transform.position += ver * velocidad * Time.deltaTime;

        
        /*if(Input.GetKey(KeyCode.W))
        {
            transform.localPosition += transform. * velocidad * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= transform.forward * velocidad * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.localPosition += transform.right * velocidad * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= transform.right * velocidad * Time.deltaTime;
        }*/


    }
    void PlayerRotation()
    {
        rotacionX = Input.GetAxis("Mouse X") * Time.deltaTime * sensibilidad;
        rotacionY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensibilidad * -1;

        transform.Rotate(0, rotacionX, 0, Space.World);

        transform.Rotate(rotacionY, 0, 0, Space.Self);

        Vector3 rotation = transform.rotation.eulerAngles;
        float rX = Mathf.Min(rotation.x, 40f);
        print(rX);
        Vector3 newRotation = new Vector3(rX, rotation.y, rotation.z);

        transform.rotation = Quaternion.Euler(newRotation);


    }
}
