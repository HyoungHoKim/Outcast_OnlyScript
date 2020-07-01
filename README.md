# Outcast

Outcast는 SF 잠입액션 컨셉의 VR 게임 콘텐츠입니다.
잡입 액션을 좀 더 실감나게 구현하기 위해 이동방식과 벽타기, 적 시야탐지 등을 개발했고, 추가로 여러 재미요소로 장전 방식이나 도탄, 드론 등을 추가했습니다.
개인적인 팀 사정으로 알파버전 중간에 중단했지만 나중에 더 나은 실력으로 다시 개발해보고 싶은 콘텐츠이기도 하고 많이 아쉬움이 남아서 기록 겸 동기부여 차원에서 작성했습니다.

### 트레일러 영상
[![Video Label](http://img.youtube.com/vi/xBQvsTsA6IA/0.jpg)](https://youtu.be/xBQvsTsA6IA?t=0s)


### 개요
- [Github Link](https://github.com/HyoungHoKim/Outcast_OnlyScript) : 프로젝트에 여러 유료 에셋을 사용했기 때문에, 저작권 문제로 직접 짠 코드만을 공개
- 메디치 어트랙션 기반 VR 개발자 양성 과정 최종 프로젝트
- 개발 기간 : 2020.04.28 ~ 2020.06.10
- 장비 : Vive Pro, Vive Tracker, KATVR mini
- 팀명 : 가산 메디치 연합
- 팀원 : 이경하(팀장, 에너미, 비주얼 이펙트, 어트랙션), 김형호(플레이어, 플레이어 IK, 특수 상호작용), 박철우(드론 AI, UI, 레벨 디자인)

### 조작법
- 이동 : 트랙패드 터치 시 방향으로 기본 이동, 트랙패드를 누르고 방향 설정 후 떼면 향하는 위치로 빠르게 이동(텔레포트)
- UI 및 플레이어, 드론 상호작용 : 트리거를 누르고 왼손을 좌에서 우로 흔들면 UI창 생성, 컨트롤러로 해당 UI 클릭 후 트리거 누르면 기능 실행
- 벽타기 : 벽에 컨트롤러를 대고 트리거를 누른 채 컨트롤러를 당김.
- 점프 : 양 손 컨트롤러 트리거를 누르고 위 아래로 힘껏 당김.
- 장전 : 총알을 다 쏘면 장전 홀더가 열림. 총알을 들어 해당 위치에 가져가면 홀더에 총알이 들어감. 그리고 홀더가 열린 방향으로 총을 돌리면 홀더가 닫히고 장전 완료.  

### 게임 구성
##### 싱글 모드
- 건물에 잡임, 적들을 죽이거나 잡임해서 해당 위치에 폭탄을 설치하면 완료

##### 멀티 모드
- 협동이나 PVP를 추가하려 했으나 시간상 문제로 무산

### 발표 문서
- [참고 링크](https://drive.google.com/file/d/1GspJt_RMxemqi3acA1pPHIJnRSlfREdh/view?usp=sharing)

### 핵심 로직
- 제가 개발한 로직만 정리했습니다.

#### 벽타기 로직
```
private void ClimbPull()
   {

       // 태그를 이용 오를 수 있는 오브젝트를 구별
       if (attachObject.tag == "CLIMB" || attachObject.tag == "GROUND")
       {
           // canGrip : 핸드 콜리전에 닿으면 true
           // ClimbAction : 크리거 누르고 있으면 true
           if (canGrip && ClimbAction.GetState(inputSource))
           {
               bodyRb.isKinematic = true;
               bodyRb.useGravity = false;

               // 컨트롤러 변동 값을 바디 trasfrom 값에 더해줌, 게임상에서는 손 위치는 변동이 없고 바디 위치값만 변하기 때문에 올라가는 것 처럼 보임
               body.transform.position += (handPrevPos - this.transform.localPosition);

           }
           else if (ClimbAction.GetStateUp(inputSource))
           {
               bodyRb.isKinematic = false;
               bodyRb.useGravity = true;

               // 트리거를 땠을 때 손 위치 변동값을 velocity로 주면 해당 방향으로 점프하는 듯한 효과를 준다.
               bodyRb.velocity += (handPrevPos - this.transform.localPosition) / Time.deltaTime;
           }

           handPrevPos = this.transform.localPosition;
       }
   }
```

#### 텔레포트 포물선 로직
```
private void UpdatePath()
    {
        // 핸드 위치
        Transform startPos = isLeft == true ? lHandPos : rHandPos;

        // 이동할 위치가 감지 됐는지
        Detected = false;

        // 플레이어 이동을 위한 좌표와 라인을 그리기 위한 좌표
        vertexList.Clear();
        lineVertexList.Clear();

        //포물선이 뻣어나갈 방향
        velocity = Quaternion.AngleAxis(-angle, startPos.right) * startPos.forward * strength;

        RaycastHit hit;

        // 플레이어 이동은 월드 좌표로
        Vector3 pos = startPos.position;
        // 포물선은 터치패드를 클릭한 컨트롤러를 따라가야 하기 때문에 로컬로 설정
        Vector3 linePos = startPos.localPosition;

        // 시작 위치 배열에 저장
        vertexList.Add(pos);
        lineVertexList.Add(linePos);

        // 리소스 사용을 줄이기 위해 정해진 버텍수까지만 그리도록
        while (!Detected && vertexList.Count < maxVertexcount)
        {

            // 포물선이 뻣어나갈 다음 위치를 계산
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;
            Vector3 newLinePos = linePos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            vertexList.Add(newPos);
            lineVertexList.Add(newLinePos);

            // 가중치
            velocity += Physics.gravity * vertexDelta;

            // 올바른 위치(tag)에 닿았는지 판단  
            if (Physics.Linecast(pos, newPos, out hit))
            {
                if (hit.transform.gameObject.tag == "GROUND"
                    && hit.transform.position.y < this.transform.position.y + 1
                    && hit.transform.position.y > this.transform.position.y - 1)
                {
                    groundDetected = true;
                }
                else groundDetected = false;

                Detected = true;
                groundPos = hit.point;
                lastNormal = hit.normal;
            }

            pos = newPos;
            linePos = newLinePos;
        }

        // 감지하면 그 위치로 마커 활성화
        if (Detected)
        {

            positionMarker.SetActive(true);
            positionMarker.transform.position = groundPos + (lastNormal * 0.01f);
            positionMarker.transform.rotation = Quaternion.LookRotation(lastNormal);
            positionMarker.transform.Rotate(90.0f, 0, 0);

            if (groundDetected)
            {
                positionMarker.GetComponent<MeshRenderer>().material = MatEnable;
            }
            else
            {
                groundPos = this.transform.position;
                positionMarker.GetComponent<MeshRenderer>().material = MatInvaild;
            }
        }

        // 위 좌표정보를 바탕으로 라인렌더러 생성
        arcRenderer.positionCount = lineVertexList.Count;
        arcRenderer.SetPositions(lineVertexList.ToArray());
    }

}
```

#### 도탄 로직
```
public void Beam()
   {

       // 도탄 이펙트를 아직 만들지 않은 경우 만듭니다. 라인 렌더러를 사용
       if (beamGO == null)
       {
           beamGO = new GameObject(beamTypeName, typeof(LineRenderer));
           beamGO.transform.parent = transform;
       }

       LineRenderer beamLR = beamGO.GetComponent<LineRenderer>();
       beamLR.material = beamMaterial;
       beamLR.material.SetColor("_TintColor", beamColor);
       beamLR.startWidth = startBeamWidth;
       beamLR.endWidth = endBeamWidth;

       반사 횟수
       int reflections = 0;

       // 도탄 빔이 반사되는 모든 좌표
       reflectionPoints = new List<Vector3>();
       reflectionHitObjects = new List<GameObject>();

       // 첫번째 좌표 저장
       reflectionPoints.Add(raycastStartSpot.position);

       // 마지막 반사 지점을 저장
       Vector3 lastPoint = raycastStartSpot.position;

       // 빔 계산을 위한 변수 선언
       Vector3 incomingDirection;
       Vector3 reflectDirection;

       // 빔이 계속 반사를 해야되는지 판단
       bool keepReflecting = true;

       Ray ray = new Ray(lastPoint, raycastStartSpot.forward);
       RaycastHit hit;

       do
       {
           // 다음 좌표를 초기화, 레이캐스트 히트가 리턴되지 않으면 forward direction * 범위
           Vector3 nextPoint = ray.direction * range;

           if (Physics.Raycast(ray, out hit, range))
           {
               // 다음 좌표를 히트 좌표로 설정
               nextPoint = hit.point;

               // 레이를 쏠 다음 방향을 계산
               incomingDirection = nextPoint - lastPoint;
               reflectDirection = Vector3.Reflect(incomingDirection, hit.normal);
               ray = new Ray(nextPoint, reflectDirection);

               // lastPoint 업데이트
               lastPoint = hit.point;

               /*
                Hit Effects 생략
               /*

               LastHitObject = hit.collider.gameObject;

               // 반사 회수 증가
               reflections++;
           }
           else
           {
               keepReflecting = false;
           }

           // 다음 좌표 값 배열에 저장
           reflectionPoints.Add(nextPoint);
           reflectionHitObjects.Add(hit.transform.gameObject);

       } while (keepReflecting && reflections < maxReflections && reflect && (reflectionMaterial == null || (FindMeshRenderer(hit.collider.gameObject) != null && FindMeshRenderer(hit.collider.gameObject).sharedMaterial == reflectionMaterial)));

       // 라인 렌더러 빔의 각 좌표 위치값 설정
       //beamLR.SetVertexCount(reflectionPoints.Count);

       beamLR.positionCount = reflectionPoints.Count;

       /*
       muzzleEffects 생략
       */
   }

   ```

#### 가속도 값 계산 (리볼버 장전 홀더 탈착 가속도 계산)
   ```
   public Vector3 LinearAcceleration(out Vector3 vector, Vector3 position, int samples){
		Vector3 averageSpeedChange = Vector3.zero;
		Vector3 averageVelocity = Vector3.zero;
		vector = Vector3.zero;
		Vector3 deltaDistance;
		float deltaTime;
		Vector3 speedA = Vector3.zero;
		Vector3 speedB = Vector3.zero;

        // 샘플 양을 고정, 가속도 계산을 하려면 적어도 두가지 변화가 필요
        // 속도가 빠르면 최소 3가지 이상의 위치 샘플이 필요
		if(samples < 3){

			samples = 3;
		}

		// 초기화
		if(positionRegister == null) {
			positionRegister = new Vector3[samples];
			posTimeRegister = new float[samples];
		}

        // 위치 및 시간 샘플 값을 배열에 저장
		for(int i = 0; i < positionRegister.Length - 1; i++){

			positionRegister[i] = positionRegister[i+1];
			posTimeRegister[i] = posTimeRegister[i+1];
		}
		positionRegister[positionRegister.Length - 1] = position;
		posTimeRegister[posTimeRegister.Length - 1] = Time.time;

		positionSamplesTaken++;

		// 가속도는 충분한 샘플을 얻었을 때만 계산 가능
		if(positionSamplesTaken >= samples){

			//평균 속도 변화를 계산
			for(int i = 0; i < positionRegister.Length - 2; i++){

				deltaDistance = positionRegister[i+1] - positionRegister[i];
				deltaTime = posTimeRegister[i+1] - posTimeRegister[i];

				//If deltaTime is 0, the output is invalid.
				if(deltaTime == 0){

					return Vector3.zero;
				}

				speedA = deltaDistance / deltaTime;
				deltaDistance = positionRegister[i+2] - positionRegister[i+1];
				deltaTime = posTimeRegister[i+2] - posTimeRegister[i+1];

				if(deltaTime == 0){

					return  Vector3.zero;
				}

				speedB = deltaDistance / deltaTime;

				//누적된 속도 변화
				averageSpeedChange += speedB - speedA;
				averageVelocity += speedB;

			}

			//평균 속도 변화
			averageSpeedChange /= positionRegister.Length - 2;
			averageVelocity /= positionRegister.Length - 2;

            // 시차
			float deltaTimeTotal = posTimeRegister[posTimeRegister.Length - 1] - posTimeRegister[0];			

            // 샘플 수에 따른 가속도 계산
			vector = averageSpeedChange / deltaTimeTotal;

			//Vector3 curVelocity = (speedA + speedB) / 2.0f;

			return averageVelocity;		
		}
		else {
			return Vector3.zero;
		}
	}
    ```
