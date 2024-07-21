using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : BasePanel<GamePanel>
{

    public UIButton btnBack;
    public UILabel labTime;

    private List<GameObject> sprList = new List<GameObject>();

    public float nowTime = 0;

    public override void Init()
    {

        // 获取控件
        for(int i = 1; i <= 10; i++)
        {
            sprList.Add(GameObject.Find("HP/spr" + i + "/Sprite"));
        }

        btnBack.onClick.Add(new EventDelegate(() => {
            // 打开确认退出面板
            QuitPanel.Instance.ShowMe();

        }));

        ChangeHp(5);
    }


    // 更新时间
    void Update()
    {
        nowTime += Time.deltaTime;
        // 更新时间显示// 时间存储单位是s，需要换算成时 分 秒
        // 计算的时候，要防止小数
        labTime.text = "";
        // 时
        if((int)nowTime / 3600 > 0)
        {
            labTime.text += ((int)nowTime / 3600) + "h";
        }
        // 分
        // 当前面已经有小时了，尽管是0分，也要写出来
        if((int)nowTime % 3600 / 60 > 0 || labTime.text != "")
        {
            labTime.text += ((int)nowTime % 3600 / 60) + "m";
        }
        // 肯定会有秒
        labTime.text += ((int)nowTime % 60) + "s";

    }

    /// <summary>
    /// 改变血量的公共方法
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="hp"></param>
    public void ChangeHp(int hp){
        for(int i = 0; i < 10; i++)
        {
            sprList[i].gameObject.SetActive(i < hp);
        }
    }


}
