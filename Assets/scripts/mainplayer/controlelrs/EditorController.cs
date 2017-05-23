using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mainplayer.models;
using System;
using mainplayer.views;
using SimpleJSON;
using System.IO;

namespace mainplayer.controllers
{

	public class EditorController : MonoBehaviour {


		[SerializeField]GameObject			m_scrolling_object=null;
		[SerializeField]GameObject			m_scrolling_part_object=null;
		[SerializeField]GameObject			m_scrolling_instruction_object=null;

		[SerializeField]GameObject			m_parts_list;
		[SerializeField]GameObject			m_instructions_list;

		List<Part>							m_parts_model = new List<Part>();

		[SerializeField]Transform			m_scrolling_objects_holder=null;
		[SerializeField]Transform			m_scrolling_parts_holder=null;
		[SerializeField]Transform			m_scrolling_instructions_holder=null;

		[SerializeField]Dropdown			m_instructions_dropDown=null; 

		[SerializeField]Text				m_instruction_param;

		[SerializeField]Text				m_scenario_name;
		[SerializeField]Text				m_scenario_desc;
		[SerializeField]InputField			m_instruction_desc;


		int									m_selected_object_index=-1;
		int 								m_selected_part_index = -1;
		int 								m_selected_inst_index = -1;

		static EditorController			    m_instance;

		public static EditorController Instance
		{
			get{

				if (m_instance == null)
				{
					EditorController manager = GameObject.FindObjectOfType<EditorController> ();
					m_instance = manager.GetComponent<EditorController> ();
				}
				return m_instance;
			}
		}

		public void UnHighlightSelectedObject()
		{
			if(m_selected_object_index!=-1)
				m_scrolling_objects_holder.transform.GetChild (m_selected_object_index).GetComponent<EditorScrollingObjectView> ().UnHighLightMe ();
		}
		public void UnHighlightSelectedPart()
		{
			if(m_selected_part_index!=-1)
				m_scrolling_parts_holder.transform.GetChild (m_selected_part_index).GetComponent<EditorScrollingPartView> ().UnHighLightMe ();
		}

		public void UnHighlightSelectedInst()
		{
			if(m_selected_inst_index!=-1)
				m_scrolling_instructions_holder.transform.GetChild (m_selected_inst_index).GetComponent<EditorScrollingInstructionView> ().UnHighLightMe ();
		}

		public void SetSelectedObject(int index)
		{
			m_selected_object_index = index;
		}

		public void SetSelectedPart(int index)
		{
			m_selected_part_index = index;
			RevalidateInstructionsBox ();
		}

		public void SetSelectedInstruction(int index)
		{
			m_selected_inst_index = index;

			if (m_parts_model [m_selected_part_index].GetInstruction (index).Manual_description != "")
				m_instruction_desc.text = m_parts_model [m_selected_part_index].GetInstruction (index).Manual_description;
			else
				m_instruction_desc.text = m_parts_model [m_selected_part_index].GetInstruction (index).Description;


		}

		public void ActiveInstructionDescriptionBeingEdited()
		{
			m_parts_model [m_selected_part_index].GetInstruction (m_selected_inst_index).Manual_description = m_instruction_desc.text;
		}

		public void SetInstructionDescription(string desc)
		{
			m_instruction_desc.text = desc;
		}

		private void RevalidateInstructionsBox()
		{
			for (int i=0;i<m_scrolling_instructions_holder.childCount;i++) {
			
				Destroy (m_scrolling_instructions_holder.GetChild(i).gameObject);
			}

			AddInstructionFromData(m_parts_model[m_selected_part_index].GetAllInstruction());
		}


		public void PrepareEditor()
		{

			m_scrolling_objects_holder.GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f, SceneController.Instance.Scene_objects.Count * 40f);

			for (int i = 0; i < SceneController.Instance.Scene_objects.Count; i++)
			{

				GameObject G = (GameObject)Instantiate (m_scrolling_object);

				G.transform.SetParent (m_scrolling_objects_holder);

				G.GetComponent<RectTransform> ().localPosition = new Vector3 (0, i * -40f, 0);
				G.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
				G.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0f, 0f, 0f);
				G.GetComponent<EditorScrollingObjectView> ().Text.text = SceneController.Instance.Scene_objects [i].name;
				G.GetComponent<EditorScrollingObjectView> ().Index = i;


			}
			int count = Enum.GetValues(typeof(ModelManager.InstructionType)).Length;
			List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();

			foreach (ModelManager.InstructionType inst in Enum.GetValues(typeof(ModelManager.InstructionType))) {
				Dropdown.OptionData opt = new Dropdown.OptionData (inst.ToString ());

				options.Add (opt);
			}

