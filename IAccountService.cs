namespace ProductAPI
{
    public interface IAccountService
    {
        Task<Response<bool>> RegisterAsync(RegisterViewModel model);
        Task<Response<string>> LoginAsync(RegisterViewModel model);
    }
}
