namespace HaberSistemi.Core.Dto
{
    public class ServiceResult<T> 
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public T Data { get; set; }

        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message };
        }

        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T> { Data = value, IsSuccess = true };
        }
    }
}
