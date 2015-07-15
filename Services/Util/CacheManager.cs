using System;
using System.Web;
using System.Web.Caching;

namespace FindMyFamilies.Util {
	public class CacheManager {
		private static HttpRuntime m_HttpRuntime;

		public const string INTERVAL_MINUTES = "minutes";
		public const string INTERVAL_HOURS = "hours";
		public const string INTERVAL_DAYS = "days";

		public static Cache Cache {
			get {
				if (m_HttpRuntime == null) {
					m_HttpRuntime = new HttpRuntime();
				}
				return HttpRuntime.Cache;
			}
		}

		public static object Get(string keyValue) {
			return Cache[keyValue];
		}

		public static void Set(string keyValue, object dataValue) {
			Cache.Insert(keyValue, dataValue);
		}

		public static void Set(string keyValue, object dataValue, string intervalType, int expireInterval) {
			switch (intervalType) {
				case INTERVAL_MINUTES:
					{
						Cache.Insert(keyValue, dataValue, null, DateTime.Now.AddMinutes(expireInterval), TimeSpan.Zero);
						break;
					}
				case INTERVAL_HOURS:
					{
						Cache.Insert(keyValue, dataValue, null, DateTime.Now.AddHours(expireInterval), TimeSpan.Zero);
						break;
					}
				case INTERVAL_DAYS:
					{
						Cache.Insert(keyValue, dataValue, null, DateTime.Now.AddDays(expireInterval), TimeSpan.Zero);
						break;
					}
			}
		}
	}
}