using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    Animator anime;
    int move;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        move = Animator.StringToHash("IsMove");
    }

    public void PlayMonsterMoveAnime(bool TorF)
    {
        anime.SetBool(move, TorF);
    }
}
