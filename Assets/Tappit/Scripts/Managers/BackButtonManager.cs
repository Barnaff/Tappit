using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonManager : Kobapps.Singleton<BackButtonManager>
{

    #region Private

    private List<IBackButtonListener> _litenersQueue;

    #endregion

    #region Update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButtonPressed();
        }
    }

    #endregion


    #region Public

    public void Init()
    {
        _litenersQueue = new List<IBackButtonListener>();
    }

    public void RegisterListener(IBackButtonListener listener)
    {
        if (_litenersQueue.Contains(listener))
        {
            _litenersQueue.Remove(listener);
        }

        _litenersQueue.Add(listener);
    }

    public void RemoveListener(IBackButtonListener listener)
    {
        if (_litenersQueue.Contains(listener))
        {
            _litenersQueue.Remove(listener);
        }
    }

    #endregion

    #region Private

    private void BackButtonPressed()
    {
        if (_litenersQueue.Contains(null))
        {
            _litenersQueue.RemoveAll(item => item == null);
        }

        if (_litenersQueue.Count > 0)
        {
            IBackButtonListener listener = _litenersQueue[_litenersQueue.Count - 1];

            listener.BackButtonCallback();

            _litenersQueue.Remove(listener);
        }
    }



    #endregion
}

public interface IBackButtonListener
{
    void BackButtonCallback();
}
