﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MenuScreenBaseController {


	#region Public

    public void PlayButtonAction()
    {
        MenuScreensController.Instance.LevelSelection();
    }

	#endregion

}
