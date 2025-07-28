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
## 프로젝트가 바로 실행은 안됩니다.
이력서와 포트폴리오에서 추가로 다운받을 리소스 파일들이있습니다.

--- 

## 🚀 실행 방법

1. 모델링 등 몇몇 폴더 다운로드 - 이메일로 요청 

2. 리포지토리 클론
   ```bash
   git clone https://github.com/jylack/MonsterHunterWorld.git

3. 압축 폴더명에 맞는 폴더에 여기에 압축풀기  

4. Unity에서 실행 

문서 자료

📄 팀 발표 자료: https://www.canva.com/design/DAGtaLjVA1k/53ipM-s4FxlRW4CR0H1xIg/edit

📄 팀 노션 자료: https://www.notion.so/1ad18710838f80f6b174cdf8791f1502?source=copy_link

---

## 💬 개발 후기
Photon, Firebase, 인벤토리 등 각자의 파트를 실제 게임처럼 기능별로 나눠 구현하며
팀 협업 기반 개발 경험을 쌓을 수 있었던 프로젝트였습니다.

| 이름     | 담당 영역 | 회고 |
|----------|-----------|------|
| **손성태** | UI, 사운드, 로딩 씬 | 프로젝트 초기에 Photon 네트워크 구조를 익히며, UI와 사운드 시스템, 로딩 씬 구현을 담당했습니다. 로비 기능은 구조상 제외되었지만, 그만큼 핵심 로직 설계에 집중할 수 있었고, 전체 흐름을 파악하는 데 큰 도움이 되었습니다. |
| **정도균** | 몬스터, 루트모션, 상태이상 | 루트모션 제어와 몬스터 동기화 구현에 많은 시간을 들였고, Photon 연동은 처음이라 시행착오도 많았지만, 실제 동작을 확인하며 보람을 느낄 수 있었습니다. 멀티플레이에 적합한 구조 설계에 대해 배운 점이 많았습니다. |
| **조영락** | 인벤토리 시스템, 아이템 UI, Firebase 연동 | 아이템/장비 데이터를 CSV와 Firebase로 연동하고, UI 자동 갱신 및 장비 장착 로직을 구현했습니다. 데이터 구조화가 생각보다 까다로웠지만, 문제를 해결하며 성장할 수 있었고, Null 예외처리 등에서도 많은 경험을 얻었습니다. |
| **정영한** | 플레이어 조작, 씬 전환, Photon 룸 로직 | 이전 프로젝트에서 Photon을 사용해본 경험은 있었지만, 씬 간 전환과 룸 입장 동기화는 여전히 어렵게 느껴졌습니다. 이번 프로젝트를 통해 더 명확한 구조와 처리 방식에 대해 고민하고 개선할 수 있었습니다. |

