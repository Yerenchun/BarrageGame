using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    // 子弹使用的数据
    private BulletInfo info;

    // 用于三角函数曲线运动的变量
    private float time;

    // 初始化子弹数据的方法
    public void InitInfo(BulletInfo bulletInfo)
    {
        this.info = bulletInfo;
        // 根据声明周期 延迟移除子弹
        // 当时间到了的时候，我们并不知道子弹是否已经被移除了
        // Destroy(this.gameObject, info.lifeTime);
        // 所以我们可以使用延迟函数来处理
        // 利用延迟函数的特点：如果物体已经被移除了，延迟函数不会再触发
        Invoke("Dead", info.lifeTime);
    }

    private void DealyDestroy(){
        Destroy(this.gameObject);
    }


    // 销毁场景上的子弹
    public void Dead(){
        // 创建特效
        GameObject effObj = Instantiate(Resources.Load<GameObject>(info.deadEffRes));
        // 设置特效的位置
        effObj.transform.position = this.transform.position;
        // 延迟销毁特效
        Destroy(effObj, 1f);

        // 销毁子弹
        Destroy(this.gameObject);
    }

    // 触发碰撞
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            // 玩家受伤扣血
            PlayerObject obj = other.gameObject.GetComponent<PlayerObject>();
            obj.Wound();

            // 销毁自己
            Dead();
        }
    }

    #region 运动逻辑

    void Update()
    {

        // 所有移动的共同特点 都是朝自己的面朝向移动
        this.transform.Translate(Vector3.forward * info.forwardSpeed * Time.deltaTime);

        // 设置移动类型
        // 1 代表 只朝自己面朝向移动 直线运动
        // 2 代表 曲线运动
        // 3 代表 右抛物线
        // 4 代表 左抛物线
        // 5 代表 跟踪导弹
        switch(info.type)
        {
            case 2:
                time += Time.deltaTime;
                // sin中的值变化的快慢，决定左右变化的频率
                // 曲线运动时，roundSpeed 旋转速度 主要用于 控制 变化速率
                this.transform.Translate(Vector3.right * Time.deltaTime * Mathf.Sin(time * info.roundSpeed) * info.upSpeed);
                break;
            case 3:
                // 右抛物线，一边向前移动，一边旋转角度
                this.transform.rotation *= Quaternion.AngleAxis(info.roundSpeed * Time.deltaTime, Vector3.up);
                break;
            case 4:
                // 左抛物线，只不过是旋转方向相反
                this.transform.rotation *= Quaternion.AngleAxis(-info.roundSpeed * Time.deltaTime, Vector3.up);
                break;
            case 5:
                // 追踪子弹
                // 让子弹不停朝玩家移动，即，将玩家的坐标作为目标，进行插值运算
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                    Quaternion.LookRotation(PlayerObject.Instance.transform.position - this.transform.position), 
                                                    info.roundSpeed * Time.deltaTime);
                break;
        }
    }

    #endregion
}
