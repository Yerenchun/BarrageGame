using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    // 单例模式
    private static PlayerObject instance;
    public static PlayerObject Instance => instance;



    // 血量
    public int nowHp;
    public int maxHp;
    // 速度
    public int speed;
    // 倾斜旋转速度
    public int roundSpeed;
    // 目标四元数角度
    private Quaternion targetQ;

    // 是否死亡
    public bool isDead;

    #region 受伤与死亡

    // 当前世界坐标转屏幕坐标上的点
    private Vector3 nowPos;
    // 上一次玩家的位置
    private Vector3 beforePos;

    /// <summary>
    /// 死亡方法
    /// </summary>
    public void Dead(){
        isDead = true;
        GameOverPanel.Instance.ShowMe();
    }

    /// <summary>
    /// 受伤方法
    /// </summary>
    public void Wound(){
        if(isDead)
            return;
        this.nowHp -= 1;
        // 更新游戏面板上面的血量显示
        GamePanel.Instance.ChangeHp(nowHp);

        if(this.nowHp <= 0){
            this.Dead();
        }
    }

    #endregion

    #region 固定方法

    private float inputX;
    private float inputY;

    void Awake()
    {
        instance = this;
    }

    // 输入检测
    void Update()
    {
        // 如果已经死亡了，就不能再移动
        if(isDead)
            return;

        // 移动逻辑
        inputY = Input.GetAxisRaw("Vertical");

        // 旋转逻辑 只会返回 -1 0 1，不会有渐变
        inputX = Input.GetAxisRaw("Horizontal");

        // 射线检测 用于销毁子弹
        ErasureBullet();
        
    }

    // 运动逻辑
    void FixedUpdate()
    {   
        // 旋转
        RoateToTarget();

        // 移动
        Move();
    }

    #endregion
    
    #region 运动逻辑

    /// <summary>
    /// 向左向右移动的时候，飞机会朝对应的方向倾斜
    /// </summary>
    private void RoateToTarget(){
        // 没有输入的时候，就要回正
        if(inputX == 0)
        {
            targetQ = Quaternion.identity;
        }else{
            // 获取对应方向的旋转角度
            // 按下左键，目标旋转量就是向左旋转20度
            targetQ = Quaternion.AngleAxis(20 * -inputX, Vector3.forward);
        }

        // 让飞机朝目标四元数旋转，使用插值运算
        this.transform.rotation = Quaternion.Slerp(transform.rotation, targetQ, Time.deltaTime * roundSpeed);
    }

    /// <summary>
    /// 飞机前后左右移动
    /// </summary>
    private void Move(){
        // 位移之前记录 之前的位置
        beforePos = this.transform.position;

        // 飞机前后移动参照自己的坐标系
        this.transform.Translate(Vector3.forward * inputY * Time.deltaTime * speed);
        // 飞机左右移动不能参照自己的坐标系，因为此时飞机是有旋转角度的
        this.transform.Translate(Vector3.right * inputX * Time.deltaTime * speed, Space.World);

        // 判断是否超出屏幕范围
        nowPos = Camera.main.WorldToScreenPoint(this.transform.position);
        // 左右溢出判断
        if(nowPos.x < 0 || nowPos.x >= Screen.width)
        {
            // 此时只是x不合法
            // 不应该限制y
            this.transform.position = new Vector3(beforePos.x, transform.position.y, transform.position.z);
        }
        // 上下溢出判断
        if(nowPos.y < 0 || nowPos.y >= Screen.height)
        {
            // 此时只是y不合法，不应该限制x
            this.transform.position = new Vector3(transform.position.x, transform.position.y, beforePos.z);
        }
    }
    #endregion

    #region 鼠标点击消除子弹

    private void ErasureBullet(){
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            // 只检测子弹层
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, 
                    1 << LayerMask.NameToLayer("Bullet")))
            {
                // 获取子弹脚本
                BulletObject bullet = hit.transform.GetComponent<BulletObject>();
                // 直接让被点中的子弹 销毁
                bullet.Dead();
            }
        }
    }

    #endregion

}
