namespace DrummersDatabaseAPI.Web.ResponseHeaders
{
    /// <summary>
    /// data to return back in header of paged entry requests
    /// </summary>
    public class PaginationMetaData
    {
        /// <summary>
        /// count of all avail entries
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// calculation of all pages
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        public PaginationMetaData(int allCount, int pageSize, int pageNum)
        {
            TotalItemCount = allCount;
            TotalPageCount = (int)Math.Ceiling(allCount / (double)pageSize);
            PageSize = pageSize;
            CurrentPage = pageNum;
        }
    }
}
