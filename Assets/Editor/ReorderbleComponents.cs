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
	[ExecuteInEditMode()]
	public class ReorderbleComponents : EditorWindow
	{
		private ReorderableList reorderbleList;

		private int selectIndex;

		private Vector2 scrollPosition = Vector2.zero;

		[MenuItem("Window/Reorderble Components")]
		private static void OnOpen()
		{
			var window = EditorWindow.GetWindow(typeof(ReorderbleComponents), false, "Reorderble Components") as ReorderbleComponents;
			window.Show();
		}

		void OnEnable()
		{
			if(this.reorderbleList == null)
			{
				this.InitializeReorderbleList();
			}
		}

		void OnGUI()
		{
			if(this.reorderbleList == null)
			{
				this.InitializeReorderbleList();
			}

			var target = Selection.activeGameObject;
			if(target == null)
			{
				GUILayout.Label("Please Select GameObject");
			}
			else
			{
				this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
				{
					this.reorderbleList.DoLayoutList();
				}
				EditorGUILayout.EndScrollView();
			}
		}

		void OnInspectorUpdate()
		{
			var target = Selection.activeGameObject;
			if(target == null)
			{
				return;
			}
			this.reorderbleList.list = target.GetComponents(typeof(Component)).Where(c => c.GetType() != typeof(Transform)).ToList();
			this.Repaint();
		}

		void OnSelectionChange()
		{
			var target = Selection.activeGameObject;
			if(target != null)
			{
				this.reorderbleList.list = target.GetComponents(typeof(Component)).Where(c => c.GetType() != typeof(Transform)).ToList();
			}
			this.Repaint();
		}

		private void InitializeReorderbleList()
		{
			this.reorderbleList = new ReorderableList(null, typeof(Component), true, false, false, false);
			this.reorderbleList.drawHeaderCallback = this.DrawHeaderCallback;
			this.reorderbleList.drawElementCallback = this.DrawElementCallback;
			this.reorderbleList.onSelectCallback = this.OnSelectCallback;
			this.reorderbleList.onChangedCallback = this.OnChangedCallback;
		}

		private void DrawHeaderCallback(Rect position)
		{
			var target = Selection.activeGameObject;
			EditorGUI.LabelField(position, target.name);
		}

		private void DrawElementCallback(Rect position, int index, bool isActive, bool isFocused)
		{
			var component = this.reorderbleList.list[index] as Component;
			var content = new GUIContent(EditorGUIUtility.ObjectContent(component, component.GetType()));
			content.text = component.GetType().Name;
			EditorGUI.LabelField(position, content);
		}

		private void OnSelectCallback(ReorderableList list)
		{
			this.selectIndex = list.index;
		}

		private void OnChangedCallback(ReorderableList list)
		{
			var component = list.list[list.index] as Component;
			var diff = this.selectIndex - list.index;
			for(int i=0; i<Mathf.Abs(diff); i++)
			{
				if(diff > 0)
				{
					UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
				}
				else if(diff < 0)
				{
					UnityEditorInternal.ComponentUtility.MoveComponentDown(component);
				}
			}
		}
	}
}