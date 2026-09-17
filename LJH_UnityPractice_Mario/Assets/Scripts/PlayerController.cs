using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 10f;
    // [SerializeField] : private 변수를 인스펙터에서 표시 및 수정 가능하게 띄워주는 것


    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    // 레이가 아래로 얼마나 길게 나갈지 설정하는 변수
    [SerializeField] private LayerMask groundLayer;
    // 어떤 레이어를 바닥으로 판단할지 설정하는 변수

    private Rigidbody2D rb;

    // 플레이어의 Collider2D를 저장할 변수
    // GroundCheck 오브젝트 대신 플레이어 Collider의 아래쪽 위치를 사용하기 위해 필요
    private Collider2D col;

    // 현재 플레이어가 바닥에 있는지 저장하는 변수
    // 바닥이면 true, 공중이면 false
    private bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 플레이어의 Rigidbody2D를 가져오는 구문
        // 계속 호출해서 쓸 수도 있지만 변수로 할당해놓는 것이 편하기에 사용
        // 한 번만 하면 되기 때문에 Awake에서 할당


        col = GetComponent<Collider2D>();
        // 플레이어의 Collider2D를 가져오는 구문
        // Collider의 가장 아래쪽 위치를 구해서
        // 그 위치에서 Ground Check용 Ray를 발사하기 위해 사용
    }


    private void Update()
    {
        // 좌우 입력
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 실제 키 입력을 받도록 하는 구문
        // 오른쪽이 1, 왼쪽이 -1, 입력하지 않으면 0으로 저장됨
        // 마리오 같은 순간 전환이 중요한 게임은 바로바로 값이 바뀌어야 하기에 GetAxisRaw 사용

        // 구버전 Input은 입력되는 키를 코드에서 직접 확인하는 방식
        // 신버전 Input System은 Move, Jump 같은 행동을 먼저 정의한 뒤 키를 연결하는 방식
        // 대부분의 실제 프로젝트에서는 신버전 Input System을 사용하는 것이 좋음
        // 하지만 현재는 기본적인 이동 구현을 먼저 배우기 위해 구버전 Input 사용
        // 신버전도 나중에 사용할 수 있도록 프로젝트 설정은 Both로 지정


        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );

        // Rigidbody2D의 현재 속도를 새로 설정
        // X축 속도는 입력값 * 이동속도로 설정
        // Y축 속도는 기존 값을 그대로 사용
        // Y값을 그대로 두는 이유는 중력이나 점프 속도에 영향을 주지 않기 위해서


        // 플레이어 Collider의 아래쪽 중앙 위치를 구함
        Vector2 rayStart = new Vector2(
            col.bounds.center.x,
            col.bounds.min.y
        );

        // col.bounds.center.x
        // Collider의 중앙 X 좌표

        // col.bounds.min.y
        // Collider가 차지하고 있는 영역 중 가장 아래쪽 Y 좌표

        // 따라서 두 값을 이용하면 플레이어의 발밑 중앙 위치를 구할 수 있음


        // 플레이어 발밑에서 아래 방향으로 Ray 발사
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,                 // 레이를 발사할 시작 위치
            Vector2.down,             // 레이를 발사할 방향, 아래쪽
            groundCheckDistance,      // 레이를 발사할 거리
            groundLayer               // 충돌을 검사할 레이어
        );

        // Physics2D.Raycast는 지정한 위치에서 지정한 방향으로
        // 보이지 않는 선을 발사해서 충돌한 오브젝트의 정보를 가져오는 기능
        // 기존의 콜라이더 형식보다 연산량이 적고 벽에 붙는 등의 문제가 일어나지 않음


        // Ray가 Ground에 닿았으면 true
        isGrounded = hit.collider != null;

        // hit.collider에는 Ray와 충돌한 Collider2D가 들어감
        // 아무것도 충돌하지 않았다면 null이 들어감
        // 따라서 null이 아니라면 바닥에 닿았다고 판단해서 true 저장


        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpPower
            );

            // 스페이스바를 누르고 바닥에 있을 때만 실행
            // X축 속도는 기존 이동 속도를 그대로 유지
            // Y축 속도만 jumpPower 값으로 변경해서 위로 올라가게 함
            // isGrounded 조건이 있기 때문에 공중에서는 다시 점프할 수 없음
        }
    }

    // 플레이어를 선택했을 때 Scene 창에서 확인 가능하도록하는 함수
    // 실제 Ground 판정에는 영향을 주지 않고 확인용으로만 사용됨
    private void OnDrawGizmosSelected()
    {
        // Scene 창에서도 Collider를 찾을 수 있도록 가져옴
        Collider2D playerCollider = GetComponent<Collider2D>();

        // Collider가 없다면 Ray를 그릴 수 없으므로 종료
        if (playerCollider == null)
            return;


        // Collider 아래쪽 중앙 위치 계산
        Vector2 rayStart = new Vector2(
            playerCollider.bounds.center.x,
            playerCollider.bounds.min.y
        );


        // Scene 창에서 Ground Check용 Ray를 눈으로 볼 수 있게 그려줌
        Gizmos.DrawRay(
            rayStart,
            Vector2.down * groundCheckDistance
        );


    }

    private int coinCount = 0;
    // 현재 플레이어가 먹은 동전 개수를 저장하는 변수

    public void AddCoin()
    {
        coinCount++;
        // 동전 개수를 1 증가시킴


        Debug.Log("동전 : " + coinCount);
        // 현재 동전 개수를 Console 창에 출력
    }
}