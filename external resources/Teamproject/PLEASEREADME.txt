// Rigidbody
 - Drag = 10 바람의저항 
 - Use Gravity = v
 - Is Kinematic =  체크하게 되면 어떤 물체가 날아와서 부딪히던
플레이어는 영향을 받지 않음 (코드의 통제는 받을 수 있음) (일단 틱 제거함)
 - Interpolate  = 일단은 none (좀 더 공부가 필요,,)
 - Collision Detection = Discrete 플레이어가 위에서 떨어질 때 땅에
부딪혀서 떨어지는데 오브젝트의 너무 작거나 떨어지는 속도가 너무 빠르면 땅을 뚫거나
충돌체크 전에 속도가 너무 빨라 지나가버리는 경우를 방지하기 위해 다른옵션을 선택해야할 듯
(세밀하게 충돌체크를 할 때 사용)

 - Freeze Rotation = X, Y, Z = v 연산을 최적화 하고 체크가 된
곳의 연산을 하지 않음 게임에선 캐릭터가 좌 우로 둘러보는 회전만 존재하면 되므로
Y 축도 코드로 통제할 것이기 때문에 체크 


// 대시 기본 버튼값 추가

//- Edit > Project Setting >Input Manager > Size 기본값 18에서 19로 변경 하면
새로 생김 > 새로생긴 cancel 이름을 Dash 로 바꿔주고 > 
Positive Button 을 left shift 로 변경 ( 꼭 소문자로 하세요)

// 대시의 부드러움 표현
//- rigidbody의 drag 값을 10을 주고 스크립트 조정


// - Edit > project setting > Input manager > Horizontal, Vertical
// - 스크립트에 axisRaw넣는 대신에 dead 값을 0.1 로 넣어주니 아주 부드럽네요 
// 값을 키우면 키를 눌렀다 뗐을 때 빨리 멈춤 (강사님이 알려주심)