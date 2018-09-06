﻿using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 加载Buffer控制器的脚本
/// </summary>
public class BufferController : MonoBehaviour {
    public BufferMgr m_BufferMgr;
    /// <summary>
    /// Buff大小
    /// </summary>
    public float buffScale = 1f;
    /// <summary>
    /// Buff在X轴的偏移
    /// </summary>
    public float xDis = 0f;
    /// <summary>
    /// Buff在Y轴的偏移
    /// </summary>
    public float yDis = 0f;
    private MonsterMessage monsterMessage;
    // Use this for initialization
    void Start() {
        m_BufferMgr = new BufferMgr(transform.parent.parent.transform);
        monsterMessage = transform.parent.parent.GetComponent<MonsterMessage>();
        buffScale = 1f;
    }

    // Update is called once per frame
    void Update() {
        m_BufferMgr.Update();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Z) && monsterMessage.HasActive && monsterMessage.targetCt.hasFindTarget) {
            //BufferAddDamage (1, 5, 1, false, false, false);
            //PerDeBufferPoison (5, 1, 10, 1, false, false);
            //PerDeBufferIgnition (10, 3, 5, 1, false, false);
            //PerDeBufferBleed(5, 3, 15, 1, false, false);
            //DeBufferSunderArmor (5, 5, 1, false, false);
            //PerBufferAddBlood (10, 3, 5, 1, false, false, false);
            //BufferSpeedUp (0.5f, 6, 1, false, false);
            //DeBufferSlowDown (0.5f, 5, 2, false, false);
            //DeBufferDizzness (5, 1, false, false);
            //DeBufferFreeze (5, 1, false, false);
            //BufferRelive (100, 100, 10, 0, false, true);
            //PerDeBufferIgnition (1, 2, 10, 1, false, false);
            //BufferAddAttackSpeed (3, 5, 1, false, false);
        }
        if (Input.GetKeyDown(KeyCode.X) && transform.parent.parent.GetComponent<MonsterMessage>().HasActive) {
            RemoveAllTypeBuffe();
        }
