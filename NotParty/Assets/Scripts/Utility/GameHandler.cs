using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using float_oat.Desktop90;
using System;

//Fusion du code fournis dans la librairie de ZombiSoft avec le nôtre
//Changement des noms de variables et des fonctions pour l'ajuster.

[RequireComponent(typeof(HighScoreHandler))]
[RequireComponent(typeof(ScenesHandler))]
public class GameHandler : MonoBehaviour
{

    private HighScoreHandler highscoreHandler;
    private ScenesHandler sceneHandler;
	private AudioSource audioSourceUI;

	//==============================================================
	// Start
	//==============================================================
	void Start()
	{
		highscoreHandler = GetComponent<HighScoreHandler>();
		sceneHandler = GetComponent<ScenesHandler>();
		audioSourceUI = GameObject.Find("GameHandling/UI/AudioSource").GetComponent<AudioSource>();
		// Get the player's actual score from the player's prefs and set the Game HUD
		SetHighScore(highscoreHandler.LoadScore());

		// Timer Start
		remainingDuration = totalDuration;
		StartCoroutine(TrackTime());

		// Update the Game HUD
		UpdateGraphics();
	}

	//==============================================================
	// Update
	//==============================================================
	void Update()
	{
		if (Input.anyKeyDown && !gameOver)
		{
			if (Input.GetKeyDown(KeyCode.Escape)) ToggleMenu();
			if (cheatCodeAllowed) CheatCodes();
		}
	}

	//==============================================================
	// Update all UI graphics
	//==============================================================
	private void UpdateGraphics()
	{
		UpdateCooldownBar();
		UpdateProgressGlobe();
		UpdateTimer();
	}

	//==============================================================
	// Actions Per Second
	//==============================================================

	//All actions that should be done every seconds
	private void ActionsPerSecond()
    {
		
	}

	//==============================================================
	// Pause Menu
	//==============================================================
	[Space(10)]
	[Header("Pause Menu")]

	[SerializeField][Tooltip("Window to open when player press escape.")]
	private GameObject pauseWindow;
	
	[SerializeField][Tooltip("Settings window.")]
	private GameObject settingsWindow;

	private bool pauseMenuOpened = false;

	public void ToggleMenu()
    {
		if (!gameOver)
        {
			if (pauseMenuOpened)
			{
				if (settingsWindow.activeSelf)
				{
					settingsWindow.GetComponent<WindowController>().Close();
				}
				pauseWindow.GetComponent<WindowController>().Close();
			}
			else
			{
				pauseWindow.GetComponent<WindowController>().Open();

			}
			pauseMenuOpened = !pauseMenuOpened;
			sceneHandler.TogglePause();
        }
	}


	//==============================================================
	// Timer
	//==============================================================

	//@Source: https://www.youtube.com/watch?v=2gPHkaPGbpI

	[Space(10)]
	[Header("Timer")]
	
	[SerializeField][Tooltip("The circle Image object around the timer's text.")]
	private Image timerFill;
	
	[SerializeField][Tooltip("Text Mesh Pro Field to output the timer's text.")]
	private TextMeshProUGUI timerText;

	[SerializeField][Tooltip("Total duration for the level. When the game has been going for this long, the player is defeated.")][Min(1)]
	private int totalDuration;

	private int remainingDuration;

	// Call it to add more time to the level.
	public void IncreaseTime(int Seconds)
	{
		if (!gameOver && !IsPaused)
		{
			remainingDuration += Seconds;
		}
	}

	// Decrement the remaining time and update the Game HUD
	private IEnumerator TrackTime()
	{
		float delai = sceneHandler.GetIntroDuration();
		yield return new WaitForSeconds(delai);
		while (remainingDuration > 0)
		{
			remainingDuration--;
			ActionsPerSecond();
			UpdateGraphics();
			yield return new WaitForSeconds(1f);
		}
		Defeat();
	}

	// Decrement the remaining time and update the Game HUD
	private void UpdateTimer()
	{
		timerText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
		timerFill.fillAmount = Mathf.InverseLerp(0, totalDuration, remainingDuration);
	}

	//==============================================================
	// Score
	//==============================================================

	[Space(10)]
	[Header("Score")]
	
	[SerializeField][Tooltip("Text Mesh Pro Field to output the score's text.")]
	private TextMeshProUGUI scoreHUD;

	// Set the current score of the player in the game HUD
	private void SetHighScore(int score)
	{
		scoreHUD.text = "Score: " + score.ToString("00");
	}

