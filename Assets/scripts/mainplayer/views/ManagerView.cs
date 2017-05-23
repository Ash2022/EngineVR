using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mainplayer.views
{

	public class ManagerView : MonoBehaviour {

		static ManagerView			    m_instance;

		[SerializeField]GameObject m_root;
		[SerializeField]GameObject m_camera;


		public static ManagerView Instance
		{
			get{

				if (m_instance == null)
				{
					ManagerView manager = GameObject.FindObjectOfType<ManagerView> ();
					m_instance = manager.GetComponent<ManagerView> ();
				}
				return m_instance;
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public GameObject Root {
			get {
				return m_root;
			}
		}

		public GameObject Camera {
			get {
				return m_camera;
			}
		}




	}
}