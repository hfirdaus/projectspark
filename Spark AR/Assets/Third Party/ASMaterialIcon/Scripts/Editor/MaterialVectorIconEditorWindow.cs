#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace ASMaterialIcon
{
	class MaterialVectorIconEditorWindow : EditorWindow
	{
		string searchInput;
		Vector2 scrollPos = Vector2.zero;

		static SerializedProperty selected;
		static float time = 0f;
		static bool doubleClicked;
		static string previouslyClicked;

		public static void ShowWindow()
		{
			time = Time.realtimeSinceStartup + 0.25f;
			doubleClicked = false;
			GetWindow<MaterialVectorIconEditorWindow>(true, "Material Vector Icon Picker", true);
		}
		public static void ShowWindow(SerializedProperty property)
		{
			selected = property;
			time = Time.realtimeSinceStartup + 0.25f;
			doubleClicked = false;
			GetWindow<MaterialVectorIconEditorWindow>(true, "Material Vector Icon Picker", true);
		}

		void OnGUI()
		{
			DrawSearchField();
			DrawIconPicker();
		}

		void DrawSearchField()
		{
			EditorGUILayout.BeginHorizontal();
			
			GUI.SetNextControlName("searchInput");
			searchInput = EditorGUILayout.TextField(searchInput, GUI.skin.FindStyle("ToolbarSeachTextField"));
			EditorGUI.FocusTextInControl("searchInput");

			if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
			{
				searchInput = "";
				GUI.FocusControl(null);
			}

			EditorGUILayout.EndHorizontal();
		}

		void DrawIconPicker()
		{
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			Dictionary<string, string> icons = MaterialVectorIcon.GetIcons();
			List<string> actualList = new List<string>();
			if (!string.IsNullOrEmpty(searchInput))
				foreach (string iconName in icons.Keys)
					if (iconName.Contains(searchInput))
						actualList.Add(iconName);

			DrawIcons(icons, (actualList.Count > 0 ? actualList : icons.Keys.ToList()));

			EditorGUILayout.EndScrollView();

			if (doubleClicked)
			{
				doubleClicked = false;
				selected = null;
				previouslyClicked = "";
				Close();
			}
		}

		void DrawIcons(Dictionary<string, string> icons, List<string> iconNameList)
		{
			string name;
			float elementWidth = 60f;
			GUIStyle iconStyle = new GUIStyle(GUI.skin.button);
			iconStyle.font = Resources.Load<Font>("ASMaterialIcon/Fonts/MaterialIcons-Regular") as Font;
			iconStyle.fontSize = 30;
			iconStyle.border = new RectOffset(5, 5, 5, 5);
			iconStyle.contentOffset = new Vector2(0, 0);
			iconStyle.alignment = TextAnchor.MiddleCenter;
			iconStyle.normal.textColor = (EditorGUIUtility.isProSkin ? Color.white : new Color(0.2f, 0.2f, 0.2f));

			for (int y = 0; y < iconNameList.Count; y++)
			{
				EditorGUILayout.BeginHorizontal();

				float remainingSpace = EditorGUIUtility.currentViewWidth - EditorGUIUtility.currentViewWidth / 10;

				while (remainingSpace > elementWidth && y < iconNameList.Count)
				{
					name = iconNameList[y];
					Color oldColor = GUI.backgroundColor;
					GUI.backgroundColor = Color.clear;

					if (Selection.gameObjects != null)
					{
						foreach (GameObject selection in Selection.gameObjects.ToList())
						{
							if (selection.GetComponent<MaterialVectorIcon>())
							{
								if (name == selection.GetComponent<MaterialVectorIcon>().iconName)
								{
									GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
								}
							}
							if (selected != null)
							{
								if (name == selected.stringValue)
								{
									GUI.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
								}
							}
						}
					}


					if (GUILayout.Button(new GUIContent(MaterialVectorIcon.Decode(name), "Icon: " + name), iconStyle, GUILayout.Width(elementWidth), GUILayout.Height(elementWidth)))
					{
						if (Selection.gameObjects != null)
						{
							foreach (GameObject selection in Selection.gameObjects.ToList())
							{
								MaterialVectorIcon mvi = selection.GetComponent<MaterialVectorIcon>();

								if (mvi)
								{
									Undo.RecordObject(mvi, "Changed icon of " + mvi.name);
									mvi.SetIcon(name);
									EditorUtility.SetDirty(mvi);

									doubleClicked = Time.realtimeSinceStartup < time && name == previouslyClicked;
								}
								else if (selected != null)
								{
									Undo.RecordObject(selected.serializedObject.targetObject, "Changed icon of " + selected.serializedObject.ToString());
									selected.stringValue = name;
									selected.serializedObject.ApplyModifiedProperties();
									EditorUtility.SetDirty(selected.serializedObject.targetObject);

									doubleClicked = Time.realtimeSinceStartup < time && name == previouslyClicked;
								}
							}

							time = Time.realtimeSinceStartup + 0.25f;
							previouslyClicked = name;
						}
					}

					GUI.backgroundColor = oldColor;
					remainingSpace -= elementWidth;
					y++;
				}

				EditorGUILayout.EndHorizontal();
			}
		}
	}
}
#endif