	// After saving the score, the game goes back to the main menu.
	public void SaveScore()
	{
		// Get the player's name
		InputField nameInput = defeatWindow.GetComponentInChildren<InputField>();
		string playerName = nameInput.text;
		
		highscoreHandler.SaveHighScore(playerName);
		highscoreHandler.RestartScore();
		
		defeatWindow.GetComponent<WindowController>().Close();
		ReturnMenu();
	}


	//==============================================================
	// Cooldown Logic
	//==============================================================

	[Space(10)]
	[Header("Cooldown")]
	
	[SerializeField][Tooltip("Image object representing the bar's interior.")]
	private Image currentCooldownBar;
	
	[SerializeField][Tooltip("Starting cooldown of the current level. Default: 0")][Min(0.0f)]
	private float cooldownLevel = 0f;
	
	[SerializeField][Tooltip("Max cooldown of the bar. When the cooldown level gets there, the DisplayCooldown function is called. Default: 100")][Min(0.01f)]
	private float maxCooldown = 100f;
	
	[SerializeField][Tooltip("If there's a natural regeneration for the cooldown system. Default: True")]
	private bool regenerate = true;

	[SerializeField][Tooltip("Influence how smooth the bar reduce. Default: 5")][Min(0.0f)]
	private float regenerationAmount = 5.0f;

	private float regenBlockTime;

	// Every second, it regenerates a number of cooldown points depending on the regen stat
	public void ActivateCooldownUI(float cooldownTime, string abilityName)
	{
		if (!gameOver && !IsPaused)
		{
			StopCoroutine(CCooldown());
			float ratio = maxCooldown / regenerationAmount;
			regenBlockTime = cooldownTime / ratio;
			cooldownLevel = maxCooldown;
			UpdateGraphics();
			PopupText.Instance.Popup(abilityName + " on Cooldown!", 1f, 1f);
			if (regenerate)
            {
				StartCoroutine(CCooldown());
            }
		}
	}

	private IEnumerator CCooldown()
    {
		while (cooldownLevel>0.0f)
        {
			yield return new WaitForSeconds(regenBlockTime);
			if (godMode)
			{
				CoolDown(maxCooldown);
			}
			else
			{
				CoolDown(regenerationAmount);
			}
        }
    }

	// Decrease the cooldown level
	public void CoolDown(float amount)
	{
		if (!gameOver && !IsPaused)
		{
			cooldownLevel -= amount;
			if (cooldownLevel < 0.0f)
				cooldownLevel = 0.0f;

			UpdateGraphics();
		}
	}

	// Update the cooldown bar in the Game HUD
	private void UpdateCooldownBar()
	{
		float ratio = cooldownLevel / maxCooldown;
		currentCooldownBar.GetComponent<Image>().fillAmount = ratio;
	}

	//==============================================================
	// Progress Logic
	//==============================================================

	[Space(10)]
	[Header("Progress")]
	
	[SerializeField][Tooltip("Image object representing the globe's interior.")]
	private Image currentProgressGlobe;

	[SerializeField][Tooltip("Actual progress of the current level. Default: 0")][Min(0.0f)]
	private float progress = 0f;

	[SerializeField][Tooltip("Max progress of the current level. When the progress gets there, the game ends. Default: 100")][Min(0.01f)]
	private float maxProgress = 100f;

	// Way to get the progress down (debuff or similar mechanics)
	public void SetBackProgress(float amount)
	{
		if (!gameOver && !IsPaused)
        {
			progress -= amount;
			if (progress < 1)
				progress = 0;

			UpdateGraphics();
        }
	}

	// Increase the level of progress, at 100% the player win
	public void Progress(float amount)
	{
		if (!gameOver && !IsPaused)
		{
			progress += amount;
			if (progress > maxProgress || progress == maxProgress)
			{
				progress = maxProgress;
				Victory();
			}

			UpdateGraphics();
		}
	}

	public void SetMaxProgress(float amount)
    {
		maxProgress = amount;
    }

	// Update the progress orb in the game HUD
	private void UpdateProgressGlobe()
	{
		float ratio = progress / maxProgress;
		currentProgressGlobe.rectTransform.localPosition = new Vector3(0, currentProgressGlobe.rectTransform.rect.height * ratio - currentProgressGlobe.rectTransform.rect.height, 0);
	}

	//==============================================================
	// Weapon's changes
	//==============================================================

	[Space(10)]
	[Header("Weapons Icons")]
	
	[SerializeField][Tooltip("List all weapon and their name. Names are used to search for the icon later.")]
	private WeaponEntry[] weapons;
	
