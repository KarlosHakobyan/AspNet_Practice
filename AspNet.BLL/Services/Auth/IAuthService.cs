using AspNet.Domain.Models.AuthModels;
using AspNet.BLL.Common.Results;

namespace AspNet.BLL.Services.Auth;

public interface IAuthService
{
    Task<Result> RegisterAsync(RegisterModel model);

    Task<Result<AuthResponseModel>> LoginAsync(LoginModel model);
}