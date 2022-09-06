# AmongUs-Project
 make Among-Us

<details>
  <summary>일지</summary>
    
### 2022-08-26
1. UI setting
- 해상도 설정 : Canvas Scaler > Scale With Screen Size 설정후 해상도에 맞게 설정
 ---

### 2022-08-29
1. Quit, Online button 구현

### 2022-08-31
 
**MainMenu 화면에 별, 캐릭터가 자유롭게 움직이도록 구현**
    
1. Particle 시스템 구현
- Prewarm : 게임이 시작되는 순간에 처음 생성되는 위치부터 파티클을 생성하지 않고 미리 계산하여 파티클을 자연스럽게 배치해주는 역할이다.
- Emission > Rate over Time : 파티클의 개수 수정 가능.
    
2. Shader 그래프 구현
- 셰이더 그래프에서 _MainTex라는 이름 사용시 Renderer 계열 컴포넌트에서 사용하는 텍스처들을 자동으로 가져와준다.
- 오류 : 이름을 _MainTex라고 해도 오류가 났는데, 셰이더그래프에서 이름이랑 Reference도 똑같이 바꿔줘야 한다.
 
### 2022-09-01
1. Online UI 구현
- 각 오브젝트들을 정렬할 때 Horizontal Layout Group, Content Size Fitter 컴포넌트를 추가해 구성해주면 정렬이 깔끔히 된다.
- 하다가 응답없어서 껏다가 키니까 Hierarchy창 다 초기화됨, 저장 프로젝트 파일이 없음...   
 
### 2022-09-05
1. Online UI 구현
- 닉네임이 비어있을 때 (흔들림) 애니메이션 구현
    
 2. Create Room UI 구현
- Interactable : 버튼 기능의 활성화/비활성화 기능을 가지며, SetActive 처럼 쓰인다.
- GetComponentInChildren : 자식 오브젝트의 컴포넌트를 가져온다.
    
 3. Online & Create Room UI 연결
    
### 2022-09-06
 
**Mirror & NetWork Settings**
1. Network Room Manager : ( Offline Scene > Game Room Scene > Gameplay Scene ) 처럼 3단 구조로 씬을 관리하면서 네트워크 통신에 도움을 주는 클래스이다.
- Offline : 게임 네트워크에 접속하지 않은 씬
- Room Player Prefab : 게임 대기실에 입장한 플레이어의 오브젝트
![Scene 구조](https://user-images.githubusercontent.com/61501112/188642700-2486399a-f4ae-4536-aaf0-99e9c6a58ea3.JPG)
- Room Manager가 Room Player 프리팹을 인스턴스화 해서 플레이어에게 할당해주고 이 오브젝트를 통해 통신하게 되는 구조이다.
![Scene 구조1](https://user-images.githubusercontent.com/61501112/188642719-13678821-1355-48e0-a7e2-e12c472e8f3c.JPG)
- Room Player Prefab : 게임 시작 전 게임대기실에서 플레이어가 서버와 상호작용을하기 위함.
- Player Prefab : 게임 시작 후 Gameplay Scene에서 서버와 상호작용을 한다.
  </details>
    

