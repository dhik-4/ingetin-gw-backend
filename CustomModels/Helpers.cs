namespace IngetinGwAPI.CustomModels
{
    public class Helpers
    {
        public static ApiResponse<T> Success<T>(T data)
        {
            return new ApiResponse<T>
            {
                Ok = true,
                Data = data
            };
        }

        public static ApiResponse<object> Fail(string err, string msg)
        {
            return new ApiResponse<object>
            {
                Ok = false,
                Err = err,
                Msg = msg
            };
        }
    }
}