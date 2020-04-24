using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


/**
 * Quick script to have clickable text
 * when a text is clicked on, this script tries to see if this text corresponds to a link
 * If so, it informs the event handler, else it doesn't do anything
 **/
public class ClickableText : MonoBehaviour, IPointerClickHandler {
	[SerializeField] Scrollbar scrollBar;
	EventHandler eh;

	private void Start() {
		eh = FindObjectOfType<EventHandler>();
	}

	public void OnPointerClick(PointerEventData eventData) {
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
		// If mouse's left button is clicked
		if (eventData.button == PointerEventData.InputButton.Left) {
			// Try to find the eventual link of the text clicked
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, Camera.main);
			Debug.Log(linkIndex);
			// If it is equal to -1, it was not found
			if (linkIndex > -1) {
				TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
				string currChoice = linkInfo.GetLinkID();
				// informs the event handler of the new event
				eh.ChangeEvent(currChoice);
				scrollBar.value = 1;
			}
		}
	}

	
}
