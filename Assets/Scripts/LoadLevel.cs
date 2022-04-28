using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private CanvasGroup rules;

    public void Start()
    {
        if (rules)
        {
            rules.alpha = 0;
            rules.blocksRaycasts = false;
            rules.interactable = false;
        }

    }
    public void Load(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ToggleRules()
    {
        rules.alpha = 1 - rules.alpha;
        rules.interactable = !rules.interactable;
        rules.blocksRaycasts = !rules.blocksRaycasts;
    }

    public void Click()
    {
        GetComponent<AudioSource>().Play();
    }
}
