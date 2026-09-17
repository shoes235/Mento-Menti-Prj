using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 동전에 닿은 오브젝트가 Player인지 확인


            PlayerController player = collision.GetComponent<PlayerController>();
            // 충돌한 Player의 PlayerController를 가져옴


            if (player != null)
            {
                player.AddCoin();
                // 플레이어의 동전 개수를 1 증가시킴
            }


            Destroy(gameObject);
            // 동전을 먹었으므로 동전 자신을 삭제
        }
    }
}