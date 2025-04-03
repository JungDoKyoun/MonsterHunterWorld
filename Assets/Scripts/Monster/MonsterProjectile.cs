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
    private bool _isReturn = false;

    public bool HasHit { get { return _hasHit; } set { _hasHit = value; } }
    public bool IsReturn { get { return _isReturn; } set { _isReturn = value; } }
    public string ProjectileID { get; private set; }

    public ProjectileType ProjectileType { get { return projectileType; } set { projectileType = value; } }

    private void Start()
    {
        if (_monsterProjectileSpawnManager == null)
        {
            _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();
        }
    }

    private void Update()
    {
        if (_data == null) return;
        Shoot();
        ReturnPool();
    }

    public void AssignID(string id)
    {
        ProjectileID = id;
    }

    [PunRPC]
    public void InitShooter(int shooterViewID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var shooterView = PhotonView.Find(shooterViewID);
        var shooterGO = shooterView != null ? shooterView.gameObject : null;

        _shooter = shooterGO;
        _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();

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
        if (_isReturn || _hasHit) return;

        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= _data.lifeTime)
        {
            _isReturn = true;
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
        if (_hasHit || other.CompareTag("Trap")) return;
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

        photonView.RPC("RPC_DisableProjectile", RpcTarget.All, ProjectileID);
    }

    [PunRPC]
    private void RPC_DisableProjectile(string projectileID)
    {
        //Debug.Log("실행됨");
        //_isReturn = true;
        //_monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
        //if (_isReturn) return;
        //_isReturn = true;

        //if (_monsterProjectileSpawnManager == null)
        //    _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    _monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
        //}
        //else
        //{
        //    gameObject.SetActive(false);
        //}

        //Debug.Log($"[RPC_DisableProjectile] called on {gameObject.name} ({PhotonNetwork.LocalPlayer.NickName})");

        //if (_isReturn) return;
        //_isReturn = true;

        //if (_monsterProjectileSpawnManager == null)
        //{
        //    _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();
        //}

        //if (_monsterProjectileSpawnManager != null)
        //{
        //    _monsterProjectileSpawnManager.ReturnProjectile(projectileType, this);
        //}
        //else
        //{
        //    // 강제 비활성화 (디버그 목적)
        //    Debug.LogWarning("SpawnManager가 존재하지 않음, 강제 비활성화 시도");
        //    gameObject.SetActive(false);
        //}

        Debug.Log($"[RPC_DisableProjectile] ID: {projectileID}");

        if (_monsterProjectileSpawnManager == null)
            _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();

        if (_monsterProjectileSpawnManager != null)
        {
            _monsterProjectileSpawnManager.ReturnProjectileByID(projectileID);
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonInstantiate 호출됨");

        // 마스터가 아닌 경우 SpawnManager 직접 찾아서 연결
        if (_monsterProjectileSpawnManager == null)
        {
            _monsterProjectileSpawnManager = FindObjectOfType<MonsterProjectileSpawnManager>();
            if (_monsterProjectileSpawnManager != null)
            {
                Debug.Log("SpawnManager 자동 세팅 완료");
            }
            else
            {
                Debug.LogWarning("SpawnManager 찾지 못함");
            }
        }
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
