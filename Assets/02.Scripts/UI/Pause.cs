using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject menu, resume, restart, option;
    bool isPausing;

    void Update()
    {
        if(isPausing)
        {
            Debug.Log("멈춰!");
            Time.timeScale = 0;
            isPausing = false;
        }
    }
    public void pause()
    {
        menu.SetActive(true);
        resume.SetActive(false);
        restart.SetActive(false);
        option.SetActive(false);
        //optionUI.SetActive(false);
        StartCoroutine("ResumeMenu");
    }
    
    IEnumerator ResumeMenu()
    {
        yield return new WaitForSeconds(0.1f);
        resume.SetActive(true);
        StartCoroutine("RestartMenu");
    }

    IEnumerator RestartMenu()
    {
        yield return new WaitForSeconds(0.1f);
        restart.SetActive(true);
        StartCoroutine("OptionMenu");
    }

    IEnumerator OptionMenu()
    {
        yield return new WaitForSeconds(0.1f);
        option.SetActive(true);
        Pausing();
    }

    void Pausing()
    {
        if(isPausing)
        {
            return;
        }
        isPausing = true;
    }

    public void Resume()
    {
        menu.SetActive(false);
        //optionUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Option()
    {
        menu.SetActive(false);
        //optionUI.SetActive(true);
        Time.timeScale = 0;
    }
}
