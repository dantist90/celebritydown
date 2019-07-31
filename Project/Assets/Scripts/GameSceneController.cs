// (с) AlexZotov 2016 dantist90@gmail.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour {

	public LocalizationBase LocalizationBase;
	public HaterPlateManager HaterPlateManager;

	public int playerMusicSetting;
	public int playerSoundSetting;
	public int playerLanguageSetting;
	public int playerLevelNumber;

	public int playerScore = 0;
	public bool roundWin;
	public bool roundLose;

	public bool bossTime;

	public GameObject[] HatersPlates;
	public int[] levelsMaxScores;

	public bool pause = false;
	public float modalScreenAlphaLevel = 0.3f;

	public bool setPlayer1 = true;
	public bool setPlayer2 = false;
	public int player1MaxHP = 100;
	public int player1CurrentHP;
	public int player2MaxHP = 50;
	public int player2CurrentHP;
	public bool player1IsDeath = false;
	public bool player2IsDeath = false;
	public int hpRecovery;

	public int MinExpValue;

	public bool setWeapons1 = true;
	public bool setWeapons2 = false;
	public int weapon1MaxReloading = 50;
	public int weapon1CurrentReloading;
	public int weapon2MaxReloading = 100;
	public int weapon2CurrentReloading;
	public int weapon2ReloadingSpeed;

	public int player1Weapon1Damage = 10;
	public int player1Weapon2Damage = 50;
	public int player2Weapon1Damage = 10;
	public int player2Weapon2Damage = 50;

	public Slider scoreSlider;
	public Slider player1Slider;
	public Slider player2Slider;
	public Slider weapon1Slider;
	public Slider weapon2Slider;

	AudioSource ButtonAudio;
	AudioSource AmbientAudio;
	public AudioClip ambient1;
	public AudioClip ambient2;
	public AudioClip ambient3;
	public AudioClip ambient4;
	public AudioClip winMusic1;
	public AudioClip loseMusic1;
	public AudioClip bossMusic1;
	public AudioClip bossMusic2;
	public AudioClip bossMusic3;

	public Sprite player1norm;
	public Sprite player1hit;

	public Sprite player2norm;
	public Sprite player2hit;

	public int lastHaterHitPlayer;
	public int lastHaterTheftExpPlayer;
	public AudioClip laugh1;
	public AudioClip laugh2;
	public AudioClip laugh3;

	public AudioClip heartbeat;

	public Image togglePlayer1Back;
	public Image togglePlayer2Back;
	public Image toggleWeapon1Back;
	public Image toggleWeapon2Back;

	public Sprite toogleBack1;
	public Sprite toogleBack2;
	public Sprite toogleBack3;

	public int kills;
	public int hits;
	public int damage;

	public AudioClip check;
	public AudioClip click;
	public Sprite CheckOn;
	public Sprite CheckOff;

	bool lock1;

	void Awake () {
		LocalizationBase = LocalizationBase.FindObjectOfType<LocalizationBase>();
		HaterPlateManager = HaterPlateManager.FindObjectOfType<HaterPlateManager>();
		ButtonAudio = GetComponent<AudioSource> ();
		AmbientAudio = GameObject.Find ("GameSceneController/AmbientMusic").GetComponent<AudioSource> ();

		playerMusicSetting = PlayerPrefs.GetInt ("PlayerMusicSetting");
		playerSoundSetting = PlayerPrefs.GetInt ("PlayerSoundSetting");
		playerLanguageSetting = PlayerPrefs.GetInt ("PlayerLanguageSetting");
		playerLevelNumber = PlayerPrefs.GetInt ("PlayerLevelNumber");
		if (PlayerPrefs.HasKey ("PlayerScore")) {playerScore = PlayerPrefs.GetInt ("PlayerScore");}

		if (playerScore == 0) {playerScore = playerScore + Mathf.FloorToInt (levelsMaxScores [playerLevelNumber] * 0.15f);}
		if (playerLevelNumber == 0) {
			MinExpValue = 0;
		} else {
			MinExpValue = Mathf.FloorToInt (playerScore * 0.8f);
		}

		UpdateSaves ();
	}
	void Start(){
		kills = 0;
		hits = 0;
		damage = 0;

		// Устанавливаем здоровье игроков
		player1Slider.maxValue = player1MaxHP;
		player2Slider.maxValue = player2MaxHP;
		player1CurrentHP = player1MaxHP;
		player2CurrentHP = player2MaxHP;

		// Устанавливаем перезарядку оружия
		weapon1Slider.maxValue = weapon1MaxReloading;
		weapon2Slider.maxValue = weapon2MaxReloading;
		weapon1CurrentReloading = weapon1MaxReloading;
		weapon2CurrentReloading = weapon2MaxReloading;

		SetPlayer1 ();
		SetWeapons1 ();
		UpdateWeaponsImage ();
		UpdateWeaponsPlate ();
		UpdateText ();
		ModalScreenDisabled ();
		UpdateAmbientMusic ();
		TutorialAlertStart ();
		UpdateSettingWinButton ();
	}

	void Update () {
		BossTutorialAlertStart ();

		scoreSlider.minValue = MinExpValue;
		scoreSlider.maxValue = levelsMaxScores [playerLevelNumber];
		scoreSlider.value = playerScore;
		GameObject.Find ("Canvas/ScoreSlider/Text2").GetComponent<Text> ().text = playerScore + "/" + levelsMaxScores[playerLevelNumber];

		//Завершение по очкам
		if (playerScore >= levelsMaxScores [playerLevelNumber] && !roundWin) {
			if (playerLevelNumber == 0) {
				roundWin = true; // Завершение игры победой
				WinLevelFinish ();
			} else {
				if (!bossTime) {
					bossTime = true;
					HaterPlateManager.ChoicePlate ();
					UpdateAmbientMusic ();
				}
			}
		}
		if (playerScore <= MinExpValue && !roundLose) {
			if (playerLevelNumber >= 1) {
				roundLose = true; // Завершение игры поражением
				WinLevelFinish ();
				SoundLaughHaterExp ();
			}
		}

		// Здоровье игроков
		player1Slider.value = player1CurrentHP;
		player2Slider.value = player2CurrentHP;

		//Смерть игроков
		if (player1CurrentHP <= 0 && !player1IsDeath) {
			player1IsDeath = true;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Image").GetComponent<Image>().color = Color.black;
			if (!player2IsDeath) {
				SetPlayer2 ();
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1").GetComponent<Button> ().interactable = false;
			}
			SoundLaughHater ();
			if (!player2IsDeath) {StartCoroutine ("Player1Resurrection");}

		}
		if (player2CurrentHP <= 0 && !player2IsDeath) {
			player2IsDeath = true;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Image").GetComponent<Image>().color = Color.black;
			if (player1IsDeath == false) {
				SetPlayer1 ();
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2").GetComponent<Button> ().interactable = false;
			}
			SoundLaughHater ();
			if (!player1IsDeath) {StartCoroutine ("Player2Resurrection");}
		}
		if (player1IsDeath && player2IsDeath && !roundLose) {
			WinLevelFinish ();
			roundLose = true;
		}

		// Перезарядка оружия
		weapon1Slider.value = weapon1CurrentReloading;
		weapon2Slider.value = weapon2CurrentReloading;
	}

	IEnumerator Player1Resurrection (){
		HeartbeatSound ();
		yield return new WaitForSeconds(10f);
		if (!pause) {
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<Text> ().text = "5";
		}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<Text> ().text = "4";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<Text> ().text = "3";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<Text> ().text = "2";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<Text> ().text = "1";}
		yield return new WaitForSeconds(1f);
		if (!roundLose || !roundWin) {
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Counter").GetComponent<CanvasGroup> ().alpha = 0;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Image").GetComponent<Image> ().color = Color.white;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1").GetComponent<Button> ().interactable = true;
			player1CurrentHP = Mathf.FloorToInt (player1MaxHP * 0.15f);
			player1IsDeath = false;
		}
	}
	IEnumerator Player2Resurrection (){
		HeartbeatSound ();
		yield return new WaitForSeconds(10f);
		if (!pause) {
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<Text> ().text = "5";
		}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<Text> ().text = "4";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<Text> ().text = "3";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<Text> ().text = "2";}
		yield return new WaitForSeconds(1f);
		if (!pause) {GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<Text> ().text = "1";}
		yield return new WaitForSeconds(1f);
		if (!roundLose || !roundWin) {
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Counter").GetComponent<CanvasGroup> ().alpha = 0;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Image").GetComponent<Image>().color = Color.white;
			GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2").GetComponent<Button> ().interactable = true;
			player2CurrentHP = Mathf.FloorToInt (player2MaxHP * 0.15f);
			player2IsDeath = false;
		}
	}

	void OnEnable(){
		InvokeRepeating("HPrec",1f,1f);
		InvokeRepeating("WeaponsReload",1f,1f);
	}

