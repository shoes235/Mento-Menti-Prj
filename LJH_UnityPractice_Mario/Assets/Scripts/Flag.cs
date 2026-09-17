using UnityEngine;

public class Flag : MonoBehaviour
{
    private bool isCleared = false;
    // 이미 게임을 클리어했는지 저장하는 변수
    // 깃발에 여러 번 닿아서 클리어 처리가 반복되는 것을 막기 위해 사용


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCleared)
        {
            // 깃발에 닿은 오브젝트가 Player이고
            // 아직 게임을 클리어하지 않은 상태라면 실행


            isCleared = true;
            // 게임을 클리어한 상태로 변경


            Debug.Log("Game Clear!");
            // Console 창에 게임 클리어 메시지 출력
        }
    }
}