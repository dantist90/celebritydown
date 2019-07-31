// (с) AlexZotov 2016 dantist90@gmail.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class MainMenuController : MonoBehaviour {

	public LocalizationBase LocalizationBase;

	// Данные об игроке
	public int playerMusicSetting;
	public int playerSoundSetting;
	public int playerLanguageSetting;
	public int playerLevelNumber;

	public Sprite langImage0;
	public Sprite langImage1;

	AudioSource ButtonAudio;
	public AudioClip check;
	public AudioClip click;

	AudioSource AmbientAudio;
	public AudioClip ambient1;

	// Контроллируем гуйню
	public float modalScreenAlphaLevel = 0.6f;

	public Sprite CheckOn;
	public Sprite CheckOff;

	void Awake () {
		LocalizationBase = LocalizationBase.FindObjectOfType<LocalizationBase>();
		ButtonAudio = GetComponent<AudioSource> ();
		AmbientAudio = GameObject.Find ("MainMenuController/AmbientMusic").GetComponent<AudioSource> ();

		UpdateSaves ();
		UpdateAmbientMusic ();
	}
	void Start(){
		UpdateLanguageImage ();
		UpdateText ();
		UpdateSoundButton ();
		MusicMute ();
		UpdateSettingWinButton ();
		GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().alpha = 0f;
	}
	void Update () {
		//Кнопка "Продолжить"
		if (playerLevelNumber == 0 || playerLevelNumber > 4) {
			GameObject.Find ("Canvas/CenterButtons/ContinueButton").GetComponent<Button> ().interactable = false;
			GameObject.Find ("Canvas/CenterButtons/ContinueButton/ContinueButtonText").GetComponent<Text> ().color = new Color(140.0f/255.0f, 120.0f/255.0f, 80.0f/255.0f);
		} else {
			GameObject.Find ("Canvas/CenterButtons/ContinueButton").GetComponent<Button> ().interactable = true;
			GameObject.Find ("Canvas/CenterButtons/ContinueButton/ContinueButtonText").GetComponent<Text> ().color = new Color(225.0f/255.0f, 188.0f/255.0f, 112.0f/255.0f);
		}
	}

	public void PushContinueButton(){
		// Действия при нажатии кнопки "Продолжить"
		SceneManager.LoadScene("CutScene");
		ClickButtonSound();
	}
	public void PushNewGameButton(){
		// Действия при нажатии кнопки "Новая игра"
		if (playerLevelNumber > 0) {
			GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().alpha = 1f; // Включаем алерт подтверждения
			GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().blocksRaycasts = true;
			GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().interactable = true;
			ModalScreenEnabled ();
			//UpdateText ();
		} else {
			SceneManager.LoadScene("CutScene");
		}
		ClickButtonSound();
	}
	public void PushConfirmNewGameButton(){
		// Действия после подтверждения начала новой игры
		PlayerPrefs.SetInt("PlayerLevelNumber", 0); // Обнуляем уровень игрока
		PlayerPrefs.SetInt("PlayerScore", 0); // Обнуляем рейтинг игрока
		PlayerPrefs.SetInt("PlayerMusicSetting", playerMusicSetting);
		PlayerPrefs.SetInt("PlayerSoundSetting", playerSoundSetting);
		PlayerPrefs.SetInt("PlayerLanguageSetting", playerLanguageSetting);
		SceneManager.LoadScene("CutScene");
		ClickButtonSound();
	}
	public void PushCancelNewGameButton(){
		// Если игрок нажал отмену в алерте подтверждения новой игры
		GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().alpha = 0f; // Выключаем алерт
		GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		GameObject.Find ("Canvas/ConfirmNewGameAlert").GetComponent<CanvasGroup> ().interactable = false;
		ModalScreenDisabled ();
		ClickButtonSound();
	}

	public void PushGameSettingsButton(){
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().alpha = 1f; // Включаем алерт подтверждения
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().interactable = true;
		ModalScreenEnabled ();
		ClickButtonSound();
	}
	public void PushSaveSettingsButton(){
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().alpha = 0f; // Включаем алерт подтверждения
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		GameObject.Find ("Canvas/GameSettingsAlert").GetComponent<CanvasGroup> ().interactable = false;
		ModalScreenDisabled ();
		ClickButtonSound();
		PlayerPrefs.SetInt("PlayerMusicSetting", playerMusicSetting);
		PlayerPrefs.SetInt("PlayerSoundSetting", playerSoundSetting);
		PlayerPrefs.SetInt("PlayerLanguageSetting", playerLanguageSetting);
	}
	public void PushAboutGameButton(){
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().alpha = 1f; // Включаем алерт подтверждения
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().interactable = true;
		ModalScreenEnabled ();
		ClickButtonSound();
	}
	public void PushCloseAboutGameAlert(){
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().alpha = 0f; // Включаем алерт подтверждения
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().blocksRaycasts = false;
		GameObject.Find ("Canvas/AboutAlert").GetComponent<CanvasGroup> ().interactable = false;
		ModalScreenDisabled ();
		ClickButtonSound();
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
		UpdateSoundButton ();
	}
	public void PushSoundButtonInSettings(){
		CheckMusicButtonSound();
		if (playerSoundSetting == 0) {
			playerSoundSetting = 1;
		} else {
			playerSoundSetting = 0;
		}
		UpdateSettingWinButton ();
		UpdateSoundButton ();
	}
	public void PushLang0ButtonInSettings(){
		CheckButtonSound();
		playerLanguageSetting = 0;
		UpdateSettingWinButton ();
		UpdateText ();
		UpdateLanguageImage ();
	}
	public void PushLang1ButtonInSettings(){
		CheckButtonSound();
		playerLanguageSetting = 1;
		UpdateSettingWinButton ();
		UpdateText ();
		UpdateLanguageImage ();
	}

	void UpdateSettingWinButton(){
		if (playerMusicSetting == 1) {
			GameObject.Find ("Canvas/GameSettingsAlert/MusicCheck").GetComponent<Image> ().sprite = CheckOn;
		} else {
			GameObject.Find ("Canvas/GameSettingsAlert/MusicCheck").GetComponent<Image> ().sprite = CheckOff;
		}
		if (playerSoundSetting == 1) {
			GameObject.Find ("Canvas/GameSettingsAlert/SoundCheck").GetComponent<Image> ().sprite = CheckOn;
		} else {
			GameObject.Find ("Canvas/GameSettingsAlert/SoundCheck").GetComponent<Image> ().sprite = CheckOff;
		}
		Color msb1 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 120.0f/255.0f);
		Color msb2 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
		GameObject.Find ("Canvas/GameSettingsAlert/Lang0Button").GetComponent<Image> ().color = msb1;
		GameObject.Find ("Canvas/GameSettingsAlert/Lang1Button").GetComponent<Image> ().color = msb1;

		if (playerLanguageSetting == 0) {
			GameObject.Find ("Canvas/GameSettingsAlert/Lang0Button").GetComponent<Image> ().color = msb2;
		} else {
			GameObject.Find ("Canvas/GameSettingsAlert/Lang1Button").GetComponent<Image> ().color = msb2;
		}
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

