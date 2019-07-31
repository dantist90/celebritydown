// (с) AlexZotov 2016 dantist90@gmail.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class HaterPlate : MonoBehaviour {

	public LocalizationBase LocalizationBase;
	public GameSceneController GameSceneController;
	public HaterPlateManager HaterPlateManager;

	public int haterType;
	public int haterSex;

	public bool IsCelebrity;
	public bool IsBoss;

	public Image haterImage;
	public Text haterName;
	public Text haterSay;
	public Image haterPlate;
	public Sprite plate1;
	public Sprite plate2;
	public Image haterStar;
	public Text text3;

	public bool haterIsDeath;
	public int haterMaxExp;
	public int haterCurrentExp;
	public int haterExpTheft;
	public int haterMaxHp;
	public int haterCurrentHp;
	public int haterDamage;
	public int HPrecovery;

	public Slider haterExpSlider;
	public Slider haterHpSlider;

	AudioSource HaterAudio;
	public AudioClip sream1;
	public AudioClip sream2;
	public AudioClip sream3;

	public AudioClip hit1;
	public AudioClip hit2;
	public AudioClip hit3;
	public AudioClip hit4;
	public AudioClip hit5;
	public AudioClip hit6;
	public AudioClip hit7;
	public AudioClip hit8;
	public AudioClip hit9;
	public AudioClip hit10;
	public AudioClip hit11;
	public AudioClip hit12;
	public AudioClip hit13;

	public Sprite hater0norm;
	public Sprite hater0hit;
	public Sprite hater1norm;
	public Sprite hater1hit;
	public Sprite hater2norm;
	public Sprite hater2hit;
	public Sprite boss1norm;
	public Sprite boss1hit;
	public Sprite boss2norm;
	public Sprite boss2hit;
	public Sprite boss3norm;
	public Sprite boss3hit;
	public Sprite boss4norm;
	public Sprite boss4hit;

	void Awake () {
		LocalizationBase = LocalizationBase.FindObjectOfType<LocalizationBase> ();
		GameSceneController = GameSceneController.FindObjectOfType<GameSceneController> ();
		HaterPlateManager = HaterPlateManager.FindObjectOfType<HaterPlateManager> ();

		HaterAudio = GetComponent<AudioSource>();
		HaterSetType ();
		BusterHater ();
		if (!IsBoss) {
			haterCurrentExp = Random.Range (Mathf.FloorToInt (haterMaxExp * 0.15f), Mathf.FloorToInt (haterMaxExp * 0.85f));
			haterCurrentHp = Random.Range (Mathf.FloorToInt (haterMaxHp * 0.3f), Mathf.FloorToInt (haterMaxHp * 1f));
		}
		else {
			SetSelebrity();
		}
		haterExpSlider.maxValue = haterMaxExp;
		haterHpSlider.maxValue = haterMaxHp;
		UpdateHaterFraze ();
		UpdateHaterName ();
		UpdateText ();
	}
	void BusterHater(){
		if (GameSceneController.playerLevelNumber == 1) {
			haterDamage = Mathf.FloorToInt (haterDamage * 1.1f);
			haterExpTheft = Mathf.FloorToInt (haterExpTheft * 1.1f);
			HPrecovery = Mathf.FloorToInt (HPrecovery * 1.1f);
		}
		if (GameSceneController.playerLevelNumber == 2) {
			haterDamage = Mathf.FloorToInt (haterDamage * 1.2f);
			haterExpTheft = Mathf.FloorToInt (haterExpTheft * 1.25f);
			HPrecovery = Mathf.FloorToInt (HPrecovery * 1.25f);
			HaterPlateManager.maxPlatesInScreen = 3;
			HaterPlateManager.repeatingPlate = 4f;
		}
		if (GameSceneController.playerLevelNumber == 3) {
			haterDamage = Mathf.FloorToInt (haterDamage * 1.3f);
			haterExpTheft = Mathf.FloorToInt (haterExpTheft * 1.2f);
			HPrecovery = Mathf.FloorToInt (HPrecovery * 1.3f);
			HaterPlateManager.maxPlatesInScreen = 4;
			HaterPlateManager.repeatingPlate = 5f;
		}
		if (GameSceneController.playerLevelNumber == 4) {
			haterDamage = Mathf.FloorToInt (haterDamage * 1.3f);
			haterExpTheft = Mathf.FloorToInt (haterExpTheft * 1.2f);
			HPrecovery = Mathf.FloorToInt (HPrecovery * 1.4f);
			HaterPlateManager.maxPlatesInScreen = 4;
			HaterPlateManager.repeatingPlate = 5f;
		}
		if (IsBoss) {
			haterMaxHp = Mathf.FloorToInt (haterMaxHp * 3f);
			haterMaxExp = Mathf.FloorToInt (haterMaxExp * 1.5f);
		}
	}

	void Update () {
		haterExpSlider.value = haterCurrentExp;
		haterHpSlider.value = haterCurrentHp;
	}

	void SetSelebrity(){
		haterCurrentHp = haterMaxHp;
		haterCurrentExp = haterMaxExp;
		CelebrytyLaugh ();
		UpdateCelebryty ();
	}

	void UpdateCelebryty(){
		if (IsCelebrity) {
			haterPlate.GetComponent<Image> ().sprite = plate2;
			text3.GetComponent<CanvasGroup> ().alpha = 1f;
			haterStar.GetComponent<CanvasGroup> ().alpha = 1f;
		} else {
			haterPlate.GetComponent<Image> ().sprite = plate1;
			text3.GetComponent<CanvasGroup> ().alpha = 0f;
			haterStar.GetComponent<CanvasGroup> ().alpha = 0f;
		}
	}

	void CelebrytyLaugh(){
		if (haterType == 0) {HaterAudio.PlayOneShot (GameSceneController.laugh1, 1f);}
		if (haterType == 1) {HaterAudio.PlayOneShot (GameSceneController.laugh2, 1f);}
		if (haterType == 2) {HaterAudio.PlayOneShot (GameSceneController.laugh3, 1f);}
	}

