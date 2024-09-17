namespace XPOS340.web.AddOns
{
    public class Pegination<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalData { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public Pegination(List<T> pageData, int totalData, int pageIndex, int pageSize) {
            PageIndex = pageIndex; 
            TotalPages = (int)Math.Ceiling(totalData / (double)pageSize); 
            TotalData = totalData;
            AddRange(pageData);
        }

        public static Pegination<T> Create(List<T> sourceData, int pageindex, int pageSize)
        {
            List<T> pageData = sourceData.Skip((pageindex - 1)* pageSize).Take(pageSize).ToList();
            return new Pegination<T>(pageData, sourceData.Count(), pageindex, pageSize);
        }
    }
}
