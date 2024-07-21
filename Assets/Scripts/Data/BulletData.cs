using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// 子弹数据集合
/// </summary>
public class BulletData
{
    public List<BulletInfo> bulletList = new List<BulletInfo>();
}

/// <summary>
/// 子弹单条数据
/// </summary>
public class BulletInfo{
    [XmlAttribute] public int id;// 子弹数据的ID 方便我们配置的时候，查看并修改数据，没有实际的作用
    [XmlAttribute] public int type;// 子弹移动规则 1~5代表五种不同的规则
    [XmlAttribute] public float forwardSpeed;// 水平方向速度
    [XmlAttribute] public float upSpeed;// 垂直方向速度
    [XmlAttribute] public float roundSpeed;// 旋转速度
    [XmlAttribute] public string resName;// 子弹特效资源路径
    [XmlAttribute] public string deadEffRes;// 子弹销毁时 创建销毁的特效
    [XmlAttribute] public float lifeTime;// 子弹的声明周期
}
