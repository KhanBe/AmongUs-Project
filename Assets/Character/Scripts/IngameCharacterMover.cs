using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//자기 타입
public enum EPlayerType
{
    Crew,
    Imposter
}

public class IngameCharacterMover : CharacterMover
{
    [SyncVar]
    public EPlayerType playerType;

    [ClientRpc]//서버에서 클라이언트로 호출하는 어트리뷰트
    public void RpcTeleport(Vector3 position)
    {
        //서버의position값을 변경해 동기화
        transform.position = position;
    }

    //인게임 색상설정 함수
    public void SetNicknameColor(EPlayerType type)
    {
        //내가임포스터이면서 다른플레이어가 임포스터이면
        if (playerType == EPlayerType.Imposter && type == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
    }

    public override void Start()
    {
        // CharacterMover의 Start()함수 호출
        base.Start();

        if (hasAuthority)
        {
            IsMoveable = true;

            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //IngameCharacter객체들이 스스로 GameSystem클래스에 자신을 등록하도록 만든다.
        GameSystem.Instance.AddPlayer(this);
    }

    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }
}
