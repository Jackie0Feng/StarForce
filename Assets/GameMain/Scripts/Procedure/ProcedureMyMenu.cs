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
    //UI�ű�
    private MyMenuForm m_MyMenuForm = null;
    private bool m_GotoGame = false;

    //��UI�ű����� ����
    public void StartGame()
    {
        m_GotoGame = true; 
    }
    protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        //�ɹ�ʱ����ȡUI�ű�
        GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        //��UI����
        GameEntry.UI.OpenUIForm(103, this);
        m_GotoGame = false;
    }
    private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
    {
        OpenUIFormSuccessEventArgs ne = e as OpenUIFormSuccessEventArgs;
        if (this != ne.UserData) return;
        //��ȡUI�߼��ű�
        m_MyMenuForm = (MyMenuForm)ne.UIForm.Logic;
    }

    protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        //��ʼ��Ϸ�л�����
        if (m_GotoGame == true)
        {
            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.MyMain"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }

    protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);

        //ע���¼�
        GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

        //�ر�UI
        if (m_MyMenuForm != null)
        {
            m_MyMenuForm.Close(isShutdown);
            m_MyMenuForm = null;
        }
    }

}
