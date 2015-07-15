///////////////////////////////////////////////////////////////////////////
// Description: AppSettings Class
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;


using FindMyFamilies.Data;
using FindMyFamilies.Util;
using FindMyFamilies.Exceptions;
using FindMyFamilies.BusinessObject;

namespace findmyfamilies.Services {

	/// <summary>
	/// Purpose: Services Facade Class for AppSettings
	/// </summary>
	public sealed class AppSettings : AppSettingsBase {

		private AppSettings() {
		}
		public static AppSettings Instance() {
			return Nested.instance;
		}
		
		class Nested {
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested() {
			}
		
			internal static readonly AppSettings instance = new AppSettings();
		}
	}
}