// Музыка и звуки
	public void PushMusicSoundButton(){
		if (playerMusicSetting == 1) {
			playerMusicSetting = 0;
			playerSoundSetting = 0;
		} else {
			playerMusicSetting = 1;
			playerSoundSetting = 1;
		}
		PlayerPrefs.SetInt ("PlayerMusicSetting", playerMusicSetting);
		PlayerPrefs.SetInt ("PlayerSoundSetting", playerSoundSetting);
		CheckMusicButtonSound ();
		UpdateSoundButton ();
		UpdateSettingWinButton ();
		MusicMute ();
	}
	void UpdateSoundButton(){
		Color msb1 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 120.0f/255.0f);
		Color msb2 = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
		if (playerMusicSetting == 0) {
			GameObject.Find ("Canvas/MusicSoundButton").GetComponent<Image> ().color = msb1;
		} else {
			GameObject.Find ("Canvas/MusicSoundButton").GetComponent<Image> ().color = msb2;
		}
		UpdateSettingWinButton ();
	}
	void MusicMute(){
		if (playerMusicSetting == 1) {
			AmbientAudio.mute = false;
		} else {
			AmbientAudio.mute = true;
		}
	}
	void CheckButtonSound(){
		if (playerSoundSetting == 1) {ButtonAudio.PlayOneShot (check, 1f);}
	}
	void CheckMusicButtonSound(){
		ButtonAudio.PlayOneShot (check, 1f);
	}
	void ClickButtonSound(){
		if (playerMusicSetting == 1) {ButtonAudio.PlayOneShot (click, 1f);}
	}
	void UpdateAmbientMusic(){
		AmbientAudio.clip = ambient1;
		AmbientAudio.Play();

	}


