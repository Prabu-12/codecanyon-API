using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizplusApi.Models;
using QuizplusApi.Models.Others;
using QuizplusApi.Services;
using QuizplusApi.ViewModels.Email;
using QuizplusApi.ViewModels.Helper;
using QuizplusApi.ViewModels.User;

namespace QuizplusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SettingsController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ISqlRepository<Faq> _faqRepo;
        private readonly IMailService _mailService;

        public SettingsController(AppDbContext context,
                                ISqlRepository<Faq> faqRepo,
                                IMailService mailService)
        {
            _context=context;
            _faqRepo=faqRepo;
            _mailService=mailService;
        }

        ///<summary>
        ///Sent Password Email
        ///</summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendPasswordMail(ForgetPassword request)
        {
            try
            {
                await _mailService.SendPasswordEmailAsync(request);
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Please check your Email"});
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }              
        }

        ///<summary>
        ///Sent Welcome Email
        ///</summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendWelcomeMail(WelcomeRequest request)
        {
            try
            {
                await _mailService.SendWelcomeEmailAsync(request);
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Email sent Successful"});
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Sent Email to checked students
        ///</summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SentEmailToCheckedStudents(ReExamRequest obj)
        {
            try
            {              
                await _mailService.SendReportEmailAsync(obj); 
                return Ok();           
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Sent Invitation Email
        ///</summary>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SendInvitationMail(List<Invitation> listOfAddress)
        {
            try
            {
                await _mailService.SendInvitationEmailAsync(listOfAddress);
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Email sent Successful"});
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }
        ///<summary>
        ///Get Site Settings
        ///</summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetSiteSettings()
        {
            try
            {              
                var siteSettings=_context.SiteSettings.SingleOrDefault();
                return Ok(siteSettings);           
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Update Settings
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut]       
        public ActionResult UpdateSettings(SiteSettings model)
        {
            try
            {
                var objSettings=_context.SiteSettings.SingleOrDefault(opt=>opt.SiteSettingsId==model.SiteSettingsId);
                objSettings.SiteTitle=model.SiteTitle;
                objSettings.WelComeMessage=model.WelComeMessage;
                objSettings.CopyRightText=model.CopyRightText;
                objSettings.EndExam=model.EndExam;
                objSettings.LogoOnExamPage=model.LogoOnExamPage;
                objSettings.PaidRegistration=model.PaidRegistration;
                objSettings.RegistrationPrice=model.RegistrationPrice==null?0:model.RegistrationPrice;
                objSettings.Currency=model.Currency;
                objSettings.CurrencySymbol=model.CurrencySymbol;
                objSettings.StripePubKey=model.StripePubKey;
                objSettings.StripeSecretKey=model.StripeSecretKey;
                objSettings.DefaultEmail=model.DefaultEmail;
                objSettings.Password=model.Password;
                objSettings.Host=model.Host;
                objSettings.Port=model.Port;
                objSettings.LogoPath=model.LogoPath;
                objSettings.FaviconPath=model.FaviconPath;
                objSettings.AppBarColor=model.AppBarColor;
                objSettings.FooterColor=model.FooterColor;
                objSettings.BodyColor=model.BodyColor;
                objSettings.AllowWelcomeEmail=model.AllowWelcomeEmail;
                objSettings.AllowFaq=model.AllowFaq;
                objSettings.LastUpdatedBy=model.LastUpdatedBy;
                objSettings.LastUpdatedDate=DateTime.Now;
                _context.SaveChanges();
                return Ok(objSettings);                                           
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Update Client Url
        ///</summary>
        [AllowAnonymous]
        [HttpPut]       
        public ActionResult UpdateClientUrl(UserInfo model)
        {
            try
            {
                var objSettings=_context.SiteSettings.SingleOrDefault();
                objSettings.ClientUrl=model.DisplayName;
                _context.SaveChanges();
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }
        ///<summary>
        ///Site Logo upload
        ///</summary>
        [Authorize(Roles="Admin")]   
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadLogo()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Logo");
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
        ///Site Favicon upload
        ///</summary>
        [Authorize(Roles="Admin")]   
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadFavicon()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Favicon");
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
        ///Get FAQ List
        ///</summary>
        [HttpGet]
        [Authorize(Roles="Admin,Student")]
        public ActionResult GetFaqList()
        {
            try
            {              
                var faqList=_faqRepo.SelectAll();
                return Ok(faqList);           
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Delete FAQ by Id
        ///</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult DeleteFaq(int id)
        {
            try
            {      
                _faqRepo.Delete(id);
                return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully deleted" });                                             
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Create Faq
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPost]       
        public ActionResult CreateFaq(Faq model)
        {
            try
            {                  
                var objCheck=_context.Faqs.SingleOrDefault(opt=>opt.Title.ToLower()==model.Title.ToLower());
                if(objCheck==null)
                {
                    model.DateAdded=DateTime.Now;
                    model.IsActive=true;
                    _faqRepo.Insert(model);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });                  
                }
                else
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate FAQ!" });
                }                    
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }

        ///<summary>
        ///Update Faq
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut]       
        public ActionResult UpdateFaq(Faq model)
        {
            try
            {
                var objFaq=_context.Faqs.SingleOrDefault(opt=>opt.FaqId==model.FaqId);
                var objCheck=_context.Faqs.SingleOrDefault(opt=>opt.Title.ToLower()==model.Title.ToLower());

                if(objCheck!=null && objCheck.Title.ToLower()!=objFaq.Title.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate FAQ!" });
                }
                else
                {
                    objFaq.Title=model.Title;
                    objFaq.Description=model.Description;
                    objFaq.LastUpdatedBy=model.LastUpdatedBy;
                    objFaq.LastUpdatedDate=DateTime.Now;
                    _context.SaveChanges();
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully updated" });
                }                                             
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }
    }
}