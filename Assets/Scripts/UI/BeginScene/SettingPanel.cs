using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : BasePanel<SettingPanel>
{
    public UIButton btnClose;
    public UISlider sliderMusic;
    public UISlider sliderSound;
    public UIToggle toggleMusic;
    public UIToggle toggleSound;

    public override void Init()
    {
        btnClose.onClick.Add(new EventDelegate(()=>{
            // 关闭当前面板
            HideMe();
        }));
        sliderMusic.onChange.Add(new EventDelegate(()=>{
            // 更改BGM音量大小，将当前数据存储到中转器
            GameDataMgr.Instance.SetMusicValue(sliderMusic.value);
        }));
        sliderSound.onChange.Add(new EventDelegate(()=>{
            // 更改VFX音量大小，存储数据
            GameDataMgr.Instance.SetSoundValue(sliderSound.value);
        }));

        toggleMusic.onChange.Add(new EventDelegate(()=>{
            // 是否开启BGM，存储数据
            GameDataMgr.Instance.SetMusicIsOpen(toggleMusic.value);
        }));
        toggleSound.onChange.Add(new EventDelegate(()=>{
            // 是否开启VFX，存储数据
            GameDataMgr.Instance.SetSoundIsOpen(toggleSound.value);
        }));

        // 初始化就关闭
        HideMe();
    }

    public void UpdatePanelInfo(){
        MusicData musicData = GameDataMgr.Instance.musicData;
        // 设置面板的内容
        toggleMusic.value = musicData.musicIsOpen;
        toggleSound.value = musicData.soundIsOpen;
        sliderMusic.value = musicData.musicValue;
        sliderSound.value = musicData.soundValue;
    }


    // 加载数据
    public override void ShowMe()
    {
        base.ShowMe();
        // 显示时，根据存储的数据更新面板内容
        UpdatePanelInfo();
    }

    // 存储数据
    public override void HideMe()
    {
        base.HideMe();
        // 隐藏时，将面板的数据，存储起来
        GameDataMgr.Instance.SaveMusicData();
    }
}
