using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace HK.Framework.Editor
{
	/// <summary>
	/// .
	/// </summary>
	public class ReorderbleComponents : EditorWindow
	{
		private ReorderableList reorderbleList;

		private GameObject target;

		[MenuItem("Window/Reorderble Components")]
		private static void OnOpen()
		{
			var window = EditorWindow.GetWindow(typeof(ReorderbleComponents), false, "Reorderble Components") as ReorderbleComponents;
			window.Show();
		}

		void OnGUI()
		{
			if(this.target == null)
			{
				GUILayout.Label("Please Select GameObject");
			}
			else
			{
				
			}
		}

		void OnSelectionChange()
		{
			this.target = Selection.activeGameObject;
			this.Repaint();
		}
	}
}