#endif
    }

    /// <summary>
    /// 加速Buffer
    /// </summary>
    /// <param name="value">加速比例(为1时加速一倍).</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase BufferSpeedUp(float value, float continueTime, int bufferUI, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = value;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferSpeedUp, args);
    }

    /// <summary>
    /// 一次性加血buff
    /// </summary>
    /// <param name="value">加血比例</param>
    /// <param name="bufferUI">UI显示</param>
    /// <param name="usePercent">是否使用百分比</param>
    /// <param name="scale">buffUI大小</param>
    public void BufferAddBlood(float value, int bufferUI, bool usePercent, float scale) {
        BufferArgs args = new BufferArgs();
        args.m_bUsePercent = usePercent;
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = value;
        args.m_Addition = false;
        args.m_fValue2 = scale;
        args.m_ContinuousTime = 0f;
        m_BufferMgr.BufferAdd(BufferType.Buffer.BufferAddBlood, args);
    }

    /// <summary>
    /// 一段时间增加攻击力Buffer
    /// </summary>
    /// <param name="value">增加攻击力的数值.</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="usePercent">If set to <c>true</c> 是否使用百分比添加.</param>
    /// <param name="permanent">If set to <c>true</c> 能够永久添加.</param>
    public BufferStateBase BufferAddDamage(float value, float continueTime, int bufferUI, bool addMore, bool usePercent, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = value;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_bUsePercent = permanent;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferAddDamage, args);
    }

    /// <summary>
    /// 重生Buffer
    /// </summary>
    /// <param name="value">重生概率.</param>
    /// <param name="reliveHP">重生时血量回复百分比.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    /// <summary>
    public BufferStateBase BufferRelive(float probability, float reliveHP, float continueTime, int bufferUI, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = probability;
        args.m_fValue2 = reliveHP;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferRelive, args);
    }

    /// 狂暴Buffer，一段时间增加攻击力和移动速度
    /// </summary>
    /// <param name="speedAdd">速度增加百分比，单位为1</param>
    /// <param name="attackAdd">攻击力增加值.</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否能重复添加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase BufferRage(float speedAdd, float attackAdd, float continueTime, int bufferUI, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = speedAdd;
        args.m_fValue2 = attackAdd;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferRage, args);
    }

    /// <summary>
    /// 增加攻速Buffer
    /// </summary>
    /// <param name="attackSpeedAdd">攻速加成百分比.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否能重复添加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久.</param>
    public BufferStateBase BufferAddAttackSpeed(float attackSpeedAdd, float continueTime, int bufferUI, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = attackSpeedAdd;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferAddAttackSpeed, args);
    }

    /// <summary>
    /// 增加防御力
    /// </summary>
    /// <param name="difensiveAdd">增加的防御力数值.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否能重复添加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase BufferAddDefensive(float difensiveAdd, float continueTime, int bufferUI, bool addMore, bool permanent,bool total,int grade,bool playAttackEffect) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = difensiveAdd;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferAddDefensive, args);
    }

    /// <summary>
    /// 无敌
    /// </summary>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否能重复添加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase BufferInvincible(float continueTime, int bufferUI, bool addMore, bool permanent,bool total,int grade,bool playAttackEffect) {
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        args.m_bValue1 = total;
        args.m_iValue1 = grade;
        args.m_bValue2 = playAttackEffect;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferInvincible, args);
    }
    /// <summary>
    /// 增加一个抵挡伤害的护盾buff
    /// </summary>
    /// <param name="shieldNum">护盾数值</param>
    /// <param name="continueTime">持续时间</param>
    /// <param name="bufferUI">UI显示</param>
    /// <param name="addMore">是否能重复添加</param>
    /// <param name="permanent">是否永久添加</param>
    /// <returns></returns>
    public BufferStateBase BufferShield(float shieldNum, float continueTime, int bufferUI, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_fValue1 = shieldNum;
        args.m_iBufferUI = bufferUI;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.Buffer.BufferShield, args);
    }
    /// <summary>
    /// 怪物周期性加血Buffer
    /// </summary>
    /// <param name="value">加血数值.</param>
    /// <param name="refreshTime">加血刷新时间.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI显示.</param>
    /// <param name="usePercent">If set to <c>true</c>是否使用百分比.</param>
    /// <param name="addMore">If set to <c>true</c> 能否重复添加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase PerBufferAddBlood(float value, float refreshTime, float continueTime, int bufferUI, bool usePercent, bool addMore, bool permanent) {
        BufferArgs args = new BufferArgs();
        args.m_bUsePercent = usePercent;
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = value;
        args.m_fValue2 = refreshTime;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.PerBuffer.PerBufferAddBlood, args);
    }

    /// <summary>
    /// 减速Buffer
    /// </summary>
    /// <param name="value">减速比例（取值范围0-1）.</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase DeBufferSlowDown(float value, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_fValue1 = value;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.DeBuffer.DeBufferSlowDown, args);
    }

    /// <summary>
    /// 眩晕Buffer，怪物会停止移动攻击等行为
    /// </summary>
    /// <param name="value">眩晕时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="usefolForSuperArmor">对霸体怪物是否起作用</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase DeBufferDizzness(float continueTime, int bufferUI, bool usefolForSuperArmor, bool addMore, bool permanent) {
        if (transform.parent.parent.GetComponent<MonsterActionCtl>().GetState() >= MonsterState.UnderThumpAttack) {
            return null;
        }
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_bValue1 = usefolForSuperArmor;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.DeBuffer.DeBufferDizzness, args);
    }

    /// <summary>
    /// 冰冻Buffer，没有霸体的怪物会停止移动攻击等行为
    /// </summary>
    /// <param name="value">冰冻时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase DeBufferFreeze(float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        GameControl.gameControl.audioManager.PlayEffectSound(AUDIOCATALOG.guaiwu, AUDIO_NAME.monster_freeze);
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.DeBuffer.DeBufferFreeze, args);
    }

    /// <summary>
    /// 怪物破甲Buffer，一段时间直接减少怪物护甲
    /// </summary>
    /// <param name="value">减少护甲值.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">显示的UI.</param>
    /// <param name="addMore">If set to <c>true</c> 能否重复增加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久.</param>
    public BufferStateBase DeBufferSunderArmor(float value, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_fValue1 = value;
        args.m_ContinuousTime = continueTime;
        args.m_iBufferUI = bufferUI;
        args.m_Addition = addMore;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.DeBuffer.DeBufferSunderArmor, args);
    }

    /// <summary>
    /// 怪物的易伤buff，一段时间内收到的伤害成比例上升
    /// </summary>
    /// <param name="value">伤害的翻倍数</param>
    /// <param name="continueTime">持续时间</param>
    /// <param name="bufferUI">显示的UI</param>
    /// <param name="addMore">是否能重复增加</param>
    /// <param name="permanent">是否永久</param>
    public BufferStateBase DeBuffAptToAtttack(float value, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_fValue1 = value;
        args.m_ContinuousTime = continueTime;
        args.m_iBufferUI = bufferUI;
        args.m_Addition = addMore;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.DeBuffer.DeBuffAptToAtttack, args);
    }
    /// <summary>
    /// 中毒Buffer，每个一段时间血量减少一定值
    /// </summary>
    /// <param name="value">血量减少的值.</param>
    /// <param name="interval">减血间隔.</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase PerDeBufferPoison(int value, float interval, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_iValue1 = value;
        args.m_fValue1 = interval;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.PerDeBuffer.PerDeBufferPoison, args);
    }

    /// <summary>
    /// 灼烧Buffer，每个一段时间血量减少一定值
    /// </summary>
    /// <param name="value">血量减少的值.</param>
    /// <param name="interval">减血间隔.</param>
    /// <param name="continueTime">持续时间.</param>	
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久.</param>
    public BufferStateBase PerDeBufferIgnition(int value, float interval, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_iValue1 = value;
        args.m_fValue1 = interval;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.PerDeBuffer.PerDeBufferIgnition, args);
    }

    /// <summary>
    /// 流血Buffer，每个一段时间血量减少一定值
    /// </summary>
    /// <param name="value">血量减少的值.</param>
    /// <param name="interval">减血间隔.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">UI样式，为0时不显示.</param>
    /// <param name="addMore">If set to <c>true</c> 是否可以叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public BufferStateBase PerDeBufferBleed(int value, float interval, float continueTime, int bufferUI, bool addMore, bool permanent) {
        if (monsterMessage.beiji.GetInvincibleInfo().IsInvincible) {
            return null;
        }
        BufferArgs args = new BufferArgs();
        args.m_iBufferUI = bufferUI;
        args.m_iValue1 = value;
        args.m_fValue1 = interval;
        args.m_Addition = addMore;
        args.m_ContinuousTime = continueTime;
        args.m_Permanent = permanent;
        return m_BufferMgr.BufferAdd(BufferType.PerDeBuffer.PerDeBufferBleed, args);
    }

    /// <summary>
    /// 移除怪物身上所有类型的Buffer
    /// </summary>
    public void RemoveAllTypeBuffe() {
        m_BufferMgr.BufferRemove(BufferType.Buffer.AllType);
        m_BufferMgr.BufferRemove(BufferType.DeBuffer.AllType);
        m_BufferMgr.BufferRemove(BufferType.PerBuffer.AllType);
        m_BufferMgr.BufferRemove(BufferType.PerDeBuffer.AllType);
    }

    /// <summary>
    /// 根据Buffer类型移除某个类型的Buffer
    /// </summary>
    /// <param name="type">Type为0移除Buffer，为1移除DeBuffer，为2移除PerBuffer，为3移除PerDeBuffer.</param>
    public void RemoveBufferByType(int type) {
        if (type == 0) {
            m_BufferMgr.BufferRemove(BufferType.Buffer.AllType);

        } else if (type == 1) {
            m_BufferMgr.BufferRemove(BufferType.DeBuffer.AllType);

        } else if (type == 2) {
            m_BufferMgr.BufferRemove(BufferType.PerBuffer.AllType);
        } else if (type == 3) {
            m_BufferMgr.BufferRemove(BufferType.PerDeBuffer.AllType);
        }
    }
    /// <summary>
    /// 移除某个指定的Buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveOneBuff(BufferStateBase buff) {
        m_BufferMgr.BufferRemoveOne(buff);
    }
    /// <summary>
    /// 更新某个buff的参数
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="args"></param>
    public void RenovateBuff(BufferStateBase buffer, BufferArgs args) {
        m_BufferMgr.RenovateBuff(buffer, args);
    }
    /// <summary>
    /// Buffers the control.
    /// </summary>
    /// <param name="ID">I.</param>
    /// <param name="f1_value">F1 value.</param>
    /// <param name="continueTime">持续时间.</param>
    /// <param name="bufferUI">Buffer的UI显示.</param>
    /// <param name="usePercent">If set to <c>true</c> 是否使用百分比.</param>
    /// <param name="addMore">If set to <c>true</c> 是否能够叠加.</param>
    /// <param name="permanent">If set to <c>true</c> 是否永久添加.</param>
    public void BufferControl(int ID, float f1_value, float continueTime, int bufferUI, bool usePercent, bool addMore, bool permanent) {
        switch (ID) {
            case 0: //加速
                BufferSpeedUp(f1_value, continueTime, bufferUI, addMore, permanent);
                break;
            case 1: //减速
                DeBufferSlowDown(f1_value, continueTime, bufferUI, addMore, permanent);
                break;
            case 2: //加血
                BufferAddBlood(f1_value, bufferUI, usePercent,1f);
                break;
            case 3: //加攻击力
                BufferAddDamage(f1_value, continueTime, bufferUI, addMore, false, permanent);
                break;
            case 4: //中毒
                    //BufferPoison (i1_value, f1_value, continueTime,bufferUI, addMore);
                break;
            case 5: //灼烧
                    //BufferIgnition (i1_value, f1_value, continueTime, bufferUI,addMore);
                break;
            case 6: //眩晕
                DeBufferDizzness(continueTime, bufferUI, false, addMore, permanent);
                break;
            case 7: //流血
                    //BufferBleed (i1_value, f1_value, continueTime, bufferUI,addMore);
                break;
            case 8: //狂暴
                BufferRage(f1_value, f1_value, f1_value, bufferUI, addMore, permanent);
                break;
        }
    }

}