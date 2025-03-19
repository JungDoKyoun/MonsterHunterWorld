using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    Animator anime;
    int idle;
    int move;
    int ro;
    int turnAngle;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        idle = Animator.StringToHash("IsIdle");
        move = Animator.StringToHash("IsMove");
        ro = Animator.StringToHash("IsRo");
        turnAngle = Animator.StringToHash("TurnAngle");
    }

    public Animator Anime { get { return anime; } set { anime = value; } }

    public void PlayMonsterIdleAnime(bool TorF)
    {
        anime.SetBool(idle, TorF);
    }

    public void PlayMonsterMoveAnime(bool TorF)
    {
        anime.SetBool(move, TorF);
    }

    public void PlayMonsterRotateAnime(bool TorF)
    {
        Debug.Log(TorF);
        anime.SetBool(ro, TorF);
    }

    public void SetRoAngle(float turnAngle)
    {
        Debug.Log("µé¾î¿Â ¾Þ±Û" + turnAngle);
        anime.SetFloat(this.turnAngle, turnAngle);
    }
}
