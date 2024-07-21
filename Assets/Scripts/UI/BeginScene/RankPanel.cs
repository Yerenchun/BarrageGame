using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : BasePanel<RankPanel>
{
    public UIButton btnClose;
    public UIScrollView svList;

    // 存储排行榜单条控件的列表
    private List<RankItem> itemList = new List<RankItem>();

    public override void Init()
    {
        btnClose.onClick.Add(new EventDelegate(()=>{
            HideMe();
        }));

        HideMe();

        
        // print(Application.persistentDataPath);
        // 测试排行榜
        // for(int i = 0; i < 7; i++)
        // {
        //     GameDataMgr.Instance.AddRankInfo("ye" + i, Random.Range(40, 4000));
        // }
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 更新面板上面的信息
        UpdatePanelInfo();
    }

    /// <summary>
    /// 刷新排行榜
    /// </summary>
    private void UpdatePanelInfo(){
        List<RankInfo> list = GameDataMgr.Instance.rankData.lists;

        // 根据排行榜更新面板
        // 排行榜上面的数据只会多不会少
        for(int i = 0; i < list.Count; i++){
            // 如果面板上已经存在 控件，只需要将这些控件的信息更新即可
            if(itemList.Count > i)
            {
                // 设置控件显示内容
                itemList[i].InitInfo(i + 1, list[i].name, list[i].time);
            }
            // 如果面板上的数据不足10条，就需要再新创建控件
            else
            {
                // 如果面板上面的控件不到10个，就还需要动态创建控件
                GameObject obj = Instantiate(Resources.Load<GameObject>("UI/RankItem"));
                // 设置父对象
                obj.transform.SetParent(svList.transform, false);
                // 设置位置
                obj.transform.localPosition = new Vector3(344, 456 - i * 45, 0);

                // 得到脚本
                RankItem item = obj.GetComponent<RankItem>();
                // 设置控件显示内容
                item.InitInfo(i + 1, list[i].name, list[i].time);
                
                // 将数据添加到排行榜中
                itemList.Add(item);
            }

        }
    }

}