// Локализация
	public void PushLanguageButton(){
		if (playerLanguageSetting == 1) {
			playerLanguageSetting = 0;
		} else {
			playerLanguageSetting = 1;
		}
		PlayerPrefs.SetInt("PlayerLanguageSetting", playerLanguageSetting);
		UpdateLanguageImage ();
		UpdateText ();
		CheckButtonSound ();
	}
	void UpdateLanguageImage(){
		if (playerLanguageSetting == 0) {GameObject.Find ("Canvas/LanguageButton").GetComponent<Image> ().sprite = langImage0;}
		if (playerLanguageSetting == 1) {GameObject.Find ("Canvas/LanguageButton").GetComponent<Image> ().sprite = langImage1;}
	}
	void UpdateText(){
		GameObject.Find ("Canvas/CenterButtons/ContinueButton/ContinueButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [1];
		GameObject.Find ("Canvas/CenterButtons/NewGameButton/NewGameButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [2];
		GameObject.Find ("Canvas/CenterButtons/GameSettingsButton/GameSettingsButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [3];
		GameObject.Find ("Canvas/CenterButtons/AboutGameButton/AboutGameButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [4];
		GameObject.Find ("Canvas/ConfirmNewGameAlert/ConfirmNewGameAlertHeader").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [5];
		GameObject.Find ("Canvas/ConfirmNewGameAlert/ConfirmNewGameAlertText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [6];
		GameObject.Find ("Canvas/ConfirmNewGameAlert/ConfirmNewGameButton/ConfirmNewGameButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [7];
		GameObject.Find ("Canvas/ConfirmNewGameAlert/CancelNewGameButton/CancelNewGameButtonText").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [8];
		GameObject.Find ("Canvas/AboutAlert/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [37];
		GameObject.Find ("Canvas/AboutAlert/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [38];
		GameObject.Find ("Canvas/AboutAlert/ConfirmButton/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [59];
		GameObject.Find ("Canvas/GameSettingsAlert/Header").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [41];
		GameObject.Find ("Canvas/GameSettingsAlert/Text1").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [42];
		GameObject.Find ("Canvas/GameSettingsAlert/Text2").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [43];
		GameObject.Find ("Canvas/GameSettingsAlert/Text3").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [1] [0];
		GameObject.Find ("Canvas/GameSettingsAlert/Text4").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [0] [0];
	}

// Шторка ModalScreen
	void ModalScreenEnabled(){
		GameObject modalScreenImage = GameObject.Find ("Canvas/ModalScreenImage");
		modalScreenImage.GetComponent<CanvasGroup> ().alpha = modalScreenAlphaLevel;
		modalScreenImage.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		modalScreenImage.GetComponent<Image> ().raycastTarget = true;
	}
	void ModalScreenDisabled(){
		GameObject modalScreenImage = GameObject.Find ("Canvas/ModalScreenImage");
		modalScreenImage.GetComponent<CanvasGroup> ().alpha = 0;
		modalScreenImage.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		modalScreenImage.GetComponent<Image> ().raycastTarget = false;
	}

// Дебаг
	public void DebugClearSettings(){
		PlayerPrefs.DeleteAll();
		UpdateSaves ();
		UpdateLanguageImage ();
		UpdateText ();
		UpdateSoundButton ();
		MusicMute ();
	}
	public void DebugSetLevel0(){
		playerLevelNumber = 0;
		PlayerPrefs.SetInt("PlayerLevelNumber", 0);
	}
	public void DebugSetLevel1(){
		playerLevelNumber = 1;
		PlayerPrefs.SetInt("PlayerLevelNumber", 1);
	}
	public void DebugSetLevel2(){
		playerLevelNumber = 2;
		PlayerPrefs.SetInt("PlayerLevelNumber", 2);
	}
	public void DebugSetLevel3(){
		playerLevelNumber = 3;
		PlayerPrefs.SetInt("PlayerLevelNumber", 3);
	}
	public void DebugSetLevel4(){
		playerLevelNumber = 4;
		PlayerPrefs.SetInt("PlayerLevelNumber", 4);
	}


}