// Игроки
	public void PushSetPlayer1Button(){
		SetPlayer1 ();
		CheckButtonSound ();
	}
	public void SetPlayer1 () {
		setPlayer1 = true;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Background").GetComponent<Image> ().sprite = toogleBack1;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1").GetComponent<Button> ().interactable = false;
		setPlayer2 = false;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Background").GetComponent<Image> ().sprite = toogleBack3;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2").GetComponent<Button> ().interactable = true;

		SetWeapons1 ();
		UpdateWeaponsImage ();
		UpdateWeaponsPlate ();
		UpdateText ();
	}

	public void PushSetPlayer2Button(){
		SetPlayer2 ();
		CheckButtonSound ();
	}
	public void SetPlayer2 () {
		setPlayer1 = false;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Background").GetComponent<Image> ().sprite = toogleBack3;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1").GetComponent<Button> ().interactable = true;
		setPlayer2 = true;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Background").GetComponent<Image> ().sprite = toogleBack1;
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2").GetComponent<Button> ().interactable = false;
		SetWeapons1 ();
		UpdateWeaponsImage ();
		UpdateWeaponsPlate ();
		UpdateText ();
	}
// Регенерация HP
	private void HPrec(){
		if (!pause && !player1IsDeath && player1CurrentHP < player1MaxHP) {
			player1CurrentHP = player1CurrentHP + hpRecovery;}
		if (!pause && !player2IsDeath && player2CurrentHP < player2MaxHP) {
			player2CurrentHP = player2CurrentHP + hpRecovery;}
	}



