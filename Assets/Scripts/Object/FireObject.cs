using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Pos_Type
{
    TopLeft,
    TopRight,
    Top,

    BottomLeft,
    BottomRight,
    Bottom,

    Left,
    Right
}

public class FireObject : MonoBehaviour
{
    public E_Pos_Type type;

    // 屏幕坐标
    private Vector3 screenPos;
    // 发射散弹的方向，作为散弹的初始方向 用于计算
    private Vector3 initDir;
    // 发射散弹时，记录上一次的方向
    private Vector3 nowDir;


    // 当前开火点信息
    private FireInfo fireInfo;
    // 用于计数的东西
    private int nowNum;
    private float nowCD;
    private float nowDelay;
    // 当前要用到的子弹
    private BulletInfo nowBulletInfo;
    // 发射散弹时，每颗子弹的间隔角度，就是每次发射方向需要旋转的角度
    private float changeAngle;


    #region 初始化

    void Awake()
    {
        // 初始化炮台的位置
        // z轴的坐标，就是摄像机的横截面，就是摄像机的y坐标值
        // 因为飞机的坐标是0 0 0，所以飞机所在横截面就是，摄像机的y坐标
        screenPos.z = Camera.main.transform.position.y;
        switch (type)
        {
            case E_Pos_Type.TopLeft:
                screenPos.x = 0;
                screenPos.y = Screen.height;
                // 散弹初始从右开始 转角度发射出去
                initDir = Vector3.right;
                break;
            case E_Pos_Type.TopRight:
                screenPos.x = Screen.width;
                screenPos.y = Screen.height;

                initDir = Vector3.left;
                break;
            case E_Pos_Type.Top:
                screenPos.x = Screen.width / 2;
                screenPos.y = Screen.height;

                initDir = Vector3.right;
                break;
            case E_Pos_Type.BottomLeft:
                screenPos.x = 0;
                screenPos.y = 0;

                initDir = Vector3.left;
                break;
            case E_Pos_Type.BottomRight:
                screenPos.x = Screen.width;
                screenPos.y = 0;

                initDir = Vector3.right;
                break;
            case E_Pos_Type.Bottom:
                screenPos.x = Screen.width / 2;
                screenPos.y = 0;

                initDir = Vector3.left;
                break;
            case E_Pos_Type.Left:
                screenPos.x = 0;
                screenPos.y = Screen.height / 2;

                initDir = Vector3.right;
                break;
            case E_Pos_Type.Right:
                screenPos.x = Screen.width;
                screenPos.y = Screen.height / 2;

                initDir = Vector3.left;
                break;
        }
        // 将屏幕点，转成世界坐标
        this.transform.position = Camera.main.ScreenToWorldPoint(screenPos);

    }

    #endregion

    void Update()
    {
        // 每次都检测 是否需要重置炮台数据
        ResetFireInfo();

        // 开火
        Fire();
    }

    #region 重置炮台

    /// <summary>
    /// 重置开火的方式
    /// </summary>
    private void ResetFireInfo()
    {

        // 只有当cd和数量都为0 时，才需要重新获取我们的 炮台数据
        if (nowCD != 0 && nowNum != 0)
            return;

        // 是否在进行组件休息
        if (fireInfo != null)
        {
            nowDelay -= Time.deltaTime;
            // 组件休息，就直接返回
            if (nowDelay > 0)
                return;
        }


        // 获取炮台数据列表
        List<FireInfo> list = GameDataMgr.Instance.fireData.fireList;

        // int随机数，是左包含，右不包含
        // 获取开火点的信息
        fireInfo = list[Random.Range(0, list.Count)];
        // 临时存储记录信息
        nowNum = fireInfo.num;
        nowCD = fireInfo.cd;
        nowDelay = fireInfo.delay;

        // 通过开火点数据， 取出 当前要使用的子弹数据信息
        // 因为cds关联了子弹的ID
        // 需要得到开始的id 和 结束的id 用来取随机子弹的信息
        string[] strs = fireInfo.ids.Split(',');
        int beginID = int.Parse(strs[0]);
        int endID = int.Parse(strs[1]);
        int randomID = Random.Range(beginID, endID + 1);
        nowBulletInfo = GameDataMgr.Instance.bulletData.bulletList[randomID - 1];

        // 如果是散弹，就需要计算发射方向每次需要旋转的角度
        if (fireInfo.type == 2)
        {
            switch (type)
            {
                case E_Pos_Type.TopLeft:
                case E_Pos_Type.TopRight:
                case E_Pos_Type.BottomLeft:
                case E_Pos_Type.BottomRight:
                    // 得到每次应该旋转的角度
                    changeAngle = 90f / (nowNum + 1);
                    break;
                case E_Pos_Type.Top:
                case E_Pos_Type.Bottom:
                case E_Pos_Type.Left:
                case E_Pos_Type.Right:
                    changeAngle = 180f / (nowNum + 1);
                    break;
            }
        }
    }

