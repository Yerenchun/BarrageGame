using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 开火点数据集合
/// </summary>
public class FireData
{
    public List<FireInfo> fireList = new List<FireInfo>();
}


/// <summary>
/// 单条开火点数据
/// </summary>
public class FireInfo{
    [XmlAttribute] public int id;// 开火点ID 主要方便我们配置
    [XmlAttribute] public int type;// 开火点类型 是散弹 还是按顺序发射 1顺序 2散弹
    [XmlAttribute] public int num;// 数量 该组子弹 有多少颗
    [XmlAttribute] public float cd;// 每颗子弹的间隔时间
    [XmlAttribute] public string ids;// 关联的 子弹ID 1,0 代表的 就是在1~10ID的 子弹数据中去随机
    [XmlAttribute] public float delay;// 组间 间隔时间
}
