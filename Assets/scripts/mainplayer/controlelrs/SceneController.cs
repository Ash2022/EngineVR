﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainplayer.views;
using mainplayer.models;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace mainplayer.controllers
{

	public class SceneController : MonoBehaviour {



		[SerializeField]GameObject			m_main_object_prefab=null;

		GameObject							m_main_object=null;

		//[SerializeField]List<Material>		m_scene_materials = new List<Material> ();
		[SerializeField]PartDisplayView		m_display;
		List<GameObject> 					m_scene_objects = new List<GameObject> ();
		[SerializeField]Transform			m_stage=null;
		static SceneController			    m_instance;
		bool								m_meterials_transparent=false;//0-opaque 1- trans

		float 								m_scale_factor=0.01f;
		float 								m_rotate_factor=0.5f;
		VRInput								m_vrinput=null;
			

		[SerializeField]SelectionSlider			m_play_button;
		[SerializeField]SelectionSlider			m_auto_play_button;
		[SerializeField]SelectionSlider			m_explode_button;
		[SerializeField]SelectionSlider			m_blend_button;
		[SerializeField]SelectionSlider			m_sclae_button;
		[SerializeField]SelectionSlider			m_path_button;
		[SerializeField]SelectionSlider			m_show_me_button;

		[SerializeField]PartDisplayView			m_part_display;

		public static SceneController Instance
		{
			get{

				if (m_instance == null)
				{
					SceneController manager = GameObject.FindObjectOfType<SceneController> ();
					m_instance = manager.GetComponent<SceneController> ();
				}
				return m_instance;
			}
		}

		void Start()
		{
			CreateMainObjectAndResetStage ();
			ModelManager.Instance.Init ();
			ValidateSceneObjects ();

			m_vrinput = GetComponent<VRInput> ();

			m_vrinput.OnCancel += CreateMainObjectAndResetStage;

			m_play_button.OnBarFilled += PlayScenarioClicked;
			m_auto_play_button.OnBarFilled += AutoPlayClicked;
			m_explode_button.OnBarFilled += ExplodeClicked;
			m_blend_button.OnBarFilled += BlendClicked;
			m_sclae_button.OnBarFilled += ScaleClicked;
			m_path_button.OnBarFilled += PathClicked;
			m_show_me_button.OnBarFilled += ShowMeClicked;

		}

		private void CreateMainObjectAndResetStage()
		{
			m_stage.GetComponent<Transform> ().localEulerAngles = new Vector3 (0f, 90f, 0);
		
		//	if (m_main_object != null)
			//	Destroy (m_main_object);

			m_main_object = (GameObject)Instantiate (m_main_object_prefab);
			m_main_object.transform.SetParent (m_stage);
			m_main_object.GetComponent<Transform> ().localPosition = new Vector3 (0f, 0.09f, 0f);
			m_main_object.GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
			m_main_object.GetComponent<Transform> ().localEulerAngles = new Vector3 (0f, 0f, 0f);



		}

		void Update()
		{
			if (Input.GetAxis("LeftRight")!=0)
				LeftRight(Input.GetAxis("LeftRight"));
			
			if (Input.GetAxis("UpDown")!=0)
				UpDown(Input.GetAxis("UpDown"));
		}

		private void LeftRight(float dir)
		{

			float adder = dir * m_rotate_factor;

			Vector3 curr = m_stage.GetComponent<Transform> ().localEulerAngles;

			m_stage.GetComponent<Transform> ().localEulerAngles = new Vector3 (curr.x, curr.y + adder,curr.z);

		}

		private void UpDown(float dir)
		{

			float adder = dir * m_scale_factor;

			Vector3 curr = m_main_object.GetComponent<Transform> ().localScale;

			m_main_object.GetComponent<Transform> ().localScale = new Vector3 (curr.x+adder, curr.y+adder, curr.z+adder);

		}

		private void PlayScenarioClicked ()
		{
			ScenarioController.Instance.PlayScenario (false);
		}

		private void AutoPlayClicked ()
		{
			ScenarioController.Instance.PlayScenario (true);
		}

		private void ExplodeClicked ()
		{
			SpecialsController.Instance.ToggleExplode ();
		}

		private void BlendClicked ()
		{
			FlipAllMaterialsBlend ();
		}

		private void ScaleClicked ()
		{
			FlipScale ();
			m_stage.GetComponent<Transform> ().localEulerAngles = new Vector3 (0,90f,0);
		}

		private void PathClicked ()
		{
			SpecialsController.Instance.TogglePath ();
		}

		private void ShowMeClicked()
		{
			m_part_display.PartDisplayClicked ();
		}

		public void FlipAllMaterialsBlend()
		{
			if (m_meterials_transparent)
				m_meterials_transparent = false;
			else
				m_meterials_transparent = true;

			SetAllInstanceMaterials (m_meterials_transparent);
		}

		public void FlipScale()
		{
			m_main_object.GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
		}
			


		public void SetAllInstanceMaterials(bool trans)
		{
			for (int i = 0; i < m_scene_objects.Count; i++) 
			{
				Material[] mats = m_scene_objects [i].GetComponent<MeshRenderer> ().materials;

				for (int j = 0; j < mats.Length; j++) {

					SetMaterialToTransparent (mats [j],trans);
				}

			}
		}

		public void SetMaterialToTransparent(Material m,bool trans)
		{
			if (trans) {

				m.SetInt ("_Mode", 3);

				Color curr_color = m.color;

				curr_color.a = 0.5f;

				m.SetColor ("_Color", curr_color);

				m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				m.SetInt ("_ZWrite", 0);
				m.DisableKeyword ("_ALPHATEST_ON");
				m.DisableKeyword ("_ALPHABLEND_ON");
				m.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
				m.renderQueue = 3000;
			}
			else 
			{
				m.SetInt ("_Mode", 0);

				Color curr_color = m.color;

				curr_color.a = 1f;

				m.SetColor ("_Color", curr_color);

				m.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
				m.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
				m.SetInt ("_ZWrite", 1);
				m.DisableKeyword ("_ALPHATEST_ON");
				m.DisableKeyword ("_ALPHABLEND_ON");
				m.DisableKeyword ("_ALPHAPREMULTIPLY_ON");
				m.renderQueue = -1;
			}
		}


		private void ValidateSceneObjects()
		{
			Transform root_transform = m_main_object.transform;

			for (int i = 0; i < root_transform.childCount; i++)
			{
				GameObject G = root_transform.GetChild (i).gameObject;

				m_scene_objects.Add(G);

				if (G.transform.childCount > 0) 
				{
					for (int j = 0; j < G.transform.childCount; j++) 
					{
						GameObject G2 = G.transform.GetChild (j).gameObject;

						m_scene_objects.Add(G2);

					}
				}

			}
		}

		public List<GameObject> Scene_objects {
			get {
				return m_scene_objects;
			}
			set {
				m_scene_objects = value;
			}
		}

		public PartDisplayView Display {
			get {
				return m_display;
			}
			set {
				m_display = value;
			}
		}


		public GameObject Main_object {
			get {
				return m_main_object;
			}
		}
	}
}