using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using System.Xml;
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public bool isPaused = true;
    public GameObject menu;

    public GameObject[] targets;
 
    private void Awake()
    {
        _instance = this;
        Pause();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }


    
    /// <summary>
    /// 暂停状态方法
    /// </summary>
    private void  Pause()
    {
        isPaused = true;
        menu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    /// <summary>
    /// 非暂停状态方法
    /// </summary>
    private void UnPause()
    {
        isPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }
    /// <summary>
    /// 创建SaveData对象并存储当前游戏状态信息
    /// </summary>
    /// <returns></returns>
    private SaveData CreateSaveGO()
    {
        //新建一个SaveData对象
        SaveData saveData = new SaveData();
        //遍历所有的target，若其中有处于激活状态的怪物，就将编号存储下落，并存储到怪物的编号
        foreach(GameObject targetGo in targets)
        {
            TargetManager targetManager = targetGo.GetComponent<TargetManager>();
            if(targetManager.activeMonster != null)
            {
                saveData.livingTarfetPositions.Add(targetManager.targetType);
                int type = targetManager.activeMonster.GetComponent<MonsterManager>().monsterIndex;
                saveData.livingMonsterTypes.Add(type);
            }
        }

        saveData.shootNum = UIManager._instance.shootNum;
        saveData.score = UIManager._instance.score;

        return saveData;
    }

    /// <summary>
    /// 通过读档信息重置我们的游戏状态
    /// </summary>
    /// <param name="saveData"></param>
    private void SetGame(SaveData saveData)
    {
        //先将所有的target里面的怪物清空，并重置所有计时
        foreach(GameObject target in targets)
        {
            target.GetComponent<TargetManager>().UpdateMonsters();
        }
        //通过反序列化得到SaveData对象中存储的信息，激活指定的怪物
        for(int i = 0; i < saveData.livingTarfetPositions.Count; i++)
        {
            int position = saveData.livingTarfetPositions[i];
            int type = saveData.livingMonsterTypes[i];

            targets[position].GetComponent<TargetManager>().ActiveMonsterByIndex(type);
        }
        //更新UI显示
        UIManager._instance.shootNum = saveData.shootNum;
        UIManager._instance.score = saveData.score;
        //调整为不暂停状态
        UnPause();
    }
    /// <summary>
    /// 二进制方法存档和读档
    /// </summary>
    private void SaveByBin()
    {
        //序列化过程，将SaveData对象转换为字节流
        //创建SaveData对象 并保存当前游戏状态
        SaveData saveData = CreateSaveGO();
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = File.Create(Application.dataPath + "/StreamingPath" + "/SaveByBin.txt");
        //用二进制格式化程序和序列化方法来序列化SavaData对象,参数:创建的文件流和需要序列化的对象
        bf.Serialize(fileStream,saveData);
        //关闭流
        fileStream.Close();

        //若文件存在，则显示保存成功
        if (File.Exists(Application.dataPath + "/StreamingPath" + "/SaveByBin.txt"))
        {
            UIManager._instance.ShowMessage("保存成功");
        }

    }
    
    private void LoadByBin()
    {
        if(File.Exists(Application.dataPath + "/StreamingPath" + "/SaveByBin.txt"))
        {
            //反序列化过程
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fileStream = File.Open(Application.dataPath + "/StreamingPath" + "/SaveByBin.txt", FileMode.Open);
            //调用格式化程序的反序列方法，将文件流转换为一个SaveData对象
            SaveData saveData = (SaveData)bf.Deserialize(fileStream);
            //关闭文件流
            fileStream.Close();
            UIManager._instance.ShowMessage("加载成功！");
            SetGame(saveData);
            UIManager._instance.ShowMessage("");
        }
        else
        {
            UIManager._instance.ShowMessage("存档文件不存在！");
        }
        
    }

    /// <summary>
    /// XML存档和读档
    /// </summary>
    private void SaveByXML()
    {
        SaveData saveData = CreateSaveGO();
        //创建XML文件的存储路径
        string filePath = Application.dataPath + "/StreamingPath" + "/SaveByXML.txt";
        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveFile1");
        //创建XmlElement
        XmlElement target;
        XmlElement targetPosition;
        XmlElement monsterType;

        //遍历saveData中存储的数据，将数据转换成XML格式
        for(int i = 0; i < saveData.livingTarfetPositions.Count; i++)
        {
            target = xmlDoc.CreateElement("target");
            targetPosition = xmlDoc.CreateElement("targetPosition");
            //设置InnerText值
            targetPosition.InnerText = saveData.livingTarfetPositions[i].ToString();
            monsterType = xmlDoc.CreateElement("monsterType");
            monsterType.InnerText = saveData.livingMonsterTypes[i].ToString();


            //设置节点间的层级关系 root target （targetPosition, monsterType）
            target.AppendChild(targetPosition);
            target.AppendChild(monsterType);
            root.AppendChild(target);
        }

        //设置射击数和分数节点 并设置 层级关系 xmlDoc -- root --(target（targetPosition, monsterType）, shootNum, score)
        XmlElement shootNum = xmlDoc.CreateElement("shootNum");
        shootNum.InnerText = saveData.shootNum.ToString();
        root.AppendChild(shootNum);

        XmlElement score = xmlDoc.CreateElement("score");
        score.InnerText = saveData.score.ToString();
        root.AppendChild(score);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if(File.Exists(Application.dataPath + "/StreamingPath" + "/SaveByXML.txt"))
        {
            UIManager._instance.ShowMessage("保存成功！");
        }
    }

    private void LoadByXML()
    {
        string filePath = Application.dataPath + "/StreamingPath" + "/SaveByXML.txt";
        if(File.Exists(filePath))
        {
            SaveData saveData = new SaveData();
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            //通过节点名称来获取元素，结果为XmlNodeList类型
            XmlNodeList targets = xmlDoc.GetElementsByTagName("target");
            //遍历所有target节点，并获取子节点和子结点的InnerText
            if(targets.Count != 0)
            {
                foreach(XmlNode target in targets)
                {
                    XmlNode targetPosition = target.ChildNodes[0];
                    int targetPositionIndex = int.Parse(targetPosition.InnerText);
                    //把得到的值存储在saveData中
                    saveData.livingTarfetPositions.Add(targetPositionIndex);

                    XmlNode monsterType = target.ChildNodes[1];
                    int monsterTypeIndex = int.Parse(monsterType.InnerText);
                    saveData.livingMonsterTypes.Add(monsterTypeIndex);
                }
                
            }
            //得到存储的射击数和分数
            XmlNodeList shootNum = xmlDoc.GetElementsByTagName("shootNum");
            int shootNumCount = int.Parse(shootNum[0].InnerText);
            saveData.shootNum = shootNumCount;

            XmlNodeList score = xmlDoc.GetElementsByTagName("score");
            int scoreCount = int.Parse(score[0].InnerText);
            saveData.score = scoreCount;

            SetGame(saveData);

            UIManager._instance.ShowMessage("");
        }
        else
        {
            UIManager._instance.ShowMessage("存档文件不存在");
        }
    }

    /// <summary>
    /// JSON 读档和存档
    /// </summary>
    private void SaveByJSON()
    {
        SaveData saveData = CreateSaveGO();
        string filePath = Application.dataPath + "/StreamingPath" + "/SaveByJSON.json";
        //利用JsonMapper将saveData对象转换为Json格式的字符串
        string saveJsonStr = JsonMapper.ToJson(saveData);
        //将这个字符串写入到文件中
        //创建一个StreamWriter，并将字符串写入文件中
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        //关闭StreamWriter
        sw.Close();

        UIManager._instance.ShowMessage("保存成功！");
    }

    private void LoadByJSON()
    {
        string filePath = Application.dataPath + "/StreamingPath" + "/SaveByJSON.json";
        if(File.Exists(filePath))
        {
            //创建一个StreamReader,用来读取流
            StreamReader sr = new StreamReader(filePath);
            //将读取到的流赋值给jsonStr
            string jsonStr = sr.ReadToEnd();
            //关闭
            sr.Close();

            //将字符串jsonStr转换为SaveData对象
            SaveData saveData = JsonMapper.ToObject<SaveData>(jsonStr);
            SetGame(saveData);
            UIManager._instance.ShowMessage("");
        } 
        else
        {
            UIManager._instance.ShowMessage("存档文件不存在！");
        }
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void OnNewGame()
    {
        foreach(GameObject target in targets)
        {
            target.GetComponent<TargetManager>().UpdateMonsters();
        }
        UIManager._instance.shootNum = 0;
        UIManager._instance.score = 0;
        UIManager._instance.ShowMessage("");

        UnPause();
    }

    /// <summary>
    /// 继续游戏，从暂停状态变为非暂停状态
    /// </summary>
    public void OnContinueGame()
    {
        UnPause();
        UIManager._instance.ShowMessage("");
    }
    /// <summary>
    /// 保存游戏
    /// </summary>
    public void OnSaveGame()
    {
        //SaveByBin();
        //SaveByJSON();
        SaveByXML();
    }
    /// <summary>
    /// 加载游戏
    /// </summary>
    public void OnLoadGame()
    {
        //LoadByBin();
        //LoadByJSON();
        LoadByXML();
        
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void OnQuitGame()
    {
        Application.Quit();
    }
}
