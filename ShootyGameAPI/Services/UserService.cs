using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPI.Services
{
    public interface IUserService
    {
        public Task<SignInResponse?> AuthenticateAsync(SignInRequest request);
        public Task<UserResponse?> AddWeaponToUserAsync(UserWeaponRequest userWeaponRequest);
        public Task<List<UserResponse>> FindAllUsersAsync();
        public Task<UserResponse?> FindUserByIdAsync(int userId);
        public Task<UserResponse?> CreateUserAsync(UserRequest newUser);
        public Task<UserResponse?> UpdateUserByIdAsync(int userId, UserRequest updatedUser);
        public Task<UserResponse?> DeleteUserByIdAsync(int userId);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserWeaponRepository _userWeaponRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IUserWeaponRepository userWeaponRepository, IJwtUtils jwtUtils, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _userWeaponRepository = userWeaponRepository;
            _jwtUtils = jwtUtils;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        private UserResponse MapUserToUserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email.ToLower(),
                PlayerTag = user.PlayerTag,
                Money = user.Money,
                Role = user.Role,
                Weapons = user.UserWeapons.Select(uw => new User_WeaponsResponse
                {
                    WeaponId = uw.WeaponId,
                    Name = uw.Weapon.Name,
                    ReloadSpeed = uw.Weapon.ReloadSpeed,
                    MagSize = uw.Weapon.MagSize,
                    FireRate = uw.Weapon.FireRate,
                    FireMode = uw.Weapon.FireMode,
                    WeaponType = new User_Weapon_WeaponTypeResponse
                    {
                        WeaponTypeId = uw.Weapon.WeaponType.WeaponTypeId,
                        Name = uw.Weapon.WeaponType.Name,
                        EquipmentSlot = uw.Weapon.WeaponType.EquipmentSlot
                    }
                }).ToList(),
                Scores = user.Scores.Select(s => new User_ScoreResponse
                {
                    ScoreId = s.ScoreId,
                    ScoreValue = s.ScoreValue,
                    AverageAccuracy = s.AverageAccuracy,
                    RoundTime = s.RoundTime
                }).ToList()
            };
        }

        private User MapUserRequestToUser(UserRequest userRequest)
        {
            var user = new User
            {
                UserName = userRequest.UserName,
                Email = userRequest.Email.ToLower(),
            };

            // Hash the password before storing it
            user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);

            // Generate a unique player tag
            user.PlayerTag = GenerateUniquePlayerTag(user.UserName);

            // Set the user's role to User by default unless the current user is an admin
            var currentUser = GetCurrentUser();
            if (currentUser != null && currentUser.Role == Role.Admin)
            {
                user.Role = userRequest.Role;
            }

            return user;
        }

        private UserWeapon MapUserWeaponRequestToUserWeapon(UserWeaponRequest userWeaponRequest)
        {
            return new UserWeapon
            {
                UserId = userWeaponRequest.UserId,
                WeaponId = userWeaponRequest.WeaponId
            };
        }

        private bool VerifyPassword(User user, string enteredPassword)
        {
            // Verify the password hash
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, enteredPassword);

            // Return true if the password is correct
            return result == PasswordVerificationResult.Success;
        }

        public async Task<SignInResponse?> AuthenticateAsync(SignInRequest request)
        {
            var user = await _userRepository.FindUserByEmailAsync(request.Email.ToLower());

            // Check if the user exists and the password is correct
            if (user == null || !VerifyPassword(user, request.Password))
            {
                return null; // Authentication failed
            }

            // Generate a JWT token for the user
            return new SignInResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                PlayerTag = user.PlayerTag,
                Money = user.Money,
                Role = user.Role,
                Token = _jwtUtils.GenerateJwtToken(user)
            };
        }

        public async Task<UserResponse?> AddWeaponToUserAsync(UserWeaponRequest userWeaponRequest)
        {
            var userWeapon = MapUserWeaponRequestToUserWeapon(userWeaponRequest);

            await _userWeaponRepository.CreateUserWeaponAsync(userWeapon);

            var user = await _userRepository.FindUserByIdAsync(userWeapon.UserId);

            if (user == null)
            {
                return null;
            }

            return MapUserToUserResponse(user);
        }

        public async Task<List<UserResponse>> FindAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(MapUserToUserResponse).ToList();
        }

        public async Task<UserResponse?> FindUserByIdAsync(int userId)
        {
            var user = await _userRepository.FindUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return MapUserToUserResponse(user);
        }

        private string GenerateUniquePlayerTag(string baseUserName)
        {
            // Use a hash of the UserName + GUID to generate a consistent but unique number
            var guidHash = Guid.NewGuid().ToString("N").Substring(0, 8);

            return $"{baseUserName}#{guidHash}";
        }

        private UserResponse? GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext?.Items["User"] as UserResponse;
        }

        public async Task<UserResponse?> CreateUserAsync(UserRequest newUser)
        {
            var user = MapUserRequestToUser(newUser);

            try
            {
                var createdUser = await _userRepository.CreateUserAsync(user);

                if (createdUser == null)
                {
                    return null;
                }

                return MapUserToUserResponse(createdUser);
            }
            // Catch the exception thrown when the email is already in use
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("duplicate") == true)
            {
                throw new InvalidOperationException("The email address is already in use.", ex);
            }
        }

        public async Task<UserResponse?> UpdateUserByIdAsync(int userId, UserRequest updatedUser)
        {
            var thisUserId = userId;
            var user = await _userRepository.UpdateUserByIdAsync(thisUserId, MapUserRequestToUser(updatedUser));

            if (user == null)
            {
                return null;
            }

            return MapUserToUserResponse(user);
        }

        public async Task<UserResponse?> DeleteUserByIdAsync(int userId)
        {
            var deletedUser = await _userRepository.DeleteUserByIdAsync(userId);

            if (deletedUser == null)
            {
                return null;
            }

            return MapUserToUserResponse(deletedUser);
        }
    }
}
