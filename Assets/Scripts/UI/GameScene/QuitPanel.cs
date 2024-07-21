using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitPanel : BasePanel<QuitPanel>
{
    public UIButton btnSubmit;
    public UIButton btnCancel;

    public override void Init()
    {
        btnSubmit.onClick.Add(new EventDelegate(() => {
            SceneManager.LoadScene("BeginScene");
            Time.timeScale = 1;
        }));

        btnCancel.onClick.Add(new EventDelegate(() => {
            HideMe();
        }));

        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Time.timeScale = 0;
    }

    public override void HideMe()
    {
        base.HideMe();
        Time.timeScale = 1;
    }
}
