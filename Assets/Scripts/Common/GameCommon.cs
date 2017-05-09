using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  玩家状态
/// </summary>
public enum CardType
{
    FANG = 0,//方板
    MEI = 1,//梅花
    HONG = 2,//红桃
    HEI = 3,//黑桃
}
public enum CardShapeType
{
    DAN_PAI = 0,
    DUI_ZI = 1,
    SHUN_ZI = 2,
    JIN_HUA = 3,
    TONG_HUA_SHUN = 4,
    BAO_ZI = 5
}
public class GameCommon
{
    //npc活动范围的半径
    public static readonly int NPC_MOVEAREA_RAD = 10;
    //城市场景ID
    public static int SCENE_CITY_ID = 1;
    //野外场景ID
    public static int SCENE_YEWAI_ID = 2;
    //登陆场景ID
    public static int SCENE_LOGIN_ID = 3;

    /// <summary>
    /// 地面层的索引
    /// </summary>
    public static int GROUND_LAYER_INDEX = 8;
    //怪物的层
    public static int ENEMY_LAYER = 12;
}


