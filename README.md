# TwoFacedPoker Client

> TwoFacedPoker는 TvN에서 방영했던 예능 프로 '더 지니어스'에 나온 '양면 포커'를 이식한 게임입니다. 양면 포커는 앞면과 뒷면을 가진 카드를 활용해 상대와 칩을 베팅하는 2인용 온라인 카드 게임입니다.  
> 플레이어는 앞면, 뒷면 또는 양면에 베팅할 수 있으며, 서버가 관리하는 턴과 승패 결과에 따라 최종 승자를 결정합니다.

- 이 저장소는 TwoFacedPoker의 **Windows 클라이언트**입니다.
- 게임 서버는 [TwoFacedPoker Server](https://github.com/fanjae/TwofacedPoker) 저장소에서 확인할 수 있습니다.

## 프로젝트 개요

| 항목 | 내용 |
|---|---|
| 프로젝트명 | TwoFacedPoker Client |
| 최초 개발 | 2024.10.08 ~ 2024.11.08 |
| 리팩토링 | 2026.07.18 ~ 2026.07.22 |
| 개발 인원 | 1명 |
| 개발 환경 | .NET 8, C#, Windows Forms |
| 실행 환경 | Windows |
| IDE | Visual Studio 2022 |

## 실행 방법

### 사전 요구 사항

- Windows 10 이상
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (`.NET 데스크톱 개발` 워크로드)
- 실행 중인 [TwoFacedPoker Server](https://github.com/fanjae/TwofacedPoker)

### Client
- 프로젝트 루트의 `server.ini`에 접속할 서버의 IPv4 주소와 포트를 입력합니다.
```ini
[server]
server=127.0.0.1
port=9190
```

## 구현 기능

| 기능 | 설명 |
|:----:|:---:|
| 서버 연결 및 로그인 | `server.ini`에서 주소를 불러와 TCP 서버에 접속하고 서버가 할당한 ID를 표시 |
| 로비 |  방 목록 조회와 새로고침, 방 생성, 선택한 방 입장 |
| 게임 방 |  참가자 ID와 준비 상태 표시, 준비 완료 후 게임 시작 |
| 실시간 채팅 | 같은 방에 입장한 사용자 간 메시지 송수신 및 Enter 키 전송 |
| 카드 표시 | 서버가 전달한 카드 정보를 앞면·뒷면 이미지로 변환하여 출력 |
| 베팅 | 앞면, 뒷면, 양면 베팅과 베팅 취소 및 포기 처리 |
| 게임 진행 | 턴, 보유 칩, 판돈, 카드 공개, 라운드 결과와 최종 승패 표시 |
| 사운드 | 내 턴, 라운드 승패, 최종 승리 이벤트에 효과음 재생 |
| 패킷 처리 | 4바이트 Big-Endian 길이 헤더와 UTF-8 본문을 사용하는 TCP 패킷 송수신 |
| 연결 종료 | 방 퇴장 응답 대기, 타임아웃 시 소켓 정리, 연결 오류 시 재접속 상태로 초기화 |

## 주요 기능

### 로비 및 방 관리
<img width="2000" height="1300" alt="2 GameLobby" src="https://github.com/user-attachments/assets/608c75c2-ee82-452e-8278-fcbb5175aaf9" />

- 서버 접속 및 클라이언트 ID 할당
- 서버의 최신 방 목록 조회
- 방 생성 후 자동 입장
- 선택한 방 입장
- 사용자 준비 상태 동기화
- 게임 중 임의 퇴장 제한

### 게임 시스템
<img width="3274" height="2056" alt="image" src="https://github.com/user-attachments/assets/9ad2c9ee-c339-4e48-992d-a6b932b9db27" />

- 2인 턴 기반 게임 진행
- 내 카드의 앞면·뒷면 및 상대 카드 표시
- 플레이어, 상대, 딜러의 칩 상태 동기화
- 라운드 초기화와 최종 승패 처리
- 상대의 뒷면 카드 공개 이벤트 처리

### 베팅 시스템
<img width="3274" height="2056" alt="image" src="https://github.com/user-attachments/assets/12df6d87-59b7-4b8b-b133-3b4e84fcdd16" />

- 앞면(Front), 뒷면(Back), 양면(Both) 베팅
- 베팅 금액 입력 및 취소
- 베팅 포기(Fold)
- 보유 칩, 상대 보유 칩, 최소 콜 금액 검증
- 양면 베팅 시 양쪽에 동일한 칩 적용
- 한 라운드의 베팅 면과 양면 베팅 사용 상태 관리

### 네트워크 및 프로토콜

- TCP Socket 기반 서버 통신
- 패킷 분할 수신과 부분 전송 대응
- 최대 패킷 크기 검증
- 송신 임계 구역을 통한 동시 전송 보호
- 방, 게임, 채팅 패킷 분류 및 기능별 파서 적용
- 백그라운드 수신 스레드와 UI 스레드 분리
- 송수신 패킷 로그 파일 기록

## 조작 방법

| 입력 | 기능 |
|---|---|
| 마우스 | 로비, 방, 베팅 UI 조작 |
| Enter | 채팅 메시지 전송 |
| F5 | 준비 상태 전환 |
| F6 | 두 플레이어가 준비된 경우 게임 시작 요청 |

## 프로젝트 구조

```text
TwofacedPoker_Client/
├── Common/                 # 설정, 상수, 카드 이미지 및 오디오 처리
├── Forms/
│   ├── Lobby/              # 서버 연결, 방 목록, 생성 및 입장
│   ├── CreateRoom/         # 방 생성 대화 상자
│   └── ChattingRoom/       # 채팅, 게임, 베팅, 네트워크, 종료 처리
├── Game/                   # 클라이언트 방/게임 상태와 베팅 규칙
├── Network/                # 패킷 송수신 및 방 정보 모델
├── Protocol/               # 요청 생성과 서버·방·게임 패킷 파싱
├── Image/                  # 카드 앞면·뒷면 이미지
├── server.ini              # 서버 접속 주소와 포트
└── TwofacedPoker_Client.sln
```

## 리팩토링 변경 사항

- Form에 집중되어 있던 코드를 기능별 `partial class` 파일로 분리
- 문자열 프로토콜의 상수, 요청 생성, 응답 파싱 책임 분리
- 로비 상태, 방 상태, 게임 상태를 별도 객체로 분리
- `ReceiveAll`·`SendAll`을 적용해 TCP 부분 송수신 처리
- 카드 이미지 교체 시 기존 Bitmap과 오디오 재생 리소스 해제
- 폼 종료 시 서버 퇴장 응답을 기다린 뒤 안전하게 소켓 정리

## 관련 저장소
- [TwoFacedPoker Server](https://github.com/fanjae/TwofacedPoker)

## 개발 일지
- [TwoFacedPoker 개발일지 Blog](https://fanjae.tistory.com/category/Projects/Two%20Faced%20Poker)

## 플레이 영상
- [플레이 영상](https://youtu.be/CBQlnrXCDQQ?si=X5Cua2wXGq2qEz2h)
