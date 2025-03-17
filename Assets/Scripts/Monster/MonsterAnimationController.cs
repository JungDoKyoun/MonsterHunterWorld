using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    Animator anime;
    int move;
    int ro;
    int turnAngle;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        move = Animator.StringToHash("IsMove");
        ro = Animator.StringToHash("IsRo");
        turnAngle = Animator.StringToHash("TurnAngle");
    }

    public Animator Anime { get { return anime; } set { anime = value; } }

    public void PlayMonsterMoveAnime(bool TorF)
    {
        anime.SetBool(move, TorF);
    }

    public void PlayMonsterRotateAnime(bool TorF)
    {
        anime.SetBool(ro, TorF);
    }

    public void SetRoAngle(float turnAngle)
    {
        anime.SetFloat(this.turnAngle, turnAngle);
    }
}
