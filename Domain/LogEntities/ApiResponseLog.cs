using Domain.Common;

namespace Domain.LogEntities
{
    public class ApiResponseLog : BaseEntity
    {
        public int StatusCode { get; set; } = 0;
        public string ResponseBody { get; set; }
        public string RequestName { get; set; }
        public string RequestBody { get; set; }
    }
}