// Говорит фразу
	void OnEnable(){
		InvokeRepeating("HPrec",1f,1f);
		InvokeRepeating("ExpTheft",Random.Range (5f,25f),Random.Range (5f,25f));}
	// Регенерация HP
	private void HPrec(){
		if (!GameSceneController.pause && haterIsDeath == false && haterCurrentHp < haterMaxHp) {
			haterCurrentHp = haterCurrentHp + HPrecovery;}
	}
	// Кража опыта
	private void ExpTheft(){
		if (!GameSceneController.pause && !haterIsDeath) {
				GameSceneController.playerScore = GameSceneController.playerScore - haterExpTheft;
				haterCurrentExp = haterCurrentExp + haterExpTheft;
			if (haterCurrentExp > haterMaxExp) {
				haterCurrentExp = haterMaxExp;
			}
			if (!IsCelebrity && haterCurrentExp >= haterMaxExp && GameSceneController.playerLevelNumber >= 2) {
				IsCelebrity = true;
				SetSelebrity ();
			}
			UpdateHaterFraze ();
			GameSceneController.lastHaterTheftExpPlayer = haterType;
		}
	}

// Нажатие на плашку
	public void pushHaterPlate() {
		// Наносим урон хейтеру
		if (GameSceneController.setPlayer1){
			if (GameSceneController.setWeapons1 && !IsCelebrity) {
				haterCurrentHp = haterCurrentHp - GameSceneController.player1Weapon1Damage;
				HaterFeedBack();
				AudioHitHater ();
				++GameSceneController.hits;
				GameSceneController.damage = GameSceneController.damage + GameSceneController.player1Weapon1Damage;
			}
			if (GameSceneController.setWeapons2 && GameSceneController.weapon2CurrentReloading >= GameSceneController.weapon2MaxReloading) {
				GameSceneController.weapon2CurrentReloading = 0;
				haterCurrentHp = haterCurrentHp - GameSceneController.player1Weapon2Damage;
				if (IsCelebrity) {
					haterCurrentExp = Mathf.FloorToInt (haterCurrentExp * 0.80f);
					IsCelebrity = false;
					UpdateCelebryty ();
				}
				HaterFeedBack();
				AudioHitHater ();
				++GameSceneController.hits;
				GameSceneController.damage = GameSceneController.damage + GameSceneController.player1Weapon2Damage;
			}
		}
		if (GameSceneController.setPlayer2){
			if (GameSceneController.setWeapons1 && !IsCelebrity) {
				haterCurrentExp = haterCurrentExp - GameSceneController.player2Weapon1Damage;
				GameSceneController.playerScore = GameSceneController.playerScore + GameSceneController.player2Weapon1Damage;
				if (GameSceneController.playerScore > GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber]) {
					GameSceneController.playerScore = GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber];
				}
				HaterFeedBack();
				AudioHitHater ();
				++GameSceneController.hits;
				GameSceneController.damage = GameSceneController.damage + GameSceneController.player2Weapon1Damage;
				if (IsCelebrity) {
					IsCelebrity = false;
					UpdateCelebryty ();
				}
			}
			if (GameSceneController.setWeapons2 && GameSceneController.weapon2CurrentReloading >= GameSceneController.weapon2MaxReloading) {
				GameSceneController.weapon2CurrentReloading = 0;
				haterCurrentExp = haterCurrentExp - GameSceneController.player2Weapon2Damage;
				GameSceneController.playerScore = GameSceneController.playerScore + GameSceneController.player2Weapon2Damage;
				if (GameSceneController.playerScore > GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber]) {
					GameSceneController.playerScore = GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber];
				}
				if (IsCelebrity) {
					haterCurrentHp = Mathf.FloorToInt (haterCurrentHp * 0.80f);
					IsCelebrity = false;
					UpdateCelebryty ();
				}
				HaterFeedBack();
				AudioHitHater ();
				++GameSceneController.hits;
				GameSceneController.damage = GameSceneController.damage + GameSceneController.player2Weapon2Damage;
			}
		}
	}
	void HaterFeedBack(){
		// Проверяем, не сдох ли хейтер
		if (haterCurrentExp <= 0 || haterCurrentHp <= 0) {
			haterIsDeath = true;}
		if (!haterIsDeath) {
			StartCoroutine ("HitPlayer");
			StartCoroutine ("AlphaPlate");
		} else {
			StartCoroutine ("HaterDeath");
		}
	}
	void UpdateHaterFraze(){
		int dice = Random.Range (0,14);
		haterSay.text = LocalizationBase.LocalHaterWordsArray [GameSceneController.playerLanguageSetting] [dice];
	}
	void HaterSetType(){
		if (!GameSceneController.bossTime) {
			int dice = Random.Range (0, 3);

			if (dice == 0) {
				haterType = 0;
				haterSex = 0;
				haterImage.GetComponent<Image> ().sprite = hater0norm;
			}
			if (dice == 1) {
				haterType = 1;
				haterSex = 0;
				haterImage.GetComponent<Image> ().sprite = hater1norm;
			}
			if (dice == 2) {
				haterType = 2;
				haterSex = 1;
				haterImage.GetComponent<Image> ().sprite = hater2norm;
			}
		} else {
			if (GameSceneController.playerLevelNumber == 1) {
				haterType = 1;
				haterSex = 0;
				haterImage.GetComponent<Image> ().sprite = boss1norm;
			}
			if (GameSceneController.playerLevelNumber == 2) {
				haterType = 2;
				haterSex = 1;
				haterImage.GetComponent<Image> ().sprite = boss2norm;
			}
			if (GameSceneController.playerLevelNumber == 3) {
				haterType = 1;
				haterSex = 0;
				haterImage.GetComponent<Image> ().sprite = boss3norm;
			}
			if (GameSceneController.playerLevelNumber == 4) {
				haterType = 1;
				haterSex = 0;
				haterImage.GetComponent<Image> ().sprite = boss4norm;
			}
			IsCelebrity = true;
			IsBoss = true;
			//SetSelebrity ();
		}
	}

	IEnumerator HitPlayer() {
		int dice = Random.Range (0,2);

		yield return new WaitForSeconds(0.2f);
		if (dice == 0 && haterIsDeath == false) {
			if (GameSceneController.setPlayer1) {
				GameSceneController.player1CurrentHP = GameSceneController.player1CurrentHP - haterDamage;
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Image").GetComponent<Image> ().sprite = GameSceneController.player1hit;
				yield return new WaitForSeconds(0.5f);
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer1/Image").GetComponent<Image> ().sprite = GameSceneController.player1norm;
			} else {
				GameSceneController.player2CurrentHP = GameSceneController.player2CurrentHP - haterDamage;
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Image").GetComponent<Image> ().sprite = GameSceneController.player2hit;
				yield return new WaitForSeconds(0.5f);
				GameObject.Find ("Canvas/ToggleGroupPlayers/TogglePlayer2/Image").GetComponent<Image> ().sprite = GameSceneController.player2norm;
			}
			GameSceneController.lastHaterHitPlayer = haterType;
		}
	}
	IEnumerator AlphaPlate() {
		if (haterType == 0) {haterImage.GetComponent<Image> ().sprite = hater0hit;}
		if (haterType == 1) {haterImage.GetComponent<Image> ().sprite = hater1hit;}
		if (haterType == 2) {haterImage.GetComponent<Image> ().sprite = hater2hit;}
		if (IsBoss) {
			if (GameSceneController.playerLevelNumber == 1) {
				haterImage.GetComponent<Image> ().sprite = boss1hit;
			}
			if (GameSceneController.playerLevelNumber == 2) {
				haterImage.GetComponent<Image> ().sprite = boss2hit;
			}
			if (GameSceneController.playerLevelNumber == 3) {
				haterImage.GetComponent<Image> ().sprite = boss3hit;
			}
			if (GameSceneController.playerLevelNumber == 4) {
				haterImage.GetComponent<Image> ().sprite = boss4hit;
			}
		}

		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 0.75f;}
		yield return new WaitForSeconds(0.025f);
		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 0.8f;}
		yield return new WaitForSeconds(0.025f);
		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 0.85f;}
		yield return new WaitForSeconds(0.025f);
		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 0.9f;}
		yield return new WaitForSeconds(0.025f);
		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 0.95f;}
		yield return new WaitForSeconds(0.025f);
		if (!haterIsDeath) {this.gameObject.GetComponent<CanvasGroup> ().alpha = 1f;}
		yield return new WaitForSeconds(0.3f);
		if (haterType == 0) {haterImage.GetComponent<Image> ().sprite = hater0norm;}
		if (haterType == 1) {haterImage.GetComponent<Image> ().sprite = hater1norm;}
		if (haterType == 2) {haterImage.GetComponent<Image> ().sprite = hater2norm;}
		if (IsBoss) {
			if (GameSceneController.playerLevelNumber == 1) {
				haterImage.GetComponent<Image> ().sprite = boss1norm;
			}
			if (GameSceneController.playerLevelNumber == 2) {
				haterImage.GetComponent<Image> ().sprite = boss2norm;
			}
			if (GameSceneController.playerLevelNumber == 3) {
				haterImage.GetComponent<Image> ().sprite = boss3norm;
			}
			if (GameSceneController.playerLevelNumber == 4) {
				haterImage.GetComponent<Image> ().sprite = boss4norm;
			}
		}
	}
	IEnumerator HaterDeath (){
		++GameSceneController.kills;

		if (haterCurrentExp > 0) {GameSceneController.playerScore = GameSceneController.playerScore + haterCurrentExp;}
		if (GameSceneController.playerScore > GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber]) {
			GameSceneController.playerScore = GameSceneController.levelsMaxScores [GameSceneController.playerLevelNumber];
		}
		this.gameObject.GetComponent<CanvasGroup> ().interactable = false;
		this.gameObject.GetComponent<CanvasGroup> ().alpha = 0f;
		if (GameSceneController.playerSoundSetting == 1){
			if (haterType == 0) {HaterAudio.PlayOneShot(sream1, 1f);}
			if (haterType == 1) {HaterAudio.PlayOneShot(sream2, 0.5f);}
			if (haterType == 2) {HaterAudio.PlayOneShot(sream3, 0.5f);}
		}
		--HaterPlateManager.currentPlatesInScreen;
		string haterPlateName = gameObject.name;
		if (haterPlateName == "plate0") {HaterPlateManager.socket0Full = false;}
		if (haterPlateName == "plate1") {HaterPlateManager.socket1Full = false;}
		if (haterPlateName == "plate2") {HaterPlateManager.socket2Full = false;}
		if (haterPlateName == "plate3") {HaterPlateManager.socket3Full = false;}
		if (haterPlateName == "plate4") {HaterPlateManager.socket4Full = false;}
		if (haterPlateName == "plate5") {HaterPlateManager.socket5Full = false;}
		if (haterPlateName == "plate6") {HaterPlateManager.socket6Full = false;}
		if (haterPlateName == "plate7") {HaterPlateManager.socket7Full = false;}
		if (haterPlateName == "plate8") {HaterPlateManager.socket8Full = false;}
		if (haterPlateName == "plate9") {HaterPlateManager.socket9Full = false;}
		if (haterPlateName == "plate10") {HaterPlateManager.socket10Full = false;}
		if (haterPlateName == "plate11") {HaterPlateManager.socket11Full = false;}

		if (IsBoss) {
			GameSceneController.roundWin = true; // Завершение игры победой
			GameSceneController.WinLevelFinish ();
		}

		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
	}
	void UpdateHaterName(){
		if (!IsBoss) {
			if (haterSex == 0) {
				int dice = Random.Range (0, 25);
				haterName.text = LocalizationBase.LocalMaleNamesArray [GameSceneController.playerLanguageSetting] [dice];
			}
			if (haterSex == 1) {
				int dice = Random.Range (0, 25);
				haterName.text = LocalizationBase.LocalFemaleNamesArray [GameSceneController.playerLanguageSetting] [dice];
			}
		} else {
			if (GameSceneController.playerLevelNumber == 1) {
				haterName.text = LocalizationBase.LocalBaseArray [GameSceneController.playerLanguageSetting] [31];
			}
			if (GameSceneController.playerLevelNumber == 2) {
				haterName.text = LocalizationBase.LocalBaseArray [GameSceneController.playerLanguageSetting] [32];
			}
			if (GameSceneController.playerLevelNumber == 3) {
				haterName.text = LocalizationBase.LocalBaseArray [GameSceneController.playerLanguageSetting] [33];
			}
			if (GameSceneController.playerLevelNumber == 4) {
				haterName.text = LocalizationBase.LocalBaseArray [GameSceneController.playerLanguageSetting] [34];
			}
		}
	}

	void AudioHitHater() {
		if (GameSceneController.playerSoundSetting == 1){
			if (haterType == 0) {
				int dice = Random.Range (0,6);

				if (dice == 0) {HaterAudio.PlayOneShot(hit4, 0.5f);}
				if (dice == 1) {HaterAudio.PlayOneShot(hit5, 0.5f);}
				if (dice == 2) {HaterAudio.PlayOneShot(hit7, 1f);}
				if (dice == 3) {HaterAudio.PlayOneShot(hit8, 0.5f);}
				if (dice == 4) {HaterAudio.PlayOneShot(hit9, 0.5f);}
				if (dice == 5) {HaterAudio.PlayOneShot(hit10, 0.5f);}
			}
			if (haterType == 1) {
				int dice = Random.Range (0,9);

				if (dice == 0) {HaterAudio.PlayOneShot(hit1, 1f);}
				if (dice == 1) {HaterAudio.PlayOneShot(hit2, 1f);}
				if (dice == 2) {HaterAudio.PlayOneShot(hit3, 1f);}
				if (dice == 3) {HaterAudio.PlayOneShot(hit4, 0.5f);}
				if (dice == 4) {HaterAudio.PlayOneShot(hit5, 0.5f);}
				if (dice == 5) {HaterAudio.PlayOneShot(hit6, 1f);}
				if (dice == 6) {HaterAudio.PlayOneShot(hit7, 1f);}
				if (dice == 7) {HaterAudio.PlayOneShot(hit10, 0.5f);}
				if (dice == 8) {HaterAudio.PlayOneShot(hit11, 1f);}
			}
			if (haterType == 2) {
				int dice = Random.Range (0,4);

				if (dice == 0) {HaterAudio.PlayOneShot(hit4, 0.5f);}
				if (dice == 1) {HaterAudio.PlayOneShot(hit7, 1f);}
				if (dice == 2) {HaterAudio.PlayOneShot(hit12, 1f);}
				if (dice == 3) {HaterAudio.PlayOneShot(hit13, 1f);}
			}
		}
	}
	void UpdateText(){
		text3.text = LocalizationBase.LocalBaseArray [GameSceneController.playerLanguageSetting] [30];
	}
}
