using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Nexus : MonoBehaviour
{
    [SerializeField] 
    float life = 10000;
    // life recover x second
    [SerializeField] 
    float regRate = 10;
    [SerializeField] 
    int farmRate = 5;
    int actualFarmRate => farmRate;
    [SerializeField] 
    Material nexus2Material;

    public static int money;

    public UnityEvent<float> OnNexusLifeChange = new UnityEvent<float>();
    public UnityEvent<int> OnNexusMoneyChange = new UnityEvent<int>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(income());
        OnNexusLifeChange.Invoke(life);
        OnNexusMoneyChange.Invoke(money);
        FindObjectOfType<AudioManager>().Play("Taladro");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IEnumerator moneyProduction(){
    //     while (isActiveAndEnabled)
    //     {
            
    //     }
    //     yield break;
    // }

    IEnumerator regenerationLife(){
        while (isActiveAndEnabled){
            if (life <= life - regRate)
            {
                life+= regRate;
                OnNexusLifeChange.Invoke(life);
            }
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag==Tags.BULLET)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet.owner.GetType() == typeof(Enemy)) {
                float damage = bullet.weapon.getDamage();
                life-=damage;
                OnNexusLifeChange.Invoke(life);
                if (life <= 0){
                    // Destroy(gameObject);
                    Renderer m_Renderer = GetComponent<Renderer>();
                    m_Renderer.material = nexus2Material; 
                    GameManager.gameOver();
                }
            }
        }
    }

    public void takeDamageRayCast(Weapon weapon){
        float damage = weapon.getDamage();
        life-=damage;
        OnNexusLifeChange.Invoke(life);
        if (life <= 0){
            // Destroy(gameObject);
            Renderer m_Renderer = GetComponent<Renderer>();
            m_Renderer.material = nexus2Material; 
            GameManager.gameOver();
        }
    }

    IEnumerator income(){
        while (isActiveAndEnabled)
        {
            // print(money = money);
            money+= actualFarmRate;
            OnNexusMoneyChange.Invoke(money);
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
}
