# AmongUs-Project

![AmongUs](https://user-images.githubusercontent.com/61501112/193694456-c481c254-5c18-45c3-b1b9-a1e92b14f813.jpg)

## 프로젝트 목표
- AmongUs 2D게임 구현

## 사용 기술
- 유니티
- C#

## Sprite
- spriters-resource.com

## 일지

<details>
 <summary>2022-08-26</summary>
 
**1. UI setting**
- 해상도 설정 : Canvas Scaler > Scale With Screen Size 설정후 해상도에 맞게 설정
 
 ---
 
</details>
 
<details>
  <summary>2022-08-29</summary>
 
  **1. Quit, Online button 구현**
 
 ---
 
</details>
 
<details>
 <summary>2022-08-31</summary>
 
 **MainMenu 화면에 별, 캐릭터가 자유롭게 움직이도록 구현**
    
 **1. Particle 시스템 구현**
 - Prewarm : 게임이 시작되는 순간에 처음 생성되는 위치부터 파티클을 생성하지 않고 미리 계산하여 파티클을 자연스럽게 배치해주는 역할이다.
 - Emission > Rate over Time : 파티클의 개수 수정 가능.
    
 **2. Shader 그래프 구현**
 - 셰이더 그래프에서 _MainTex라는 이름 사용시 Renderer 계열 컴포넌트에서 사용하는 텍스처들을 자동으로 가져와준다.
 - 오류 : 이름을 _MainTex라고 해도 오류가 났는데, 셰이더그래프에서 이름이랑 Reference도 똑같이 바꿔줘야 한다.
 
 ---
 
</details>
 
<details>
 <summary>2022-09-01</summary>
 
 **1. Online UI 구현**
 - 각 오브젝트들을 정렬할 때 Horizontal Layout Group, Content Size Fitter 컴포넌트를 추가해 구성해주면 정렬이 깔끔히 된다.
 - 하다가 응답없어서 껏다가 키니까 Hierarchy창 다 초기화됨, 저장 프로젝트 파일이 없음...  
 
 ---
 
</details>
 
<details>
 <summary>2022-09-05</summary>
 
 **1. Online UI 구현**
 - 닉네임이 비어있을 때 (흔들림) 애니메이션 구현
    
 **2. Create Room UI 구현**
 - Interactable : 버튼 기능의 활성화/비활성화 기능을 가지며, SetActive 처럼 쓰인다.
 - GetComponentInChildren : 자식 오브젝트의 컴포넌트를 가져온다.
    
 **3. Online & Create Room UI 연결**
 
 ---
 
</details>
 
<details>
 <summary>2022-09-06</summary>
 
 **Mirror & NetWork Settings**
 **1. Network Room Manager** : ( Offline Scene > Game Room Scene > Gameplay Scene ) 처럼 3단 구조로 씬을 관리하면서 네트워크 통신에 도움을 주는 클래스이다.
 - Offline : 게임 네트워크에 접속하지 않은 씬
 - Room Player Prefab : 게임 대기실에 입장한 플레이어의 오브젝트
 ![Scene 구조](https://user-images.githubusercontent.com/61501112/188642700-2486399a-f4ae-4536-aaf0-99e9c6a58ea3.JPG)
 - Room Manager가 Room Player 프리팹을 인스턴스화 해서 플레이어에게 할당해주고 이 오브젝트를 통해 통신하게 되는 구조이다.
 ![Scene 구조1](https://user-images.githubusercontent.com/61501112/188642719-13678821-1355-48e0-a7e2-e12c472e8f3c.JPG)
 - Room Player Prefab : 게임 시작 전 게임대기실에서 플레이어가 서버와 상호작용을하기 위함.
 - Player Prefab : 게임 시작 후 Gameplay Scene에서 서버와 상호작용을 한다.
 
 ---
 
</details>
 
<details>
 <summary>2022-09-13</summary>
 
 1. 다른 플레이어가 접속하는 기능 구현
 2. 게임 룸 구현
 - 썼던 애니메이션을 재활용 하려면 게임 오브젝트 이름이 같아야 한다.
 - 호스트가 방을 나갔을 때 호스트를 다른 클라이언트에 넘겨줘야하는 기능 (호스트 마이그레이션)을 구현해야한다.
 
 ---
 
</details>
 
<details>
 <summary>2022-09-14</summary>
 
 **1. Hierarchy창 다 초기화되는 이유를 찾음. Project만 저장하는게 아니고 Scene도 따로 저장해야 된다.**
 
 **2. Editor에서 재생시킨 object rotation보다 빌드 된 게임에서 object rotation이 더 빠르게 도는 버그를 해결함.**
 - Update() method 내에서 회전력을 주는게 아니고 FixedUpdate()에서 회전력을 줘야 한다.
 - 이유 : Update()는 매 프레임마다 호출하게 되고, FIxedUpdate()는 Fixed Timestep에 설정된 값에 따라 일정한 간격으로 호출하게 된다.   
 그래서 각 CPU에 따라 초당 처리되는 프레임이 많거나 적을 수 있기 때문이다.
 - https://forum.unity.com/threads/rotation-speed-different-in-standalone-build.883579/
 
 ---
 
</details>
 
<details>
 <summary>2022-09-15</summary>
 
 **1. 게임 대기실 캐릭터 조작 및 동기화**
 - 캐릭터에 network transform component 추가 : transform 동기화 기능 컴포넌트
 - Client Authority 체크 : 각 클라이언트에게 동기화 권한 부여
 - Sync Interval : 동기화 반응속도 (값이 작아지면 초당 동기화 회수가 많아져 반응속도는 빨라지지만 그만큼 데이터 소모량이 커진다)
 
 ```
 //Vector3.ClampMagnitude(Vector3 vector, float maxLength);
 //만약 Vector3 값이 (30f, 10f, 3f)이고 maxLength값이 5이면 ClampMagnitude로 인해
 //Vector3 값은 (5f, 5f, 3f)로 바뀌게 된다.
 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
 ```
 
 ---
 
 </details>
 
 <details>
 <summary>2022-09-16</summary>
 
 **1. 캐릭터 애니메이션**
 - 캐릭터에 network animator component :  animtor 프로퍼티에 Animator 컴포넌트 할당
 - network animator component : 네트워크를 통해 animation 동기화 역할 component
 - 캐릭터 Flip이 안됐던 이유 : Sync Scale 체크해야 됨, LocalScale로 Flip을 했기 때문에
 
 **2. 캐릭터 스폰**
 ```
 //인스턴스화
 Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
 ```
 
 **3. 다른 클래스 함수 가져오는 방법**
 ```
 //SpawnPositions 클래스의 GetSpawnPosition함수의 반환 Vector3 포지션 값을 가져온다
 Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
 ```
 
 **4. 애니메이션 add Event 오류**
 **animation event has no function name specified**   
 - 애니메이션에 추가된 event가 설정되어 있지 않아서 생가는 오류 메세지
 
 **5. 캐릭터 충돌**
 - 각 오브젝트마다 Collider추가
 - 캐릭터끼리 충돌 안하게 : Player Layer 추가 > Project Settings 에서 Physics 2D Layer Collision Matrix 에서 플레이어끼리 체크 해제 후 원하는 오브젝트를 Player Layer로 설정
 
 **6. 오브젝트 정렬**
 - Lerp 에 대해 공부하기 (선형보간)
 - Sorting Order에 대해 공부하기
 
 ---
 
 </details>
 
 <details>
 <summary>2022-09-18</summary>
 
 **1. 선형보간(Lerp)을 통한 SortingOrder(Order in Layer)를 완벽히 이해함**
 - SpriteSorter, SortingSprite Scripts에 주석
 ---
 
</details>
 
 <details>
 <summary>2022-09-20</summary>
 
 **1. 캐릭터 입장시 자동 색상 구현**
 - Command Attribute : Mirror API 제공, 클라이언트에서 함수 실행 시 서버에서 함수 동작, **함수이름 앞 Cmd를 접두사로 써야한다.**
 - SyncVar hook 기능 :
 - singleton :
 - 색상 선택 기능 구현 다시학습
 
 **2. 캐릭터 색상 선택 UI 구현**
 - get/set Property : **정보은닉** 목적으로 private 변수를 외부에서 불러오기 위해 쓰인다.
 - 예시 :
 ```
 class Person
 {
  private string age; // field
  public string Age   // property
   {
    get { return age; }
    set { age = value; }
   }
 }
```

 ---
 
</details>

 <details>
 <summary>2022-09-21</summary>
 
 **1. 거리에 따른 상호작용 가능한 오브젝트 셰이더 구현**
 
 **2. 캐릭터 색상 선택 Customize UI 구현**
 
 ps. Scene만 잘 저장하고 불러오면 문제 안된다.   
 
 ---
 
</details>

 <details>
 <summary>2022-09-22</summary>
 
 **1. 색상 선택 UI에서 클라이언트가 접속시 업데이트가 안되는 버그 잡기**
 
 **2. 스폰 위지 Flip 버그 잡기**
 
 ---
 
</details>

 <details>
 <summary>2022-09-24</summary>
 
 **1. 게임 규칙 UI 배치**
 - 스크롤뷰에서 목록이 많이 있으면 Viewport의 Mask 컴포넌트를 꺼서 작업하면 편리함.
 
 ---
 
</details>

<details>
 <summary>2022-09-25</summary>
 
 **1. 중앙 하단 플레이어 인원 Text 구현**
 
 **2. 플레이어 닉네임 표시 구현**
 - 각 캐릭터마다 캔버스를 가지도록 구현
 - OnlineUI의 ```PlayerSettings.nickname = nicknameInputField.text;``` > AmongUsRoomPlayer의 ```CmdSetNickname(string nick)``` > CharacterMover의 nickname으로 전달
 
 **3. 게임 시작 버튼 구현**
 
 ---
 
 </details>
 
 <details>
 <summary>2022-09-26</summary>
 
 **1. 게임 맵 구현**
 - 스프라이트 시퀀스 애니메이션 : 스프라이트 이미지를 교체하는 방식의 애니메이션
 - Weapon, O2, Navigation 맵, 애니메이션 구현
 
 ---
 
</details>

 <details>
 <summary>2022-09-27</summary>
 
 **1. 게임 맵 전체 구현**
 - 스프라이트 시퀀스 애니메이션 : 스프라이트 이미지를 교체하는 방식의 애니메이션
 - Cafeteria, Admin, Medbay, Upper Engine, Lower Engine, Security, Reactor, Electrical , Storage, Communications, Shields, Rocket Engine 맵, 애니메이션 구현
 
 ---
 
</details>

 <details>
 <summary>2022-09-28</summary>
 
 **1. 인게임 플레이어 세팅**
 - Scale : Lobby > 0.5, Ingame > 0.36
 ---
 
</details>
 
<details>
 <summary>2022-09-29</summary>
 
 **1. 플레이어 타입 설정 크루원/임포스터**
 - 인트로에서 텍스트랑 이미지 하나가 안나오는 오류, [SerializeField] 로 Color값을 줬는데 알파가 0으로 Default 였음 개고생
 ---
 
</details>

<details>
 <summary>2022-10-01</summary>
 
 **1. 인트로 닫기**
 
 **2. 플레이어 스폰 위치 설정**
 - 선형보간과 cos, sin으로 배치
 
 **3. 맵 전체 Collider 설정**
 
 **4. 임포스터일 시 서로 플레이어 NicknameText RED로 구현**
 
 ---
 
</details>

<details>
 <summary>2022-10-03</summary>
 
 **1. 인게임 그림자 구현**
 - Composite Shadow Caster 2D Component : 해당 오브젝트의 자식 오브젝트이 가진 Shadow Caster들을 통합해주는 기능 컴포넌트 
 
 ---
 
</details>

<details>
 <summary>2022-10-04</summary>
 
 **1. 임무 UI 구현**

 **2. 인게임 Setting UI 구현**
 
 **3. 인게임 미니맵/현위치 구현**
 - 미니맵을 normalized(정규화)하여 구현
 
 **4. 킬 버튼 구현**
 - 킬의 쿨타임 표시
 - Circle Collider 2D컴포넌트로 킬 범위 설정
 - 킬버튼 클릭 시 시체 표현
 - 시체 애니메이션
 
 ---
 
</details>

<details>
 <summary>2022-10-05</summary>
 
 **1. KILL UI/Animation 구현**
 
 **2. 죽은 크루원 유령으로 만들기**
 - 최신버전 shader alpha값이 따로 빠져나와있어 오류 때문에 color.a 값 변경시 오류로 spriteRenderer.color로 변경
 
 ```
    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            var color = PlayerColor.GetColor(playerColor);
            
            //color.a = 0f;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            spriteRenderer.material.SetColor("_PlayerColor", color);
            nicknameText.text = nickname;
        }
        else
        {
            var color = PlayerColor.GetColor(playerColor);

            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            spriteRenderer.material.SetColor("_PlayerColor", color);
            nicknameText.text = "";
        }
    }
 ```
 
 ---
 
</details>

<details>
 <summary>2022-10-07</summary>
 
 **1. Light**
 - Shadow Light : 빛이 닿는 영역은 밝게, 아닌 부분은 어둡게
 - Global Light : 어두운 부분을 보이게 해주는 라이트
 - Light Map Light : 빛이 닿는 영역에 있는 캐릭터만 보이게하는 라이트
 
 **2. Ghost Type Light/Collider 구현**
 
 **3. Report UI / Button 구현**
 
 ---
 
</details>

<details>
 <summary>2022-10-08</summary>
 
 **1. Meeing UI 구현**

 ---
 
</details>

<details>
 <summary>2022-10-09</summary>
 
 **1. Vote 기능 구현**

 ---
 
</details>

<details>
 <summary>2022-10-10</summary>
 
 **1. 투표 기능 구현**
 - 투표 스킵 기능 구현
 - 투표 시간제한 구현
 - Mathf.Clamp(value, min, max) : value가 범위보다 작으면 min반환, max보다 크면 max반환
 ex)
 ```
 if (meetingState == EMeetingState.Meeting)
 {
     meetingTimeText.text = string.Format("회의시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
 }
 ```
 
 - 투표 스킵되면 이미 활성화된 Select버튼과 Cancel버튼중 Select버튼을 누르면 투표가 되는 버그 있음 **(해결)**
 ```
 //투표 끝난 후
    public void CompleteVote()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.OpenResult();
            panel.Unselect();//추가  : (Select버튼, Cancel버튼)을 비활성화 시키는 함수
        }

        skipVoteButton.SetActive(false);
        skipVotePlayers.SetActive(true);
    }
 ```
 
 - skip 버튼누르면 모든 플레이어의 skip버튼이 비활성화되는 버그 있음 **(해결)** , 그냥 비활성화 풀었음 
 ---
 
 </details>
 
 <details>
 <summary>2022-10-11</summary>
 
 **1. Ejection UI 구현**
 
 
 ---
 
 </details>
 
 <details>
 <summary>2022-10-12</summary>
 
 **1. Task전선 미니게임 구현**
 - 길이가 마우스에 맞춰 길어지는게 아니라, 마우스position보다 길어지는 현상이 있음
 
 ---
 
 </details>
 
 <details>
 <summary>2022-10-13</summary>
 
 **1. Task전선 미니게임 구현**
 - 길이가 마우스에 맞춰 길어지는게 아니라, 마우스position보다 길어지는 현상이 있음 (해결)
 - TaskUI의 size 값을 1.5에서 1로 맞춤
 - 전선 이어 붙히기
 
 ---
 
 </details>

 <details>
 <summary>2022-10-14</summary>
 
 **1. Task전선 미니게임 구현**
 - 전선 색상 랜덤 지정
 - 전선 연결 색상일치 시 불빛 구현
 - 전선 클릭이 잘 안되는 문제 > FixedUpdate에서 Update로 변경
 
 ---
 
 </details>

