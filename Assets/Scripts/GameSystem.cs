using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Mirror;

//�÷��̾�� ��ü�� ã�� GameSystem�� ������ �����ؾ���
public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    private List<IngameCharacterMover> players = new List<IngameCharacterMover>();

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private float spawnDistance;

    [SyncVar]
    public float killCooldown;

    [SyncVar]
    public int killRange;


    [SerializeField]
    private Light2D shadowLight;

    [SerializeField]
    private Light2D lightMapLight;

    [SerializeField]
    private Light2D globalLight;

    //��ü �÷��̾� ����Ʈ �������� �Լ�
    public List<IngameCharacterMover> GetPlayerList() { return players; }

    public void AddPlayer(IngameCharacterMover player)
    {
        //player(�ڽ�) ������ ��
        if (!players.Contains(player)) players.Add(player);
    }

    
    private IEnumerator GameReady()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;

        //�� �ʱ�ȭ
        killCooldown = manager.gameRuleData.killCooldown;
        killRange = (int)(manager.gameRuleData.killRange);

        //players�� IngameCharacterMover��ü���� �� ������ ��ٸ�
        while (manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }

        //�������� �̱�
        for (int i = 0; i < manager.imposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];

            if (player.playerType != EPlayerType.Imposter)
                player.playerType = EPlayerType.Imposter;
            else
                i--;
        }

        //ĳ���� ��ġ
        for (int i = 0; i <players.Count; i++)
        {
            float radian = (2f * Mathf.PI) / players.Count;
            radian *= i;

            //transform.position���� �����ϸ� ����ȭ�� �ȵ�
            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * spawnDistance));
        }

        yield return new WaitForSeconds(2f);
        RpcStartGame();

        foreach (var player in players)
        {
            player.SetKillCooldown();
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    //ShowIntroSequence�Լ��� ȣ��Ʈ�� Ŭ���̾�Ʈ���� ��� ����Ǿ���Ѵ�
    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(IngameUIManager.Instance.IngameIntroUI.ShowIntroSequence());

        IngameCharacterMover myCharacter = null;
        foreach (var player in players)
        {
            if (player.hasAuthority)
            {
                myCharacter = player;
                break;
            }
        }

        foreach (var player in players)
        {
            player.SetNicknameColor(myCharacter.playerType);
        }

        yield return new WaitForSeconds(3f);
        IngameUIManager.Instance.IngameIntroUI.Close();
    }

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //ȣ��Ʈ�϶��� �����ؾ��Ѵ�
        if (isServer) StartCoroutine(GameReady());
    }

    //ĳ����Ÿ�Կ� ���� ���� ��� ���� �Լ�
    public void ChangeLightMode(EPlayerType type)
    {
        if (type == EPlayerType.Ghost)
        {
            lightMapLight.lightType = Light2D.LightType.Global;
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else
        {
            lightMapLight.lightType = Light2D.LightType.Point;
            shadowLight.intensity = 0.5f;
            globalLight.intensity = 0.5f;
        }
    }
}
