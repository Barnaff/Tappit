using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : Kobapps.Singleton<FlowManager> {

	
    public void FirstScreen()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MainMenuScene, () =>
        {
			MenuScreensController.Instance.MainMenu();

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void LevelsSelectionScreen()
    {
        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.MainMenuScene, () =>
        {
				MenuScreensController.Instance.LevelSelection();

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void StartLevel(LevelDefenition level)
    {
        GameSetupManager.Instance.SelectedLevel = level;

        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.GameScene, () =>
        {

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void NextLevel()
    {
        LevelDefenition currentLevel = GameSetupManager.Instance.SelectedLevel;
        int currentLevelIndex = GameSetupManager.Instance.CurrentChepter.Levels.IndexOf(currentLevel);
        if (currentLevelIndex <= GameSetupManager.Instance.CurrentChepter.Levels.Count)
        {
            LevelDefenition nextLevel = GameSetupManager.Instance.CurrentChepter.Levels[currentLevelIndex + 1];

            StartLevel(nextLevel);
        }
        else
        {
            Debug.Log("No next level");
        }
    }

}
