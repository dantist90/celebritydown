// (с) AlexZotov 2016 dantist90@gmail.com

using UnityEngine;
using System.Collections;

public class HaterPlateManager : MonoBehaviour {

	public GameSceneController GameSceneController;

	public GameObject[] hatersPlates;
	public GameObject haterPlatePref;
	public int maxPlatesInScreen;
	public int currentPlatesInScreen;

	public float repeatingPlate;

	public bool bossIsCreated;

	public bool socket0Full;
	public bool socket1Full;
	public bool socket2Full;
	public bool socket3Full;
	public bool socket4Full;
	public bool socket5Full;
	public bool socket6Full;
	public bool socket7Full;
	public bool socket8Full;
	public bool socket9Full;
	public bool socket10Full;
	public bool socket11Full;

	void Awake(){
		GameSceneController = GameSceneController.FindObjectOfType<GameSceneController> ();
	}

// Создаем плашки
	void OnEnable(){
		InvokeRepeating ("ChoicePlate", 1.5f, repeatingPlate);
	}
	// Выбор плашки
	public void ChoicePlate(){
		//if (GameSceneController.bossTime && !bossIsCreated) {++maxPlatesInScreen;}

		if ((currentPlatesInScreen < maxPlatesInScreen || GameSceneController.bossTime) && !GameSceneController.pause && !bossIsCreated) {

			int dice = Random.Range (0,hatersPlates.Length);

			if (dice == 0 && !socket0Full) {
				socket0Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket0").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 1 && !socket1Full) {
				socket1Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket1").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 2 && !socket2Full) {
				socket2Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket2").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 3 && !socket3Full) {
				socket3Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket3").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 4 && !socket4Full) {
				socket4Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket4").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 5 && !socket5Full) {
				socket5Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket5").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 6 && !socket6Full) {
				socket6Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket6").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 7 && !socket7Full) {
				socket7Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket7").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 8 && !socket8Full) {
				socket8Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket8").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 9 && !socket9Full) {
				socket9Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket9").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 10 && !socket10Full) {
				socket10Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket10").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
			if (dice == 11 && !socket11Full) {
				socket11Full = true;
				++currentPlatesInScreen;
				GameObject plate = Instantiate (haterPlatePref);
				plate.name = "plate" + dice;
				plate.transform.SetParent (GameObject.Find ("Canvas/HatersPlates/Socket11").transform, false);
				if (GameSceneController.bossTime) {bossIsCreated = true;}
			}
		}
	}
}
