using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Character
{

    [Header("Referencias")]
    [SerializeField]
    Camera camara;

    [Header("Configuración")]
    [SerializeField]
    float sensibilidad;

    float mouseX, mouseY, xRotation;

    float movimientoX, movimientoZ;

    [Header("Stats")]
    [SerializeField]
    float altura;
    // inmortal
    [SerializeField] bool god;

    [Header("Variables")]
    const float LIMIT_ANGLE = 45;

    //Velocidad extra cuando sprintamos
    const float SPRINT_SPEED = 2;

    [SerializeField]
    float movementSpeed;

    public float actualLife => life;

    public UnityEvent<Weapon> OnWeaponStateChange = new UnityEvent<Weapon>();
    public UnityEvent<Weapon> OnReloadWeapon = new UnityEvent<Weapon>();
    public UnityEvent<float> OnPlayerLifeStateChange = new UnityEvent<float>();

    private void Awake()
    {
        Application.targetFrameRate = 144;
    }
    void Start()
    {
        altura = transform.localScale.y;

        //Bloqueo del cursor al centro e invisible cuando el juego esta en primera instancia
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Velocidad del Jugador = velocidad del Character
        movementSpeed = speed;

        //Instancia de Arma
        InstanciaArmas();
        OnWeaponStateChange.Invoke(selectedWeapon);
        OnPlayerLifeStateChange.Invoke(actualLife);
    }

    void FixedUpdate()
    {
        PlayerMovement();

    }

    void Update()
    {

        PlayerRotation();

        if (Input.GetKey(KeyCode.K))
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y+0.25f, transform.localScale.z);
        }

        InputRecargar();
        InputDisparar();
        InputCambiarArma();

    }

    void LateUpdate()
    {
    }

    void PlayerMovement()
    {

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = speed + SPRINT_SPEED;
        }else
        {
            movementSpeed = speed;
        }
        //Variables donde se captura y guarda los desplazamientos
        movimientoX = Input.GetAxis("Horizontal") * movementSpeed;
        movimientoZ = Input.GetAxis("Vertical") * movementSpeed;

        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 move = transform.right * movimientoX + transform.forward * movimientoZ;

        rb.MovePosition(transform.position+move * Time.fixedDeltaTime);
    }

    void PlayerRotation()
    {
        //Variables donde se captura y guardan las rotaciones
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensibilidad;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensibilidad;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camara.transform.localRotation = Quaternion.Euler(xRotation , 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    protected override bool decideDamage(Bullet bullet)
    {
        if (god) return false;
        // if is an enemy return true
        return (bullet.owner.GetType() == typeof(Enemy));
    }
    
    void InputRecargar()
    {
        if (Input.GetKeyUp(KeyCode.R))
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
        OnWeaponStateChange.Invoke(selectedWeapon);
    }

}
