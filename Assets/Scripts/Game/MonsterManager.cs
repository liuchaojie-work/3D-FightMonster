using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private Animation anim;

    //定义两个状态的动画
    public AnimationClip idleClip;
    public AnimationClip dieClip;

    public AudioSource kickAudio;

    public int monsterIndex;
    // Start is called before the first frame update
    void Start()
    {
        //保存动画组件
        anim = GetComponent<Animation>();
        anim.clip = idleClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 当子弹碰到自己的时候，销毁子弹
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            Destroy(collision.collider.gameObject);
            //播发碰撞音效
            kickAudio.Play();
            anim.clip = dieClip;
            anim.Play();
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine("Deactive");

            UIManager._instance.ChangeScore(1);
        }
    }
    /// <summary>
    /// 当怪物隐藏的时候，使动画片段改为默认的idle
    /// </summary>
    private void OnDisable()
    {
        anim.clip = idleClip;
    }

    IEnumerator Deactive()
    {
        yield return new WaitForSeconds(0.99f);
        //使当前的怪物变为未激活状态，并使整个循环重新开始
        GetComponentInParent<TargetManager>().UpdateMonsters();
    }
}
