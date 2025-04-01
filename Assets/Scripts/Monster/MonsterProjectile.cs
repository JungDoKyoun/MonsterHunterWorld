using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterProjectile : MonoBehaviourPunCallbacks
{
    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private ParticleSystem OnHitEffect;
    private List<Collider> _ignorColliders = new List<Collider>();
    private MonsterProjectileData _data;
    private MonsterProjectileSpawnManager _monsterProjectileSpawnManager;
    private GameObject _shooter;
    private float _elapsedTime;

    public ProjectileType ProjectileType { get { return projectileType; } set { projectileType = value; } }

    private void Update()
    {
        Shoot();
        ReturnPool();
    }

    public void InitShooter(GameObject shooter, MonsterProjectileSpawnManager projectileSpawnManager)
    {
        _shooter = shooter;
        _monsterProjectileSpawnManager = projectileSpawnManager;

        Collider[] shooterCholliders = _shooter.GetComponentsInChildren<Collider>();
        Collider myCollider = GetComponent<Collider>();

        foreach(var co in shooterCholliders)
        {
            Debug.Log(co);
            Physics.IgnoreCollision(myCollider, co);
            _ignorColliders.Add(co);
        }
    }

    public void SetData(MonsterProjectileData newData)
    {
        _data = newData;
    }

    public void Shoot()
    {
        int Projectilespeed = _data.speed;
        transform.Translate(Vector3.forward * Projectilespeed * Time.deltaTime, Space.Self);
    }

    public void ReturnPool()
    {
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= _data.lifeTime)
        {
            _monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
            _elapsedTime = 0;
        }
    }

    public void ResetProjectile()
    {
        Collider mycollider = GetComponent<Collider>();

        foreach(var co in _ignorColliders)
        {
            if(co != null && mycollider != null)
            {
                Physics.IgnoreCollision(mycollider, co, false);
            }
        }
        _ignorColliders.Clear();
    }

    //[PunRPC]
    //public void SpawnHitEffect(Vector3 pos)
    //{
    //    if (OnHitEffect != null)
    //    {
    //        Instantiate(OnHitEffect, pos, Quaternion.identity);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (other.CompareTag("Player"))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Transform current = other.transform;

            while (current != null)
            {
                PlayerController player = current.GetComponent<PlayerController>();

                if (player != null)
                {
                    player.GetComponent<PlayerController>().TakeDamage(contactPoint, _data.damage);
                    return;
                }

                current = current.parent;
            }
        }

        if (PhotonNetwork.IsMasterClient && OnHitEffect != null)
        {
            PhotonNetwork.Instantiate(OnHitEffect.name, transform.position, Quaternion.identity);
        }

        _monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
    }
}
