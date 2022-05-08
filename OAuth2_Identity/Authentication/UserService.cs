using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using OAuth2_Identity.Core.Helpers;
using OAuth2_Identity.Core.Repositories;
using OAuth2_Identity.Entities;
using OAuth2_Identity.Helpers;
using OAuth2_Identity.Models;
using OAuth2_Identity.Models.Enums;

namespace OAuth2_Identity.Authentication;

public interface IUserService
{
    ApiResponse<object> GetUser(string uname);
    ApiResponse<string> AuthenticateUser(UserRequestDTO model);
    ApiResponse<bool> Register(UserRequestDTO model);
    ApiResponse<string> AuthenticateClient(ClientRequestDTO model);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IOptions<Settings> _settings;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IClientRepository clientRepository, IOptions<Settings> settings)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _clientRepository = clientRepository;
        _settings = settings;
    }

    public ApiResponse<object> GetUser(string uname)
    {
        var result = new ApiResponse<object>();
        try
        {
            var users = _userRepository.Find(x => x.Username.Equals(uname)).ToList();
            if (!users.Any())
            {
                result.Message.Eng = "Invalid Username";
                result.Success = false;
                return result;
            }

            var user = users.First();
            var roles = _roleRepository.Find(x => x.User.Id == user.Id);
            var data = new {User = user, Roles = roles};

            result.Data = data;
            result.Success = true;
        }
        catch (Exception e)
        {
            var ex = e.Message;
            result.Message.Eng = ex;
            result.Success = false;
        }

        return result;
    }

    public ApiResponse<string> AuthenticateUser(UserRequestDTO model)
    {
        var result = new ApiResponse<string>();
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                result.ResponseCode = StatusCodes.Status400BadRequest;
                result.Message.Eng = "Username and password are required fields";
                result.Success = false;
                return result;
            }

            var users = _userRepository.Find(x => x.Username.Equals(model.Username)).ToList();

            if (!users.Any())
            {
                result.ResponseCode = StatusCodes.Status400BadRequest;
                result.Message.Eng = "Invalid Username";
                result.Success = false;
                return result;
            }

            var user = users.First();
            var salt = user.Salt;
            var hashOrigin = user.Hash;
            //var password = new Password();
            var hashForCompare = PasswordHelper.Hash(model.Password, salt);
            var isValidUser = PasswordHelper.CompareHashes(Encoding.ASCII.GetBytes(hashOrigin),
                Encoding.ASCII.GetBytes(hashForCompare));

            if (!isValidUser)
            {
                result.ResponseCode = StatusCodes.Status400BadRequest;
                result.Message.Eng = "Invalid Password";
                result.Success = false;
                return result;
            }

            if (user.Terminated)
            {
                result.ResponseCode = (int) OperationCodes.TERMINATED_USER;
                result.Message.Eng = "Terminated user";
                result.Success = false;
                return result;
            }

            var jwt = new JwtHelper();
            result.Data = JwtHelper.Generate(_settings.Value.Jwt.Key, model);
            result.Success = true;
            result.ResponseCode = (int) OperationCodes.SUCCESS;
            result.Message.Eng = OperationCodes.SUCCESS.ToString();
        }
        catch (Exception e)
        {
            throw;
        }

        return result;
    }

    public ApiResponse<bool> Register(UserRequestDTO model)
    {
        var result = new ApiResponse<bool>();
        try
        {
            var u = _userRepository.Find(x => x.Username.Equals(model.Username));
            if (u.Any())
            {
                result.Message.Eng = "Existing user, please pick another username";
                result.Success = false;
                return result;
            }

            var salt = PasswordHelper.Salt(model.Username);
            var hash = PasswordHelper.Hash(model.Password, salt);

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Hash = hash,
                Salt = salt,
                UserSecret = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = ""
            };

            _userRepository.Add(newUser);
            result.Success = true;
        }
        catch (Exception e)
        {
            var ex = e.Message;
            result.Message.Eng = ex;
            result.Success = false;
        }

        return result;
    }

    public ApiResponse<string> AuthenticateClient(ClientRequestDTO model)
    {
        var result = new ApiResponse<string>();
        try
        {
            var client = _clientRepository.Find(x => x.ClientID.Equals(model.ClientId)  && x.ClientKey.Equals(model.ClientKey))
                .FirstOrDefault();
            var user = _userRepository.Find(x => model.UserSecret.Equals(x.UserSecret)).FirstOrDefault();
            if (client != null && user != null)
            {
                var userInfo = new UserRequestDTO {Username = user.Username, Email = user.Email};
                var jwt = new JwtHelper();
                result.Data = JwtHelper.Generate(_settings.Value.Jwt.Key, userInfo);
                result.Success = true;
                result.ResponseCode = (int) OperationCodes.SUCCESS;
                result.Message.Eng = OperationCodes.SUCCESS.ToString();
            }
        }
        catch (Exception e)
        {
            var ex = e.Message;
            result.Message.Eng = ex;
            result.Success = false;
        }

        return result;
    }
}