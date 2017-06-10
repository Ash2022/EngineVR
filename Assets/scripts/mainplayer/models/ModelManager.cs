using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mainplayer.views;
using mainplayer.controllers;

namespace mainplayer.models
{

	public class ModelManager : MonoBehaviour {

		Dictionary<string,Scenario> m_scenarios = new Dictionary<string, Scenario> ();


		public enum InstructionType
		{
			rotate=0,
			move_x,
			move_y,
			move_z
		};

		static ModelManager			    m_instance;

		public static ModelManager Instance
		{
			get{

				if (m_instance == null)
				{
					ModelManager manager = GameObject.FindObjectOfType<ModelManager> ();
					m_instance = manager.GetComponent<ModelManager> ();
				}
				return m_instance;
			}
		}

		public void readScenario(int index)
		{
			m_scenarios.Clear ();
			if (index == 0) {
				Scenario scenario = new Scenario (SceneController.Instance.Main_object, "replace the under the hood thingy");
				Part part = new Part ("26-rvECR", "take out bolt");
				List<Instruction> instructions = new List<Instruction> ();

				instructions.Add (new Instruction (InstructionType.rotate, 360f));
				instructions.Add (new Instruction (InstructionType.move_x, 5f));

				part.SetAllInstructions (instructions);

				scenario.AddPart (part);

				Part part2 = new Part ("26-ECrdR", "Rotate to unblock");
				List<Instruction> instructions2 = new List<Instruction> ();

				instructions2.Add (new Instruction (InstructionType.rotate, -80f));

				part2.SetAllInstructions (instructions2);

				scenario.AddPart (part2);


				Part part3 = new Part ("26-EcrkR", "Remove the blocking ");
				List<Instruction> instructions3 = new List<Instruction> ();

				instructions3.Add (new Instruction (InstructionType.move_x, 5f));

				part3.SetAllInstructions (instructions3);

				scenario.AddPart (part3);

				Part part4 = new Part ("26-MNrdR", "Rotate to unblock ");
				List<Instruction> instructions4 = new List<Instruction> ();

				instructions4.Add (new Instruction (InstructionType.rotate, 50f));
				instructions4.Add (new Instruction (InstructionType.move_x, 5f));

				part4.SetAllInstructions (instructions4);
				scenario.AddPart (part4);

				Part part5 = new Part ("26-MNcpR", "Pull to disconnect ");
				List<Instruction> instructions5 = new List<Instruction> ();

				instructions5.Add (new Instruction (InstructionType.move_x, 5f));

				part5.SetAllInstructions (instructions5);
				scenario.AddPart (part5);

				Part part6 = new Part ("26-SDrdR", "Pull to unblock ");
				List<Instruction> instructions6 = new List<Instruction> ();

				instructions6.Add (new Instruction (InstructionType.move_x, 5f));

				part6.SetAllInstructions (instructions6);
				scenario.AddPart (part6);

				Part part7 = new Part ("26-JOrdR", "Pull to unblock ");
				List<Instruction> instructions7 = new List<Instruction> ();

				instructions7.Add (new Instruction (InstructionType.move_x, 5f));

				part7.SetAllInstructions (instructions7);
				scenario.AddPart (part7);

				Part part8 = new Part ("26-ax2fR", "Pull to unblock ");
				List<Instruction> instructions8 = new List<Instruction> ();

				instructions8.Add (new Instruction (InstructionType.move_x, 8f));

				part8.SetAllInstructions (instructions8);
				scenario.AddPart (part8);

				Part part9 = new Part ("26-plW2R", "Pull down to remove ");
				List<Instruction> instructions9 = new List<Instruction> ();

				instructions9.Add (new Instruction (InstructionType.move_x, 8f));

				part9.SetAllInstructions (instructions9);
				scenario.AddPart (part9);


				Part part10 = new Part ("26-spW2R", "Pull to unblock ");
				List<Instruction> instructions10 = new List<Instruction> ();

				instructions10.Add (new Instruction (InstructionType.rotate, 580f));
				instructions10.Add (new Instruction (InstructionType.move_x, 8f));

				part10.SetAllInstructions (instructions10);
				scenario.AddPart (part10);

				Part part11 = new Part ("26-arc2R", "Pull to unblock ");
				List<Instruction> instructions11 = new List<Instruction> ();

				instructions11.Add (new Instruction (InstructionType.move_x, 8f));

				part11.SetAllInstructions (instructions11);
				scenario.AddPart (part11);

				m_scenarios.Add ("Scenario1", scenario);
			} 
			else if (index == 1) //CAR
			{
				Scenario scenario = new Scenario (SceneController.Instance.Main_object, "Lets look under the hood - view the car from the back ");
				Part part = new Part ("NONE-DC_Shell 23", "Pull the left glass cover upwards");
				List<Instruction> instructions = new List<Instruction> ();

				instructions.Add (new Instruction (InstructionType.move_z, 2f));

				part.SetAllInstructions (instructions);

				scenario.AddPart (part);

				Part part2 = new Part ("NONE-DC_Shell 22", "Pull the right glass cover upwards");
				List<Instruction> instructions2 = new List<Instruction> ();

				instructions2.Add (new Instruction (InstructionType.move_z, 2f));

				part2.SetAllInstructions (instructions2);

				scenario.AddPart (part2);


				Part part3 = new Part ("NONE-DC_Shell 538", "Pull The left Glass Rim upwards");
				List<Instruction> instructions3 = new List<Instruction> ();

				instructions3.Add (new Instruction (InstructionType.move_z, 2f));

				part3.SetAllInstructions (instructions3);

				scenario.AddPart (part3);

				Part part4 = new Part ("NONE-DC_Shell 46", "Pull The right Glass Rim upwards ");
				List<Instruction> instructions4 = new List<Instruction> ();

				instructions4.Add (new Instruction (InstructionType.move_z, 2f));

				part4.SetAllInstructions (instructions4);
				scenario.AddPart (part4);

				Part part5 = new Part ("NONE-DC_Shell 12", "Remove the engine cover by sliding it backwards to expose hood ");
				List<Instruction> instructions5 = new List<Instruction> ();

				instructions5.Add (new Instruction (InstructionType.move_y, 3f));

				part5.SetAllInstructions (instructions5);
				scenario.AddPart (part5);

				Part part6 = new Part ("NONE-DC_Shell 454", "Disassemble and pull the Engine front air tube");
				List<Instruction> instructions6 = new List<Instruction> ();

				instructions6.Add (new Instruction (InstructionType.move_y, 3f));

				part6.SetAllInstructions (instructions6);
				scenario.AddPart (part6);


				Part part7 = new Part ("NONE-DC_Shell 455", "Disassemble and pull the Engine rear air tube");
				List<Instruction> instructions7 = new List<Instruction> ();

				instructions7.Add (new Instruction (InstructionType.move_y, 3f));

				part7.SetAllInstructions (instructions7);
				scenario.AddPart (part7);


				Part part8 = new Part ("NONE-DC_Shell 378", "Remove the right air filter cap");
				List<Instruction> instructions8 = new List<Instruction> ();

				instructions8.Add (new Instruction (InstructionType.move_y, 5f));

				part8.SetAllInstructions (instructions8);
				scenario.AddPart (part8);

				Part part9 = new Part ("NONE-DC_Shell 381", "Remove the left air filter cap");
				List<Instruction> instructions9 = new List<Instruction> ();

				instructions9.Add (new Instruction (InstructionType.move_y, 5f));

				part9.SetAllInstructions (instructions9);
				scenario.AddPart (part9);


				Part part10 = new Part ("NONE-DC_Shell 377", "Rotate and pull the right air filter head");
				List<Instruction> instructions10 = new List<Instruction> ();

				instructions10.Add (new Instruction (InstructionType.rotate, 180f));
				instructions10.Add (new Instruction (InstructionType.move_y, 5f));

				part10.SetAllInstructions (instructions10);
				scenario.AddPart (part10);

				Part part11 = new Part ("NONE-DC_Shell 380", "Rotate and pull the left air filter head");
				List<Instruction> instructions11 = new List<Instruction> ();

				instructions11.Add (new Instruction (InstructionType.rotate, 180f));
				instructions11.Add (new Instruction (InstructionType.move_y, 5f));

				part11.SetAllInstructions (instructions11);
				scenario.AddPart (part11);

				Part part12 = new Part ("NONE-DC_Shell 573", "The End and head");
				List<Instruction> instructions12 = new List<Instruction> ();

			
				instructions12.Add (new Instruction (InstructionType.move_y, 0f));

				part12.SetAllInstructions (instructions12);
				scenario.AddPart (part12);

				m_scenarios.Add ("Scenario1", scenario);
			}
		}


		public Scenario GetScenarion(string scenraio_id)
		{
			Scenario scenario;

			m_scenarios.TryGetValue (scenraio_id, out scenario);

			return scenario;
		}



	}
}