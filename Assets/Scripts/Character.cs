using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public List<Weapon> weapons;
    public Weapon selectedWeapon => weapons[selectedIndex];
    public int selectedIndex = 0;

    [Header("Stats")]
    [SerializeField] protected float life;
    [SerializeField] protected float speed;
    [SerializeField] public Transform slotWeapon;
    [SerializeField] Canvas hitCanvas;

    [System.NonSerialized] public UnityEvent death = new UnityEvent();

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
        if (other.gameObject.tag==TagsEnum.Bullet.ToString())
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
        }

        // if character touch death tag instadeath
        if (other.gameObject.tag==TagsEnum.Death.ToString())
        {
            GameManager.gameOver();
        }
    }

    protected abstract bool decideDamage(Bullet bullet);

    public void takeDamageRayCast(Weapon weapon){
        life = life - weapon.getDamage();
        // if hit player
        if (gameObject.tag == Tags.PLAYER)
        {
            setOpacityHitScren();
            gameObject.GetComponentInChildren<CameraShake>().shakecamera();
            if (life <= 0) {
                GameManager.gameOver();
            }
        }

        // if hit enemy
        if (gameObject.tag == Tags.ENEMY)
        {
            AudioManager am = FindObjectOfType<AudioManager>();
            print("play hitmarker");
            am.Play("Hitmarker");
            if (life <= 0) {
                Nexus.money += WaveManager.moneyDropByEnemies;
                death.Invoke();
            }
        }

        //if hit boss
        if (gameObject.tag == Tags.BOSS)
        {
            AudioManager am = FindObjectOfType<AudioManager>();
            print("play hitmarker");
            am.Play("Hitmarker");
            if (life <= 0)
            {
                Nexus.money += WaveManager.moneyDropByEnemies;
                death.Invoke();
            }
        }
    }

    protected void InstanciaArmas()
    {
        for (int i = 0; i < weapons.Count; i++)
        {

            GameObject weaponPrefab = weapons[i].gameObject;
            
            // for player weapon not go through terrain and objects
            if (gameObject.tag == Tags.PLAYER) {
                foreach (MeshRenderer item in weaponPrefab.GetComponentsInChildren<MeshRenderer>())
                {
                    item.gameObject.layer = LayerMask.NameToLayer("Weapon");
                }
            }

            weapons[i] = Instantiate(weaponPrefab, slotWeapon.transform.position, slotWeapon.transform.rotation, slotWeapon.transform).GetComponent<Weapon>();
            weapons[i].transform.localPosition = weaponPrefab.transform.position;
            weapons[i].transform.localRotation = weaponPrefab.transform.rotation;
            weapons[i].owner = this;
            weapons[i].gameObject.SetActive(false);
        }
        weapons[0].gameObject.SetActive(true);
    }
    public void setOpacityHitScren(){
        PlayerController pc = gameObject.GetComponent<PlayerController>();
        Color colorHitCanvas = hitCanvas.GetComponentInChildren<Image>().color;
        // actual life % for get HUD opacity
        float lifeP = life * 100 / pc.maxPlayerLife;
        colorHitCanvas.a = 0.5f - (lifeP/100);
        hitCanvas.GetComponentInChildren<Image>().color = colorHitCanvas;
    }
}
