namespace MyWebApiApp.Models
{
    public class APIResponse
    {
        public bool Success { set; get; }
        public string Message { set; get; }
        public object Data { set; get; }
    }
}
