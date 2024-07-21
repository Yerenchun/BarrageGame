using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        // 根据开始场景中，选择的角色，动态创建该角色
        RoleInfo info = GameDataMgr.Instance.GetNowHeroInfo();

        GameObject plane = Instantiate(Resources.Load<GameObject>(info.resName), Vector3.zero, Quaternion.identity);
        PlayerObject playerObj = plane.AddComponent<PlayerObject>();
        playerObj.speed = info.speed * 20;
        playerObj.maxHp = 10;
        playerObj.nowHp = info.hp;
        playerObj.roundSpeed = 20;
        
        // 更新面板上显示的血量
        GamePanel.Instance.ChangeHp(info.hp);

        // print(Application.persistentDataPath);
    }
}
