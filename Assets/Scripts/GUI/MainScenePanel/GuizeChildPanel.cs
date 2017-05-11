using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuizeChildPanel : IViewBase
{

    private Text guodiText;
    private Text dingzhuText;
    private Text zongzhuText;
    private Text zuidalunText;
    private Text kebilunText;
    private int totalZongzhu = 0;
    protected override void OnStart()
    {
        guodiText = Find<Text>("GuodiText");
        dingzhuText = Find<Text>("DingzhuText");
        zongzhuText = Find<Text>("ZongzhuText");
        zuidalunText = Find<Text>("ZuidalunText");
        kebilunText = Find<Text>("KebilunText");
    }
    public void SetGuoDi(int guodiNum)
    {
        guodiText.text = guodiNum + "";
    }
    public void SetDingZhu(int dingzhuNum)
    {
        dingzhuText.text = dingzhuNum + "";
    }
    public void SetZongZhuAdd(int zongzhuNum)
    {
        totalZongzhu += zongzhuNum;
        zongzhuText.text = totalZongzhu + "";
    }
    public void SetZuidalun(int curlun,int maxlun)
    {
        zuidalunText.text = curlun + "/" + maxlun;
    }
    public void SetKebilun(int curlun,int maxKebilun)
    {
        kebilunText.text = Mathf.Min(curlun,maxKebilun) + "/" + maxKebilun;
    }
    public void Reset()
    {
        totalZongzhu = 0;
        zongzhuText.text = 0 + "";
    }
    protected override void OnShow(params object[] args)
    {
        
    }
    protected override void OnDestory()
    {
    }
    protected override void OnHide()
    {
    }
}
