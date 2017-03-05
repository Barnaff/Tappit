using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MenuScreenBaseController {


	#region Public

    public void PlayButtonAction()
    {
        LevelDefenition level = LevelsSettigs.Instance.Levels[0];

        FlowManager.Instance.StartLevel(level);
    }

	#endregion

}
