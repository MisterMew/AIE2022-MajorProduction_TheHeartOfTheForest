
using UnityEngine;
using UnityEngine.TextCore;

namespace HotF
{
	[System.Serializable]
	public class GlyphInfo
	{
		[System.Serializable]
		public struct FormattingOptions
		{
			[Tooltip("Horizontal space on left of glyph. Can be negative")]
			public float LeftMargin;

			[Tooltip("Horizontal space on right of glyph. Can be negative")]
			public float RightMargin;

			public string Size;

			public float VerticalOffset;
		}

		[System.Serializable]
		public struct Pair
		{
			public string Key;
			public string Value;

			public Pair(string key, string value)
			{
				Key = key;
				Value = value;
			}
		}

		public Pair[] ReplacementPairs = new Pair[]
		{
			new Pair("<\"", " <size=#SIZE#><space=#LEFT_MARGIN#><voffset=#VOFFSET#><sprite=\"ControlGlyphs\" name=\""),
			new Pair("\">", "\"></voffset><space=#RIGHT_MARGIN#><size=100%>")
		};

		public FormattingOptions Preprocessing;


		/// <summary>
		/// Replaces parts of <paramref name="input"/> with values from glyph
		/// </summary>
		public string Format(string input) => Format(input, Preprocessing);

		/// <summary>
		/// Replaces parts of <paramref name="input"/> with values from glyph
		/// </summary>
		/// <param name="useDefaultFormatting">Whent true, ignores <paramref name="formattingOptions"/></param>
		public string Format(string input, FormattingOptions formattingOptions, bool useDefaultFormatting = false)
		{
			if (useDefaultFormatting)
				formattingOptions = Preprocessing;

			foreach(Pair pair in ReplacementPairs)
			{
				input = input.Replace(pair.Key, pair.Value)
							 .Replace("#LEFT_MARGIN#", formattingOptions.LeftMargin.ToString())
							 .Replace("#RIGHT_MARGIN#", formattingOptions.RightMargin.ToString())
							 .Replace("#VOFFSET#", formattingOptions.VerticalOffset.ToString())
							 .Replace("#SIZE#", string.IsNullOrEmpty(formattingOptions.Size) ? "100%" : formattingOptions.Size);
			}
			return input;
		}
	}
}