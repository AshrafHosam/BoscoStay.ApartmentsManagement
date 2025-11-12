namespace Application.Response
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
