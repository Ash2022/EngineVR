using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mainplayer.models
{

	public class Scenario {

		GameObject					m_root;
		string 						m_description;
		List<Part> 					m_parts = new List<Part> ();


		public Scenario(GameObject root, string description, List<Part> parts=null)
		{
			m_root = root;
			m_description = description;

		}
	
		public void AddPart(Part part)
		{
			m_parts.Add (part);
		}

		public List<Part> GetParts()
		{
			return m_parts;
		}

		public Part GetPart(int index)
		{
			return m_parts[index];
		}

		public GameObject Root {
			get {
				return m_root;
			}
		}

		public string Description {
			get {
				return m_description;
			}
		}


	}
}