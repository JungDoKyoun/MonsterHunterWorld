# 🐲 Monster Hunter-like Unity Multiplayer Project

Photon PUN2과 Firebase를 활용하여 제작한  
멀티플레이 기반 몬스터 헌터 스타일의 팀 프로젝트입니다.

---

## 📽️ 시연 영상

[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://youtu.be/YOUR_VIDEO_LINK)

## 🐉 프로젝트 소개
Unity와 Photon PUN2로 개발한 멀티플레이어 보스 몬스터 시스템입니다. State Pattern 기반 AI와 Object Pooling을 활용한 효율적인 전투 시스템을 구현했습니다.

## 👨‍💻 개발자
**정도균 (JungDoKyoun)**

## 📌 주요 기능

### 🎯 보스 몬스터 AI 시스템

#### 1. **13가지 State Pattern 기반 AI**
<img width="932" height="315" alt="몬헌 상태패턴 drawio" src="https://github.com/user-attachments/assets/c8d401c0-6bb5-419f-be98-123500163c13" />

```csharp
// 13개의 독립적인 상태 클래스
MonsterIdleState, MonsterSleepState, MonsterWakeUpState,
MonsterRotationState, MonsterRoarState, MonsterChaseState,
MonsterAttackState, MonsterBackMoveState, MonsterStunState,
MonsterTrapState, MonsterRunState, MonsterTakeOffState,
MonsterPatrolState, MonsterLandingState, MonsterDieState
```
- **복잡한 행동 패턴**: 비행, 착륙, 수면, 후퇴 등 다양한 상태 전환
- **조건부 상태 전환**: HP 20% 이하 시 도망, 함정 감지 시 특수 상태
- **독립적 상태 관리**: 각 상태별 Enter(), Exit(), Update(), Move() 구현
- **확장 가능한 구조**: 새로운 보스 패턴 추가 시 기존 코드 수정 불필요

#### 2. **Photon 네트워크 동기화**
![몬헌 RPC (2)](https://github.com/user-attachments/assets/93df27fd-493d-437f-b106-20157addefc1)

```csharp
[PunRPC] public void TakeDamage(int damage)
[PunRPC] public void Attack(int attackType)
[PunRPC] public void ShootProjectile(int index, string id)
```
- **RPC 기반 동기화**: 모든 전투 액션 실시간 동기화
- **마스터 클라이언트 권한**: 보스 AI는 마스터가 제어, 모든 클라이언트에 전파
- **애니메이션 동기화**: SetBool, SetTrigger, SetFloat RPC로 완벽 동기화
- **위치 보정**: NavMeshAgent와 Transform 위치 실시간 매칭

### 🚀 투사체 시스템

#### 3. **Object Pooling 투사체 관리**
<img width="1611" height="897" alt="캡처_2025_09_09_02_51_52_3" src="https://github.com/user-attachments/assets/a8247685-9401-419c-b7f1-fb57beadbd55" />

```csharp
Dictionary<ProjectileType, ObjectPool<MonsterProjectile>> MonsterProjectilePool
Dictionary<string, MonsterProjectile> _projectileInstances
```
- **메모리 최적화**: 투사체 재활용으로 GC 부담 최소화
- **GUID 기반 추적**: 각 투사체에 고유 ID 부여로 네트워크 동기화
- **다중 투사체 타입**: FireBall 등 확장 가능한 enum 구조
- **충돌 처리 동기화**: OnTriggerEnter 후 모든 클라이언트에 RPC 전파

#### 4. **전투 메커니즘**
![asd (1) (1)](https://github.com/user-attachments/assets/cf2c42a2-b75d-425a-b94a-dda149dd1367)

```csharp
public void ChooseAttackType() // 랜덤 공격 패턴 선택
public IEnumerator WaitForEndAttackAnime() // 애니메이션 동기화
```
- **다양한 공격 패턴**: 근접, 원거리, 비행 공격 등 데이터 기반 패턴
- **부위별 데미지**: 머리 타격 시 스턴, 누적 데미지 시스템
- **콜라이더 동적 제어**: 공격 애니메이션과 동기화된 무기 콜라이더
- **쿨타임 관리**: 공격별 독립적인 쿨타임 시스템

### 🎮 보스 행동 시스템

#### 5. **NavMesh, 루트 모션 기반 이동 AI**
![녹화_2025_09_08_00_00_00_579 (1)](https://github.com/user-attachments/assets/2ea0df17-dc3f-4224-95df-5997ab2e786c)

```csharp
public void RequestLink() // 점프 링크 처리
public IEnumerator WaitForEndLink() // 링크 애니메이션
```
- **지형 적응 이동**: NavMesh로 장애물 회피 및 최적 경로 탐색
- **순찰 시스템**: RestIndex, SleepIndex 기반 웨이포인트 순찰
- **추적 모드**: 플레이어 감지 시 즉시 추적 모드 전환

## 🛠 기술 스택
- **Engine**: Unity 2022.3.21f
- **Language**: C#
- **Networking**: Photon PUN2
- **Tools**: GitHub, Notion, Visual Studio

## 🎯 시스템 특징
- **완벽한 네트워크 동기화**: 모든 액션과 애니메이션 실시간 동기화
- **효율적인 메모리 관리**: Object Pool과 재사용 가능한 컴포넌트

## 📚 주요 학습 내용
- Photon PUN2를 활용한 실시간 멀티플레이어 구현
- State Pattern으로 복잡한 AI 로직 체계화
- Object Pooling으로 성능 최적화
- NavMesh와 루트 모션을 활용한 3D 이동
- Coroutine을 활용한 애니메이션 타이밍 제어

## 🔧 개선하고 싶은 부분
- 페이즈별 패턴 변화 시스템
- 환경 상호작용 (맵 파괴, 장애물 생성)
- 보스 처치 보상 시스템
