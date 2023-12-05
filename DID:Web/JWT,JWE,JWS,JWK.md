# JSON(JavaScript Object Notation)
* 텍스트를 사용하여 데이터를 저장하고 전송하는 데이터 공유를 위한 개방형 표준 파일 형식
* 키 : 값 형식으로 작성

# JOSE(JavaScript Object Signing and Encryption)
- 웹에서 데이터를 안전하게 교환하기 위한 일련의 표준
  
- JSON Web Signature (JWS)
  * 데이터의 무결성을 보장하기 위해 사용
  * JWS는 JSON 객체를 서명하여 데이터의 변조를 방지하고, 서명을 검증하여 데이터가 원본 그대로인지 확인

- JSON Web Encryption (JWE)
  * 데이터의 기밀성을 보장하기 위해 사용
  *  JWE는 JSON 객체를 암호화하여 안전한 전송을 가능케 하고, 복호화하여 원본 데이터를 복원

- JSON Web Key (JWK)
  * 공개 키 및 개인 키를 JSON 형식으로 나타내는 표준
  *  JWK는 JOSE에서 사용되는 키를 교환하고 관리하는 데 사용

- JSON Web Token (JWT)
  * 인증 및 정보 교환을 위한 토큰 JWT는 클레임(claim)이라 불리는 속성들을 포함
  * 이를 서명하여 검증함으로써 클라이언트와 서버 간의 안전한 통신을 보장

  
# JWT
- 데이터 전달을 위해 사용됨
- 인증에 필요한 정보를 암호화 한 JSON 토큰 
- JSON 데이터를 base64로 인코딩해 직렬화 한 것
- .을 구분자로 Header(헤더).Payload(데이터).Signature(서명)으로 나뉨

## 장점
- 데이터 위변조를 막을 수 있다.
- 인증 정보에 대한 별도의 저장소가 필요 없다.
- JWT는 검증 정보, 데이터 정보 등 모든 정보를 자체적으로 가지고 있다.
- 다른 로그인 시스템에 접근 및 권한 공유가 가능하다. (ex. 타 브라우저 등)

## 단점
- 토큰 자체에 모든 정보를 담고 있으므로 양날의 검이 될 수 있다.
- 토큰 길이가 길어지면 네트워크에 부하를 줄 수 있다.
- payload는 인코딩/디코딩만으로 데이터를 볼 수 있으므로 중요데이터를 넣으면 안 된다.
- 토큰 자체를 탈취당하면 대처가 어렵다.


# JWS
- 헤더,데이터, 서명으로 구성
- 헤더는 서명 알고리즘 등이 정의
- 서명은 비밀 키를 사용하여 생성
- 웹 통신에서 메시지의 신뢰성을 확보하기 위해 사용

# JWK
- kty 키의 유형
- kid 키식별자
- use 키의용도
- alg 연산 알고리즘
