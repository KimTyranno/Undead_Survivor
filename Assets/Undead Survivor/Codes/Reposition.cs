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

    switch (transform.tag)
    {
      case "Ground":
        // 플레이어 위치 - 타일맵위치를 계산하여 거리를 구함
        float diffX = playerPos.x - myPos.x;
        float diffY = playerPos.y - myPos.y;

        // 플레이어 방향
        float dirX = diffX < 0 ? -1 : 1;
        float dirY = diffY < 0 ? -1 : 1;
        diffX = Mathf.Abs(diffX);
        diffY = Mathf.Abs(diffY);

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
          Vector3 dist = playerPos - myPos;

          // 살짝 랜덤으로 배치되게함
          Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
          transform.Translate(ran + dist * 2);
        }
        break;
    }
  }
}
