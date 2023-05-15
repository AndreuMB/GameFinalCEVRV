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
    //Pociones de Curacion
    [SerializeField]
    int maxCuras;
    int curas;
    public int actualCuras => curas;
    public int maxActualCuras => maxCuras;
    // inmortal
    [SerializeField] bool god;

    [Header("Variables")]
    const float LIMIT_ANGLE = 45;

    //Velocidad extra cuando sprintamos
    const float SPRINT_SPEED = 2;
    [SerializeField]
    float maxPlayerLife;

    // [SerializeField]
    float movementSpeed;

    public float maxActualPlayerLife => maxPlayerLife;
    public float actualLife => life;

    public UnityEvent<Weapon> OnWeaponStateChange = new UnityEvent<Weapon>();
    public UnityEvent<Weapon> OnReloadWeapon = new UnityEvent<Weapon>();
    public UnityEvent<float, float> OnPlayerLifeStateChange = new UnityEvent<float, float>();
    public UnityEvent<int, int> OnPotionsStateChange = new UnityEvent<int, int>();
    string mode = "";
    string modeBk = "";
    Vector3 prevPos;
    Vector3 actualPos;
    AudioManager am;

    private void Awake()
    {
        Application.targetFrameRate = 144;
    }
    void Start()
    {
        
        

        //Bloqueo del cursor al centro e invisible cuando el juego esta en primera instancia
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Velocidad del Jugador = velocidad del Character
        movementSpeed = speed;

        //Instancia de Arma
        InstanciaArmas();
        OnWeaponStateChange.Invoke(selectedWeapon);

        //Instanciar vida
        life = maxPlayerLife;
        OnPlayerLifeStateChange.Invoke(actualLife, maxPlayerLife);
        StartCoroutine(checkPlayerMovement());
        am = GameObject.FindObjectOfType<AudioManager>();

        //Instanciar curas
        curas = maxCuras;
        OnPotionsStateChange.Invoke(actualCuras, maxActualCuras);
    }

    void FixedUpdate()
    {
        PlayerMovement();

    }

    void Update()
    {

        // if menu open stop player actions
        if (FindObjectOfType<Shop>() || FindObjectOfType<GameOver>()) return;

        PlayerRotation();
        InputRecargar();
        InputDisparar();
        InputCambiarArma();
        InputCuracion();
        // InputTienda();
        
    }

    void LateUpdate()
    {
    }

    void PlayerMovement()
    {
        AudioManager am = GameObject.FindObjectOfType<AudioManager>();
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = speed + SPRINT_SPEED;
            mode = "Run";
        }else
        { 
            movementSpeed = speed;
            mode = "Walk";
        }
        //Variables donde se captura y guarda los desplazamientos
        movimientoX = Input.GetAxis("Horizontal") * movementSpeed;
        movimientoZ = Input.GetAxis("Vertical") * movementSpeed;

        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 move = transform.right * movimientoX + transform.forward * movimientoZ;

        rb.MovePosition(transform.position+move * Time.fixedDeltaTime);

    }

    IEnumerator checkPlayerMovement() {
        const float MIN_MOVEMENT = 0.1f;
        while (isActiveAndEnabled)
        {
            prevPos = transform.position;
            yield return new WaitForSeconds(0.1f);
            actualPos = transform.position;
            float distance = Vector3.Distance(prevPos,actualPos);
            if (distance<MIN_MOVEMENT){
                am.Stop(mode);
                modeBk = "";
            }
            if (distance>=MIN_MOVEMENT){
                if (mode != modeBk)
                {
                    am.Stop(modeBk);
                    modeBk = mode;
                    am.Play(mode);
                }
            };
        }
        yield break;
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
        am.Play("SwapWeapon");
        selectedWeapon.gameObject.SetActive(false);
        if(selectedIndex == 0 && a < 0){
            selectedIndex = weapons.Count-1;
        }else if(selectedIndex == weapons.Count-1 && a > 0){
            selectedIndex = 0;
        }else{
            selectedIndex += a;
        }
        OnWeaponStateChange.Invoke(selectedWeapon);
        selectedWeapon.gameObject.SetActive(true);
    }

    void InputCuracion()
    {
        if(Input.GetKeyDown(KeyCode.Q) && curas > 0)
        {
            life = maxPlayerLife;
            curas -= 1;
            OnPotionsStateChange.Invoke(actualCuras, maxActualCuras);
            OnPlayerLifeStateChange.Invoke(actualLife, maxPlayerLife);
            print("curas = " + curas);
        }
    }

    // void InputTienda(){
    //     if(Input.GetKeyDown(KeyCode.F))
    //     {
    //         life = maxPlayerLife;
    //         curas -= 1;
    //         OnPlayerLifeStateChange.Invoke(actualLife);
    //     }
    // }

    public void upgradeHealth(int upgradeValue){
        maxPlayerLife += upgradeValue;
        life = maxPlayerLife;
        OnPlayerLifeStateChange.Invoke(actualLife, maxPlayerLife);
        // actualLife = life;
    }

    public void addPotions(int potionsNumber){
        curas += potionsNumber;
        curas = Mathf.Clamp(curas, 0, maxActualCuras);
        OnPotionsStateChange.Invoke(actualCuras, maxActualCuras);
    }

    public void upgradeSpeed(int upgradeValue){
        speed += upgradeValue;
        // actualLife = life;
    }

}
