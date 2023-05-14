using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
  Collider2D coll;

  void Awake()
  {
    coll = GetComponent<Collider2D>();
  }
  // trigger가 체크된 콜라이더에서 나갔을때 체크되는함수
  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.CompareTag("Area")) return;

    Vector3 playerPos = GameManager.instance.player.transform.position;
    Vector3 myPos = transform.position;
    // 플레이어 위치 - 타일맵위치를 계산하여 거리를 구함
    float diffX = Mathf.Abs(playerPos.x - myPos.x);
    float diffY = Mathf.Abs(playerPos.y - myPos.y);

    // 플레이어 방향
    Vector3 playerDir = GameManager.instance.player.inputVec;
    float dirX = playerDir.x < 0 ? -1 : 1;
    float dirY = playerDir.y < 0 ? -1 : 1;

    switch (transform.tag)
    {
      case "Ground":
        if (diffX > diffY)
        {
          transform.Translate(Vector3.right * dirX * 40);
        }
        else if (diffX < diffY)
        {
          transform.Translate(Vector3.up * dirY * 40);
        }
        break;
      case "Enemy":
        // 몬스터가 살아있고, 플레이어와 너무 멀어지면 플레이어가 이동하고 있는쪽 맞은편에서 몬스터가 재배치되도록함
        if (coll.enabled)
        {
          transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
        }
        break;
    }
  }
}
