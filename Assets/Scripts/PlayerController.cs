using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float velocidad;
    [SerializeField]
    float sensibilidad;

    float rotacionX, rotacionY;

    float movimientoX, movimientoZ;




    // Start is called before the first frame update
    void Start()
    {
        





    }

    void FixedUpdate(){
        PlayerMovement();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerRotation();




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
    }
}
