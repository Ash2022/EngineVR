using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainplayer.models;
using common;
using mainplayer.views;
using VRStandardAssets.Utils;

namespace mainplayer.controllers
{

	public class ScenarioController : MonoBehaviour {




		public const float 						INSTRUCTION_TIME = 1;
		public const float 						DELAY_BETWEEN_INST = 0.2f;
		[SerializeField]Material				m_High_Light_Mat=null;
		[SerializeField]Material				m_over_mat=null;

		[SerializeField]AudioSource				m_audio_source;

		List<PartView> 							m_active_scenario_parts = new List<PartView> ();

		bool									m_inst_done=false;

		int										m_curr_part_index=0;
		int										m_curr_instruction_index=0;

		bool									m_scenraio_ready=false;

		static ScenarioController			    m_instance;



		public static ScenarioController Instance
		{
			get{

				if (m_instance == null)
				{
					ScenarioController manager = GameObject.FindObjectOfType<ScenarioController> ();
					m_instance = manager.GetComponent<ScenarioController> ();
				}
				return m_instance;
			}
		}


		private void PrepareScenario(Scenario scenario,bool auto)
		{
			//loop over the scene from the root 
			//for each part - need to put a partview on that part

			foreach (Part P in scenario.GetParts()) 
			{
				Transform T = scenario.Root.transform.FindDeepChild(P.Id);

				T.gameObject.AddComponent<MeshCollider> ();

				T.gameObject.AddComponent<VRInteractiveItem> ();

				T.gameObject.AddComponent<PartView> ();

				PartView part_view = T.GetComponent<PartView> ();

				part_view.Clip = Resources.Load<AudioClip> ("Audio/" + P.Id);

				m_active_scenario_parts.Add (part_view);

				part_view.SetPartModel (P);

			}
		
			m_scenraio_ready = true;

			if (auto)
				StartCoroutine(RunScenario1 ());
			else
				RunStepScenario();
		}



		IEnumerator RunScenario1()
		{
			Debug.Log ("Running Scenario ");

			for (int i = 0; i < m_active_scenario_parts.Count; i++) 
			{
				PartView part = m_active_scenario_parts [i];



				part.SetMaterial (m_High_Light_Mat);

				yield return new WaitForSeconds(DELAY_BETWEEN_INST);

				Part	part_model = part.Part_model;

				for (int j = 0; j < part_model.GetAllInstruction ().Count; j++)
				{
					m_inst_done = false;

					Debug.Log (part_model.Description);

					DoInstruction (part_model.GetInstruction (j),part.Object,part_model.Id);
					while (!m_inst_done)
					{
						yield return new WaitForSeconds (0.5f);
					}

					yield return new WaitForSeconds (DELAY_BETWEEN_INST);
				}

				part.SetDefaultMaterial ();

			}

		}

		public GameObject GetCurrPart()
		{
			return m_active_scenario_parts [m_curr_part_index].Object;
		}

		public void RunStepScenario()
		{
			SceneController.Instance.Display.UpdateInstText (-1);
			PlayAudio (m_active_scenario_parts [m_curr_part_index].Clip);
			m_active_scenario_parts [m_curr_part_index].WaitForClick (NextPartClicked);
			SceneController.Instance.Display.SetModel (m_active_scenario_parts [m_curr_part_index].Part_model,HelpRequested);
		}

		public void HelpRequested()
		{
			//user clicked on the display enough times to be asking for help
			//show him the next step
			NextPartClicked();
		}

		public void NextPartClicked()
		{
			//run the parts instructions
			PartView part = m_active_scenario_parts [m_curr_part_index];

			StartCoroutine (ExecutePartInstructions (part));
		}


