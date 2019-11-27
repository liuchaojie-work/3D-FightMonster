using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{

    //最大和最小的x，y轴的旋转角度
    private float maxYRotation = 120.0f;
    private float minYRotation = 0.0f;

    private float maxXRotation = 60.0f;
    private float minXRotation = 0.0f;
    //射击的间隔时长
    private float shootTime = 0.5f;
    //射击间隔的时间的计时器
    private float shootTimer = 0.0f;
    //子弹和子弹的发射位置
    public GameObject bullet;
    public Transform firePosition;
    private AudioSource gunAudio;

    private void Awake()
    {
        gunAudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //当游戏为非暂停状态时，才可以移动枪支和发射子弹
        if(GameManager._instance.isPaused == false)
        {
            ShootBullet();
            GunMove();
        }
        
    }

    /// <summary>
    /// 鼠标控制枪的朝向
    /// </summary>
    private void GunMove()
    {
        float xPosPrecent = Input.mousePosition.x / Screen.width;
        float yPosPrecent = Input.mousePosition.y / Screen.height;

        float xAngle = -Mathf.Clamp(yPosPrecent * maxXRotation, minXRotation, maxXRotation) + 15.0f;
        float yAngle = Mathf.Clamp(xPosPrecent * maxYRotation, minYRotation, maxYRotation) - 60.0f;

        transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
    }
    /// <summary>
    /// 发射子弹
    /// </summary>
    private void ShootBullet()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //实例化子弹
                GameObject go = Instantiate(bullet, firePosition.position, Quaternion.identity);
                //给子弹增加朝向为z轴正方向的力
                go.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
                GetComponent<Animation>().Play();
                shootTimer = 0.0f;
                //播放手枪开火音效
                gunAudio.Play();

                UIManager._instance.ChangeshootNum(1);
            }
            
        }
    }
}
