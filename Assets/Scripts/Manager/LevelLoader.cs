using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject CharacterModels;
    public GameObject loadingScreen;
    public GameObject RoomMenu;
    public Slider slider;
    public Text ProgressText;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        RoomMenu.SetActive(false);
        CharacterModels.SetActive(false);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            ProgressText.text = progress * 100f - 1 + "%";

            yield return null;
        }
    }
}
