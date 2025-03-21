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
    int howl;
    int chase;
    int attackIdle;
    int bite;
    int taile;
    int charge;
    int bakcMove;
    int die;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        idle = Animator.StringToHash("IsIdle");
        move = Animator.StringToHash("IsMove");
        ro = Animator.StringToHash("IsRo");
        turnAngle = Animator.StringToHash("TurnAngle");
        howl = Animator.StringToHash("IsHowl");
        chase = Animator.StringToHash("IsChase");
        attackIdle = Animator.StringToHash("IsAttackIdle");
        bite = Animator.StringToHash("Bite");
        taile = Animator.StringToHash("TaileAttack");
        charge = Animator.StringToHash("Charge");
        bakcMove = Animator.StringToHash("BackMove");
        die = Animator.StringToHash("IsDie");
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
        anime.SetBool(ro, TorF);
    }

    public void SetRoAngle(float turnAngle)
    {
        anime.SetFloat(this.turnAngle, turnAngle);
    }

    public void PlayMonsterRoarAnime()
    {
        anime.SetTrigger(howl);
    }

    public void PlayMonsterChaseAnime(bool TorF)
    {
        anime.SetBool(chase, TorF);
    }

    public void PlayMonsterAttackIdleAnime(bool TorF)
    {
        anime.SetBool(attackIdle, TorF);
    }

    public void PlayMonsterBiteAnime()
    {
        anime.SetTrigger(bite);
    }
    public void PlayMonsterTaileAttackAnime()
    {
        anime.SetTrigger(taile);
    }

    public void PlayMonsterChargeAnime()
    {
        anime.SetTrigger(charge);
    }

    public void PlayMonsterBackMoveAnime()
    {
        anime.SetTrigger(bakcMove);
    }

    public void PlayMonsterDieAnime(bool TorF)
    {
        anime.SetBool(die, TorF);
    }
}
