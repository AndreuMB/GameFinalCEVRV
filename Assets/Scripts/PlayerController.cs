using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{

    [Header("Referencias")]
    [SerializeField]
    Camera camara;

    [Header("Configuración")]
    [SerializeField]
    float sensibilidad;

    float rotacionX, rotacionY;

    float movimientoX, movimientoZ;

    [Header("Stats")]
    [SerializeField]
    float altura;
    // inmortal
    [SerializeField] bool god;

    [Header("Variables")]
    const float LIMIT_ANGLE = 45;


    void Start()
    {
        // INI WEAPON
        // GameObject weaponObj = Instantiate(selectedWeapon);
        // GameObject player = GameObject.Find(Tags.PLAYER);
        // weaponObj.transform.parent = player.transform;
        // weaponObj.transform.localPosition = new Vector3(1,0,0);
        // weaponObj.transform.localRotation=Quaternion.identity;

        altura = transform.localScale.y;

        //Bloqueo del cursor al centro e invisible cuando el juego esta en primera instancia
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



        //Instancia de Arma
        InstanciaArmas();
    }

    void FixedUpdate(){
        
    }

    void Update()
    {
        PlayerMovement();
        PlayerRotation();

        if(Input.GetKey(KeyCode.K)){
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y+0.25f, transform.localScale.z);
        }

        InputRecargar();
        InputDisparar();
        InputCambiarArma();

    }

    void PlayerMovement()
    {
        //Variables donde se captura y guarda los desplazamientos
        movimientoX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        movimientoZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        //Vectores donde transladaremos al personaje segun su rotación.
        Vector3 ver = new Vector3(transform.forward.x, 0, transform.forward.z) * movimientoZ;
        Vector3 hor = transform.right * movimientoX;

        //Movimiento del personaje
        transform.localPosition += hor * speed * Time.deltaTime;
        transform.position += ver * speed * Time.deltaTime;
    }
    void PlayerRotation()
    {
        //Variables donde se captura y guardan las rotaciones
        rotacionX = Input.GetAxis("Mouse X") * Time.deltaTime * sensibilidad;
        rotacionY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensibilidad * -1;

        //Quaternion donde guardamos la rotación en el eje X desde la captura
        Quaternion q = Quaternion.AngleAxis(rotacionY, Vector3.right);

        //Prevision del angulo en el eje X antes de moverlo
        float prevAngleX = Quaternion.Angle(transform.rotation, Camera.main.transform.rotation * q);

        //Rotamos la camara en vertical si esta dentro del rango permitido
        if (prevAngleX < LIMIT_ANGLE)
        {
            camara.transform.rotation *= q;
            //rotacionY = 0;
        }

        //Rotamos al personaje en el eje Y
        Vector3 rotacionJugador = new Vector3(transform.eulerAngles.x, rotacionX+transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(rotacionJugador);

    }

    protected override bool decideDamage(Bullet bullet)
    {
        if (god) return false;
        // if is an enemy return true
        return (bullet.owner.GetType() == typeof(Enemy));
    }
    
    void InputRecargar()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedWeapon.ReLoad();
        }
    }

    void InputDisparar()
    {
        if ((Input.GetMouseButton(0) && selectedWeapon.weaponData.auto) || (Input.GetMouseButton(0) && !selectedWeapon.weaponData.auto))
        {
            Fire();
        }
    }

    void InputCambiarArma()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ChangeWeapon(1);
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ChangeWeapon(-1);
        }
    }

    void ChangeWeapon(int a)
    {
        selectedWeapon.gameObject.SetActive(false);
        if(selectedIndex == 0 && a < 0){
            selectedIndex = weapons.Count-1;
        }else if(selectedIndex == weapons.Count-1 && a > 0){
            selectedIndex = 0;
        }else{
            selectedIndex += a;
        }
        selectedWeapon.gameObject.SetActive(true);
    }

}
