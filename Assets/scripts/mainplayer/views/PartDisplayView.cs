using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mainplayer.models;
using mainplayer.controllers;
using TMPro;
using VRStandardAssets.Utils;

namespace mainplayer.views
{

	public class PartDisplayView : MonoBehaviour {

		public delegate	void 			HelpRequstedDelegate();

		[SerializeField]TMP_Text 			m_part_description;
		[SerializeField]TMP_Text 			m_inst_description;

		[SerializeField]Image				m_display_bg_image=null;

		[SerializeField]GameObject		m_dup_object_holder;
		GameObject						m_dup_object=null;
		Part							m_part_model;

		HelpRequstedDelegate			m_help_requested_delegate;	

		int 							m_currently_showing=0;//0-nothing 1 - desc tet 2 - object


		private VRInteractiveItem 		m_InteractiveItem;  

		public void Reset()
		{
			m_currently_showing = 0;

			m_InteractiveItem = GetComponent<VRInteractiveItem>();

			if (m_InteractiveItem != null) {

				m_InteractiveItem.OnDown -= PartClicked;
				m_InteractiveItem.OnOver -= LookingAtPart;
				m_InteractiveItem.OnOut -= StopLookingAtPart;
			}

			if (m_dup_object != null)
				Destroy (m_dup_object);

			m_part_description.text = "Post Reset";
			m_inst_description.text = "Post Reset";

			m_help_requested_delegate = null;

			m_part_model = null;

		}

		public void LinkInteractive()
		{
			m_InteractiveItem = GetComponent<VRInteractiveItem>();

			m_InteractiveItem.OnDown -= PartClicked;
			m_InteractiveItem.OnOver -= LookingAtPart;
			m_InteractiveItem.OnOut -= StopLookingAtPart;

			m_InteractiveItem.OnDown += PartClicked;
			m_InteractiveItem.OnOver += LookingAtPart;
			m_InteractiveItem.OnOut += StopLookingAtPart;
		}

		private void LookingAtPart()
		{
			m_display_bg_image.color = Color.red;
		}

		private void StopLookingAtPart()
		{
			m_display_bg_image.color = Color.white;
		}

		private void PartClicked()
		{
			PartDisplayClicked ();
		}

		public void SetModel(Part part,HelpRequstedDelegate help_requested)
		{
			Debug.Log ("Setting model");

			m_currently_showing = 0;

			m_help_requested_delegate = help_requested;

			if (m_dup_object != null)
				Destroy (m_dup_object);
			
			m_part_model = part;

			LinkInteractive ();

			ShowText ();

		}

		public void PartDisplayClicked()
		{
			Debug.Log ("Part disp clicked");

			if (m_currently_showing == 1)
				ShowObject ();
			else if (m_currently_showing == 2) {
				//need to report that user asked to show him the actual part moving
				if (m_help_requested_delegate != null)
					m_help_requested_delegate ();
			}
		}

		public void UpdateInstText(int index)
		{
			if (index == -1) {
				m_inst_description.text = "";
			} else {
				if (m_part_model.GetInstruction (index).Manual_description != "")
					m_inst_description.text = m_part_model.GetInstruction (index).Manual_description;
				else
					m_inst_description.text = m_part_model.GetInstruction (index).Description;
			}
		}

		private void ShowText()
		{
			m_currently_showing = 1;
			m_part_description.gameObject.SetActive (true);
			m_part_description.text = m_part_model.Description;
		}

		private void HideText()
		{
			m_part_description.gameObject.SetActive (false);
		}

		private void ShowObject()
		{
			HideText ();
			m_currently_showing = 2;

			m_dup_object = (GameObject)Instantiate (ScenarioController.Instance.GetCurrPart());
			RectTransform rect = m_dup_object.AddComponent<RectTransform> ();
			m_dup_object.transform.SetParent (m_dup_object_holder.transform);

			m_dup_object.GetComponent<MeshCollider> ().enabled = false;

			m_dup_object.GetComponent<Renderer> ().material = ScenarioController.Instance.Over_mat;


			rect.localPosition = new Vector3 (0, 0, 0);
			rect.localScale = new Vector3 (500f, 500f, 500f);

			Camera main_cam = ManagerView.Instance.Camera.GetComponent<Camera>();
			Bounds	obj_bounds = m_dup_object.GetComponent<Renderer> ().bounds;

			Vector3 origin = main_cam.WorldToScreenPoint (new Vector3 (obj_bounds.min.x, obj_bounds.max.y,obj_bounds.center.z));
			Vector3 extent = main_cam.WorldToScreenPoint (new Vector3 (obj_bounds.max.x, obj_bounds.min.y,obj_bounds.center.z));

			/*
			Debug.Log (obj_bounds.extents.x*500f);
			Debug.Log (obj_bounds.extents.y*500f);
			Debug.Log (obj_bounds.extents.z*500f);

			Debug.Log (origin);
			Debug.Log (extent);
*/
			Vector3 temp = new Vector3(obj_bounds.max.x-obj_bounds.min.x,obj_bounds.max.y - obj_bounds.min.y,0);
			/*
			Debug.Log (main_cam.WorldToViewportPoint (origin));
			Debug.Log (main_cam.WorldToViewportPoint (extent));

			Debug.Log (main_cam.WorldToScreenPoint (origin));
			Debug.Log (main_cam.WorldToScreenPoint (extent));

			Debug.Log (main_cam.ScreenToWorldPoint (origin));
			Debug.Log (main_cam.ScreenToWorldPoint (extent));

			Debug.Log (main_cam.ScreenToViewportPoint (origin));
			Debug.Log (main_cam.ScreenToViewportPoint(extent));
*/


			//rect.anchorMin = new Vector2 (0, 0);
			//rect.anchorMax = new Vector2 (300f, 300f);



			iTween.RotateAdd (m_dup_object, iTween.Hash ("z", 360f, "islocal",true, "easeType", "linear", "time", 5f,"looptype","loop"));


		}


	}
}