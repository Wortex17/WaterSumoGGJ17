using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EShape {Circle = 0, Rectangle = 1}

namespace WaterSumo
{
	public class CreateBoundingBox : EditorWindow
	{

		private EShape shapeSelection;
		private GUILayoutOption option;


		[MenuItem("Window/Create Bounding Box")]
		static void MenuButton()
		{
			CreateBoundingBox boundingBox = (CreateBoundingBox)EditorWindow.GetWindow(typeof(CreateBoundingBox));
			boundingBox.Show();

		}

		void OnGUI()
		{
			shapeSelection = (EShape)EditorGUILayout.EnumPopup("Bounding Box to create:", shapeSelection);

			if (GUILayout.Button("Create"))
				CreateTheBoundingBox(shapeSelection);

		}

		void CreateTheBoundingBox(EShape _selectedShape)
		{
			switch ((int)_selectedShape)
			{
				case 0:
					GameObject circle = new GameObject("Arena");
					circle.AddComponent<CapsuleCollider>().radius = 0.5f;
					circle.GetComponent<CapsuleCollider>().height = 5.0f;
					circle.AddComponent<ArenaBehaviour>();
					Debug.Log("Circle");
					break;
				case 1:
					GameObject rectangle = new GameObject("Arena");
					rectangle.AddComponent<BoxCollider>().size = new Vector3(1, 0.1f, 1);
					rectangle.AddComponent<ArenaBehaviour>();
					Debug.Log("Rectangle");
					break;
			}
		}
	}
}
