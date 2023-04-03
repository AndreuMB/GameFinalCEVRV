using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected List<Weapon> weapons;
    protected Weapon selectedWeapon => weapons[selectedIndex];
    protected int selectedIndex = 0;

    [Header("Stats")]
    [SerializeField] protected float life;
    [SerializeField] protected float speed;

    public UnityEvent death = new UnityEvent();

    void AddWeapon(Weapon weapon)
    {
        // Comprobar max armas
        weapons.Add(weapon);
    }

    protected void Fire()
    {
        selectedWeapon.Fire();
    }

    protected void OnCollisionEnter(Collision other){
        // if GO is a bullet
        if (other.gameObject.tag==Tags.BULLET)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            // if isn't friendly fire takeDamage
            // if(decideDamage(bullet)) takeDamage(bullet);
        }
    }

    protected abstract bool decideDamage(Bullet bullet);

    // protected void takeDamage(Bullet bullet){
    //     // update character life
    //     life = life - bullet.weapon.getDamage();
    //     print("Characterlife = " + life);
    //     if (life <= 0) {
    //         if (gameObject.tag == Tags.PLAYER)
    //         {
    //             GameManager.gameOver();
    //         }else{
    //             death.Invoke();
    //             Destroy(gameObject);
    //         }
    //     }
    // }

    public void takeDamageRayCast(Weapon weapon){
        life = life - weapon.getDamage();
        print("life " + GetType() + " = " + life);

        // if hit player
        if (gameObject.tag == Tags.PLAYER)
        {
            gameObject.GetComponentInChildren<CameraShake>().shakecamera();
            if (life <= 0) {
                GameManager.gameOver();
            }
        }

        // if hit enemy
        if (gameObject.tag == Tags.ENEMY)
        {
            AudioManager am = FindObjectOfType<AudioManager>();
            am.Play("Hitmarker");
            if (life <= 0) {
                Nexus.money += WaveManager.moneyDropByEnemies;
                death.Invoke();
            }
        }
        // if (life <= 0) {
        //     if (gameObject.tag == Tags.PLAYER)
        //     {
        //         GameManager.gameOver();
        //     }else{
        //         death.Invoke();
        //         // Destroy(gameObject);
        //     }
        // }
    }

    protected void InstanciaArmas()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            // Transform slotWeapon = transform.GetChild(0).GetChild(0);
            // set animator to weapon
            Transform slotWeapon = transform.Find("Head").transform.Find("SlotArma");
            Animator  slotAnimator = slotWeapon.GetComponent<Animator>();
            Animator  weaponAnimator = weapons[i].GetComponent<Animator>();
            slotAnimator.runtimeAnimatorController = weaponAnimator.runtimeAnimatorController;
            // Vector3 offsetWeapon = weapons[i].transform.GetChild(0).position;
            // weapons[i] = Instantiate(weapons[i].gameObject, slotWeapon.position + offsetWeapon, Quaternion.identity, slotWeapon.transform).GetComponent<Weapon>();
            // print(weapons[i].name);
            weapons[i] = Instantiate(weapons[i].gameObject, slotWeapon.position, Quaternion.identity, slotWeapon.transform).GetComponent<Weapon>();
            weapons[i].owner = this;
            weapons[i].gameObject.SetActive(false);
        }
        weapons[0].gameObject.SetActive(true);
    }
}
