using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace ASMaterialIcon
{
	[ExecuteInEditMode, Serializable]
	public class MaterialVectorIcon : Text
	{
		static Dictionary<string, string> _icons;

		public static readonly string DefaultIcon = "3d_rotation";
		public static readonly string DefaultIconName = "3d_rotation";

		[SerializeField]
		string _iconName = "";
		public string iconName
		{
			get
			{
				if (string.IsNullOrEmpty(_iconName))
				{
					_iconName = DefaultIconName;
				}
				return _iconName;
			}
			set
			{
				_iconName = value;
			}
		}

		public bool raycast;

		protected override void Start()
		{
			base.Start();
			raycastTarget = raycast;

#if UNITY_EDITOR
			font = Resources.Load<Font>("ASMaterialIcon/Fonts/MaterialIcons-Regular") as Font;
#endif
			alignment = TextAnchor.MiddleCenter;
			fontSize = (int)Mathf.Floor(Mathf.Min(rectTransform.rect.width, rectTransform.rect.height));
			if (string.IsNullOrEmpty(text))
			{
				SetIcon(DefaultIcon);
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();

			fontSize = (int)Mathf.Floor(Mathf.Min(rectTransform.rect.width, rectTransform.rect.height));
		}

		public static void LoadIconResource()
		{
			_icons.Clear();
			TextAsset txt = (TextAsset)Resources.Load("ASMaterialIcon/Fonts/MaterialIcons", typeof(TextAsset));
			string[] lines = txt.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);

			string key, value;
			foreach (string line in lines)
			{
				if (!line.StartsWith("#") && line.IndexOf("=") >= 0)
				{
					key = line.Substring(0, line.IndexOf("="));
					if (!_icons.ContainsKey(key))
					{
						value = line.Substring(line.IndexOf("=") + 1,
							line.Length - line.IndexOf("=") - 1);
						_icons.Add(key, value);
					}
				}
			}
		}

		public static Dictionary<string, string> GetIcons()
		{
			if (_icons == null)
			{
				_icons = new Dictionary<string, string>();
				LoadIconResource();
			}
			return _icons;
		}

		public static string Decode(string iconName)
		{
			string raw = "\\u" + GetIcons()[iconName];
			return new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})").Replace(raw, m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
		}

		public static bool IsIcon(string icon)
		{
			return GetIcons().ContainsKey(icon);
		}

		public void SetIcon(string icon)
		{
			if (!IsIcon(icon))
			{
				color = new Color(color.r, color.g, color.b, 0f);
				return;
			}

			color = new Color(color.r, color.g, color.b, 1f);
			iconName = icon;
			text = Decode(icon);
		}

		public void SetIcon(MaterialVectorIconInfo info)
		{
			color = info.color;
			SetIcon(info.icon);
		}
	}

	[Serializable]
	public class MaterialVectorIconInfo
	{
		public string icon = "3d_rotation";
		public Color color = Color.white;
		public Color bgColor = Color.grey;

		public MaterialVectorIconInfo()
		{
		}

		public MaterialVectorIconInfo(string icon)
		{
			this.icon = icon;
		}

		public MaterialVectorIconInfo(string icon, Color color)
		{
			this.icon = icon;
			this.color = color;
		}
	}
}
