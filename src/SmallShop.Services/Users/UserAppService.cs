using SmallShop.Entities;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Hashing;
using SmallShop.Infrastructure.Jwt;
using SmallShop.Services.Roles.Contracts;
using SmallShop.Services.Roles.Exceptions;
using SmallShop.Services.Users.Contracts;
using SmallShop.Services.Users.Contracts.Dtos;
using SmallShop.Services.Users.Exceptions;

namespace SmallShop.Services.Users
{
    public class UserAppService : UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IJwtService _jwtService;
        private readonly UnitOfWork _unitOfWork;

        public UserAppService(UserRepository userRepository,
            RoleRepository roleRepository,
            IJwtService jwtService,
            UnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddUserDto dto)
        {
            await StopIfRoleDoesNotExists(dto);
            await StopIfUserNameAlreadyExists(dto);

            var hashedPassword = PasswordHashing
                .HashPassword(dto.Password);
            var roleId = _roleRepository
                .GetRoleIdByName(dto.RoleName);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Password = hashedPassword,
                Email = dto.Email,
                RoleId = roleId
            };

            _userRepository.Add(user);
            _unitOfWork.Commit();

            return user.Id;
        }
        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto, string ipAddress)
        {
            var user = await _userRepository.GetByUserNameAsync(loginDto.UserName);

            if (user == null || !PasswordHashing.VerifyPassword(loginDto.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid username or password.");

            var jwtToken = _jwtService.GenerateToken
                (loginDto.UserName, user.Role.Name);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddMinutes(15),
                CreatedDate = DateTime.Now,
                UserId = user.Id,
                IpAddress = ipAddress
            });

            _unitOfWork.Commit();

            return new TokenResponseDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken,string ipAddress)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var existingToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                throw new UnauthorizedAccessException("Refresh token is no longer valid.");

            var newToken = _jwtService.GenerateToken(user.UserName,user.Role.Name);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiryDate = DateTime.Now.AddMinutes(15),
                CreatedDate = DateTime.Now,
                UserId = user.Id,
                IpAddress = ipAddress
            });

            existingToken.ExpiryDate = DateTime.Now;

            _unitOfWork.Commit();

            return new TokenResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }

        private async Task StopIfUserNameAlreadyExists(AddUserDto dto)
        {
            var userName = await _userRepository
                .IsUserNameExist(dto.UserName);
            if (userName)
            {
                throw new UserNameAlreadyExistException();
            }
        }

        private async Task StopIfRoleDoesNotExists(AddUserDto dto)
        {
            var role = await _roleRepository.
                IsRoleExist(dto.RoleName);
            if (!role)
            {
                throw new RoleDoesNotExistException();
            }
        }

    }
}
