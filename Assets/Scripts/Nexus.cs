using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Nexus : MonoBehaviour
{
    [SerializeField] float life = 10000;
    // life recover x second
    [SerializeField] float regRate = 10;
    [SerializeField] float farmRate = 5;
    [SerializeField] Material nexus2Material;

    float money;

    public UnityEvent<float> OnNexusLifeChange = new UnityEvent<float>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(income());
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

    public float getFarmRate(){
        return farmRate;
    }

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
            money+= getFarmRate();
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
}
