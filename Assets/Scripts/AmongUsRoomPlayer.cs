using UnityEngine;
using Mirror;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{
    private static AmongUsRoomPlayer myRoomPlayer; 

    //null 값이면 자기 자신플레이어를 반환하는 Property
    public static AmongUsRoomPlayer MyRoomPlayer
    {
        get
        {
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach (var player in players)
                {
                    if (player.hasAuthority)
                        myRoomPlayer = player;
                }
            }
            return myRoomPlayer;
        }
    }

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    //playerColor가 변경될 시 호출 Hook
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(oldColor);//비활성
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(newColor);//활성
    }

    [SyncVar]
    public string nickname;

    public CharacterMover myCharacter;

    public void Start()
    {
        //이미 상속된 부모 클래스 NetworkRoomPlayer에서 Start()함수가 호출되어있기 때문에
        //base.Start()로 Start함수를 호출한다.
        base.Start();

        //서버역할일 경우
        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
            LobbyUIManager.Instance.ActiveStartButton();
        }

        if (isLocalPlayer) CmdSetNickname(PlayerSettings.nickname);

        //중앙 하단 플레이어 수
        LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
    }

    //방에서 나가 오브젝트 파괴 시 색상선택버튼 활성화
    private void OnDestroy()
    {
        if (LobbyUIManager.Instance != null)
        {
            LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(playerColor);

            //중앙 하단 플레이어 수
            LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
        }   
    }

    //Command Attribute (Mirror API 제공 : 클라이언트에서 함수 실행 시 서버에서 함수 동작)
    //클라이언트에서 서버로 신호를 보내는 기능
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        myCharacter.playerColor = color;
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        myCharacter.nickname = nick;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        // as 연산자 : 캐스팅이 안되면 null반환 되면 캐스팅
        //대기실에 접속 중인 플레이어를 가져오는 변수 RoomManager의 roomSlots
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;
        for (int i = 0; i < (int)EPlayerColor.Lime + 1; i++)
        {
            bool isFindSameColor = false;
            foreach (var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                //netId 고유식별 network id
                if (amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }

            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }
        playerColor = color;

        var spawnPositions = FindObjectOfType<SpawnPositions>();
        int index = spawnPositions.Index;

        //SpawnPositions 클래스의 GetSpawnPosition함수의 반환 Vector3 포지션 값을 가져온다
        Vector3 spawnPos = spawnPositions.GetSpawnPosition();
        
        //Room Manager 인스펙터의 Registered Spawnable Prefabs에 추가된 프리펩을
        //인스턴스화 한다 
        var playerChracter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        //6번째 클라부터는 flip해서 소환
        playerChracter.transform.localScale = index < 5 ? new Vector3(0.5f, 0.5f, 1f) : new Vector3(-0.5f, 0.5f, 1f);

        //클라이언트들에게 게임오브젝트가 소환됨을 알림
        //connectionToClient와 connectionToServer은 서버와 클라이언트 중 어느 곳에서 호출하느냐 차이
        //서버 측에서 connection을 확인하려고 하면 conntectionToServer를 사용하고 클라이언트 측에서 connection을 확인할 때는 connectionToClient를 사용하면 된다.
        NetworkServer.Spawn(playerChracter.gameObject, connectionToClient);

        playerChracter.ownerNetId = netId;
        playerChracter.playerColor = color;
    }
}
