using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    internal enum CollectableTag
    {
        None = default, coin, TreasureChestBag
    }

    [SerializeField] CollectableTag collectableTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (collectableTag)
            {
                case CollectableTag.coin:
                    GameManager.CollectedCoinsEvent?.Invoke();
                    break;

                case CollectableTag.TreasureChestBag:
                    GameManager.GameEndEvent?.Invoke();
                    break;

            }

            Destroy(this.gameObject, 0.1f);
        }

    }
}
