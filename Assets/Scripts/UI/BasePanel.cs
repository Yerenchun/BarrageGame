using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面板基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BasePanel<T> : MonoBehaviour where T : class
{
    // 单例模式
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        // 需要是class才能使用as
        // 所以泛型需要加一个class约束
        instance = this as T;
    }

    void Start()
    {
        // 父类默认调用初始化方法
        // 子类就不用去写Start方法了
        Init();
    }

    /// <summary>
    /// 初始化方法
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void ShowMe(){
        // 小项目适合这么做，这样不存在卡顿和内存问题
        // 后面学习UGUI再学习动态创建UI
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void HideMe(){
        this.gameObject.SetActive(false);
    }
}
