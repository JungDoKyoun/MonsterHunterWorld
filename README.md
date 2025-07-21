🎮 Monster Hunter-like Unity Multiplayer Project
비스트 애니멀즈 팀의 유니티 기반 네트워크 멀티플레이 협업 프로젝트

<p align="center"> <img src="https://img.shields.io/badge/Unity-2022.3.21f-blue?logo=unity"/> <img src="https://img.shields.io/badge/Photon-PUN2-brightgreen?logo=photon"/> <img src="https://img.shields.io/badge/Firebase-Auth%20&%20DB-yellow?logo=firebase"/> </p>
📽️ 시연 영상
👉 [YouTube 시연 링크] https://youtu.be/VCp4IfyKoJc?si=v91WG-ZnZITtIgwS

🧑‍💻 팀원 소개
이름	역할
조영락	팀장, 인벤토리 시스템, 아이템 UI, CSV/DB 연동
정영한	플레이어 조작, 씬 전환 및 룸 입장 로직
정도균	몬스터 AI, 루트 모션, 오브젝트 풀링, 상태이상
손성태	UI 시스템, 사운드, 로딩 씬, Firebase/Photon 기본 기능

🔧 기술 스택
엔진: Unity 2022.3.21f

네트워크: Photon PUN2

DB/Auth: Firebase (Authentication, Database)

언어: C#

버전 관리: GitHub (트렁크 기반 협업)

🗂️ 폴더 구조 요약
bash
복사
편집
Assets/
├── Scripts/
│   ├── Inventory/        # 인벤토리 및 아이템 관련
│   ├── Monster/          # 몬스터 루트모션, 공격 등
│   ├── Network/          # Photon, Firebase 연동
│   ├── Player/           # 플레이어 이동, 입력 처리
│   └── UI/               # 각종 UI 매니저 및 시스템
├── Resources/
│   ├── CSV/              # 아이템 데이터 테이블
│   └── Sound/            # 효과음 파일
🧩 주요 기능 요약
✅ 조영락 (팀장)
CSV 기반 아이템 데이터 테이블 로딩 및 인벤토리 초기화

Firebase에 유저 정보 저장 및 비동기 세이브/로드

장비/소모품 구분 및 아이템 장착/교환 로직 구현

옵저버 패턴 기반 UI 자동 갱신

퀵슬롯 기능과 이펙트 재생 (함정, 포션 등)

✅ 정영한
카메라 기준 방향으로 이동 가능한 WASD 조작

각도 계산 및 쿼터니언을 활용한 회전 처리

집회소 → 사냥터로의 동기화된 씬 이동 처리

해시테이블 기반 방 공유 시스템 구현

✅ 정도균
루트모션 기반 몬스터 이동 및 공중 순찰

몬스터 공격/휴식/기절/상태이상 처리

Photon RPC 기반 투사체 풀링 및 충돌 처리

Monster ScriptableObject로 데이터 관리

✅ 손성태
Firebase 회원가입, 로그인, 닉네임 연동

AudioMixer 기반 효과음 볼륨 제어 (JSON 저장/불러오기)

로딩 씬 구현: 회전 이미지, 페이드인/아웃, 씬 전환

UI 동적 포커싱 및 사운드 Trigger

🚀 실행 방법
GitHub 클론

bash
복사
편집
git clone https://github.com/jylack/MonsterHunterWorld.git
Firebase 세팅 (Auth, DB)
Firebase 콘솔에서 웹 앱 등록 후 .json 설정파일 추가

Unity에서 Assets/Scripts/Scripts.zip 압축 해제

Photon App ID 등록

https://www.photonengine.com/

App ID를 Unity Photon Server 설정에 등록

실행 (에디터 혹은 빌드)

📁 관련 자료
팀 발표 PDF: 몬헌 모작 유니티 네트워크 팀프로젝트 PPT.pdf

코드 압축본: Scripts.zip

📝 개발 후기 및 교훈
실시간 멀티플레이 게임에서 Photon의 룸 구조 이해가 중요

루트모션 + 네비메시 동기화는 예외처리와 타이밍 조절이 핵심

Firebase 사용 시 async/await 처리로 데이터 누락 방지 필요

UI 매니저 중심의 설계로 화면 상태 전환 유지에 유리함
