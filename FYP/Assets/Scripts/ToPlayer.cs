using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayer : MonoBehaviour
{
    [SerializeField] Player player;
    public Player getplayer()
    {
        return player;
    }
}
