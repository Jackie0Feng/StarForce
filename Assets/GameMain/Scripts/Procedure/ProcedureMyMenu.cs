using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class ProcedureMyMenu : ProcedureBase
{
    //UI脚本
    private MyMenuForm m_MyMenuForm = null;
    private bool m_GotoGame = false;

    //供UI脚本调用 交流
    public void StartGame()
    {
        m_GotoGame = true; 
    }
    protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        //成功时，获取UI脚本
        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        //打开UI界面
        GameEntry.UI.OpenUIForm(103, this);
        m_GotoGame = false;
    }
    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = e as OpenUIFormSuccessEventArgs;
        if (this != ne.UserData) return;
        //获取UI逻辑脚本
        m_MyMenuForm = (MyMenuForm)ne.UIForm.Logic;
    }

    protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //开始游戏切换流程
        if (m_GotoGame == true)
        {
            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.MyMain"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        //注销事件
        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        //关闭UI
        if (m_MyMenuForm != null)
        {
            m_MyMenuForm.Close(isShutdown);
            m_MyMenuForm = null;
        }
    }

}
