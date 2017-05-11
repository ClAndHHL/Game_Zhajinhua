using UnityEngine;
using System.Collections;

public abstract class ViewChild{
    public GameObject panelObj = null;
    public void Start(bool isSetAnchor = true)
    {
        OnStart();
    }
    public void Destroy()
    {
        OnDestory();
    }
    public void Show(params object[] args)
    {
        panelObj.gameObject.SetActive(true);
        OnShow(args);
        AddEventListener();
    }
    public void Hide()
    {
        RemoveEventListener();
        panelObj.gameObject.SetActive(false);
        OnHide();
    }
    /// <summary>
    /// OnShow 后调用添加事件
    /// </summary>
    protected virtual void AddEventListener()
    {

    }
    /// <summary>
    /// OnHide 前移除事件
    /// </summary>
    protected virtual void RemoveEventListener()
    {

    }

    protected abstract void OnStart();

    protected abstract void OnShow(params object[] args);

    protected abstract void OnHide();
    protected abstract void OnDestory();
    protected T Find<T>(string namePath)
    {
        return panelObj.transform.Find(namePath).GetComponent<T>();
    }
    public bool IsHide()
    {
        return panelObj.gameObject.activeSelf == false;
    }
}
