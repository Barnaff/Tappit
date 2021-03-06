﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupsManager : Kobapps.Singleton<PopupsManager> {

    #region private properties

    [SerializeField]
    private GameObject _overlayController;

    [SerializeField]
    private List<PopupBaseController> _activePopups;

    #endregion


    #region Public

    public T DisplayPopup<T>(System.Action closePopupAction = null) where T : PopupBaseController
    {
        PopupBaseController popupControllerPrefab = GetPopupPrefab<T>();
        if (popupControllerPrefab != null)
        {
            PopupBaseController popupController = Instantiate(popupControllerPrefab) as PopupBaseController;
            popupController.SetCloseAction(closePopupAction);
            if (_activePopups == null)
            {
                _activePopups = new List<PopupBaseController>();
            }
            _activePopups.Add(popupController);

            popupController.OnPopupRemoved += (popup) =>
            {
                if (_activePopups.Contains(popup))
                {
                    _activePopups.Remove(popup);
                }
            };

            return popupController as T;
        }
        return null;
    }

    public void ClosePopup<T>() where T : PopupBaseController
    {
        List<PopupBaseController> popupsToClose = new List<PopupBaseController>();
        foreach (PopupBaseController popupController in _activePopups)
        {
            if (popupController != null && popupController.GetType() == typeof(T))
            {
                popupsToClose.Add(popupController);   
            }
        }

        foreach (PopupBaseController popupToClose in popupsToClose)
        {
            ClosePopup(popupToClose);
        }

        _activePopups.Remove(null);
    }

    public void ClosePopup(PopupBaseController popupController)
    {
        popupController.ClosePopup();
    }

    public void CloseAllPopups()
    {
        foreach (PopupBaseController popupController in _activePopups)
        {
            ClosePopup(popupController);
        }
    }

    #endregion


    #region Private

    public PopupBaseController GetPopupPrefab<T>() where T : PopupBaseController
    {
        T[] popupsPrefabs = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];
        if (popupsPrefabs != null && popupsPrefabs.Length > 0)
        {
            return popupsPrefabs[0] as PopupBaseController;
        }

        T popupPrefab = Resources.Load<T>(typeof(T).ToString());
        if (popupPrefab != null)
        {
            return popupPrefab;
        }

        Debug.LogError("Could not load popup: " + typeof(T).ToString());
        return null;
    }

    #endregion
}


public class PopupBaseController : MonoBehaviour, IBackButtonListener
{
    #region Private Properties

    [SerializeField]
    private bool _displyOverlay;

    [SerializeField]
    private bool _isSingle;

    [SerializeField]
    private bool _dontDestoryOnLoad;

    [SerializeField]
    private bool _closeOnBackButton;

    private System.Action _closePopupAction;

    private System.Action <PopupBaseController> _onPopupRemoved;

    #endregion


    #region Lifecycle

    void OnEnable()
    {
        if (_closeOnBackButton)
        {
            BackButtonManager.Instance.RegisterListener(this);
        }
    }

    void OnDisable()
    {
        if (_closeOnBackButton)
        {
            BackButtonManager.Instance.RemoveListener(this);
        }
        if (_onPopupRemoved != null)
        {
            _onPopupRemoved(this);
            _onPopupRemoved = null;
        }
    }

    #endregion


    #region Public

    protected virtual void DisplayPopupCloseAnimation(System.Action completionAction)
    {
        if (completionAction != null)
        {
            completionAction();
        }
    }

    #endregion


    #region Public

    public void ClosePopup(System.Action closeAction = null)
    {
        DisplayPopupCloseAnimation(()=>
        {
            if (_closePopupAction != null)
            {
                _closePopupAction();
            }
            if (closeAction != null)
            {
                closeAction();
            }
            if (_onPopupRemoved != null)
            {
                _onPopupRemoved(this);
                _onPopupRemoved = null;
            }
            Destroy(this.gameObject);
        });
    }

    public System.Action OnCloseAction
    {
        set
        {
            _closePopupAction = value;
        }
        get
        {
            return _closePopupAction;
        }
    }

    public System.Action <PopupBaseController> OnPopupRemoved
    {
        set
        {
            _onPopupRemoved = value;
        }
        get
        {
            return _onPopupRemoved;
        }
    }

    public void SetCloseAction(System.Action closePopupAction)
    {
        _closePopupAction = closePopupAction;
    }

    public void BackButtonCallback()
    {
        ClosePopup();
    }

    #endregion
}