		IEnumerator ExecutePartInstructions(PartView part_view)
		{
			
			part_view.SetMaterial (m_High_Light_Mat);

			yield return new WaitForSeconds(DELAY_BETWEEN_INST);

			Part	part_model = part_view.Part_model;

			for (int j = 0; j < part_model.GetAllInstruction ().Count; j++)
			{
				m_inst_done = false;

				Debug.Log (part_model.Description);

				SceneController.Instance.Display.UpdateInstText (j);
				DoInstruction (part_model.GetInstruction (j),part_view.Object,part_model.Id);

				while (!m_inst_done)
				{
					yield return new WaitForSeconds (0.5f);
				}

				yield return new WaitForSeconds (DELAY_BETWEEN_INST);
			}

			part_view.SetDefaultMaterial ();

			m_curr_part_index++;

			if (m_curr_part_index < m_active_scenario_parts.Count)
				RunStepScenario ();
			else
				Debug.Log ("Scenario Complete");
		}

		IEnumerator RunScenario()
		{
			Debug.Log ("Running Scenario ");

			for (int i = 0; i < m_active_scenario_parts.Count; i++) 
			{
				PartView part = m_active_scenario_parts [i];

				//indication of the mission - click on the part to make it do its thing

				part.SetMaterial (m_High_Light_Mat);

				yield return new WaitForSeconds(DELAY_BETWEEN_INST);

				Part	part_model = part.Part_model;

				for (int j = 0; j < part_model.GetAllInstruction ().Count; j++)
				{
					m_inst_done = false;

					Debug.Log (part_model.Description);

					DoInstruction (part_model.GetInstruction (j),part.Object,part_model.Id);
					while (!m_inst_done)
					{
						yield return new WaitForSeconds (0.5f);
					}

					yield return new WaitForSeconds (DELAY_BETWEEN_INST);
				}

				part.SetDefaultMaterial ();


			}

			//SceneController.Instance.SetAllInstanceMaterials (false);

		}

		private void DoInstruction(Instruction inst,GameObject target_object, string part_name)
		{
			switch (inst.Type) 
			{
			case(ModelManager.InstructionType.rotate):
				{
					float amount = inst.Param;
					Debug.Log ("Rotate on X Axis the " +part_name + " part by " +amount + " degrees");
					iTween.RotateAdd (target_object, iTween.Hash ("x", amount, "islocal",true, "easeType", "easeOutCubic", "time", 2f, "onComplete", "OnInstDone", "onCompleteTarget", this.gameObject));
					break;
				}

			case(ModelManager.InstructionType.move_x):
			{
				float amount = inst.Param;
					Debug.Log ("Move on X Axis the " +part_name + " part by " +amount + " meters");
					iTween.MoveAdd (target_object, iTween.Hash ("x", amount, "islocal",true, "easeType", "easeOutCubic", "time", 2f, "onComplete", "OnInstDone", "onCompleteTarget", this.gameObject));
				break;
			}
			case(ModelManager.InstructionType.move_y):
				{
					float amount = inst.Param;
					Debug.Log ("Move on Y Axis the " +part_name + " part by " +amount + " meters");
					iTween.MoveAdd (target_object, iTween.Hash ("y", amount, "islocal",true, "easeType", "easeOutCubic", "time", 2f, "onComplete", "OnInstDone", "onCompleteTarget", this.gameObject));
					break;
				}
			case(ModelManager.InstructionType.move_z):
				{
					float amount = inst.Param;
					Debug.Log ("Move on Z Axis the " +part_name + " part by " +amount + " meters");
					iTween.MoveAdd (target_object, iTween.Hash ("z", amount, "islocal",true, "easeType", "easeOutCubic", "time", 2f, "onComplete", "OnInstDone", "onCompleteTarget", this.gameObject));
					break;
				}
			default:
				{
					OnInstDone ();
					break;
				}

			}
		}

		private void OnInstDone()
		{
			m_inst_done = true;
		}

		 
		public void PlayAudio(AudioClip clip)
		{
			m_audio_source.clip = clip;
			m_audio_source.Play ();
		}

		public void PlayScenario(bool auto)
		{
			PrepareScenario(ModelManager.Instance.GetScenarion ("Scenario1"),auto);
		}

		public Material High_Light_Mat {
			get {
				return m_High_Light_Mat;
			}
		}

		public List<PartView> Active_scenario_parts {
			get {
				return m_active_scenario_parts;
			}
		}

		public bool Scenraio_ready {
			get {
				return m_scenraio_ready;
			}
		}

		public Material Over_mat {
			get {
				return m_over_mat;
			}
		}
	}
}

