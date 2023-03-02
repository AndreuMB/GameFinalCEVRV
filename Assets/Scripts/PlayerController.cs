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
    float velocidad;
    [SerializeField]
    int vida;
    [SerializeField]
    float altura;

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

        vida = 100;
        altura = transform.localScale.y;

        //Bloqueo del cursor al centro e invisible cuando el juego esta en primera instancia
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



        //Instancia de Arma
        InstanciaArmas();
    }

    void FixedUpdate(){
        PlayerMovement();
    }

    void Update()
    {
        PlayerRotation();

        if(Input.GetKey(KeyCode.K)){
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y+0.25f, transform.localScale.z);
        }

        if ((Input.GetMouseButtonDown(0) && selectedWeapon.GetComponent<Weapon>().auto) || (Input.GetMouseButtonDown(0) && !selectedWeapon.GetComponent<Weapon>().auto))
        {
            Fire();
        }

        InputCambiarArma();

    }

    void PlayerMovement()
    {
        //Variables donde se captura y guarda los desplazamientos
        movimientoX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;
        movimientoZ = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;

        //Vectores donde transladaremos al personaje segun su rotación.
        Vector3 ver = new Vector3(transform.forward.x, 0, transform.forward.z) * movimientoZ;
        Vector3 hor = transform.right * movimientoX;

        //Movimiento del personaje
        transform.localPosition += hor * velocidad * Time.deltaTime;
        transform.position += ver * velocidad * Time.deltaTime;
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
        }

        //Rotamos al personaje en el eje Y
        Vector3 rotacionJugador = new Vector3(transform.eulerAngles.x, rotacionX+transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(rotacionJugador);

    }

    protected override bool decideDamage(Bullet bullet)
    {
        print("bullet.owner.GetType() = " + bullet.owner.GetType());
        return (bullet.owner.GetType() == typeof(Enemy));
        // if (bullet.owner.tag == Tags.ENEMY) return true;
        // return false;
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

    void InstanciaArmas()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i] = Instantiate(weapons[i].gameObject, transform.position, Quaternion.identity, transform).GetComponent<Weapon>();
            weapons[i].gameObject.SetActive(false);
        }
        weapons[0].gameObject.SetActive(true);
    }


}
