using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SettingsUI를 상속 받아 이미 쓰였던 기능을 다시 쓸 수 있게 
public class GameRoomSettinsUI : SettingsUI
{
    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = false;
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = true;
    }

    public void ExitGameRoom()
    {
        var manager = AmongUsRoomManager.singleton;

        //매니저 모드가 호스트이면
        if (manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        //클라이언트이면
        else if (manager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }
}
