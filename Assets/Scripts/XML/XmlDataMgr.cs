using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlDataMgr
{
    private static XmlDataMgr instance = new XmlDataMgr();

    public static XmlDataMgr Instance => instance;

    private XmlDataMgr() { }

    /// <summary>
    /// 数据存储方法
    /// </summary>
    /// <param name="data">需要存储的数据</param>
    /// <param name="fileName">文件名</param>
    public void SaveData(object data, string fileName)
    {
        // 1.得到存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".xml";
        // 2.写入文件流
        using (StreamWriter writer = new StreamWriter(path))
        {
            // 3.创建序列化器
            XmlSerializer s = new XmlSerializer(data.GetType());
            s.Serialize(writer, data);
        }
    }
    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="type">需要加载的类型</param>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public object LoadData(Type type, string fileName)
    {
        // 1.还是得到存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".xml";
        // 如果不存在
        if(!File.Exists(path))
        {
            // 加载streamingAssets文件夹中的默认xml文件
            path = Application.streamingAssetsPath + "/" + fileName + ".xml";
            if(!File.Exists(path))
            {
                //如果两个路径都不存在
                // 那么直接new 一个对象给外部，返回一个默认值
                return Activator.CreateInstance(type);
            }
        }

        // 存在路径
        // 2.读取文件流
        using (StreamReader reader = new StreamReader(path))
        {
            // 反序列化
            XmlSerializer s = new XmlSerializer(type);
            return s.Deserialize(reader);
        }
    }
}


