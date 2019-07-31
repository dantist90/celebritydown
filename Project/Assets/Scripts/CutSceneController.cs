// (с) AlexZotov 2016 dantist90@gmail.com

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour {

	public LocalizationBase LocalizationBase;

	public int playerMusicSetting;
	public int playerSoundSetting;
	public int playerLanguageSetting;
	public int playerLevelNumber;

	public string gameSceneName = "GameScene";

	public bool modalScreenEnabled = false;
	public float modalScreenAlphaLevel = 0.3f;

	public int cutSceneStep = 0;

	//Настройки
	public int level0MaxCutSceneStep;
	public int level1MaxCutSceneStep;
	public int level2MaxCutSceneStep;
	public int level3MaxCutSceneStep;
	public int level4MaxCutSceneStep;
	public int level5MaxCutSceneStep;

	public Sprite image1;
	public Sprite image2;
	public Sprite image3;
	public Sprite image4;
	public Sprite image5;
	public Sprite image6;
	public Sprite image7;
	public Sprite image8;
	public Sprite image9;
	public Sprite image10;
	public Sprite image11;
	public Sprite image12;
	public Sprite image14;

	public AudioSource ButtonAudio;
	public AudioClip check;
	public AudioClip click;

	public AudioSource AmbientAudio;
	public AudioClip ambient1;
	public AudioClip ambient2;
	public AudioClip ambient3;

	// Use this for initialization
	void Awake () {
		LocalizationBase = LocalizationBase.FindObjectOfType<LocalizationBase>();

		ButtonAudio = GetComponent<AudioSource> ();
		AmbientAudio = GameObject.Find ("CutSceneController/AmbientMusic").GetComponent<AudioSource> ();

		UpdateSaves();
		UpdateAmbientMusic ();
	}

	void Start (){
		//Музыка
		if (playerMusicSetting == 0) {
			GameObject.Find ("CutSceneController/AmbientMusic").GetComponent<AudioSource> ().mute = true;
		}
		Content ();
		UpdateText ();
	}

	void Content () {
		if (playerLevelNumber == 0) {
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [16];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image1;
			}
			if (cutSceneStep == 1) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [17];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image2;
			}
			if (cutSceneStep == 2) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [18];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image5;
			}
		
		}
		if (playerLevelNumber == 1) {
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [19];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image3;
			}
		}
		if (playerLevelNumber == 2) {
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [20];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image4;
			}
		}
		if (playerLevelNumber == 3) {
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [21];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image14;
			}
			if (cutSceneStep == 1) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [22];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image6;
			}
			if (cutSceneStep == 2) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [23];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image5;
			}
		}
		if (playerLevelNumber == 4) {
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [24];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image9;
			}
			if (cutSceneStep == 1) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [25];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image10;
			}
			if (cutSceneStep == 2) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [26];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image8;
			}
			if (cutSceneStep == 3) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [27];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image7;
			}
		}
		if (playerLevelNumber == 5) { // победа
			if (cutSceneStep == 0) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [35];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image12;
			}
			if (cutSceneStep == 1) {
				GameObject.Find ("Canvas/CutSceneTextPlate/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [36];
				GameObject.Find ("Canvas/Image").GetComponent<Image> ().sprite = image11;
			}
		}

	}

	// Кнопка "Пропустить Катсцену"
	public void PushSkipCutSceneButton(){
		ClickButtonSound ();
		if (playerLevelNumber < 5) {
			SceneManager.LoadScene (gameSceneName);
		} else{
			SceneManager.LoadScene ("MainMenu");
		}
	}

	// Кнопка "Следующая сцена"
	public void PushNextCutSceneButton(){
		ClickButtonSound ();

		if (playerLevelNumber == 0) {
			if (cutSceneStep < level0MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				SceneManager.LoadScene(gameSceneName);
			}
			Content ();
		}
		if (playerLevelNumber == 1) {
			if (cutSceneStep < level1MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				SceneManager.LoadScene(gameSceneName);
			}
			Content ();
		}
		if (playerLevelNumber == 2) {
			if (cutSceneStep < level2MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				SceneManager.LoadScene(gameSceneName);
			}
			Content ();
		}
		if (playerLevelNumber == 3) {
			if (cutSceneStep < level3MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				SceneManager.LoadScene(gameSceneName);
			}
			Content ();
		}
		if (playerLevelNumber == 4) {
			if (cutSceneStep < level4MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				SceneManager.LoadScene(gameSceneName);
			}
			Content ();
		}
		if (playerLevelNumber == 5) {
			if (cutSceneStep < level5MaxCutSceneStep) {
				++cutSceneStep;
			} else {
				PlayerPrefs.SetInt("PlayerLevelNumber", 0);
				PlayerPrefs.SetInt("PlayerScore", 0);
				SceneManager.LoadScene("MainMenu");
			}
			Content ();
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

	void UpdateText(){
		GameObject.Find ("Canvas/SkipCutSceneButton/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [28];
		GameObject.Find ("Canvas/CutSceneNextButton/Text").GetComponent<Text> ().text = LocalizationBase.LocalBaseArray [playerLanguageSetting] [29];
	}

	void CheckButtonSound(){
		if (playerMusicSetting == 1) {ButtonAudio.PlayOneShot (check, 1f);}
	}
	void ClickButtonSound(){
		if (playerMusicSetting == 1) {ButtonAudio.PlayOneShot (click, 1f);}
	}
	void UpdateAmbientMusic(){
		AmbientAudio.clip = ambient1;
		AmbientAudio.Play();
	}


	// Дебаг
	public void DebugSetLevel0(){
		cutSceneStep = 0;
		playerLevelNumber = 0;
		PlayerPrefs.SetInt("PlayerLevelNumber", 0);
	}
	public void DebugSetLevel1(){
		cutSceneStep = 0;
		playerLevelNumber = 1;
		PlayerPrefs.SetInt("PlayerLevelNumber", 1);
	}
	public void DebugSetLevel2(){
		cutSceneStep = 0;
		playerLevelNumber = 2;
		PlayerPrefs.SetInt("PlayerLevelNumber", 2);
	}
	public void DebugSetLevel3(){
		cutSceneStep = 0;
		playerLevelNumber = 3;
		PlayerPrefs.SetInt("PlayerLevelNumber", 3);
	}
	public void DebugSetLevel4(){
		cutSceneStep = 0;
		playerLevelNumber = 4;
		PlayerPrefs.SetInt("PlayerLevelNumber", 4);
	}
	public void DebugSetLevel5(){
		cutSceneStep = 0;
		playerLevelNumber = 5;
		PlayerPrefs.SetInt("PlayerLevelNumber", 5);
	}
}
