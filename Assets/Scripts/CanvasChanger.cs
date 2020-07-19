using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasChanger : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private TerrainManager tm;

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameCanvas.SetActive(false);
        endCanvas.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        Time.timeScale = 1;
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
    }
}
