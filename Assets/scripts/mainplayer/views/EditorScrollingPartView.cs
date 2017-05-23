using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mainplayer.controllers;


namespace mainplayer.views
{

	public class EditorScrollingPartView : MonoBehaviour {

		[SerializeField]Text 		m_text=null;
		[SerializeField]Image		m_image=null;
		int							m_index;


		public void ButtonClicked()
		{
			EditorController.Instance.UnHighlightSelectedPart ();
			EditorController.Instance.SetSelectedPart (m_index);
			m_image.color = new Color (0.5f, 1f, 1f);
		}

		public void UnHighLightMe()
		{
			m_image.color = Color.white;
		}

		public Text Text {
			get {
				return m_text;
			}
		}

		public int Index {
			get {
				return m_index;
			}
			set {
				m_index = value;
			}
		}
	}
}