using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    float money;
    // Start is called before the first frame update
    void Start()
    {
        GameObject nexus = GameObject.FindWithTag("Nexus");
        StartCoroutine(income(nexus.GetComponent<Nexus>().getFarmRate()));
    }

    IEnumerator income(float fr){
        while (isActiveAndEnabled)
        {
            print(money);
            money+= fr;
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

    public void setMoney(float moneySet){
        money=moneySet;
    }
    
    public float getMoney(){
        return money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
