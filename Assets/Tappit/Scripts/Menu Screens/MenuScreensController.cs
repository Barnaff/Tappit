using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreensController : MonoBehaviour {


	#region Private Properties

	[SerializeField]
	private MenuScreenBaseController _mainMenuController;

	[SerializeField]
	private MenuScreenBaseController _levelSelectionController;

	[SerializeField]
	private MenuScreenBaseController _currentScreen = null;

	#endregion



	#region Instance

	private static MenuScreensController _instance;

	void Awake()
	{
		_instance = this;
	}

	void OnDisable()
	{
		_instance = null;
	}

	public static MenuScreensController Instance
	{
		get
		{
			return _instance;
		}
	}

	#endregion


	#region Public

	public void MainMenu()
	{
		ShowScreen(_mainMenuController);
	}

	public void LevelSelection()
	{
		ShowScreen(_levelSelectionController);
	}

	#endregion


	#region Private

	private void ShowScreen(MenuScreenBaseController screenController)
	{
		if (_currentScreen != null)
		{
			_currentScreen.Hide();
		}

		if (screenController == null)
		{
			Debug.Log("ERROR - missing menu screen controller!");
		}

		screenController.Show();

		_currentScreen = screenController;
	}

	#endregion
}


public class MenuScreenBaseController : MonoBehaviour
{

	void Awake()
	{
		this.gameObject.SetActive(false);
	}

	#region Public

	public virtual void Show()
	{
		this.gameObject.SetActive(true);
	}

	public virtual void Hide()
	{
		this.gameObject.SetActive(false);
	}

	#endregion
}
