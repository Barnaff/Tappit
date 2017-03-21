using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintsShopPopupController : PopupBaseController {

    #region Private Properties

    [SerializeField]
    private GameObject _shopItemButton;

    [SerializeField]
    private Transform _buttonsContent;

    [SerializeField]
    private List<CanvasGroup> _canvasGroups;

    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    #region Private

    private void PopulateShop()
    {

    }

    #endregion
}
