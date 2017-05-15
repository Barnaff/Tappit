using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAssets : Kobapps.ScriptableSingleton<GameplayAssets> {

    public GameObject VertialFlipIndicator;

    public GameObject HorizontalFlipIndicator;

    public GameObject LockedIndicitator;

    public GameObject LinkedAIndicator;

    public GameObject LinkedBIndicator;

    public GameObject LinkedCIndicator;

    public GameObject LinkedDIndicator;


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
            case eTileType.Locked:
                {
                    return LockedIndicitator;
                }
            case eTileType.LinkA:
                {
                    return LinkedAIndicator;
                }
            case eTileType.LinkB:
                {
                    return LinkedBIndicator;
                }
            case eTileType.LinkC:
                {
                    return LinkedCIndicator;
                }
            case eTileType.LinkD:
                {
                    return LinkedDIndicator;
                }
        }

        return null;
        
    }

}