			m_instructions_dropDown.AddOptions (options);


		}

		private void AddInstructionFromData(List<Instruction> inst_data)
		{
			for (int i = 0; i < inst_data.Count; i++) 
			{
				int childs_count = m_scrolling_instructions_holder.transform.childCount;

				Debug.Log ("Child Count :" + childs_count);

				GameObject G = (GameObject)Instantiate (m_scrolling_instruction_object);

				G.transform.SetParent (m_scrolling_instructions_holder);

				G.GetComponent<RectTransform> ().localPosition = new Vector3 (0, i * -40f-30f, 0);
				G.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
				G.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0f, 0f, 0f);
				G.GetComponent<EditorScrollingInstructionView> ().Text.text = inst_data [i].Type.ToString() + inst_data [i].Param.ToString();
				G.GetComponent<EditorScrollingInstructionView> ().Index = i;
			}

		}

		public void AddSelectedInstruction()
		{
			if (m_selected_part_index != -1) {

				if (m_instruction_param.text != "") {
					
					int childs_count = m_scrolling_instructions_holder.transform.childCount;
					GameObject G = (GameObject)Instantiate (m_scrolling_instruction_object);

					G.transform.SetParent (m_scrolling_instructions_holder);

					G.GetComponent<RectTransform> ().localPosition = new Vector3 (0, childs_count * -40f-30f, 0);
					G.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
					G.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0f, 0f, 0f);
					G.GetComponent<EditorScrollingInstructionView> ().Text.text = ((ModelManager.InstructionType)m_instructions_dropDown.value+ m_instruction_param.text); 
					G.GetComponent<EditorScrollingInstructionView> ().Index = childs_count;

					Instruction inst = new Instruction ((ModelManager.InstructionType)m_instructions_dropDown.value, float.Parse(m_instruction_param.text));

					m_parts_model [m_selected_part_index].AddInstruction (inst);
				}
			}
		}



		public void AddSelectedPart()
		{
			if (m_selected_object_index != -1)
			{
				int childs_count = m_scrolling_parts_holder.transform.childCount;
				GameObject G = (GameObject)Instantiate (m_scrolling_part_object);

				G.transform.SetParent (m_scrolling_parts_holder);

				G.GetComponent<RectTransform> ().localPosition = new Vector3 (0, childs_count * -40f-30f, 0);
				G.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
				G.GetComponent<RectTransform> ().localEulerAngles = new Vector3 (0f, 0f, 0f);
				G.GetComponent<EditorScrollingPartView> ().Text.text = SceneController.Instance.Scene_objects [m_selected_object_index].name;
				G.GetComponent<EditorScrollingPartView> ().Index = childs_count;

				m_parts_model.Add(new Part(SceneController.Instance.Scene_objects [m_selected_object_index].name,"desc",new List<Instruction>()));

			}

		}

		public void SaveScenario()
		{
			JSONClass root_node = new JSONClass ();

			root_node ["ScenarioName"] = m_scenario_name.text;
			root_node ["ScenarioDesc"] = m_scenario_desc.text;

			JSONArray parts_arr = new JSONArray ();

			for (int j = 0; j < m_parts_model.Count; j++) {

				JSONClass part_node = new JSONClass ();
				JSONArray inst_arr = new JSONArray ();
				for (int i = 0; i < m_parts_model[j].GetAllInstruction().Count; i++) {

					JSONClass inst_node = new JSONClass ();
					inst_node ["id"] = m_parts_model [j].GetInstruction (i).Type.ToString();
					inst_node ["param"] = m_parts_model [j].GetInstruction (i).Param.ToString();

					if (m_parts_model [j].GetInstruction (i).Manual_description != "")
						inst_node ["manual_desc"] = m_parts_model [j].GetInstruction (i).Manual_description;
					
					inst_arr.Add (inst_node);
				}


				part_node ["PartName"] = m_parts_model [j].Id;
				part_node ["PartDesc"] = m_parts_model [j].Description;
				part_node.Add ("Instruction", inst_arr);

				parts_arr.Add (part_node);
			}

			root_node.Add ("Parts",parts_arr);

			string path = "Assets/Resources/ItemInfo.json";

			string str = root_node.ToString();
			using (FileStream fs = new FileStream(path, FileMode.Create)){
				using (StreamWriter writer = new StreamWriter(fs)){
					writer.Write(str);
				}
			}

			Debug.Log ("USER ACHIEVEMENTS: "+ root_node.ToString());

		}

		public void CreateScenarioFromData()
		{
			string path = "Assets/Resources/ItemInfo.json";
			string JSONData = File.ReadAllText (path);

			JSONClass root = (JSONClass)JSON.Parse (JSONData);

			Scenario scenaio = new Scenario(new GameObject(),root["ScenarioDesc"],new List<Part>());

			JSONArray parts_arr = root ["Parts"].AsArray;

			foreach (JSONNode item in parts_arr) {

				Part curr_part = new Part (item ["PartName"], item ["PartDesc"]);

				JSONArray inst_arr = item ["Instruction"].AsArray;

				foreach (JSONNode inst in inst_arr) {

					curr_part.AddInstruction(new Instruction((ModelManager.InstructionType)inst["id"].AsInt,inst["param"].AsFloat));
					
				}

						scenaio.AddPart(curr_part);
			}
			Debug.Log (scenaio.ToString ());
		}

	}
}