using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager _instance;

    private Text txtShootNum;
    private Text txtScore;
    private Text txtMessage;

    public int shootNum = 0;
    public int score = 0;


    public AudioSource musicAudio;
    public Toggle musicToggle;
    
    private void Awake()
    {
        _instance = this;
        if(PlayerPrefs.HasKey("isMusicOn"))
        {
            if(1 == PlayerPrefs.GetInt("isMusicOn"))
            {
                musicToggle.isOn = true;
                musicAudio.enabled = true;
            }
            else
            {
                musicToggle.isOn = false;
                musicAudio.enabled = false;
            }
        }
        else
        {
            musicToggle.isOn = true;
            musicAudio.enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        txtShootNum = transform.Find("TxtShootNum").GetComponent<Text>();
        txtScore = transform.Find("TxtScore").GetComponent<Text>();
        txtMessage = transform.Find("Menu").Find("TxtMessage").GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateData();
    }
    /// <summary>
    /// 控制音乐播放
    /// </summary>
     public void OnMusicSwitch()
     {
        if(musicToggle.isOn == false)
        {
            musicAudio.enabled = false;
            PlayerPrefs.SetInt("isMusicOn", 0);
        }
        else
        {
            musicAudio.enabled = true;
            PlayerPrefs.SetInt("isMusicOn", 1);
        }

        PlayerPrefs.Save();
     }
    /// <summary>
    /// 改变分数和射击次数
    /// </summary>
    /// <param name="shootNum"></param>
    /// <param name="score"></param>
    public void ChangeshootNum(int shootNum)
    {
        this.shootNum += shootNum;
    }

    public void ChangeScore(int score)
    {
        this.score += score;
    }
    /// <summary>
    /// 更新UI数据
    /// </summary>
    void UpdateData()
    {
        txtShootNum.text = shootNum.ToString();
        txtScore.text = score.ToString();
    }

    public void ShowMessage(string str)
    {
        txtMessage.text = str;
    }
}
