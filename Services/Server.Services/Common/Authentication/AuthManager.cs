using AutoMapper;
using Server.Core;
using System.Text;
using Server.Domain;
using Server.Models;
using System.Text.Json;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Server.Services
{
    public class AuthManager : IAuthManager
    {

        #region Fields
        private readonly IMapper                      _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration               _configuration;
        private readonly RoleManager<CustomRole>      _roleManager;
        private readonly IEmail_Service               _emailService;
        private readonly IPasswordReset_Service       _passwordResetService;
        private readonly IGeneralTask_Service         _generalTask_Service;
        private readonly INotifications_Service       _notifications_Service;
        private readonly ERPDb _context;
        #endregion

        #region Constructor
        public AuthManager
            (
            ERPDb context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            RoleManager<CustomRole> roleManager,
            IEmail_Service emailService,
            IPasswordReset_Service passwordResetService,
            IGeneralTask_Service generalTask_Service,
            INotifications_Service notifications_Service


            )
        {
            _context               = context;
            _mapper                = mapper;
            _userManager           = userManager;
            _configuration         = configuration;
            _roleManager           = roleManager;
            _emailService          = emailService;
            _passwordResetService  = passwordResetService;
            _generalTask_Service   = generalTask_Service;
            _notifications_Service = notifications_Service;

        }

        #endregion 

        public async Task<AuthResponseModel> Login(UserLoginModel loginDto)
        {
            var user            = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return new AuthResponseModel { Message = "User not found." };
            }
            bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isValidUser)
            {
                return new AuthResponseModel { Message = "Invalid credentials." };
            }
            var token = await GenerateToken(user,1);
            return new AuthResponseModel
            {
                Token       = token,
                UserId      = user.Id,
                EmailStatus = user.EmailConfirmed,
                Message     = "Success",


            };
        }
        public async Task<IEnumerable<IdentityError>> RegisterAdmin(RegisterUserModel adminDto)
        {
            try
            {
                var admin        = new ApplicationUser();
                admin.FirstName  = adminDto.FirstName;
                admin.LastName   = adminDto.LastName;
                admin.MiddleName = adminDto.MiddleName;
                admin.Email      = adminDto.Email; ;
                admin.UserName   = adminDto.Email;
                admin.EmployeeId = await GetMaxId()+1;
                var result = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("Administrator");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = "Administrator", Permissions = "Read,Write,Delete,Update" });
                    }

                    await _userManager.AddToRoleAsync(admin, "Administrator");
                   
                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> RegisterCandidates(RegisterUserModel adminDto)
        {
            try
            {
                var admin        = new ApplicationUser();
                admin.FirstName  = adminDto.FirstName;
                admin.LastName   = adminDto.LastName;
                admin.MiddleName = adminDto.MiddleName;
                admin.Email      = adminDto.Email; ;
                admin.UserName   = adminDto.Email;
                admin.EmployeeId = await GetMaxId() + 1;
                admin.image      = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/deafut.jpg?alt=media&token=b9fac53e-2606-44b3-89c2-e4bc03479051";
                var result = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(adminDto.Role);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = adminDto.Role, Permissions = "Read,Write,Update,Delete" }); 
                    }
                    
                    await _userManager.AddToRoleAsync(admin,adminDto.Role);
                    var roles= await _roleManager.FindByNameAsync(adminDto.Role);
                    foreach (var permission in roles.Permissions.Split(','))
                    {
                        await _userManager.AddClaimAsync(admin, new Claim("Permission", permission));
                    }

                    var user = await _userManager.FindByNameAsync(admin.Email);

                    if (user != null)
                    {

                        if (adminDto.Role == "Candidate")
                        {
                            var tasks = new List<GENERALTASK>
                            {
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Personal Information",
                                Description = "Please Add Your Personal Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Emergency Contacts Information",
                                Description = "Please Add Your Emergency Contacts Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Dependents Information",
                                Description = "Please Add Your Dependents Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Education Information",
                                Description = "Please Add Your Education Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Professional License Information",
                                Description = "Please Add Your Professional License Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Job Experience Information",
                                Description = "Please Add Your Job History  ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Certifications Information",
                                Description = "Please Add Your Certifications Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                            };
                            await _generalTask_Service.AddRange(tasks);
                            await _generalTask_Service.CompleteAync();
                        }
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Your Account Created Successfully";
                        notification.UserId       = user.Id;
                        notification.WorkflowStep = "Registration";
                        notification.Timestamp    = DateTime.Now;
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                    }

                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> RegisterUsers(AddUsersModel adminDto)
        {
            try
            {
                var admin             = new ApplicationUser();
                admin.FirstName       = adminDto.FirstName;
                admin.LastName        = adminDto.LastName;
                admin.MiddleName      = adminDto.MiddleName;
                admin.Email           = adminDto.Email; ;
                admin.UserName        = adminDto.Email;
                admin.defaultPassword = adminDto.Password;
                admin.EmployeeId      = await GetMaxId() + 1;
                admin.isAdmin         = adminDto.isAdmin;
                admin.isEmployee      = adminDto.isEmployee;
                admin.image           = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/deafut.jpg?alt=media&token=b9fac53e-2606-44b3-89c2-e4bc03479051";
                var result            = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(adminDto.role);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = adminDto.role, Permissions = "Read,Write,Update,Delete" });
                    }

                    await _userManager.AddToRoleAsync(admin, adminDto.role);
                    var roles = await _roleManager.FindByNameAsync(adminDto.role);
                    foreach (var permission in roles.Permissions.Split(','))
                    {
                        await _userManager.AddClaimAsync(admin, new Claim("Permission", permission));
                    }

                    var user = await _userManager.FindByNameAsync(admin.Email);

                    if (user != null)
                    {

                        if (adminDto.role == "Candidate")
                        {
                            var tasks = new List<GENERALTASK>
                            {
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Personal Information",
                                Description = "Please Add Your Personal Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Emergency Contacts Information",
                                Description = "Please Add Your Emergency Contacts Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Dependents Information",
                                Description = "Please Add Your Dependents Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Education Information",
                                Description = "Please Add Your Education Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Professional License Information",
                                Description = "Please Add Your Professional License Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Job Experience Information",
                                Description = "Please Add Your Job History  ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Certifications Information",
                                Description = "Please Add Your Certifications Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id
                            },
                            };
                            await _generalTask_Service.AddRange(tasks);
                            await _generalTask_Service.CompleteAync();
                        }
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Your Account Created Successfully";
                        notification.UserId       = user.Id;
                        notification.WorkflowStep = "Registration";
                        notification.Timestamp    = DateTime.Now;
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                    }

                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> RegisterHospital(RegisterUserModel adminDto)
        {
            try
            {
                var admin       = new ApplicationUser();
                admin.FirstName = adminDto.FirstName;
                admin.LastName  = adminDto.LastName;
                admin.Email     = adminDto.Email; ;
                admin.UserName  = adminDto.Email;
               
                var result      = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("Hospital");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = "Hospital", Permissions = "Read,Write" });
                    }
                    await _userManager.AddToRoleAsync(admin, "Hospital");
                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<dynamic> FindById(string uid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(uid);
                return user;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> UpdateUser(UpdateUserModel user)
        {
            try
            {
                ApplicationUser user1 = await _userManager.FindByEmailAsync(user.Email);
                if (user1 == null)
                {
                    var message = "No User Found Against Email " + user.Email;
                    return JsonSerializer.Serialize(message);
                }

                user1.FirstName  = user.FirstName;
                user1.LastName   = user.LastName;
                user1.MiddleName = user.MiddleName;
                user1.image      = user.Image;
                var result = await _userManager.UpdateAsync(user1);
                if (result.Succeeded == true)
                {
                    var notification     = new NOTIFICATIONS();
                    notification.IsRead  = false;
                    notification.Message = "Your Account Detail Updated Successfully";
                    notification.UserId  = user1.Id;
                    notification.Timestamp = DateTime.Now;
                    notification.WorkflowStep = "Account Setting";
                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();
                    var message       = "User neccessary Details Updated Successfully";
                    return JsonSerializer.Serialize(message);
                }
                return user;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> FindbyEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<dynamic> UpdatePassord(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    var notification = new NOTIFICATIONS();
                    notification.IsRead = false;
                    notification.Message = "Your Password Detail Updated Successfully";
                    notification.UserId = user.Id;
                    notification.Timestamp = DateTime.Now;
                    notification.WorkflowStep = "Password Update";
                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();
                    return "OK Password updated successfully";
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).FirstOrDefault();
                }
            }
            return "User not Found";
        }
        public async Task<dynamic> ComfirmEmail(string email, string otp)
        {
            try
            {
                var currentTime = DateTime.Now;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not Found";
                }
                dynamic retVlaue = await VerifyOTP(email, otp);
                if (retVlaue is Guid)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    string subject      = "Email Confirmed Successfully";
                    string salutation   = "Dear " + user.FirstName + " " + user.LastName;
                    string messageBody  = $"<h1>Your Email confirmed Successfully thanks to be part of Sapient Medical  </h1>";
                    string emailMessage = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmailAsync(user.Email, subject, emailMessage);
                    var res             = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        var notification = new NOTIFICATIONS();
                        notification.IsRead = false;
                        notification.Message = "Your Emai Confirmed Successfully";
                        notification.UserId = user.Id;
                        notification.Timestamp = DateTime.Now;
                        notification.WorkflowStep = "Email Confirmation";
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                        return "Your Email Confirmed Successfully";
                    }

                    return res;
                }
                return retVlaue;



            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> SendComfirmEmail(string email)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not found";
                }
                var otp            = GenerateRandomOTP();
                var expirationTime = DateTime.Now.AddMinutes(15);
                var resetRequest   = new PasswordResetDomain
                {
                    Email          = user.Email,
                    ExpireTime     = expirationTime,
                    OTP            = otp
                };

                string retValue = await StorePasswordResetRequest(resetRequest);
                if (!retValue.StartsWith("OK"))
                {
                    return retValue;
                }
                string subject = "Verify Email";
                string salutation = "Dear" + " " + user.FirstName + " " + user.LastName;
                string messageBody = $"\r\n\r\n\n\n\nThank you for registering with PelicanHRM.\r\n\r\nEmail Verification OTP\r\n\r\n\n\n\n:<h1>{otp}</h1>";

                string emailMessage = $"\r\n\r\n\n<h1>{salutation}</h1>\r\n\r\n\n\n\n{messageBody}";
                await _emailService.SendEmailAsync(user.Email, subject, emailMessage, true);
                var notification = new NOTIFICATIONS();
                notification.IsRead = false;
                notification.Message = "Email Confirmation sent Successfully";
                notification.UserId = user.Id;
                notification.Timestamp = DateTime.Now;
                notification.WorkflowStep = "Email Confirmation";
                await _notifications_Service.InsertAsync(notification);
                await _notifications_Service.CompleteAync();
                return "Email has been sent to You for Verification";
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> VerifyOTP(string email, string otp)
        {
            var currentTime = DateTime.Now;
            IList<PasswordResetDomain> otplist = (IList<PasswordResetDomain>)await _passwordResetService.GetAll();
            if (otplist.Count == 0)
            {
                return "You did not have generated OTP";
            }
            PasswordResetDomain otpf = otplist.Where(x => x.Email == email).OrderBy(x => x.ExpireTime).LastOrDefault();
            if (currentTime <= otpf.ExpireTime && otpf.OTP == otp)
            {
                return otpf.Id;
            }

            return "OTP expired generate new ";
        }
        private async Task<string> StorePasswordResetRequest(PasswordResetDomain resetRequest)
        {

            try
            {

                await _passwordResetService.InsertAsync(resetRequest);
                await _passwordResetService.CompleteAync();
                var message = "OK";
                return message;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }



        }
        private static string      GenerateRandomOTP()
        {
            const string validChars = "0123456789";
            int leng = 8;
            var random = new Random();
            var otp = new string(Enumerable.Repeat(validChars, leng)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return otp;
        }
        private async Task<string> GenerateToken(ApplicationUser user,int tenantId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            // Fetch the first role's permissions (you may adjust this based on your logic)
            var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault());
            var permissions = role?.Permissions;

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var permissionClaims = !string.IsNullOrEmpty(permissions)
                                   ? new List<Claim> { new Claim("Permission", permissions) }
                                   : new List<Claim>();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("isAdmin", user.isAdmin ? "true" : "false"),
                new Claim("isEmployee", user.isEmployee ? "true" : "false"),
                new Claim("EmailConfirmed", user.EmailConfirmed ? "true" : "false"),
                new Claim("TenantId", tenantId.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region Role Management
        public async Task<IEnumerable<IdentityError>> CreateRole(string roleName,string Permissions)
        {
            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new CustomRole { Name = roleName, Permissions = Permissions});
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "Role already exists." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> UpdateRoleAndPermissions(string Id,string roleName, string permissions)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Id);
                if (role == null)
                {
                    return new List<IdentityError> { new IdentityError { Description = "Role not found" } };
                }
                role.Permissions    = permissions;
                role.NormalizedName = roleName.ToUpper();
                role.Name           = roleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Enumerable.Empty<IdentityError>(); 
                }
                else
                {
                    return result.Errors; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> UpdateUserRole(string userId, string newRole)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Remove existing roles
                    var existingRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, existingRoles);

                    // Add the new role
                    await _userManager.AddToRoleAsync(user, newRole);

                    return null; // Success
                }

                return new List<IdentityError> { new IdentityError { Description = "User not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<string> UpdateRole()
        {
            try
            {
                var existingRole = await _roleManager.FindByNameAsync("Candidate");
                existingRole.Permissions = "Read";
                await _roleManager.UpdateAsync(existingRole);
                return "OK";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable<IdentityError>> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "Role not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable<IdentityError>> DeleteUser(string id)
        {
            try
            {
                var role = await _userManager.FindByIdAsync(id);
                if (role != null)
                {
                    var result = await _userManager.DeleteAsync(role);
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "User not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var Students = users.Where(u => _userManager.IsInRoleAsync(u, "Candidate").Result).ToList();
                return Students.Select(x=>new
                {
                    x.Id,
                    x.FirstName,x.MiddleName, x.LastName,
                    x.Email,
                    x.image,

                });

            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }


        public async Task<IEnumerable> GetAllUsersWithRoles()
        {
            try
            {
                var users       = await _userManager.Users.ToListAsync();
                var Students    = users.Where(u => !_userManager.IsInRoleAsync(u, "Candidate").Result).ToList();
                var userResults = Students.Select(  user =>
                {
                    var roles =   _userManager.GetRolesAsync(user);

                    return new
                    {
                        user.Id,
                        user.FirstName,
                        user.MiddleName,
                        user.LastName,
                        user.Email,
                        Roles = roles.Result.FirstOrDefault(),
                        user.image,
                        user.defaultPassword
                    };
                }).ToList();

                return userResults.ToList();
               
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }


        public async Task<IEnumerable> GetusersAll()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userResults = users.Select(user =>
                {
                    var roles = _userManager.GetRolesAsync(user);

                    return new
                    {
                        user.Id,
                        user.FirstName,
                        user.MiddleName,
                        user.LastName,
                        user.Email,
                        Roles = roles.Result.FirstOrDefault(),
                        user.image,
                       
                    };
                }).ToList();

                return userResults.ToList();

            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }


        public async Task<IEnumerable> GetAll()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();

                var userResults = users.Select(user =>
                {
                    var roles = _userManager.GetRolesAsync(user);
                    return new AllUsersModel
                    {
                      Id=  user.Id,
                      FirstName=  user.FirstName,
                      MiddleName=  user.MiddleName,
                      LastName=  user.LastName,
                      Email=  user.Email,
                      Roles = roles.Result.FirstOrDefault(),
                      Image= user.image,

                    };
                }).ToList();

                return userResults.ToList();

            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }

        private  async Task<int> GetMaxId()
        {
            
                var users  = await _userManager.Users.ToListAsync();
                return users.Max(x=>x.EmployeeId);
        }

        public async Task<AllUsersModel> GetByIduser(string uid)
        {
            try
            {
                var user    = await _userManager.FindByIdAsync(uid);
                var roles = _userManager.GetRolesAsync(user);

                var myuser = new AllUsersModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roles.Result.FirstOrDefault(),
                    Image = user.image,
                };




                return myuser;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<dynamic> UpdateCRMUser(AddUsersModel user)
        {
            try
            {
                ApplicationUser user1 = await _userManager.FindByEmailAsync(user.Email);

                if (user1 == null)
                {
                    var errorMessage = "No User Found Against Email " + user.Email;
                    return errorMessage;
                }

                // Update user details
                user1.FirstName = user.FirstName;
                user1.LastName = user.LastName;
                user1.MiddleName = user.MiddleName;
                user1.Email = user.Email;
                user1.isEmployee = user.isEmployee;
                user1.isAdmin = user1.isAdmin;
                // Update user role
                var existingRoles = await _userManager.GetRolesAsync(user1);
                var roleToRemove = existingRoles.FirstOrDefault();
                if (roleToRemove != null)
                {
                    var removeRoleResult = await _userManager.RemoveFromRoleAsync(user1, roleToRemove);
                    if (!removeRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to remove existing role.";
                        return errorMessage;
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user1, user.role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to add the new role.";
                        return errorMessage;
                    }
                }
                else
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user1, user.role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to add the new role.";
                        return errorMessage;
                    }
                }


                // Update other details
                var updateResult = await _userManager.UpdateAsync(user1);
                if (updateResult.Succeeded)
                {
                    var notification = new NOTIFICATIONS
                    {
                        IsRead = false,
                        Message = "Your Account Detail Updated Successfully",
                        UserId = user1.Id,
                        Timestamp = DateTime.Now,
                        WorkflowStep = "Account Setting"
                    };

                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();

                    var successMessage = "User's necessary Details Updated Successfully";
                    return successMessage;
                }

                var failureMessage = "Failed to update user details.";
                return failureMessage;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion
    }



}
