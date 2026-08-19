using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollidable collidable = collision.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnCollide();
        }
    }
    // Đề phòng trường hợp đứng im trong block thì dùng tiếp OnTriggerStay2D
    private void OnTriggerStay2D(Collider2D collision)
    {
        ICollidable collidable = collision.GetComponent<ICollidable>();
        if (collidable != null)
        {
            collidable.OnCollide();
        }
    }
}
