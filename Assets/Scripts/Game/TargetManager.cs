using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    
    //保存所有target下的怪物
    public GameObject[] monsters;

    public GameObject activeMonster = null;

    public int targetType;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject monster in monsters)
        { 
            monster.GetComponent<BoxCollider>().enabled = false;
            monster.SetActive(false);
        }
        //调用协程
        StartCoroutine("AliveTimer");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 激活怪物
    /// </summary>
    private void ActiveMonster()
    {
        int index = Random.Range(0, monsters.Length);
        activeMonster = monsters[index];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("DeathTimer");
    }

    /// <summary>
    /// 迭代器方法，设置生成的等待时间
    /// </summary>
    /// <returns></returns>
    IEnumerator AliveTimer()
    {
        //等待1-4s后执行下面方法
        yield return new WaitForSeconds(Random.Range(1, 5));
        ActiveMonster();  
    }

    /// <summary>
    /// 使激活状态的怪物变为未激活状态
    /// </summary>
    private void DeActiveMonster()
    {
        if(activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }

        StartCoroutine("AliveTimer");
    }

    /// <summary>
    /// 迭代器，设置怪物死亡的时间
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Random.Range(3, 7));
        DeActiveMonster();
    }
    /// <summary>
    /// 更新生命周期
    /// </summary>
    public void UpdateMonsters()
    {
        StopAllCoroutines();
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);      
            activeMonster = null;   
        }
        StartCoroutine("AliveTimer");
    }
    /// <summary>
    /// 按照给定的怪物类型激活怪物
    /// </summary>
    /// <param name="type"></param>
    public void ActiveMonsterByIndex(int type)
    {
        StopAllCoroutines();
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        activeMonster = monsters[type];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine("DeathTimer");
    }

}
