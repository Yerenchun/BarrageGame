using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 排行榜列表
/// </summary>
public class RankData
{
    public List<RankInfo> lists = new List<RankInfo>();
}

/// <summary>
/// 排行榜单条数据
/// </summary>
public class RankInfo{
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int time;
}
