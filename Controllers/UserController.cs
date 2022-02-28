using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizplusApi.Models;
using QuizplusApi.Models.User;
using QuizplusApi.ViewModels.Helper;
using QuizplusApi.ViewModels.User;

namespace QuizplusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly ISqlRepository<Users> _userRepo;
        private readonly ISqlRepository<UserRole> _userRoleRepo;
        private readonly ISqlRepository<LogHistory> _logHistoryRepo;

        public UserController(IConfiguration config,
                                AppDbContext context, 
                                ISqlRepository<Users> userRepo,
                                ISqlRepository<UserRole> userRoleRepo,
                                ISqlRepository<LogHistory> logHistoryRepo)
        {
            _config=config;
            _context = context;
            _userRepo = userRepo;
            _userRoleRepo=userRoleRepo;
            _logHistoryRepo=logHistoryRepo;
        }

        ///<summary>
        ///Get Log in Detail
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{email}/{password}")]      
        public ActionResult GetLoginInfo(string email, string password)
        {
            try
            {
                var user=(from u in _context.Users join r in _context.UserRoles on u.UserRoleId
                equals r.UserRoleId where u.IsActive.Equals(true) && u.Email.Equals(email) && u.Password.Equals(password)
                select new {u.UserId,r.UserRoleId,r.RoleName,r.DisplayName,u.FullName,u.Mobile,u.Email,u.ImagePath,u.Password,u.Address,u.DateOfBirth}).FirstOrDefault();
                if(user!=null)
                {
                    UserInfo userInfo=new UserInfo{UserId=user.UserId,UserRoleId=user.UserRoleId,RoleName=user.RoleName,DisplayName=user.DisplayName,
                    Email=user.Email,Password=user.Password,FullName=user.FullName,Mobile=user.Mobile,Address=user.Address,
                    ImagePath=user.ImagePath,DateOfBirth=user.DateOfBirth};
                    var token=GenerateJwtToken(userInfo); 
                    return Ok(new LogInResponse{Token=token,Obj=userInfo});
                }
                return NoContent();              
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation{Status="error",ResponseMsg=ex.Message});           
            }
        }

        ///<summary>
        ///Get User Info for Forget password option
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{email}")]      
        public ActionResult GetUserInfoForForgetPassword(string email)
        {
            try
            {
                var user=_context.Users.SingleOrDefault(q=>q.Email==email);
                if(user!=null)
                {
                    return Ok(user);
                }
                else
                {
                    return Accepted(new Confirmation{Status="error",ResponseMsg="There is no user for this email"});
                }
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation{Status="error",ResponseMsg=ex.Message});           
            }
        }

        ///<summary>
        ///Student Registration
        ///</summary>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult StudentRegistration(Users model)
        {
            try
            {
                bool duplicateRegistration=false;
                if(model.StripeSessionId!="")
                {
                    var chkDuplicateSessionId=_context.Users.SingleOrDefault(p=>p.StripeSessionId==model.StripeSessionId);
                    if(chkDuplicateSessionId!=null)
                    {
                        duplicateRegistration=true;
                    }
                }
                
                var chkDuplicate=_context.Users.SingleOrDefault(p=>p.Email==model.Email);
                if(duplicateRegistration)
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Please pay first. This is an invalid session!" });
                }
                else if(chkDuplicate==null)
                {
                    model.UserRoleId=2;
                    model.DateAdded=DateTime.Now;
                    model.IsActive=true;
                    _userRepo.Insert(model);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });
                }
                else
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "This Email already have a user!" });
                }
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Create History Log
        ///</summary>
        [AllowAnonymous]
        [HttpPost]       
        public ActionResult CreateLoginHistory(LogHistory model)
        {
            try
            {  
                model.LogDate=DateTime.Now;    
                model.LogInTime=DateTime.Now;
                model.LogCode=Guid.NewGuid().ToString();        
                _logHistoryRepo.Insert(model);
                return Ok(new Confirmation { Status = "success", ResponseMsg = model.LogCode });              
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Update Login History
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{logCode}")]       
        public ActionResult UpdateLoginHistory(string logCode)
        {
            try
            {
                var objLogHistory=_context.LogHistories.SingleOrDefault(opt=>opt.LogCode==logCode);
                objLogHistory.LogOutTime=DateTime.Now;
                _context.SaveChanges();
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Get Browsing List
        ///</summary>
        [Authorize(Roles="Admin")]    
        [HttpGet]        
        public ActionResult GetBrowseList()
        {
            try
            {
                var browsingList=_context.LogHistories.Join(_context.Users,
                log=>log.UserId,
                user=>user.UserId,
                (log,user)=>new{    
                    FullName=user.FullName,
                    Email=user.Email,
                    LogInTime=log.LogInTime,
                    LogOutTime=log.LogOutTime,
                    Ip=log.Ip,
                    Browser=log.Browser,
                    BrowserVersion=log.BrowserVersion,
                    Platform=log.Platform
                }).OrderByDescending(e=>e.LogInTime);
                return Ok(browsingList);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get User List
        ///</summary>
        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult GetUserList()
        {
            try
            {
                var userList=(from u in _context.Users join r in _context.UserRoles on 
                u.UserRoleId equals r.UserRoleId
                select new {u.UserId,u.UserRoleId,u.FullName,r.RoleName,r.DisplayName,u.Mobile,u.Email,u.Password,u.DateOfBirth,
                u.Address,u.ImagePath});
                return Ok(userList);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get Single User by id
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Admin,Student")]
        public ActionResult GetSingleUser(int id)
        {
            try
            {
                var singleUser=_userRepo.SelectById(id);
                return Ok(singleUser);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Delete User by User Id
        ///</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult DeleteSingleUser(int id)
        {
            try
            {
                _userRepo.Delete(id);
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully deleted" });
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Create User
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult CreateUser(Users model)
        {
            try
            {
                var chkDuplicate=_context.Users.SingleOrDefault(p=>p.Email.ToLower()==model.Email.ToLower());
                if(chkDuplicate==null)
                {
                    model.DateAdded=DateTime.Now;
                    model.IsActive=true;
                    _userRepo.Insert(model);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });
                }
                else
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "This Email already have a user!" });
                }
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Update User
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut]
        public ActionResult UpdateUser(Users model)
        {
            try
            {
                var objUser=_context.Users.SingleOrDefault(p=>p.UserId==model.UserId);
                var objCheck=_context.Users.SingleOrDefault(p=>p.Email.ToLower()==model.Email.ToLower());
                if(objCheck!=null && objCheck.Email.ToLower()!=objUser.Email.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "This Email already have a user!" });
                }
                else
                {
                    objUser.UserRoleId=model.UserRoleId;
                    objUser.FullName=model.FullName;
                    objUser.Mobile=model.Mobile;
                    objUser.Email=model.Email;
                    objUser.DateOfBirth=model.DateOfBirth;
                    objUser.Address=model.Address;
                    objUser.Password=model.Password;
                    objUser.ImagePath=model.ImagePath;
                    objUser.LastUpdatedBy=model.LastUpdatedBy;
                    objUser.LastUpdatedDate=DateTime.Now;
                    _context.SaveChanges();
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully updated" });
                }
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Update User Profile
        ///</summary>
        [Authorize(Roles="Admin,Student")]
        [HttpPut]       
        public ActionResult UpdateUserProfile(UserInfo model)
        {
            try
            {
                var objUser=_context.Users.SingleOrDefault(opt=>opt.UserId==model.UserId);               
                objUser.FullName=model.FullName;
                objUser.Mobile=model.Mobile;
                objUser.Address=model.Address;
                objUser.DateOfBirth=model.DateOfBirth;
                objUser.ImagePath=model.ImagePath;
                objUser.LastUpdatedBy=model.LastUpdatedBy;
                objUser.LastUpdatedDate=DateTime.Now;
                _context.SaveChanges();
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully updated" });                         
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Change User Password
        ///</summary>
        [Authorize(Roles="Admin,Student")]
        [HttpPut]       
        public ActionResult ChangeUserPassword(UserInfo model)
        {
            try
            {
                var objUser=_context.Users.SingleOrDefault(opt=>opt.UserId==model.UserId);
                objUser.Password=model.Password;           
                _context.SaveChanges();
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully changed" });                          
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Profile picture upload
        ///</summary>
        [Authorize(Roles="Admin,Student")]   
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "ProfileImages");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0 && file.ContentType.StartsWith("image/"))
                {
                    var fileName = Guid.NewGuid().ToString()+"_"+ ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return Accepted(new Confirmation { Status = "error", ResponseMsg = "Not an image" });
                }
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });               
            }
        }

        ///<summary>
        ///Get Role List
        ///</summary>
        [Authorize(Roles="Admin")]     
        [HttpGet]        
        public ActionResult GetUserRoleList()
        {
            try
            {
                var userRoleList=_userRoleRepo.SelectAll();
                return Ok(userRoleList);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Delete Role by id
        ///</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult DeleteSingleRole(int id)
        {
            try
            {
                if(id==1 || id==2)
                {
                    return Accepted(new Confirmation { Status = "restricted", ResponseMsg = "This Role is restricted to delete." });
                }
                else
                {
                    _userRoleRepo.Delete(id);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully deleted" });
                }          
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Create User Role
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPost]       
        public ActionResult CreateUserRole(UserRole model)
        {
            try
            {                  
                var objCheck=_context.UserRoles.SingleOrDefault(opt=>opt.RoleName.ToLower()==model.RoleName.ToLower());
                if(objCheck==null)
                {
                    model.DateAdded=DateTime.Now;
                    model.IsActive=true;
                    _userRoleRepo.Insert(model);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });                  
                }
                else
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Role name!" });
                }                    
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Update User Role
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut]       
        public ActionResult UpdateUserRole(UserRole model)
        {
            try
            {
                var objUserRole=_context.UserRoles.SingleOrDefault(opt=>opt.UserRoleId==model.UserRoleId);
                var objCheck=_context.UserRoles.SingleOrDefault(opt=>opt.RoleName.ToLower()==model.RoleName.ToLower());

                if(objCheck!=null && objCheck.RoleName.ToLower()!=objUserRole.RoleName.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate User Role name!" });
                }
                else if(model.UserRoleId==1 || model.UserRoleId==2)
                {
                    objUserRole.DisplayName=model.DisplayName;
                    objUserRole.RoleDesc=model.RoleDesc;
                    objUserRole.LastUpdatedBy=model.LastUpdatedBy;
                    objUserRole.LastUpdatedDate=DateTime.Now;
                    _context.SaveChanges();
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully updated" });
                }
                else
                {
                    objUserRole.RoleName=model.RoleName;
                    objUserRole.DisplayName=model.DisplayName;
                    objUserRole.RoleDesc=model.RoleDesc;
                    objUserRole.LastUpdatedBy=model.LastUpdatedBy;
                    objUserRole.LastUpdatedDate=DateTime.Now;
                    _context.SaveChanges();
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully updated" });
                }                                             
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }


        string GenerateJwtToken(UserInfo userInfo)
        {
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserId.ToString()),
                new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role",userInfo.RoleName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(360),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}