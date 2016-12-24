using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.Linq;

namespace HK.Framework.Editor
{
	/// <summary>
	/// .
	/// </summary>
	public class ReorderbleComponents : EditorWindow
	{
		private ReorderableList reorderbleList;

		private GameObject target;

		private List<Component> components;

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
				this.reorderbleList.DoLayoutList();
			}
		}

		void OnSelectionChange()
		{
			this.target = Selection.activeGameObject;
			this.Repaint();
			if(this.target == null)
			{
				return;
			}

			this.components = this.target.GetComponents(typeof(Component)).Where(c => c.GetType() != typeof(Transform)).ToList();
			this.reorderbleList = new ReorderableList(this.components, typeof(Component), true, true, false, false);
			this.reorderbleList.drawHeaderCallback = (rect) =>
			{
				EditorGUI.LabelField(rect, this.target.name);
			};
			this.reorderbleList.drawElementCallback = (rect, index, isActive, isFocused) =>
			{
				var component = this.components[index];
				var content = new GUIContent(EditorGUIUtility.ObjectContent(component, component.GetType()));
				content.text = component.GetType().Name;
				EditorGUI.LabelField(rect, content);
			};
		}
	}
}