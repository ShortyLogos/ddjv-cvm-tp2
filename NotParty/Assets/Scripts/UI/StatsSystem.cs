using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using float_oat.Desktop90;

//Fusion du code fournis dans la librairie de ZombiSoft avec le nôtre
//Changement des noms de variables et des fonctions pour l'ajuster.

public class StatsSystem : MonoBehaviour
{
	private static StatsSystem Instance;
	public StatsSystem Stats
    {
		get
        {
			return Instance;
        }
    }

	[SerializeField] private GameObject gameHandler;
	[SerializeField] private GameObject victoryWindow;
	[SerializeField] private bool GodMode;

	[SerializeField] private Image currentHeatBar;
	[SerializeField] private float heatLevel = 0f;
	[SerializeField] private float maxHeat = 100f;

	[SerializeField] private Image currentProgressGlobe;
	[SerializeField] private float progress = 0f;
	[SerializeField] private float maxProgress = 100f;

	[SerializeField] private WeaponEntry[] weapons;
	[SerializeField] private GameObject currentWeapon;

	//==============================================================
	// Regenerate Heat
	//==============================================================
	[SerializeField] private bool Regenerate = true;
	[SerializeField] private float regen = 15f;
	private float timeleft = 0.0f;	// Left time for current interval
	[SerializeField] private float regenUpdateInterval = 1f;


	//==============================================================
	// Awake
	//==============================================================
	void Awake()
	{
		Instance = this;
	}
	
	//==============================================================
	// Start
	//==============================================================
  	void Start()
	{
		UpdateGraphics();
		timeleft = regenUpdateInterval; 
	}

	//==============================================================
	// Update
	//==============================================================
	void Update ()
	{
		if (Regenerate)
			Regen();
	}

	//==============================================================
	// Regenerate Heat
	//==============================================================
	private void Regen()
	{
		timeleft -= Time.deltaTime;

		if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
		{
			// Debug mode
			if (GodMode)
			{
				CoolDown(maxHeat);
			}
			else
			{
				CoolDown(regen);
			}

			UpdateGraphics();

			timeleft = regenUpdateInterval;
		}
	}

	//==============================================================
	// Heat Logic
	//==============================================================
	private void UpdateHeatBar()
	{
		float ratio = heatLevel / maxHeat;
		currentHeatBar.rectTransform.localPosition = new Vector3(currentHeatBar.rectTransform.rect.width * ratio - currentHeatBar.rectTransform.rect.width, 0, 0);
	}

	public void HeatUp(float heat)
	{
		heatLevel += heat;
		if (heatLevel > maxHeat)
			heatLevel = maxHeat;

		UpdateGraphics();

		StartCoroutine(Heating());
	}

	public void CoolDown(float amount)
	{
		heatLevel -= amount;
		if (heatLevel < 0)
			heatLevel = 0;

		UpdateGraphics();
	}

	public void SetMaxHeat(float max)
	{
		maxHeat += (int)(maxHeat * max / 100);

		UpdateGraphics();
	}

	//==============================================================
	// Progress Logic
	//==============================================================

	private void UpdateProgressGlobe()
	{
		float ratio = progress / maxProgress;
		currentProgressGlobe.rectTransform.localPosition = new Vector3(0, currentProgressGlobe.rectTransform.rect.height * ratio - currentProgressGlobe.rectTransform.rect.height, 0);
	}

	public void SetBackProgress(float amount)
	{
		progress -= amount;
		if (progress < 1)
			progress = 0;

		UpdateGraphics();
	}

	public void Progress(float amount)
	{
		progress += amount;
		if (progress > maxProgress || progress == maxProgress)
        {
			progress = maxProgress;
			StartCoroutine(CCompleteProject());
        } 
			

		UpdateGraphics();
	}
	public void SetMaxProgress(float max)
	{
		maxProgress += (int)(maxProgress * max / 100);
		
		UpdateGraphics();
	}

	//==============================================================
	// Weapon's logo
	//==============================================================

	public void ChangeWeapon(string newWeapon)
    {
		foreach (WeaponEntry weapon in weapons)
        {
			if (weapon.name == newWeapon)
            {
				currentWeapon.SetActive(false);
				currentWeapon = weapon.gameObject;
				currentWeapon.SetActive(true);
            }
        }
	}

	//==============================================================
	// Update all Bars & Globes UI graphics
	//==============================================================
	private void UpdateGraphics()
	{
		UpdateHeatBar();
		UpdateProgressGlobe();
	}

	//==============================================================
	// Coroutine Weapon Heating Up
	//==============================================================
	IEnumerator Heating()
	{
		// Heat augments. Do stuff.. play anim, sound..

		if (heatLevel > 99) // OverHeating
		{
			yield return StartCoroutine(Overheat());
		}

		else
			yield return null;
	}

	//==============================================================
	// Coroutine Weapon is overheating
	//==============================================================
	IEnumerator Overheat()
	{
		// Weapon is unusable until it cool down. Do stuff.. play anim, sound..
		PopupText.Instance.Popup("Weapon overheating!", 1f, 1f);

		yield return null;
	}

	//==============================================================
	// Coroutine Progress is complete
	//==============================================================
	private IEnumerator CCompleteProject()
	{
		// Won the game or whatever. Do stuff.. play anim, sound..
		gameHandler.GetComponent<ScenesHandler>().TogglePause();
		victoryWindow.GetComponent<WindowController>().Open();
		yield return null;
	}
}

[Serializable]
public struct WeaponEntry
{
	public string name;
	public GameObject gameObject;
}