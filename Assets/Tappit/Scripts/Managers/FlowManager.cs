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

        AccountManager.Instance.LastPlayedLevelID = level.LevelID;

        Kobapps.SceneLoaderutil.LoadSceneAsync(GeneratedConstants.Scenes.GameScene, () =>
        {

        }, Kobapps.eSceneTransition.FadeOutFadeIn);
    }

    public void NextLevel()
    {
        LevelDefenition currentLevel = GameSetupManager.Instance.SelectedLevel;
        int currentLevelIndex = LevelsSettigs.Instance.Levels.IndexOf(currentLevel);
        if (currentLevelIndex <= LevelsSettigs.Instance.Levels.Count)
        {
            LevelDefenition nextLevel = LevelsSettigs.Instance.Levels[currentLevelIndex + 1];

            StartLevel(nextLevel);
        }
        else
        {
            Debug.Log("No next level");
        }
    }

}
