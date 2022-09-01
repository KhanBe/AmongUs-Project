# AmongUs-Project
 make Among-Us


<details>
  <summary>일지</summary>
    
    2022-08-26
    1. UI setting
    
    2022-08-29
    1. Quit, Online button 구현
    
    2022-08-31
    **MainMenu 화면에 별, 캐릭터가 자유롭게 움직이도록 구현**
    
    1. Particle 시스템 구현
    - Prewarm : 게임이 시작되는 순간에 처음 생성되는 위치부터 파티클을 생성하지 않고 미리 계산하여 파티클을 자연스럽게 배치해주는 역할이다.
    - Emission > Rate over Time : 파티클의 개수 수정 가능.
    
    2. Shader 그래프 구현
    - 셰이더 그래프에서 _MainTex라는 이름 사용시 Renderer 계열 컴포넌트에서 사용하는 텍스처들을 자동으로 가져와준다.
    - 오류 : 이름을 _MainTex라고 해도 오류가 났는데, 셰이더그래프에서 이름이랑 Reference도 똑같이 바꿔줘야 한다.
    
    2022-09-01
    1. Online UI 구현
    - 각 오브젝트들을 정렬할 때 Horizontal Layout Group, Content Size Fitter 컴포넌트를 추가해 구성해주면 정렬이 깔끔히 된다.
    하다가 응답없어서 껏다가 키니까 Hierarchy창 다 초기화됨, 시발
  </details>