    #endregion

    #region 开火逻辑
    private void Fire()
    {
        // 当cd和剩余子弹数量为0时，是不需要发射子弹的
        if(nowCD == 0 && nowCD == 0)
            return;

        // 更新cd
        nowCD -= Time.deltaTime;
        if (nowCD > 0)
            return;

        GameObject bullet;
        BulletObject bulletObj;

        // 根据开火点的信息，来发射子弹
        switch (fireInfo.type)
        {
            // 一个一个的发射子弹 朝向玩家
            case 1:
                // 动态创建子弹
                bullet = Instantiate(Resources.Load<GameObject>(nowBulletInfo.resName));
                // 动态挂载脚本
                bulletObj = bullet.AddComponent<BulletObject>();
                // 传入子弹的信息，初始化子弹
                bulletObj.InitInfo(nowBulletInfo);

                // 设置子弹的位置 和朝向
                bullet.transform.position = this.transform.position;
                // 设置子弹的朝向
                bullet.transform.rotation = Quaternion.LookRotation(PlayerObject.Instance.transform.position - this.transform.position);

                // 弹匣中的子弹数量减1
                nowNum--;
                // 重置cd
                nowCD = nowNum == 0 ? 0 : fireInfo.cd;

                break;
            // 发射散弹
            case 2:
                // 如果没有cd 就一瞬间发射完所有子弹
                if (nowCD == 0)
                {
                    for (int i = 0; i < nowNum; i++)
                    {
                        // 动态创建子弹
                        bullet = Instantiate(Resources.Load<GameObject>(nowBulletInfo.resName));
                        // 动态挂载脚本
                        bulletObj = bullet.AddComponent<BulletObject>();
                        // 传入子弹的信息，初始化子弹
                        bulletObj.InitInfo(nowBulletInfo);
                        // 设置子弹的位置 和朝向
                        bullet.transform.position = this.transform.position;

                        // 每次旋转一个角度 得到一个新的发射方向
                        nowDir = Quaternion.AngleAxis(changeAngle * i, Vector3.up) * initDir;
                        // 将朝向转为四元数
                        bullet.transform.rotation = Quaternion.LookRotation(nowDir);

                        // 因为是瞬间创建完所有子弹 所以重置数据
                        nowCD = nowNum = 0;

                    }
                }
                else
                {
                    // 动态创建子弹
                    bullet = Instantiate(Resources.Load<GameObject>(nowBulletInfo.resName));
                    // 动态挂载脚本
                    bulletObj = bullet.AddComponent<BulletObject>();
                    // 传入子弹的信息，初始化子弹
                    bulletObj.InitInfo(nowBulletInfo);
                    // 设置子弹的位置 和朝向
                    bullet.transform.position = this.transform.position;

                    // 每次旋转一个角度 得到一个新的发射方向
                    nowDir = Quaternion.AngleAxis(changeAngle * (fireInfo.num - nowNum), Vector3.up) * initDir;
                    // 将朝向转为四元数
                    bullet.transform.rotation = Quaternion.LookRotation(nowDir);

                    // 弹匣中的子弹数量减1
                    nowNum--;
                    // 重置cd
                    nowCD = nowNum == 0 ? 0 : fireInfo.cd;
                }
                break;
        }
    }

    #endregion

}
