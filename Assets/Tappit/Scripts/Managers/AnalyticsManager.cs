using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnalyticsEvents
{
    public static class AnalyticsScreens
    {
        public const string MainMenu = "MainMenu";
        public const string LevelsSelection = "LevelsSelection";
        public const string Gameplay = "Gameplay";

    }


}

public class AnalyticsManager : Kobapps.Singleton<AnalyticsManager> {

	
    public void ScreenView(string screenName)
    {

    }

    public void ButtonClick(string buttonTitle)
    {

    }

    public void GameplayEvent(string eventName, Hashtable eventData)
    {

    }

}

