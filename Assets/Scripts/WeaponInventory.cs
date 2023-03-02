using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{

    [SerializeField]
    List<Weapon> armas = new List<Weapon>(2);

    GameObject pistola;
    GameObject fusil;

    int selectedIndex;


    //TODO: [PROPIEDADES] Montar un cursor con un setter personalizado(L:79)
    Weapon Arma2 => armas[selectedIndex];
    Weapon Arma
    {
        get
        {
            return armas[selectedIndex];
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // foreach(Weapon arma in armas)
        // {
        //     Instantiate(arma.gameObject, transform.position, Quaternion.identity, transform);
        // }

        //Instanciar arma

        for (int i = 0; i < armas.Count; i++)
        {
            armas[i] = Instantiate(armas[i].gameObject, transform.position, Quaternion.identity, transform).GetComponent<Weapon>();
            armas[i].gameObject.SetActive(false);
        }
        armas[0].gameObject.SetActive(true);

        // pistola = Instantiate(armas[0], transform.position, Quaternion.identity, transform);
        // fusil = Instantiate(armas[1], transform.position, Quaternion.identity, transform);
        
        //fusil.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ChangeWeapon(1);
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ChangeWeapon(-1);
        }


    }

    void ChangeWeapon(int a){

        //int aux;
        int elegido= 0;

        for(int i = 0; i < armas.Count; i++)
        {
            if(armas[i].gameObject.activeSelf)
            {
                elegido = i;
                
            }
        }

        if(elegido == 0 && a < 0){
            elegido = armas.Count-1;
        }else if(elegido == armas.Count-1 && a > 0){
            elegido = 0;
        }else{
            elegido += a;
        }




        // if(pistola.activeSelf)
        // {
        //     pistola.SetActive(false);
        //     fusil.SetActive(true);
        // }else
        // {
        //     pistola.SetActive(true);
        //     fusil.SetActive(false);
        // }



    }
}