	[SerializeField][Tooltip("Starting weapon's icon if there is one.")]
	private GameObject currentWeapon;
	
	// Change the currently displayed weapon
	public void ChangeWeapon(string newWeapon)
	{
		if (!gameOver && !IsPaused) {
			foreach (WeaponEntry weapon in weapons)
			{
				if (weapon.name == newWeapon)
				{
					if (currentWeapon != null)
					{
						currentWeapon.SetActive(false);
					}
					currentWeapon = weapon.gameObject;
					currentWeapon.SetActive(true);
				}
			}
		}
	}

	//==============================================================
	// Game Endings & Ways To Quit The Game
	//==============================================================

	[Space(10)]
	[Header("Game Ending Windows")]

	[SerializeField][Tooltip("Window to open when a victory occurs.")]
	private GameObject victoryWindow;
	[SerializeField][Tooltip("Sound to play when a victory occurs.")]
	private AudioClip victorySound;

	[SerializeField][Tooltip("Window to open when a defeat occurs.")]
	private GameObject defeatWindow;
	[SerializeField][Tooltip("Sound to play when a defeat occurs.")]
	private AudioClip defeatSound;
	

	private bool gameOver = false;

	// Open a prompt to ask for the user's name to keep its highscore
	public void Defeat()
	{
		audioSourceUI.Stop();
		audioSourceUI.ignoreListenerPause = true;
		audioSourceUI.PlayOneShot(defeatSound);
		gameOver = true;
		sceneHandler.TogglePause();
		defeatWindow.GetComponent<WindowController>().Open();
	}

	// Increment the score and load a new random level
	public void Victory()
	{
		audioSourceUI.Stop();
		audioSourceUI.ignoreListenerPause = true;
		audioSourceUI.PlayOneShot(victorySound);
		gameOver = true;
		sceneHandler.TogglePause();
		highscoreHandler.IncreaseScore();
		victoryWindow.GetComponent<WindowController>().Open();
	}

	public void NextLevel()
    {
		sceneHandler.PlayRandomLevel();
	}

	public void ReturnMenu()
	{
		sceneHandler.MainMenu();
	}

	//==============================================================
	// Dispatching
	//==============================================================

	public bool IsPaused
	{
		get
		{
			return sceneHandler.IsPaused;
		}
	}

    public void Pause()
    {
		sceneHandler.TogglePause();
    }

    //==============================================================
    // Big Secrets
    //==============================================================

    [Space(10)]
	[Header("Cheat Codes")]

	[SerializeField]
	private bool cheatCodeAllowed;

	private bool godMode = false;
	private readonly string[] cheatGod = new string[] { "g", "o", "d", "m", "o", "d", "e" };
	private int godIndex;

	[SerializeField]
	private GameObject assemblyGun;
	[SerializeField]
	private GameObject cPlusPlusGun;
	[SerializeField]
	private GameObject javascriptGun;
	[SerializeField]
	private GameObject pythonGun;
	private readonly string[] cheatAssembly = new string[] { "p", "e", "w", "p", "e", "w" };
	private int assemblyIndex;

	private void CheatCodes ()
    {
		if (Input.GetKeyDown(cheatGod[godIndex]))
		{
			godIndex++;
		}
		else
		{
			godIndex = 0;
		}

		if (Input.GetKeyDown(cheatAssembly[assemblyIndex]))
        {
			assemblyIndex++;
		}
        else
        {
			assemblyIndex = 0;
        }

		if (godIndex == cheatGod.Length)
		{
			godMode = !godMode;
			PlayerController player = GameObject.Find("Map/Player").GetComponent<PlayerController>();
			if (godMode)
            {
				player.AddEfficiency(10);
				player.AddSpeed(3);
            } else
            {
				player.RemoveEfficiency(10);
				player.RemoveSpeed(3);
            }
			godIndex = 0;
		}

		if (assemblyIndex == cheatAssembly.Length)
        {
			ItemSpawner itemSpawner = GameObject.Find("Map/ItemsSpawner").GetComponent<ItemSpawner>();
			itemSpawner.GiveLoot(assemblyGun);
			itemSpawner.GiveLoot(cPlusPlusGun);
			itemSpawner.GiveLoot(javascriptGun);
			itemSpawner.GiveLoot(pythonGun);
			assemblyIndex = 0;
		}
	}
}


//==============================================================
// Specialised Class
//==============================================================

[Serializable]
public struct WeaponEntry
{
    public string name;
    public GameObject gameObject;
}