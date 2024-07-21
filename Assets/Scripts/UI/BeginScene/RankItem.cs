using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 排行榜单条控件
/// </summary>
public class RankItem : MonoBehaviour
{

    public UILabel labRank;
    public UILabel labName;
    public UILabel labTime;

    /// <summary>
    /// 根据排行榜单条数据 对组合控件 进行显示初始化
    /// </summary>
    /// <param name="rank">排名</param>
    /// <param name="name">名字</param>
    /// <param name="time">时间</param>
    public void InitInfo(int rank, string name, int time)
    {
        labRank.text = rank.ToString();
        labName.text = name.ToString();

        // 时间存储单位是s，需要换算成时 分 秒
        labTime.text = "";
        // 时
        if(time / 3600 > 0)
        {
            labTime.text += (time / 3600) + "h";
        }
        // 分
        // 当前面已经有小时了，尽管是0分，也要写出来
        if(time % 3600 / 60 > 0 || labTime.text != "")
        {
            labTime.text += (time % 3600 / 60) + "m";
        }
        // 肯定会有秒
        labTime.text += (time % 60) + "s";
    }
}