// Оружие
	public void PushSetWeapons1Button(){
		SetWeapons1 ();
		CheckButtonSound ();
	}
	public void SetWeapons1 () {
		setWeapons1 = true;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Background").GetComponent<Image> ().sprite = toogleBack2;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1").GetComponent<Button> ().interactable = false;
		setWeapons2 = false;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Background").GetComponent<Image> ().sprite = toogleBack3;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2").GetComponent<Button> ().interactable = true;
	}
	public void PushSetWeapons2Button(){
		SetWeapons2 ();
		CheckButtonSound ();
	}
	public void SetWeapons2 () {
		setWeapons1 = false;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Background").GetComponent<Image> ().sprite = toogleBack3;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1").GetComponent<Button> ().interactable = true;
		setWeapons2 = true;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Background").GetComponent<Image> ().sprite = toogleBack2;
		GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2").GetComponent<Button> ().interactable = false;
	}
	void UpdateWeaponsImage(){
		if (setPlayer1) {
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Image1").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Image2").GetComponent<CanvasGroup> ().alpha = 0;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Image1").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Image2").GetComponent<CanvasGroup> ().alpha = 0;
		} else {
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Image1").GetComponent<CanvasGroup> ().alpha =0;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Image2").GetComponent<CanvasGroup> ().alpha =1;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Image1").GetComponent<CanvasGroup> ().alpha =0;
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Image2").GetComponent<CanvasGroup> ().alpha =1;
		}
	}
	void UpdateWeaponsPlate(){
		GameObject toggleWeapon2 = GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2");
		GameObject sliderWeapon2 = GameObject.Find ("Canvas/ToggleGroupWeapons/SliderWeapon2");

		if ((setPlayer1 && playerLevelNumber < 1) || (setPlayer2 && playerLevelNumber < 2)) {
			toggleWeapon2.GetComponent<CanvasGroup> ().alpha = 0;
			toggleWeapon2.GetComponent<CanvasGroup> ().interactable = false;
			toggleWeapon2.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			sliderWeapon2.GetComponent<CanvasGroup> ().alpha = 0;
		} else {
			toggleWeapon2.GetComponent<CanvasGroup> ().alpha = 1;
			toggleWeapon2.GetComponent<CanvasGroup> ().interactable = true;
			toggleWeapon2.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			sliderWeapon2.GetComponent<CanvasGroup> ().alpha = 1;
		}
	}
	private void WeaponsReload(){
		if (!pause && weapon2CurrentReloading < weapon2MaxReloading) {
			weapon2CurrentReloading = weapon2CurrentReloading + weapon2ReloadingSpeed;}
	}

