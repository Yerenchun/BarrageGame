using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : BasePanel<GameOverPanel>
{

    public UIButton btnSubmit; 
    public UILabel labTime;
    public UIInput inputName;

    private int endTime;
    public override void Init()
    {
        btnSubmit.onClick.Add(new EventDelegate(() => {
            // 要把玩家的成绩，保存到排行榜中 
            GameDataMgr.Instance.AddRankInfo(inputName.value, endTime);
            // 切回开始场景
            Time.timeScale = 1;
            SceneManager.LoadScene("BeginScene");
        }));

        HideMe();

        // 使用延迟函数进行测试
        // Invoke("TestFun", 6);
    }

    void TestFun(){
        GameOverPanel.Instance.ShowMe();
    }
    
    public override void ShowMe()
    {
        base.ShowMe();
        // 显示的时候游戏暂停
        Time.timeScale = 0;
        // 显示面板时，就应该去记录 当前的成绩
        endTime = (int)GamePanel.Instance.nowTime;
        // 从游戏界面 得到显示的时间
        labTime.text = GamePanel.Instance.labTime.text;
    }

}
