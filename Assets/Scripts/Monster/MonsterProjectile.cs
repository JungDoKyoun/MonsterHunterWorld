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
    private bool _hasHit = false;

    public bool HasHit { get { return _hasHit; } set { _hasHit = value; } }
    public string ProjectileID { get; private set; }

    public ProjectileType ProjectileType { get { return projectileType; } set { projectileType = value; } }

    private void Update()
    {
        Shoot();
        ReturnPool();
    }

    public void AssignID(string id)
    {
        ProjectileID = id;
    }

    public void InitShooter(GameObject shooter, MonsterProjectileSpawnManager projectileSpawnManager)
    {
        _shooter = shooter;
        _monsterProjectileSpawnManager = projectileSpawnManager;

        Collider[] shooterCholliders = _shooter.GetComponentsInChildren<Collider>();
        Collider myCollider = GetComponent<Collider>();

        foreach(var co in shooterCholliders)
        {
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
        if (_hasHit || !PhotonNetwork.IsMasterClient) return;
        _hasHit = true;

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
                    break;
                }

                current = current.parent;
            }
        }

        if (PhotonNetwork.IsMasterClient && OnHitEffect != null)
        {
            PhotonNetwork.Instantiate(OnHitEffect.name, transform.position, Quaternion.identity);
        }

        photonView.RPC("RPC_DisableProjectile", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_DisableProjectile()
    {
        _monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
    }


    //[PunRPC]
    //public void RPC_ReturnToPool(int viewID, int type)
    //{
    //    GameObject obj = PhotonView.Find(viewID)?.gameObject;
    //    Debug.Log($"[RPC] Return to Pool called for viewID: {viewID}, type: {(ProjectileType)type}");

    //    if (obj != null)
    //    {
    //        var proj = obj.GetComponent<MonsterProjectile>();
    //        if (proj != null)
    //        {
    //            _monsterProjectileSpawnManager.ReturnProjectile((ProjectileType)type, proj);
    //        }
    //    }
    //}
}