// Текст
	void UpdateText(){
		// Опыт игрока
		GameObject.Find ("Canvas/ScoreSlider/Text1").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [9];

		// Имена игроков
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [10];
		GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [11];
		// Имена оружия
		if (setPlayer1){
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [12];
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [13];
		}
		if (setPlayer2){
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon1/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [14];
			GameObject.Find ("Canvas/ToggleGroupWeapons/ToggleWeapon2/Name").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [15];
		}
		GameObject.Find ("Canvas/ModalScreenImage/PauseText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [44];
		GameObject.Find ("Canvas/FinishWindow/WinStats/StatText1").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [45];
		GameObject.Find ("Canvas/FinishWindow/WinStats/StatText2").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [46];
		GameObject.Find ("Canvas/FinishWindow/WinStats/StatText3").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [47];
		GameObject.Find ("Canvas/FinishWindow/WinStats/StatText4").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [48];
		GameObject.Find ("Canvas/FinishWindow/LoseText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [49];
		GameObject.Find ("Canvas/FinishWindow/Button/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];

		GameObject.Find ("Canvas/TutorialAlert1/Button/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];
		GameObject.Find ("Canvas/TutorialAlert1/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [51];
		GameObject.Find ("Canvas/TutorialAlert1/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [52];

		GameObject.Find ("Canvas/TutorialAlert2/Button/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];
		GameObject.Find ("Canvas/TutorialAlert2/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [53];
		GameObject.Find ("Canvas/TutorialAlert2/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [54];

		GameObject.Find ("Canvas/TutorialAlert3/Button/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];
		GameObject.Find ("Canvas/TutorialAlert3/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [53];
		GameObject.Find ("Canvas/TutorialAlert3/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [55];

		GameObject.Find ("Canvas/TutorialBossAlert/Button/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];
		GameObject.Find ("Canvas/TutorialBossAlert/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [56];
		GameObject.Find ("Canvas/TutorialBossAlert/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [57];

		GameObject.Find ("Canvas/SettingAlert/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [41];
		GameObject.Find ("Canvas/SettingAlert/Button1/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [58];
		GameObject.Find ("Canvas/SettingAlert/Button2/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [50];
		GameObject.Find ("Canvas/SettingAlert/MusicButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [42];
		GameObject.Find ("Canvas/SettingAlert/SoundButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [43];
		GameObject.Find ("Canvas/SettingAlert/SoundButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [43];
		GameObject.Find ("Canvas/SettingAlert/Lang0ButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [0] [0];
		GameObject.Find ("Canvas/SettingAlert/Lang1ButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [1] [0];

	}
// Звуки и музыка
	void UpdateAmbientMusic (){
		if (playerMusicSetting == 1) {
			AmbientAudio.Stop();
			if (!bossTime) {
				if (playerLevelNumber == 0) {AmbientAudio.clip = ambient1;}
				if (playerLevelNumber == 1) {AmbientAudio.clip = ambient2;}
				if (playerLevelNumber == 2) {AmbientAudio.clip = ambient3;}
				if (playerLevelNumber == 3) {AmbientAudio.clip = ambient2;}
				if (playerLevelNumber == 4) {AmbientAudio.clip = ambient4;}
			} else {
				if (playerLevelNumber == 1) {AmbientAudio.clip = bossMusic1;}
				if (playerLevelNumber == 2) {AmbientAudio.clip = bossMusic2;}
				if (playerLevelNumber == 3) {AmbientAudio.clip = bossMusic3;}
				if (playerLevelNumber == 4) {AmbientAudio.clip = bossMusic1;}
			}
			AmbientAudio.Play ();
		}
	}
	void UpdateAmbientMusicWin(){
		if (playerMusicSetting == 1) {
			AmbientAudio.Stop();
			AmbientAudio.clip = winMusic1;
			AmbientAudio.Play();
		}
	}
	void UpdateAmbientMusicLose(){
		if (playerMusicSetting == 1) {
			AmbientAudio.Stop ();
			AmbientAudio.clip = loseMusic1;
			AmbientAudio.Play ();
		}
	}

	void SoundLaughHater(){
		if (playerSoundSetting == 1) {
			if (lastHaterHitPlayer == 0) {ButtonAudio.PlayOneShot (laugh1, 1f);}
			if (lastHaterHitPlayer == 1) {ButtonAudio.PlayOneShot (laugh2, 1f);}
			if (lastHaterHitPlayer == 2) {ButtonAudio.PlayOneShot (laugh3, 1f);}
		}
	}
	void SoundLaughHaterExp(){
		if (playerSoundSetting == 1) {
			if (lastHaterTheftExpPlayer == 0) {ButtonAudio.PlayOneShot (laugh1, 1f);}
			if (lastHaterTheftExpPlayer == 1) {ButtonAudio.PlayOneShot (laugh2, 1f);}
			if (lastHaterTheftExpPlayer == 2) {ButtonAudio.PlayOneShot (laugh3, 1f);}
		}
	}
	void HeartbeatSound(){
		if (playerSoundSetting == 1) {
			ButtonAudio.PlayOneShot (heartbeat, 1f);
		}
	}
	void CheckButtonSound(){
		if (playerSoundSetting == 1) {ButtonAudio.PlayOneShot (check, 0.5f);}
	}
	void CheckMusicButtonSound(){
		ButtonAudio.PlayOneShot (check, 0.5f);
	}
	void ClickButtonSound(){
		if (playerMusicSetting == 1) {ButtonAudio.PlayOneShot (click, 0.5f);}
	}

	void MusicMute(){
		if (playerMusicSetting == 1) {
			AmbientAudio.mute = false;
		} else {
			AmbientAudio.mute = true;
		}
	}

// Шторка ModalScreen
	void ModalScreenEnabled(){
		GameObject modalScreenImage = GameObject.Find ("Canvas/ModalScreenImage");
		pause = true;
		modalScreenImage.GetComponent<CanvasGroup> ().alpha = modalScreenAlphaLevel;
		modalScreenImage.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		modalScreenImage.GetComponent<Image> ().raycastTarget = true;
	}
	void ModalScreenDisabled(){
		GameObject modalScreenImage = GameObject.Find ("Canvas/ModalScreenImage");
		pause = false;
		modalScreenImage.GetComponent<CanvasGroup> ().alpha = 0;
		modalScreenImage.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		modalScreenImage.GetComponent<Image> ().raycastTarget = false;
	}


// Алерты туториала
	void TutorialAlertStart(){
		if (playerLevelNumber == 0) {
			ModalScreenEnabled ();
			GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().interactable = true;
			GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
		if (playerLevelNumber == 1) {
			ModalScreenEnabled ();
			GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().interactable = true;
			GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
		if (playerLevelNumber == 2) {
			ModalScreenEnabled ();
			GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().interactable = true;
			GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
	}
	void BossTutorialAlertStart (){
		if (playerLevelNumber == 1 && bossTime && !lock1) {
			lock1 = true;
			ModalScreenEnabled ();
			GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().interactable = true;
			GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
	}

	public void CloseTutorialAlert1(){
		ModalScreenDisabled ();
		GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().interactable = false;
		GameObject.Find ("Canvas/TutorialAlert1").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		ClickButtonSound();
	}
	public void CloseTutorialAlert2(){
		ModalScreenDisabled ();
		GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().interactable = false;
		GameObject.Find ("Canvas/TutorialAlert2").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		ClickButtonSound();
	}
	public void CloseTutorialAlert3(){
		ModalScreenDisabled ();
		GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().interactable = false;
		GameObject.Find ("Canvas/TutorialAlert3").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		ClickButtonSound();
	}
	public void CloseTutorialBossAlert(){
		ModalScreenDisabled ();
		GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().interactable = false;
		GameObject.Find ("Canvas/TutorialBossAlert").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		ClickButtonSound();
	}

// Окно настроек
	public void PushSettingsAlert(){
		ModalScreenEnabled ();
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().interactable = true;
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		ClickButtonSound();
	}
	public void PushStartOverButton(){
		ClickButtonSound();
		SceneManager.LoadScene("CutScene");
	}
	public void PushSaveSettingButton(){
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().alpha = 0;
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().interactable = false;
		GameObject.Find ("Canvas/SettingAlert").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		ModalScreenDisabled ();
		ClickButtonSound();
		PlayerPrefs.SetInt("PlayerMusicSetting", playerMusicSetting);
		PlayerPrefs.SetInt("PlayerSoundSetting", playerSoundSetting);
		PlayerPrefs.SetInt("PlayerLanguageSetting", playerLanguageSetting);
	}

	public void PushMusicButtonInSettings(){
		CheckButtonSound();
		if (playerMusicSetting == 0) {
			playerMusicSetting = 1;
		} else {
			playerMusicSetting = 0;
		}
		MusicMute ();
		UpdateSettingWinButton ();
	}
	public void PushSoundButtonInSettings(){
		CheckMusicButtonSound();
		if (playerSoundSetting == 0) {
			playerSoundSetting = 1;
		} else {
			playerSoundSetting = 0;
		}
		UpdateSettingWinButton ();
	}
	public void PushLang0ButtonInSettings(){
		CheckButtonSound();
		playerLanguageSetting = 0;
		UpdateSettingWinButton ();
		UpdateText ();
	}
	public void PushLang1ButtonInSettings(){
		CheckButtonSound();
		playerLanguageSetting = 1;
		UpdateSettingWinButton ();
		UpdateText ();
	}

	void UpdateSettingWinButton(){
		if (playerMusicSetting == 1) {
			GameObject.Find ("Canvas/SettingAlert/MusicButton").GetComponent<Image> ().sprite = CheckOn;
		} else {
			GameObject.Find ("Canvas/SettingAlert/MusicButton").GetComponent<Image> ().sprite = CheckOff;
		}
		if (playerSoundSetting == 1) {
			GameObject.Find ("Canvas/SettingAlert/SoundButton").GetComponent<Image> ().sprite = CheckOn;
		} else {
			GameObject.Find ("Canvas/SettingAlert/SoundButton").GetComponent<Image> ().sprite = CheckOff;
		}
		Color msb1 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 120.0f/255.0f);
		Color msb2 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
		GameObject.Find ("Canvas/SettingAlert/Lang0Button").GetComponent<Image> ().color = msb1;
		GameObject.Find ("Canvas/SettingAlert/Lang1Button").GetComponent<Image> ().color = msb1;

		if (playerLanguageSetting == 0) {
			GameObject.Find ("Canvas/SettingAlert/Lang0Button").GetComponent<Image> ().color = msb2;
		} else {
			GameObject.Find ("Canvas/SettingAlert/Lang1Button").GetComponent<Image> ().color = msb2;
		}
	}

// Завершение раунда
	public void WinLevelFinish (){
		ModalScreenEnabled ();

		GameObject.Find ("Canvas/FinishWindow").GetComponent<CanvasGroup> ().alpha = 1;
		GameObject.Find ("Canvas/FinishWindow").GetComponent<CanvasGroup> ().interactable = true;
		GameObject.Find ("Canvas/FinishWindow").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (roundWin) {
			GameObject.Find ("Canvas/FinishWindow/LoseText").GetComponent<CanvasGroup> ().alpha = 0;
			GameObject.Find ("Canvas/FinishWindow/WinStats").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/FinishWindow/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [39];

			int startScoreCount;
			if (PlayerPrefs.HasKey ("PlayerScore")) {startScoreCount = PlayerPrefs.GetInt ("PlayerScore");
			} else {startScoreCount = 0;}
			int totalScore = playerScore - startScoreCount;
			GameObject.Find ("Canvas/FinishWindow/WinStats/StatCountText1").GetComponent<Text> ().text = totalScore.ToString();

			GameObject.Find ("Canvas/FinishWindow/WinStats/StatCountText2").GetComponent<Text> ().text = kills.ToString();
			GameObject.Find ("Canvas/FinishWindow/WinStats/StatCountText3").GetComponent<Text> ().text = hits.ToString();
			GameObject.Find ("Canvas/FinishWindow/WinStats/StatCountText4").GetComponent<Text> ().text = damage.ToString();

			PlayerPrefs.SetInt ("PlayerLevelNumber", playerLevelNumber + 1);
			PlayerPrefs.SetInt ("PlayerScore", playerScore);
			UpdateAmbientMusicWin ();
		} else {
			UpdateAmbientMusicLose ();
			GameObject.Find ("Canvas/FinishWindow/LoseText").GetComponent<CanvasGroup> ().alpha = 1;
			GameObject.Find ("Canvas/FinishWindow/WinStats").GetComponent<CanvasGroup> ().alpha = 0;
			GameObject.Find ("Canvas/FinishWindow/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [40];
		}
	}
	public void ConfurmContinueGame(){
		SceneManager.LoadScene("CutScene");
	}

	void UpdateSaves(){
		if (PlayerPrefs.HasKey ("PlayerMusicSetting")) {
			playerMusicSetting = PlayerPrefs.GetInt ("PlayerMusicSetting");
		} else {
			playerMusicSetting = 1;
		}
		if (PlayerPrefs.HasKey ("PlayerSoundSetting")) {
			playerSoundSetting = PlayerPrefs.GetInt ("PlayerSoundSetting");
		} else {
			playerSoundSetting = 1;
		}
		if (PlayerPrefs.HasKey ("PlayerLanguageSetting")) {
			playerLanguageSetting = PlayerPrefs.GetInt ("PlayerLanguageSetting");
		} else {
			playerLanguageSetting = 1;
		}
		if (PlayerPrefs.HasKey ("PlayerLevelNumber")) {
			playerLevelNumber = PlayerPrefs.GetInt ("PlayerLevelNumber");
		} else {
			playerLevelNumber = 0;
		}

		Debug.Log("PlayerMusicSetting " + playerMusicSetting);
		Debug.Log("PlayerSoundSetting " + playerSoundSetting);
		Debug.Log("PlayerLanguageSetting " + playerLanguageSetting);
		Debug.Log("PlayerLevelNumber " + playerLevelNumber);
	}

}
