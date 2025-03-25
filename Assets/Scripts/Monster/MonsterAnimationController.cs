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
    int bakcMove;
    int die;
    int sturn;
    int sturn2;
    int sturn3;
    int attack1;
    int attack2;
    int attack3;
    int attack4;

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
        bakcMove = Animator.StringToHash("BackMove");
        die = Animator.StringToHash("IsDie");
        sturn = Animator.StringToHash("IsSturn");
        sturn2 = Animator.StringToHash("IsSturn2");
        sturn3 = Animator.StringToHash("IsSturn3");
        attack1 = Animator.StringToHash("attack1");
        attack2 = Animator.StringToHash("attack2");
        attack3 = Animator.StringToHash("attack3");
        attack4 = Animator.StringToHash("attack4");
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

    //public void PlayMonsterBiteAnime()
    //{
    //    anime.SetTrigger(bite);
    //}
    //public void PlayMonsterTaileAttackAnime()
    //{
    //    anime.SetTrigger(taile);
    //}

    //public void PlayMonsterChargeAnime()
    //{
    //    anime.SetTrigger(charge);
    //}

    public void PlayMonsterBackMoveAnime()
    {
        anime.SetTrigger(bakcMove);
    }

    public void PlayMonsterDieAnime(bool TorF)
    {
        anime.SetBool(die, TorF);
    }

    public void PlayMonsterStrunAnime(bool TorF)
    {
        anime.SetBool(sturn, TorF);
    }

    public void PlayMonsterStrun2Anime()
    {
        anime.SetTrigger(sturn2);
    }

    public void PlayMonsterStrun3Anime()
    {
        anime.SetTrigger(sturn3);
    }

    //public void PlayMonsterShootProjectileAnime()
    //{
    //    anime.SetTrigger(shootProjectile);
    //}

    public void PlayAttackAnime(string animeName)
    {
        anime.SetTrigger(animeName);
    }
}
