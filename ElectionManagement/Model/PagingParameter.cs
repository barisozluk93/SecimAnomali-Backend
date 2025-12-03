namespace ElectionManagement.Model
{
    public class PagingParameter
    {
        public long ElectionId { get; set; }
        public long CityId { get; set; }
        public long DistrictId { get; set; }
        public long NeighborhoodId { get; set; }

        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
