using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float maxLevelTime = 240;

    private float currentTime = 0f;
    [SerializeField] private CanvasGroup endUI;
    [SerializeField] private CanvasGroup hudUI;
    [SerializeField] private CanvasGroup shopUI;

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI xp;
    [SerializeField] private PlayerController player;

    public bool IsPaused { get; set; }

    private void Start()
    {
        hudUI.alpha = 1;
        hudUI.interactable = true;
        hudUI.blocksRaycasts = true;
        shopUI.alpha = 0;
        shopUI.interactable = false;
        shopUI.blocksRaycasts = false;
        endUI.alpha = 0;
        endUI.interactable = false;
        endUI.blocksRaycasts = false;
    }

    private void Update()
    {
        if (!IsPaused)
        {
            currentTime += Time.deltaTime;
            timer.text = "Time Remaining: " + (maxLevelTime - currentTime);
            if (currentTime > maxLevelTime)
            {
                ShowEndUI();
            }
        }

    }

    private void ShowEndUI()
    {
        hudUI.alpha = 0;
        hudUI.interactable = false;
        hudUI.blocksRaycasts = false;
        shopUI.alpha = 0;
        shopUI.interactable = false;
        shopUI.blocksRaycasts = false;
        xp.text = player.XP.ToString();
        endUI.alpha = 1;
        endUI.interactable = true;
        endUI.blocksRaycasts = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    

}
