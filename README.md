# 🐲 Monster Hunter-like Unity Multiplayer Project

Photon PUN2과 Firebase를 활용하여 제작한  
멀티플레이 기반 몬스터 헌터 스타일의 팀 프로젝트입니다.

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3.21f-blue?logo=unity"/>
  <img src="https://img.shields.io/badge/Photon-PUN2-brightgreen?logo=photon"/>
  <img src="https://img.shields.io/badge/Firebase-Auth%20&%20DB-yellow?logo=firebase"/>
</p>

---

## 📽️ 시연 영상
👉 [YouTube 시연영상 ](https://youtu.be/VCp4IfyKoJc?si=AvvPspbcFZPn77_F)

---

## 🧑‍💻 팀원 소개

| 이름     | 역할 |
|----------|------|
| **조영락** | 팀장, 아이템 시스템, 인벤토리 UI, CSV/Firebase 연동 |
| 정영한    | 플레이어 조작, 씬 전환, 룸 입장 처리 |
| 정도균    | 몬스터 AI, 루트 모션, 상태이상, 풀링 |
| 손성태    | UI 시스템, 사운드, 로딩 씬, Firebase/Photon 연동 |

---

## 🔧 기술 스택

- **Engine**: Unity 2022.3.21f
- **Language**: C#
- **Networking**: Photon PUN2
- **Backend**: Firebase (Auth + DB)
- **Tools**: GitHub, Notion, Visual Studio

---

## 📁 폴더 구조

```
Assets/
├── Scripts/
│   ├── Inventory/
│   ├── Monster/
│   ├── Network/
│   ├── Player/
│   └── UI/
├── Resources/
│   ├── CSV/
│   └── Sound/
```


---

## 🧩 주요 기능 요약

### ✅ 조영락 (팀장)
- CSV 기반 아이템 데이터 로딩
- 장비/소모품 구분 및 장착 로직
- Firebase 연동 세이브/로드
- 퀵슬롯 기능 및 이펙트 연동

### ✅ 정영한
- WASD 방향 이동 + 쿼터니언 회전
- 씬 간 전환 처리
- Photon 룸 입장/생성 시스템

### ✅ 정도균
- 루트모션 기반 몬스터 AI
- 공중 이동, 공격, 패턴 처리
- 투사체 풀링 및 RPC 처리

### ✅ 손성태
- Firebase 인증, 닉네임 저장
- 로딩 씬, 오디오 설정 저장
- UI 사운드 및 포커싱 구현

---
## 실행 자료 요청 이메일
jyl011202@gmail.com

--- 

## 🚀 실행 방법

1. 모델링 등 몇몇 폴더 다운로드 - 이메일로 요청 

2. 리포지토리 클론
   ```bash
   git clone https://github.com/jylack/MonsterHunterWorld.git

3. 압축 폴더명에 맞는 폴더에 여기에 압축풀기  

4. Unity에서 실행 

📄 문서 자료
팀 발표 자료: https://www.canva.com/design/DAGtaLjVA1k/53ipM-s4FxlRW4CR0H1xIg/edit
팀 노션 자료: https://www.notion.so/1ad18710838f80f6b174cdf8791f1502?source=copy_link

💬 개발 후기
Photon, Firebase, 인벤토리 등 각자의 파트를 실제 게임처럼 기능별로 나눠 구현하며
팀 협업 기반 개발 경험을 쌓을 수 있었던 프로젝트였습니다.
