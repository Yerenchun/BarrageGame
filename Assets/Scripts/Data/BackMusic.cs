using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMusic : MonoBehaviour
{
    // 经典单例模式
    private static BackMusic instance;
    public static BackMusic Instance => instance;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        // 根据数据初始化BGM的播放
        ChangeValue(GameDataMgr.Instance.musicData.musicValue);
        ChangeOpen(GameDataMgr.Instance.musicData.musicIsOpen);
    }
    /// <summary>
    /// 改变实际的BGM的音量大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeValue(float value){
        audioSource.volume = value;
    }

    /// <summary>
    /// 改变实际的BGM是否播放
    /// </summary>
    /// <param name="isOpen"></param>
    public void ChangeOpen(bool isOpen)
    {
        // 将是否开启设置为静音
        // 开启就是不静音
        audioSource.mute = !isOpen;
    }

}
