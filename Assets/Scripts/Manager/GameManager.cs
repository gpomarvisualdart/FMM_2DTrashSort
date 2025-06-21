using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Transform trashPool;

    [Header("PlayerStats")]
    [SerializeField] int maxWrongTrash;
    public int currentWrongTrash;



    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }


    private void OnEnable()
    {
        currentWrongTrash = maxWrongTrash;
        UIManager.instance.ChangeMissedTrashValue(currentWrongTrash);
        if (CO_StartSpawningTrash != null) { StopCoroutine(CO_StartSpawningTrash); CO_StartSpawningTrash = null; }
        CO_StartSpawningTrash = StartCoroutine(StartSpawningTrash());
    }


    public void RestartScene()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        PauseGame(false);
    }


    float currentTime;
    Coroutine CO_StartSpawningTrash;
    public Coroutine GetStartSpawningTrashCO() => CO_StartSpawningTrash;
    IEnumerator StartSpawningTrash()
    {
        if (trashPool == null) { CO_StartSpawningTrash = null; yield break; }
        var flt_MaxTime = 4f;
        currentTime = flt_MaxTime;

        for (int i = 0; i < trashPool.childCount; i++)
        {
            while(currentTime <= flt_MaxTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            trashPool.GetChild(i).gameObject.SetActive(true);
            currentTime = 0f;
        }
        CO_StartSpawningTrash = null;
    }


    public void OnSortCorrect(bool correct, int trashType)
    {
        if (correct)
        {
            ScoreManager.instance.AddCoins(1, trashType);
        }
        else
        {
            if (currentWrongTrash < 1) return; 
            currentWrongTrash--;
            ScoreManager.instance.ComboBreak();
            UIManager.instance.ChangeMissedTrashValue(currentWrongTrash);
            if (currentWrongTrash < 1)
            {
                UIManager.instance.ActivateGameOverScreen(true);
                PauseGame(true);
            }
        }
    }


    bool isPaused = false;
    public void PauseGame(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
           Time.timeScale = 1f;
           isPaused = false;
        }
    }

    private void Update()
    {
        //Debug.Log(Time.time);
    }
}
