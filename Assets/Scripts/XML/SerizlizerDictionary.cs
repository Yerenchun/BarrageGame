using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class SerizlizerDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {   // 固定返回空
        return null;
    }
    // 自定义 字典的反序列化规则
    public void ReadXml(XmlReader reader)
    {
        // 读取用类型包裹的节点
        XmlSerializer keySer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSer = new XmlSerializer(typeof (TValue));

        // 跳过根节点
        reader.Read();
        // 直到读到</dic>时，退出循环
        // <dic>
        //     <int>1</int>
        //     <string>一号位</string>
        //     <int>2</int>
        //     <string>二号位</string>
        //     <int>3</int>
        //     <string>三号位</string>
        // </dic>
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            // 这样是一行一行地反序列化解析，所以每一行有数据的EndElement都读不到
            // 反序列化键值
            TKey key = (TKey)keySer.Deserialize(reader);
            TValue value = (TValue)valueSer.Deserialize(reader);
            this.Add(key, value);
        }
    }

    // 自定义 字典的序列化规则
    public void WriteXml(XmlWriter writer)
    {
        // 获取对应类型的序列化方式
        // 因为字典实际上，是由两种类型的数据组合起来的
        // 所以实际上序列化字典，我们只不需要再去造序列化里面两个数据的序列化轮子的
        // 我们需要做的是，将两个数据对应起来
        XmlSerializer keySer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSer = new XmlSerializer(typeof (TValue));

        foreach (KeyValuePair<TKey,TValue> kv in this) 
        {
            // 键值对的序列化
            keySer.Serialize(writer, kv.Key);
            valueSer.Serialize(writer, kv.Value);
        }
    }
}
