using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePanel : BasePanel<ChoosePanel>
{
    // 各个按钮
    public UIButton btnLeft;
    public UIButton btnRight;
    public UIButton btnStart;
    public UIButton btnClose;

    // 模型父对象
    public Transform heroPosition;

    // 角色属性
    private List<GameObject> hpObjs = new List<GameObject>();
    private List<GameObject> speedObjs = new List<GameObject>();
    private List<GameObject> volumeObjs = new List<GameObject>();

    // 当前显示的飞机模型对象
    private GameObject airPlaneObj;


    #region 面板的内容
    public override void Init()
    {

        // 循环添加属性子控件
        for (int i = 1; i <= 10; i++)
        {
            hpObjs.Add(GameObject.Find("sprBK/labHp/spr" + i + "/Sprite"));
            speedObjs.Add(GameObject.Find("sprBK/labSpeed/spr" + i + "/Sprite"));
            volumeObjs.Add(GameObject.Find("sprBK/labVolume/spr" + i + "/Sprite"));
        }

        btnLeft.onClick.Add(new EventDelegate(() =>{
            // 控制索引值左移
            GameDataMgr.Instance.ChangeIndexLeft();
            ChangeNowHero();// 刷新角色
        }));
        btnRight.onClick.Add(new EventDelegate(() =>{
            // 控制索引值右移
            GameDataMgr.Instance.ChangeIndexRight();
            ChangeNowHero();// 刷新角色
        }));
        btnClose.onClick.Add(new EventDelegate(() =>{
            // 隐藏自己
            HideMe();
            // 显示开始界面
            BeginPanel.Instance.ShowMe();
        }));
        btnStart.onClick.Add(new EventDelegate(() =>{
            // 开始游戏
            SceneManager.LoadScene("GameScene");
        }));

        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 每次显示的时候，都从第一个开始选择
        GameDataMgr.Instance.nowHeroIndex = 0;
        ChangeNowHero();// 刷新角色
    }

    public override void HideMe()
    {
        base.HideMe();
        // 隐藏的时候，要删除当前的模型
        DestroyObj();
    }

    #endregion

    #region 更换角色模型


    /// <summary>
    /// 刷新角色信息
    /// 更换角色模型，更换属性数据
    /// </summary>
    public void ChangeNowHero(){
        // 得到当前选择的角色信息
        RoleInfo info = GameDataMgr.Instance.GetNowHeroInfo();

        // 更新角色模型
        // 先删除上一次的模型
        DestroyObj();

        // 再创建
        airPlaneObj = Instantiate(Resources.Load<GameObject>(info.resName));
        // 设置父对象
        airPlaneObj.transform.SetParent(heroPosition, false);
        // 设置位置，角度，缩放
        airPlaneObj.transform.localPosition = Vector3.zero;
        airPlaneObj.transform.localRotation = Quaternion.identity;
        airPlaneObj.transform.localScale = Vector3.one * info.scale;

        // 更新角色属性数据
        for(int i = 0; i < 10; i++)
        {
            // 如果i小于某个属性的值，那么该 hpObjs[i]就需要显示
            hpObjs[i].SetActive(info.hp > i);
            speedObjs[i].SetActive(info.speed > i);
            volumeObjs[i].SetActive(info.volume > i);
        }

        // 修改模型的层级
        airPlaneObj.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// 用于删除上一次显示的模型对象
    /// </summary>
    private void DestroyObj(){
        if(airPlaneObj != null){
            Destroy(airPlaneObj);
            airPlaneObj = null;
        }
    }

    #endregion


    #region 鼠标拖动模型

    private float time;
    private bool isSel;// 是否按下鼠标左键
    public Camera uiCamera;

    void Update()
    {
        // 角色上下浮动
        time += Time.deltaTime;
        // 一定要使用世界坐标系，因为当前的本地坐标系，是歪的
        heroPosition.Translate(Vector3.up * Mathf.Sin(time) * 0.0003f, Space.World);

        // 射线检测 可以左右拖动角色
        if(Input.GetMouseButtonDown(0))
        {
            // 如果点击到了 Player层的碰撞器 就可以拖动角色
            // 要使用UI摄像机来发射射线
            if(Physics.Raycast(uiCamera.ScreenPointToRay(Input.mousePosition), 1000, 
                1 << LayerMask.NameToLayer("Player")))
            {
                isSel = true;
                // Debug.Log("检测到了");
            }
        }
        // 抬起 取消选中
        if(Input.GetMouseButtonUp(0))
            isSel = false;

        if(Input.GetMouseButton(0) && isSel)
        {
            // 设置飞机物体的旋转角度
            airPlaneObj.transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse X") * 20, Vector3.up);
        }

    }
    #endregion

}
