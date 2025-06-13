using System;

namespace Metreos.Types.Http
{
	/// <summary> Contains standard values and methods for Metreos.Types.Http </summary>
	public abstract class IHttp
	{
        public const string HTTP_TYPE_NAMESPACE = "Metreos.Types.Http";
        public const string QUERY_PARAM_COLLECTION = "QueryParamCollection";

		public const char PARAM_DELIMITER           = '&';
        public const char PARAM_EQUATES             = '=';
        public const char PARAM_START               = '?';

        public const string HTTP_PROTOCOL           = "http";
        public const string DUMMY_HOST              = "www.metreos.com";
        public const int DUMMY_PORT                 = 80;
        public const string DUMMY_PAGE              = "randomPage.htm";
	}
}
