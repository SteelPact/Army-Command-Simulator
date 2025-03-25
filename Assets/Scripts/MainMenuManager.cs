using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{

    public GameObject canvasMain;
    public GameObject canvasDifficulty;
    public GameObject canvasTutorial;
    public GameObject canvasSettings;
    public GameObject picturesTutorial;
    public AudioMixer audioMixer;

    public int slotToReset;

    public void Load(int slot)
    {

        PlayerPrefs.SetInt("Slot", slot);

        if(PlayerPrefs.HasKey(slot.ToString() + "Money"))
        {

            SceneManager.LoadScene("Headquarters");

        }
        else
        {

            canvasMain.SetActive(false);
            canvasDifficulty.SetActive(true);

        }

    }

    public void New(string nationAndDifficulty)
    {

        PlayerPrefs.SetString(PlayerPrefs.GetInt("Slot").ToString() + "Nation", nationAndDifficulty.Split()[0]);
        PlayerPrefs.SetString(PlayerPrefs.GetInt("Slot").ToString() + "Difficulty", nationAndDifficulty.Split()[1]);

        SceneManager.LoadScene("Headquarters");

    }

    public void Reset(int slot)
    {
    
        slotToReset = slot;
        PlayerPrefs.DeleteKey(slot.ToString() + "Money");
        PlayerPrefs.DeleteKey(slot.ToString() + "Level");
        for(int i = 1; i <= 5; i++)
        {

            PlayerPrefs.DeleteKey(slot.ToString() + "Unit" + i.ToString());
            PlayerPrefs.DeleteKey(slot.ToString() + "Hitpoints" + i.ToString());
            PlayerPrefs.DeleteKey(slot.ToString() + "Index" + i.ToString());

        }

    }

    public void Quit()
    {

        Application.Quit();

    }

    public void Restart()
    {

        canvasDifficulty.SetActive(false);
        canvasMain.SetActive(true);

    }

    public void Tutorial()
    {

        canvasMain.SetActive(!canvasMain.activeSelf);
        canvasTutorial.SetActive(!canvasTutorial.activeSelf);
        picturesTutorial.SetActive(!picturesTutorial.activeSelf);

    }

    public void Settings()
    {

        canvasMain.SetActive(!canvasMain.activeSelf);
        canvasSettings.SetActive(!canvasSettings.activeSelf);

    }

    public void SetQuality(int qualityLevel)
    {

        QualitySettings.SetQualityLevel(qualityLevel);

    }

    public void SetVolume(float volume)
    {

        audioMixer.SetFloat("Volume", volume);

    }

}
