using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] float life = 10000;
    // life recover x second
    [SerializeField] float regRate = 10;
    [SerializeField] float farmRate = 5;
    [SerializeField] Material nexus2Material;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (life <= 0){
            // Destroy(gameObject);
            Renderer m_Renderer = GetComponent<Renderer>();
            m_Renderer.material = nexus2Material; 
            GameManager.gameOver();
        }
    }
}
