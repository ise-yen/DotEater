using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janken : MonoBehaviour
{
    bool flagJanken = false; // 묵찌빠 시작 플래그
	int modeJanken = 0;

	public AudioClip voiceStart;
	public AudioClip voicePon;
	public AudioClip voiceGoo;
	public AudioClip voicePar;
	public AudioClip voiceChoki;
	public AudioClip voiceWin;
	public AudioClip voiceDraw;
	public AudioClip voiceLoose;

	const int JANKEN = -1;
	const int GOO = 0;
	const int CHOKI = 1;
	const int PAR = 2;
	const int DRAW = 3;
	const int WIN = 4;
	const int LOOSE = 5;

	private Animator animator;
	private AudioSource univoice;

	int myHand;
	int unityHand;
	int flagResult;
	int[,] tableResult = new int[3, 3];

	float waitDelay;

	public GUIStyle guiBtnGame;
	public GUIStyle guiBtnGoo;
	public GUIStyle guiBtnPar;
	public GUIStyle guiBtnChoki;

	private Rect rtBtnGame = new Rect();
	private Rect rtBtnGoo = new Rect();
	private Rect rtBtnPar = new Rect();
	private Rect rtBtnChoki = new Rect();


	private void OnGUI()
	{
		// GUI 크기: 16:9의 1280 x 720 해상도 기준
		const float guiScreen = 1280;
		const float guiPadding= 10; // 10 픽셀 간격
		const float guiButton = 200; // 200x200 픽셀의 버튼들
		const float guiTop = 720 - guiButton - guiPadding; // 버튼 높이

		// 현재 화면과의 비율
		float gui_scale = Screen.width / guiScreen;
		float scaledPadding = guiPadding * gui_scale;
		float scaledButton= guiButton * gui_scale;
		float scaledTop = guiTop * gui_scale;

		// 버튼들 위치 계산
		rtBtnGame.x = scaledPadding;
		rtBtnGame.y = scaledTop;
		rtBtnGame.width = scaledButton;
		rtBtnGame.height = scaledButton;

		float left = (guiScreen - guiPadding * 2 - guiButton * 3) / 2 * gui_scale;
		rtBtnGoo.x = left;
		Debug.Log("묵.x: " + left);
		rtBtnGoo.y = scaledTop;
		rtBtnGoo.width = scaledButton;
		rtBtnGoo.height = scaledButton;

		left += scaledButton + scaledPadding;
		Debug.Log("빠.x: " + left);
		rtBtnPar.x = left;
		rtBtnPar.y = scaledTop;
		rtBtnPar.width = scaledButton;
		rtBtnPar.height = scaledButton;

		left += scaledButton + scaledPadding;
		Debug.Log("찌.x: " + left);
		rtBtnChoki.x = left;
		rtBtnChoki.y = scaledTop;
		rtBtnChoki.width = scaledButton;
		rtBtnChoki.height = scaledButton;


		// 묵찌빠가 아니면
		if (!flagJanken)
		{
			// UI에 게임 버튼 추가
			flagJanken = (GUI.Button(rtBtnGame, "묵찌빠", guiBtnGame));
		}

		// 묵찌빠 모드
		if (modeJanken == 1)
		{
			// UI 게임 버튼 추가
			if(GUI.Button(rtBtnGoo, "목", guiBtnGoo))
			{
				myHand = GOO;
				modeJanken++;
			}
			if(GUI.Button(rtBtnChoki, "찌", guiBtnChoki))
			{
				myHand = CHOKI;
				modeJanken++;
			}
			if (GUI.Button(rtBtnPar, "빠", guiBtnPar))
			{
				myHand = PAR;
				modeJanken++;
			}
		}
	}

	private void Start()
    {
		animator = GetComponent<Animator>();
		univoice = GetComponent<AudioSource>();

		// 결과 테이블 미리 결정 [유니티짱, 플레이어]
		tableResult[GOO, GOO] = DRAW;
		tableResult[GOO, CHOKI] = WIN;
		tableResult[GOO, PAR] = LOOSE;
		tableResult[CHOKI, GOO] = LOOSE;
		tableResult[CHOKI, CHOKI] = DRAW;
		tableResult[CHOKI, PAR] = WIN;
		tableResult[PAR, GOO] = WIN;
		tableResult[PAR, CHOKI] = LOOSE;
		tableResult[PAR, PAR] = DRAW;
    }

	// 유니티짱의 액션
	void UnityChanAction(int act) // 이벤트 함수
	{
		switch (act)
		{
			case JANKEN:
				animator.SetBool("Janken", true);
				univoice.clip = voiceStart;
				break;
			
			case GOO:
				animator.SetBool("Goo", true);
				univoice.clip = voiceGoo;
				break;

			case CHOKI:
				animator.SetBool("Choki", true);
				univoice.clip = voiceChoki;
				break;

			case PAR:
				animator.SetBool("Par", true);
				univoice.clip = voicePar;
				break;

			case DRAW:
				animator.SetBool("Aiko", true);
				univoice.clip = voiceDraw;
				break;

			case WIN:
				animator.SetBool("Win", true);
				univoice.clip = voiceWin;
				break;

			case LOOSE:
				animator.SetBool("Loose", true);
				univoice.clip = voiceLoose;
				break;

		}
		univoice.Play();

	}

    private void Update()
    {
		// 묵찌빠 상태라면
		if (flagJanken)
		{
			// 게임 모드에 따라
			switch (modeJanken)
			{
				case 0: // 묵찌빠 시작
					UnityChanAction(JANKEN);
					modeJanken++;
					break;

				case 1: // 플레이어 입력 대기
					// 애니메이션 초기화
					animator.SetBool("Janken", false);
					animator.SetBool("Goo", false);
					animator.SetBool("Choki", false);
					animator.SetBool("Par", false);
					animator.SetBool("Aiko", false);
					animator.SetBool("Win", false);
					animator.SetBool("Loose", false);
					break;

				case 2: // 판정
					flagResult = JANKEN;
					// 유니티짱의 손을 무작위로 선택
					unityHand = Random.Range(GOO, PAR + 1);
					// 유니티짱 액션
					UnityChanAction(unityHand);
					// 결과
					flagResult = tableResult[unityHand, myHand];
					modeJanken++;
					break;

				case 3: // 결과
					// 약간의 시간 간격
					waitDelay += Time.deltaTime;
					if(waitDelay > 1.5f)
					{
						// 유니티짱 결과 액션
						UnityChanAction(flagResult);

						waitDelay = 0;
						modeJanken++;
					}
					break;

				default: // 초기화
					flagJanken = false;
					modeJanken = 0;
					break;
			}
		}
    }
}
