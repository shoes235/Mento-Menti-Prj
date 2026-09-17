using UnityEngine;

public class Brick : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        // 벽돌과 충돌한 오브젝트가 Player가 아니면
        // 아래 코드를 실행하지 않고 종료


        ContactPoint2D contact = collision.GetContact(0);
        // 이번 충돌이 일어난 지점의 정보를 가져옴


        if (contact.normal.y > 0.5f)
        {
            Destroy(gameObject);
        }
        // 충돌 방향이 벽돌의 아래쪽이라면 벽돌을 삭제
        // 플레이어가 아래에서 점프해서 머리로 친 경우에 해당
        // 옆이나 위에서 닿았을 때는 부서지지 않음
    }
}