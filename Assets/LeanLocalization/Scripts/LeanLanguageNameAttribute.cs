using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Localization
{
	[CustomPropertyDrawer(typeof(LeanLanguageNameAttribute))]
	public class LeanLanguageNameDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var left  = position; left.xMax -= 40;
			var right = position; right.xMin = left.xMax + 2;
			
			EditorGUI.PropertyField(left, property);
			
			if (GUI.Button(right, "List") == true)
			{
				var menu = new GenericMenu();
				
				for (var i = 0; i < LeanLocalization.CurrentLanguages.Count; i++)
				{
					var language = LeanLocalization.CurrentLanguages[i];
					
					menu.AddItem(new GUIContent(language), property.stringValue == language, () => { property.stringValue = language; property.serializedObject.ApplyModifiedProperties(); });
				}
				
				if (menu.GetItemCount() > 0)
				{
					menu.DropDown(right);
				}
				else
				{
					Debug.LogWarning("Your scene doesn't contain any languages, so the language name list couldn't be created.");
				}
			}
		}
	}
}
#endif

namespace Lean.Localization
{
	// This attribute allows you to select a language from all the localizations in the scene
	public class LeanLanguageNameAttribute : PropertyAttribute
	{
	}
}