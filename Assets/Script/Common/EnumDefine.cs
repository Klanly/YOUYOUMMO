using UnityEngine;
using System.Collections;

public enum Language
{
    CN,
    EN
}

#region SceneType 场景类型
/// <summary>
/// 场景类型
/// </summary>
public enum SceneType
{
    LogOn,
    WorldMap,
    SelectRole,
    GameLevel,
}
#endregion

#region WindowUIType 窗口类型
/// <summary>
/// 窗口类型
/// </summary>
public enum WindowUIType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None,
    /// <summary>
    /// 登录窗口
    /// </summary>
    LogOn,
    /// <summary>
    /// 注册窗口
    /// </summary>
    Reg,
    /// <summary>
    /// 角色信息窗口
    /// </summary>
    RoleInfo,
    /// <summary>
    /// 进入区服
    /// </summary>
    GameServerEnter,
    /// <summary>
    /// 区服选择
    /// </summary>
    GameServerSelect,
    /// <summary>
    /// 剧情关卡地图
    /// </summary>
    GameLevelMap,
    /// <summary>
    /// 剧情关卡详情
    /// </summary>
    GameLevelDetail,
    /// <summary>
    /// 游戏胜利
    /// </summary>
    GameLevelVictory,
    /// <summary>
    /// 游戏失败
    /// </summary>
    GameLevelFail,
    /// <summary>
    /// 世界地图
    /// </summary>
    WorldMap,
    /// <summary>
    /// 世界地图
    /// </summary>
    WorldMapFail,

}
#endregion
[XLua.LuaCallCSharp]
public enum MessageViewType
{
    Ok,
    OkAndCancel
}


#region WindowUIContainerType UI容器类型
/// <summary>
/// UI容器类型
/// </summary>
public enum WindowUIContainerType
{
    /// <summary>
    /// 左上
    /// </summary>
    TopLeft,
    /// <summary>
    /// 右上
    /// </summary>
    TopRight,
    /// <summary>
    /// 左下
    /// </summary>
    BottomLeft,
    /// <summary>
    /// 右下
    /// </summary>
    BottomRight,
    /// <summary>
    /// 居中
    /// </summary>
    Center
}
#endregion

#region WindowShowStyle 窗口打开方式
/// <summary>
/// 窗口打开方式
/// </summary>
public enum WindowShowStyle
{
    /// <summary>
    /// 正常打开
    /// </summary>
    Normal,
    /// <summary>
    /// 从中间放大
    /// </summary>
    CenterToBig,
    /// <summary>
    /// 从上往下
    /// </summary>
    FromTop,
    /// <summary>
    /// 从下往上
    /// </summary>
    FromDown,
    /// <summary>
    /// 从左向右
    /// </summary>
    FromLeft,
    /// <summary>
    /// 从右向左
    /// </summary>
    FromRight
}
#endregion

#region RoleType 角色类型
/// <summary>
/// 角色类型
/// </summary>
public enum RoleType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 当前玩家
    /// </summary>
    MainPlayer = 1,
    /// <summary>
    /// 怪
    /// </summary>
    Monster = 2,
    /// <summary>
    /// 其他玩家
    /// </summary>
    OtherPlayer=3
}
#endregion

public enum PlayType
{
    PVE,
    PVP
}

/// <summary>
/// 数值变化类型
/// </summary>
public enum ValueChangeType
{
    /// <summary>
    /// 增加
    /// </summary>
    Add=0,
    /// <summary>
    /// 减少
    /// </summary>
    Subtrack=1
}

/// <summary>
/// 角色状态
/// </summary>
public enum RoleState
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 待机
    /// </summary>
    Idle = 1,
    /// <summary>
    /// 跑了
    /// </summary>
    Run = 2,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack = 3,
    /// <summary>
    /// 受伤
    /// </summary>
    Hurt = 4,
    /// <summary>
    /// 死亡
    /// </summary>
    Die = 5,
    /// <summary>
    /// 胜利
    /// </summary>
    Select=6
}

public enum RoleAttackType
{
    /// <summary>
    /// 物理攻击
    /// </summary>
    PhyAttack,
    /// <summary>
    /// 技能攻击
    /// </summary>
   SkillAttack,
}

/// <summary>
/// 角色动画状态名称
/// </summary>
public enum RoleAnimatorState
{
    Idle_Normal=1,
    Idle_Fight=2,
    Run=3,
    Hurt=4,
    Die=5,
    Select=6,
    XiuXian=7,
    Died = 8,
    PhyAttack1 =11,
    PhyAttack2=12,
    PhyAttack3=13,
    Skill1=14,
    Skill2 = 15,
    Skill3 = 16,
    Skill4 = 17,
    Skill5 = 18,
    Skill6 = 19,
}

public enum RoleIdleState
{
    IdleNormal,
    IdleFight,
}
public enum ToAnimatorCondition
{
    ToIdleNormal,
    ToIdleFight,
    ToRun,
    ToHurt,
    ToDie,
    ToDied,
    ToPhyAttack,
    ToXiuXian,
    CurrState,
    ToSelect,
    ToSkill,
}

/// <summary>
/// 游戏难度等级
/// </summary>
public enum GameLevelGrade
{
    /// <summary>
   /// 普通
   /// </summary>
    Normal=0,
    /// <summary>
    /// 困难
    /// </summary>
    Hard=1,
    /// <summary>
    /// 地狱
    /// </summary>
    Hell=2,

}
/// <summary>
/// 物品类型
/// </summary>
public enum GoodsType
{
    /// <summary>
    /// 装备
    /// </summary>
    Equip = 0,
    /// <summary>
    /// 道具
    /// </summary>
    Item = 1,
    /// <summary>
    /// 材料
    /// </summary>
    Material = 2,

}
public enum SpriteSourceType
{
    /// <summary>
    /// 剧情关卡图标
    /// </summary>
    GameLevelIco,
    /// <summary>
    /// 剧情关卡详情图标
    /// </summary>
    GameLevelDetail,
    /// <summary>
    /// 世界地图图标
    /// </summary>
    WorldMapIco,
    /// <summary>
    /// 世界地图小地图
    /// </summary>
    WorldMapSmall
}
public enum UIAudioEffectType
{
    /// <summary>
    /// 按钮点击
    /// </summary>
    ButtonClick = 0,
    /// <summary>
    /// UI关闭
    /// </summary>
    UIClose = 1,
       
}

