using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainplayer.models;
using VRStandardAssets.Utils;
using mainplayer.controllers;

namespace mainplayer.views
{

	public class PartView : MonoBehaviour {

		public delegate	void 			PartClickedDelegate();

		[SerializeField]GameObject		m_object;
		Part 							m_part_model;
		Material						m_org_material;		
		Material						m_over_material;
		Renderer						m_renderer;
		PartClickedDelegate				m_part_clicked_delegate;
		AudioClip						m_clip;

		private VRInteractiveItem 		m_InteractiveItem;  

		public void Start()
		{
			m_InteractiveItem = GetComponent<VRInteractiveItem>();
			m_renderer = GetComponent<Renderer> ();
			m_InteractiveItem.OnDown += PartClicked;
			m_InteractiveItem.OnOver += LookingAtPart;
			m_InteractiveItem.OnOut += StopLookingAtPart;
		}


		public void WaitForClick(PartClickedDelegate part_clicked_delegate)
		{
			m_part_clicked_delegate = part_clicked_delegate;
		}

		private void LookingAtPart()
		{
			SetMaterial (ScenarioController.Instance.Over_mat);
		}

		private void StopLookingAtPart()
		{
			SetMaterial (m_org_material);
		}

		private void PartClicked()
		{
			if(m_part_clicked_delegate!=null)
				m_part_clicked_delegate ();
		}

		void OnMouseDown()
		{
			if(m_part_clicked_delegate!=null)
				m_part_clicked_delegate ();
		}

		public PartView(Part part)
		{
			m_part_model = part;
		}

		public void SetPartModel(Part part)
		{
			m_part_model = part;
			m_object = gameObject;
			m_renderer = GetComponent<Renderer> ();
			m_org_material = m_renderer.material;

		}

		public void SetMaterial(Material mat)
		{
			
			if (m_renderer.material != null && m_renderer !=null)
					m_renderer.material = mat;
		}

		public void SetDefaultMaterial()
		{
			m_renderer.material = m_org_material;
		}

		public GameObject Object {
			get {
				return m_object;
			}
		}

		public Part Part_model {
			get {
				return m_part_model;
			}
		}

		public Material Org_material {
			get {
				return m_org_material;
			}
		}

		public AudioClip Clip {
			get {
				return m_clip;
			}
			set {
				m_clip = value;
			}
		}
	}
}