using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mainplayer.models
{

	public class Part  {

		string 						m_description;
		string 						m_id;
		List<Instruction>			m_instructions = new List<Instruction> ();


		public Part(string id, string description, List<Instruction> instructions=null)
		{
			m_description = description;
			m_id = id;
		}

		public void AddInstruction(Instruction inst)
		{
			m_instructions.Add (inst);
		}


		public void SetAllInstructions(List<Instruction> instructions)
		{
			m_instructions = instructions;
		}

		public List<Instruction> GetAllInstruction()
		{
			return m_instructions;
		}

		public Instruction GetInstruction(int index)
		{
			return m_instructions[index];
		}

		public string Id {
			get {
				return m_id;
			}
		}

		public string Description {
			get {
				return m_description + " part# " + m_id;
			}
		}


	}
}