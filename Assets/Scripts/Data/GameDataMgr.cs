using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();

    public static GameDataMgr Instance => instance;

    // 音乐相关数据
    public MusicData musicData;

    // 排行榜数据
    public RankData rankData;

    // 角色数据
    public RoleData roleData;

    // 子弹数据列表
    public BulletData bulletData;
    // 子弹开火点数据列表
    public FireData fireData;

    // 当前角色的索引值
    public int nowHeroIndex = 1;

    private GameDataMgr(){

        // 初始化数据管理器
        // 获取本地存储的音乐相关数据
        musicData = XmlDataMgr.Instance.LoadData(typeof(MusicData), "MusicData") as MusicData;

        // 初始化排行榜
        rankData = XmlDataMgr.Instance.LoadData(typeof (RankData), "RankData") as RankData;

        // 初始化角色数据
        roleData = XmlDataMgr.Instance.LoadData(typeof(RoleData), "RoleData") as RoleData;

        // 初始化子弹数据
        bulletData = XmlDataMgr.Instance.LoadData(typeof(BulletData), "BulletData") as BulletData;

        // 初始化开火点数据
        fireData = XmlDataMgr.Instance.LoadData(typeof(FireData), "FireData") as FireData;
    }

    #region 音乐相关数据的方法

    /// <summary>
    /// 保存音乐相关数据的方法
    /// </summary>
    public void SaveMusicData(){
        XmlDataMgr.Instance.SaveData(musicData, "MusicData");
    }

    // 这个方法甚至用不到，因为中间数据也存储了面板的内容，初始化的时候，会将内存中的东西加载到中转站
    // 而打开面板的时候，就会将中转站的数据放在面板上去
    // 调整面板上面的数据，就会修改中转站的数据
    // 关闭面板就会将中转站的数据存储到硬盘中
    // 由此形成了闭环
    // public void LoadMusicData(){
    //     musicData = XmlDataMgr.Instance.LoadData(typeof(MusicData), "MusicData") as MusicData;
    // }

    /// <summary>
    /// 设置是否开启BGM
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetMusicIsOpen(bool isOpen){
        // 改当前临时变量的数据
        musicData.musicIsOpen = isOpen;
        // 设置BGM播放器是否静音
        BackMusic.Instance.ChangeOpen(isOpen);
    }

    /// <summary>
    /// 设置BGM的音量大小
    /// </summary>
    /// <param name="value"></param>
    public void SetMusicValue(float value){
        musicData.musicValue = value;
        // 设置实际的播放器的音量
        BackMusic.Instance.ChangeValue(value);
    }

    /// <summary>
    /// 设置是否开启VFX
    /// </summary>
    /// <param name="isOpen"></param>
    public void SetSoundIsOpen(bool isOpen){
        // 改当前临时变量的数据
        musicData.soundIsOpen = isOpen;
    }

    /// <summary>
    /// 设置音效的音量大小
    /// </summary>
    /// <param name="value"></param>
    public void SetSoundValue(float value){
        musicData.soundValue = value;
    }

    #endregion


    #region 排行榜数据相关

    /// <summary>
    /// 保存当前排行榜
    /// </summary>
    public void SaveRankData(){
        XmlDataMgr.Instance.SaveData(rankData, "RankData");
    }

    /// <summary>
    /// 添加单条排行榜数据
    /// </summary>
    /// <param name="name">玩家名</param>
    /// <param name="time">游戏时长</param>
    public void AddRankInfo(string name, int time){
        rankData.lists.Add(new RankInfo(){name = name, time = time});

        // 维护排行榜
        ServiceRankData();
    }

    /// <summary>
    /// 维护十条有序的排行榜数据
    /// </summary>
    public void ServiceRankData(){
        // 默认排序是从小到大
        // 现在要从大到小排序
        // 返回 1 表示 a 在 b 的后面，所以是降序排列
        rankData.lists.Sort((a, b)=> a.time < b.time ? 1 : -1);

        // 移除10条以外的数据
        if(rankData.lists.Count > 10)
        {
            // 因为是一条一条的添加，所以可以直接移除最后一条数据
            rankData.lists.RemoveAt(10);
            // rankData.lists.RemoveRange(10, rankData.lists.Count - 10);
        }

        SaveRankData();
    }

    #endregion


    #region 角色数据相关

    /// <summary>
    /// 提供给外部 获取当前选择角色的数据
    /// </summary>
    /// <returns></returns>
    public RoleInfo GetNowHeroInfo(){
        return roleData.roleList[nowHeroIndex];
    }

    /// <summary>
    /// 左移索引值
    /// </summary>
    public void ChangeIndexLeft(){
        nowHeroIndex--;
        // 索引超过最小，就重置为最大
        if(nowHeroIndex < 0)
        {
            nowHeroIndex = roleData.roleList.Count - 1;
        }
    }

    /// <summary>
    /// 右移索引值
    /// </summary>
    public void ChangeIndexRight(){
        nowHeroIndex++;
        // 索引值超过最大，就重置为最小
        if(nowHeroIndex >= roleData.roleList.Count)
        {
            nowHeroIndex = 0;
        }
    }

    #endregion
}
