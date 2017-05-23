using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mainplayer.models

{

	public class Instruction {

		string 									m_description;
		string 									m_manual_description = "";
		ModelManager.InstructionType 			m_type;
		float									m_param;

		public Instruction(ModelManager.InstructionType inst_type,float amount)
		{
			m_param = amount;
			m_type = inst_type;
			SetAutoDesc ();
		}

		private void SetAutoDesc()
		{
			switch (m_type) 
			{
			case(ModelManager.InstructionType.rotate):
				{
					m_description = "Rotate on X Axis by " +m_param + " degrees";
					break;
				}

			case(ModelManager.InstructionType.move_x):
				{
					m_description = "Move on X Axis by " +m_param + " degrees";
					break;
				}
			case(ModelManager.InstructionType.move_y):
				{
					m_description = "Move on Y Axis by " +m_param + " degrees";
					break;
				}
			case(ModelManager.InstructionType.move_z):
				{
					m_description = "Move on Z Axis by " +m_param + " degrees";
					break;
				}
			
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public ModelManager.InstructionType Type {
			get {
				return m_type;
			}
			set{ 
				m_type = value;
			}
		}

		public float Param {
			get {
				return m_param;
			}
			set {
				m_param = value;
			}
		}

		public string Description {
			get {
				return m_description;
			}
			set {
				m_description = value;
			}
		}

		public string Manual_description {
			get {
				return m_manual_description;
			}
			set {
				m_manual_description = value;
			}
		}
	}
}