namespace DrummersDatabaseAPI.Web.Controllers.RequestHelpers
{
    #pragma warning disable CS1591

    public static class DefaultRequestParameters
    {
        private const int _minPageNumber = 1;
        private const int _maxPageNumber = 100;
        private const int _minPageSize = 5;
        private const int _maxPageSize = 1000;
        private const int _defaultPageSize = 100;
        private const int _defaultPageNumber = 1;

        // defaults
        public static int DefaultPageSize => _defaultPageSize;

        public static int DefaultPageNumber => _defaultPageNumber;

        public static int GetPageNumber(int pageNum, int allCount, int pageSize)
        {
            if (pageNum < _minPageNumber)
            {
                return _minPageNumber;
            }

            if (pageNum > _maxPageNumber)
            {
                return _maxPageNumber;
            }

            // if page number exceeds the max calculated pages, send back the calculated amount
            if (pageNum > (int)Math.Ceiling(allCount / (double)pageSize))
            {
                pageNum = (int)Math.Ceiling(allCount / (double)pageSize);

                if (pageNum < 1)
                {
                    pageNum = 1;
                }
            }
            return pageNum;
        }

        public static int GetPageSize(int pageSize)
        {
            if (pageSize < _minPageSize)
            {
                return _minPageSize;
            }

            if (pageSize > _maxPageSize)
            {
                return _maxPageSize;
            }
            return pageSize;
        }
    }

    #pragma warning restore CS1591
}
