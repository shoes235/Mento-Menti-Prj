using UnityEngine;

public class GoombaAI : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    // 굼바의 이동 속도를 설정하는 변수


    [Header("Wall Check")]
    [SerializeField] private float wallCheckDistance = 0.2f;
    // 벽을 확인하기 위해 Ray를 얼마나 길게 발사할지 설정하는 변수

    [SerializeField] private LayerMask wallLayer;
    // 어떤 레이어를 벽으로 판단할지 설정하는 변수


    private Rigidbody2D rb;
    // 굼바의 Rigidbody2D를 저장할 변수


    private Collider2D col;
    // 굼바의 Collider2D를 저장할 변수
    // 별도의 WallCheck 오브젝트 없이
    // Collider의 왼쪽 또는 오른쪽 끝 위치에서 Ray를 발사하기 위해 사용


    private int moveDirection = -1;
    // 굼바가 현재 이동하고 있는 방향
    // -1 = 왼쪽
    //  1 = 오른쪽


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 굼바 오브젝트에 붙어있는 Rigidbody2D를 가져와 저장
        // 이동 속도를 변경할 때 사용


        col = GetComponent<Collider2D>();
        // 굼바 오브젝트에 붙어있는 Collider2D를 가져와 저장
        // Collider의 왼쪽 또는 오른쪽 끝 위치를 알아낼 때 사용
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            moveDirection * moveSpeed,
            rb.linearVelocity.y
        );

        // Rigidbody2D의 현재 속도를 설정
        // X축은 현재 이동 방향 * 이동 속도로 설정
        // Y축은 기존 속도를 유지해서 중력이 정상적으로 작동하게 함

        // moveDirection이 -1이면
        // -1 * moveSpeed가 되어 왼쪽으로 이동

        // moveDirection이 1이면
        // 1 * moveSpeed가 되어 오른쪽으로 이동


        Vector2 rayStart = new Vector2(
            moveDirection == 1 ? col.bounds.max.x : col.bounds.min.x,
            col.bounds.center.y
        );

        // 벽 감지용 Ray를 시작할 위치를 계산

        // moveDirection == 1이면 오른쪽으로 이동 중이므로
        // col.bounds.max.x를 사용해서 Collider의 가장 오른쪽 위치를 가져옴

        // moveDirection == -1이면 왼쪽으로 이동 중이므로
        // col.bounds.min.x를 사용해서 Collider의 가장 왼쪽 위치를 가져옴

        // Y 위치는 col.bounds.center.y를 사용해서
        // Collider의 세로 중앙 위치에서 Ray를 발사


        RaycastHit2D wallHit = Physics2D.Raycast(
            rayStart,
            new Vector2(moveDirection, 0),
            wallCheckDistance,
            wallLayer
        );

        // 굼바가 이동하고 있는 방향으로 Ray를 발사

        // rayStart
        // Ray가 시작될 위치

        // new Vector2(moveDirection, 0)
        // Ray를 발사할 방향

        // moveDirection이 -1이면 (-1, 0)이므로 왼쪽
        // moveDirection이 1이면 (1, 0)이므로 오른쪽

        // wallCheckDistance
        // Ray가 얼마나 멀리 나갈지 설정

        // wallLayer
        // 어떤 레이어와 충돌할지를 설정


        if (wallHit.collider != null)
        {
            moveDirection *= -1;

            // Ray가 벽과 충돌했다면 이동 방향을 반대로 변경
        }
    }


    private void OnDrawGizmosSelected()
    {
        Collider2D goombaCollider = GetComponent<Collider2D>();
        // Scene 창에서도 Collider 위치를 알아낼 수 있도록 가져옴


        if (goombaCollider == null)
            return;

        // Collider가 없다면 Ray의 시작점을 계산할 수 없으므로 함수 종료


        Vector2 rayStart = new Vector2(
            moveDirection == 1 ? goombaCollider.bounds.max.x : goombaCollider.bounds.min.x,
            goombaCollider.bounds.center.y
        );

        // 현재 이동 방향에 따라서
        // Collider의 왼쪽 끝 또는 오른쪽 끝을 Ray 시작 위치로 설정


        Gizmos.DrawRay(
            rayStart,
            new Vector2(moveDirection, 0) * wallCheckDistance
        );

        // Scene 창에서 실제 벽 감지용 Ray를 확인할 수 있도록 그림
        // 실제 벽 충돌 판정에는 영향을 주지 않는 확인용 기능
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 오브젝트가 Player인지 확인


            Collider2D playerCollider = collision.gameObject.GetComponent<Collider2D>();
            // 플레이어의 Collider2D를 가져옴
            // 플레이어가 굼바의 위쪽에 있는지 확인하기 위해 사용


            bool isPlayerAbove = playerCollider.bounds.min.y >= col.bounds.center.y;

            // playerCollider.bounds.min.y
            // 플레이어 Collider의 가장 아래쪽 위치

            // col.bounds.center.y
            // 굼바 Collider의 중앙 위치

            // 플레이어의 발 위치가 굼바 중앙보다 위에 있다면
            // 플레이어가 굼바 위쪽에서 충돌했다고 판단


            if (isPlayerAbove)
            {
                Destroy(gameObject);

                // 플레이어가 굼바 위에서 닿았다면
                // 이 스크립트가 붙어있는 굼바 자신을 제거
            }
            else
            {
                Destroy(collision.gameObject);

                // 위쪽이 아닌 옆이나 아래쪽에서 충돌했다면
                // 충돌한 Player 오브젝트를 제거
            }
        }
    }
}