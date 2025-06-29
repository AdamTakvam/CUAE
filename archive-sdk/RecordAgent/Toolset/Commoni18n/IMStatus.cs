using System;
using System.Globalization;

using Metreos.Toolset.Interfaces;

namespace Metreos.Toolset.Commoni18n
{
	/// <summary>
	/// i18n support for IM Status
	/// </summary>
	public class IMStatus
	{
		private Metreos.Toolset.Commoni18n.Locale i18n;
		private System.Resources.ResourceManager resources;

		public IMStatus()
		{
			i18n = Locale.Instance;
			resources = new System.Resources.ResourceManager(typeof(IMStatus));			
		}

		public string GetIMStatusCultureString(IIMStatus.IM_STATUS imStatus)
		{
			string s = "";

			switch(imStatus)
			{
				case IIMStatus.IM_STATUS.ONLINE:
					s = resources.GetString(IMStatusResourceKeys.ONLINE, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.BUSY:
					s = resources.GetString(IMStatusResourceKeys.BUSY, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.BE_RIGHT_BACK:
					s = resources.GetString(IMStatusResourceKeys.BE_RIGHT_BACK, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.AWAY:
					s = resources.GetString(IMStatusResourceKeys.AWAY, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.OUT_TO_LUNCH:
					s = resources.GetString(IMStatusResourceKeys.OUT_TO_LUNCH, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.APPEAR_OFFLINE:
					s = resources.GetString(IMStatusResourceKeys.APPEAR_OFFLINE, i18n.CurrentCulture);
					break;

				case IIMStatus.IM_STATUS.UNKNOWN:
				default:
					break;
			}

			return s;
		}

		public string GetString(string name)
		{
			string s = "";

			s = resources.GetString(name, i18n.CurrentCulture);

			return s;
		}
	}
}
