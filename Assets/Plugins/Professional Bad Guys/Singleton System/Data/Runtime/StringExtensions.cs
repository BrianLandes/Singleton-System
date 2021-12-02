
using System;
using System.Collections.Generic;
using System.Linq;

namespace SingletonSystem {

	public static class StringExtensions {

		public static string Nicify(this string text) {

			if (string.IsNullOrEmpty(text)) {
				return text;
			}

			text = Capitalize(text);

			string AddSpaceBeforeUpperCaseChars(char @char) {
				if (Char.IsUpper(@char)) {
					return " " + @char;
				}
				return @char.ToString();
			}

			var characterList = text.Select(x => AddSpaceBeforeUpperCaseChars(x));

			return string.Concat(characterList).TrimStart(' ');
		}

		public static string Capitalize(this string text) {
			if (string.IsNullOrEmpty(text)) {
				return text;
			}
			return Char.ToUpper(text[0]) + text.Substring(1);
		}

		public static string RemoveSubstringAndRemoveBefore(this string original, string substring) {
			var index = original.IndexOf(substring);
			if (index >= 0) {
				// Chop it off
				var length = substring.Length;
				return original.Substring(index + length);
			}
			return original;
		}

		public static string RemoveBefore(this string original, string substring) {
			var index = original.IndexOf(substring);
			if (index >= 0) {
				return original.Substring(index);
			}
			return original;
		}
	}
}