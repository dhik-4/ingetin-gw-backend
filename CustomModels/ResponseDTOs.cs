namespace IngetinGwAPI.CustomModels
{
    public class ResponseDTOs
    {
    }

    public class ApiResponse<T>
    {
        public bool Ok { get; set; }
        public T Data { get; set; }
        public string Err { get; set; }
        public string Msg { get; set; }
    }

    public class LoginSuccessData
    {
        public UserDto User { get; set; }
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
