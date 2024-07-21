using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPanel : BasePanel<BeginPanel>
{
    // 获取UI按钮
    public UIButton btnStart;
    public UIButton btnRank;
    public UIButton btnSetting;
    public UIButton btnQuit;
    public override void Init()
    {
        // 监听按钮事件
        btnStart.onClick.Add(new EventDelegate(() => {
            // 显示选角面板
            ChoosePanel.Instance.ShowMe();
            // 隐藏自己
            HideMe();
        }));
        btnRank.onClick.Add(new EventDelegate( () => {
            // 显示排行榜
            RankPanel.Instance.ShowMe();

        }));
        btnSetting.onClick.Add(new EventDelegate( () => {
            // 显示设置面板
            SettingPanel.Instance.ShowMe();
        }));
        btnQuit.onClick.Add(new EventDelegate( () =>{
            print("显示退出");
            // 退出游戏
            Application.Quit();
        }));
    }
}
