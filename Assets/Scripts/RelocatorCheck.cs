using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocatorCheck : MonoBehaviour
{
    internal enum CheckerTag
    {
        None = default, FallingCheck, AchieveCheck
    }

    [SerializeField] CheckerTag checkingTag;
    [SerializeField] int locateId;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (checkingTag)
            {
                case CheckerTag.FallingCheck:
                    GameManager.PlayerRelocateEvent.Invoke();
                    break;

                case CheckerTag.AchieveCheck:
                    GameManager.AchieveLocationEvent.Invoke(locateId);
                    break;
            }
        }
    }
}
