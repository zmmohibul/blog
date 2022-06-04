namespace API.Helpers
{
    public class Result<T>
    {
        public int StatusCode { get; set; }
        
        public bool IsSuccesful { get; set; }

        public string? ErrorMessage { get; set; }

        public T? Data { get; set; }
    }
}