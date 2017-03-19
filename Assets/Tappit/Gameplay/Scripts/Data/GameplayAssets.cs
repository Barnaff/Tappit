using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAssets : Kobapps.ScriptableSingleton<GameplayAssets> {

    public GameObject VertialFlipIndicator;

    public GameObject HorizontalFlipIndicator;


    public GameObject IndicatorForTile(eTileType tileType)
    {
        switch (tileType)
        {
            case eTileType.LineHorizontal:
                {
                    return HorizontalFlipIndicator;
                }
            case eTileType.LineVertial:
                {
                    return VertialFlipIndicator;
                }
        }

        return null;
        
    }

}
