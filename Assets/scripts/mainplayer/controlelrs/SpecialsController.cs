using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainplayer.views;
using mainplayer.models;

namespace mainplayer.controllers
{
	public class SpecialsController : MonoBehaviour {

		[SerializeField]Material				m_path_start_material;
		[SerializeField]Material				m_path_end_material;
		List<Vector3> 							m_org_locations_list = new List<Vector3> ();
		List<GameObject> 						m_org_objects_list = new List<GameObject> ();
		List<Color>								m_org_path_colors = new List<Color>();

		bool									m_exploded=false;
		bool									m_show_path=false;

		static SpecialsController			    m_instance;

		public static SpecialsController Instance
		{
			get{

				if (m_instance == null)
				{
					SpecialsController manager = GameObject.FindObjectOfType<SpecialsController> ();
					m_instance = manager.GetComponent<SpecialsController> ();
				}
				return m_instance;
			}
		}

		public void ShowPathScenario()
		{
			

			if (ScenarioController.Instance.Active_scenario_parts != null) {

				int num_scenario_parts = ScenarioController.Instance.Active_scenario_parts.Count;

				Material[] mats = new Material[num_scenario_parts];
				Color[] cols = new Color[num_scenario_parts];
				for (int i = 0; i <num_scenario_parts ; i++) {

					int k = i;

					if (m_show_path) {
						m_org_path_colors.Add (ScenarioController.Instance.Active_scenario_parts [i].GetComponent<Renderer> ().material.color);
						ScenarioController.Instance.Active_scenario_parts [i].GetComponent<Renderer> ().material.color = Color.Lerp (Color.white, Color.red, (float)k / num_scenario_parts);
					} else {
						ScenarioController.Instance.Active_scenario_parts [i].GetComponent<Renderer> ().material.color = m_org_path_colors [i];
					}
				}
			}
		}

		public void ResetToggles()
		{
			m_org_path_colors.Clear ();
			m_show_path = false;
			m_exploded = false;
		}


		public void TogglePath()
		{
			if (m_show_path)
			{
				m_show_path = false;
				ShowPathScenario ();

			}
			else 
			{
				m_org_path_colors.Clear ();
				m_show_path = true;
				ShowPathScenario ();

			}

		}

		public void ToggleExplode()
		{
			if (m_exploded) {
				Implode ();
				m_exploded = false;
			} else {
				Arrange ();
				m_exploded = true;
			}

		}

		private void Implode()
		{
			Debug.Log ("Implode");
			for (int i = 0; i < m_org_objects_list.Count; i++) 
			{
				iTween.MoveTo (m_org_objects_list[i], iTween.Hash ("position", m_org_locations_list[i], "easeType", "easeOutCubic", "time", 1f));
			}
		}

		private void Arrange()
		{
			Debug.Log ("explode");
			m_org_locations_list.Clear ();
			m_org_objects_list.Clear ();

			float initial_radius = 17f;
			float radius = 30f;
			float height = 1f;

			List<GameObject> list = new List<GameObject> ();

			Transform root_transform = SceneController.Instance.Main_object.transform;

			for (int i = 0; i < root_transform.childCount; i++)
			{
				GameObject G = root_transform.GetChild (i).gameObject;

				list.Add(G);
				m_org_locations_list.Add (G.GetComponent<Transform> ().localPosition);
				m_org_objects_list.Add (G);

				if (G.transform.childCount > 0) 
				{
					for (int j = 0; j < G.transform.childCount; j++) 
					{
						GameObject G2 = G.transform.GetChild (j).gameObject;

						list.Add(G2);
						m_org_locations_list.Add (G2.GetComponent<Transform> ().localPosition);
						m_org_objects_list.Add (G2);
					}
				}

			}

			float offset_x = 0;

			//i=0 is the first point - easy to calculate

			float centerX = 0;
			float centerZ = 0;
			float total_angle = 0;

		
			float point_x = 0;
			float point_y = 0;
			float point_height = 0;

	
			float Distance = 0;
			float added_angle = 0;

			for (int i = 1; i < list.Count-1; i++) 
			{
				Distance = Mathf.Max (list [i].GetComponent<Renderer> ().bounds.extents.x, list [i].GetComponent<Renderer> ().bounds.extents.z) + Mathf.Max (list [i+1].GetComponent<Renderer> ().bounds.extents.x, list [i+1].GetComponent<Renderer> ().bounds.extents.z);
				added_angle = Mathf.Atan(Distance/radius);

				total_angle += added_angle;

				point_x = Mathf.Sin (total_angle) * radius;
				point_y = Mathf.Cos (total_angle) * radius;
				Debug.Log (Mathf.Rad2Deg*total_angle);

				point_height = (int)((Mathf.Rad2Deg * total_angle) / 360) * 2f;

				radius = initial_radius - point_height*2;

				iTween.MoveTo (list [i], iTween.Hash ("position", new Vector3 (point_x, point_height, point_y), "easeType", "easeOutCubic", "time", 1f));


			}

		}


	}
}