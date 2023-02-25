using UnityEngine;
using UnityEngine.InputSystem;

public enum HideUIState : int
{
	None = 0,
	UI,
	Player
}

public class HideUI : MonoBehaviour
{
	public InputActionReference toggleButton;

	public GameObject[] uiObjects;
	public GameObject[] playerObjects;

	[SerializeField]
	private HideUIState state = HideUIState.None;

	private void Start() => toggleButton.action.started += OnToggleUI;
	private void OnDestroy() => toggleButton.action.started -= OnToggleUI;

	private void OnToggleUI(InputAction.CallbackContext obj)
	{
		state = (state >= HideUIState.Player) ? HideUIState.None : (state + 1);
		foreach (GameObject go in uiObjects)
			go.SetActive(state < HideUIState.UI);
		foreach (GameObject go in playerObjects)
			go.SetActive(state < HideUIState.Player);
	}
}
