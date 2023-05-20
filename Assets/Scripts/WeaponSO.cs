using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string wName;
    public GameObject bullet;
    // 6 pistol
    // 10 auto
    public float loaderMaxAmmo;
    // 1 pistol
    // 2 auto
    public float loadTime;
    // 0.5f pistol
    // 0.2f auto
    public float fireRate;
    // 15 pistol
    // 10 auto
    public float damage;
    // 0 pistol
    // 1 auto
    public bool auto;
    public float zoom = 1;
    public string audioFire = "DisparoAuto";
    public string audioReload = "RecargaAuto";
    public RuntimeAnimatorController animatorController;
    public float maxDistance;
    public Sprite customCrossAir;
    public int bulletsNumber;
    // public List<GameObject> upgrades;
    // public int price;
}
