using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuizplusApi.Models.Question;
using QuizplusApi.Models.Quiz;
using QuizplusApi.ViewModels.Helper;
using QuizplusApi.Models;
using QuizplusApi.ViewModels.UserLog;
using QuizplusApi.ViewModels.User;

namespace QuizplusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DashboardController:ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context=context;
        }
        
        ///<summary>
        ///Get running Quizes
        ///</summary>
        [HttpGet("{email}")]
        [Authorize(Roles="Admin,Student")]
        public ActionResult GetRunningQuizes(string email)
        {
            try
            {
                List<QuizTopic> availableQuizes=new List<QuizTopic>();
                List<QuizTopic> availableQuizesFilter=new List<QuizTopic>();

                var quizes=_context.QuizTopics.Where(q=>q.IsRunning==true);
                var listWithOutCustomInput= quizes.Where(q=>q.QuizParticipantOptionId==1);
                var customInputsList=quizes.Where(q=>q.QuizParticipantOptionId==2 && 
                email==_context.QuizParticipants.SingleOrDefault(m=>m.Email==email && m.QuizTopicId==q.QuizTopicId).Email);

                //check for all students
                foreach(QuizTopic item in listWithOutCustomInput)
                {
                    if(item.QuizscheduleStartTime==null && item.QuizscheduleEndTime==null)
                    {
                        availableQuizes.Add(item);
                    }
                }
                foreach(QuizTopic item in listWithOutCustomInput)
                {
                    if(item.QuizscheduleStartTime!=null && item.QuizscheduleEndTime==null)
                    {
                        if(DateTime.UtcNow.Date>=item.QuizscheduleStartTime)
                        {
                            availableQuizes.Add(item);
                        }                       
                    }
                }
                foreach(QuizTopic item in listWithOutCustomInput)
                {
                    if(item.QuizscheduleStartTime!=null && item.QuizscheduleEndTime!=null)
                    {
                        if(DateTime.UtcNow.Date>=item.QuizscheduleStartTime && DateTime.UtcNow.Date<=item.QuizscheduleEndTime)
                        {
                            availableQuizes.Add(item);
                        }                       
                    }
                }

                //check for custom inputs
                foreach(QuizTopic item in customInputsList)
                {
                    if(item.QuizscheduleStartTime==null && item.QuizscheduleEndTime==null)
                    {
                        availableQuizes.Add(item);
                    }
                }
                foreach(QuizTopic item in customInputsList)
                {
                    if(item.QuizscheduleStartTime!=null && item.QuizscheduleEndTime==null)
                    {
                        if(DateTime.UtcNow.Date>=item.QuizscheduleStartTime)
                        {
                            availableQuizes.Add(item);
                        }                       
                    }
                }
                foreach(QuizTopic item in customInputsList)
                {
                    if(item.QuizscheduleStartTime!=null && item.QuizscheduleEndTime!=null)
                    {
                        if(DateTime.UtcNow.Date>=item.QuizscheduleStartTime && DateTime.UtcNow.Date<=item.QuizscheduleEndTime)
                        {
                            availableQuizes.Add(item);
                        }                       
                    }
                }

                foreach(QuizTopic item in availableQuizes)
                {
                    if(item.AllowMultipleAttempt==true)
                    {
                        availableQuizesFilter.Add(item);
                    }
                    else if(item.AllowMultipleAttempt==false)
                    {
                        int recordCount=_context.QuizResponseInitials.Where(q=>q.QuizTopicId==item.QuizTopicId && q.Email==email).Count();
                        if(recordCount==0)
                        {
                            availableQuizesFilter.Add(item);
                        }
                    }
                }


                var listWithQuestionCount=from q in availableQuizesFilter select new {q.QuizTopicId,q.QuizTitle,
                q.QuizTime,q.QuizTotalMarks,q.QuizMarkOptionId,q.QuizParticipantOptionId,q.AllowMultipleInputByUser,
                q.AllowMultipleAnswer,q.AllowMultipleAttempt,q.AllowCorrectOption,q.QuizPassMarks,
                q.QuizscheduleStartTime,q.QuizscheduleEndTime,q.IsRunning,q.QuizPrice,
                PaymentCount=_context.QuizPayments.Where(m=>m.Email==email && m.QuizTopicId==q.QuizTopicId).Count(),
                QuestionsCount=_context.QuizQuestions.Where(m=>m.QuizTopicId==q.QuizTopicId).Count()};
                return Ok(listWithQuestionCount);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get overall status
        ///</summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetStatus()
        {
            try
            {
                int totalStudents=_context.Users.Where(q=>q.UserRoleId==2).Count();
                int totalQuizes=_context.QuizTopics.Count();
                int liveQuizes=_context.QuizTopics.Where(q=>q.IsRunning==true).Count();
                int totalQuestions=_context.QuizQuestions.Count();
                return Ok(new Status{TotalStudents=totalStudents,TotalQuizes=totalQuizes,LiveQuizes=liveQuizes,TotalQuestions=totalQuestions});
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }
        ///<summary>
        ///Get date wise Login summary
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{id}")]      
        public ActionResult GetLogInSummaryByDate(int id)
        {
            try
            {
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.LogHistories.GroupBy(e=>e.LogInTime.Date).OrderByDescending(e=>e.Key).Take(10)
                    .Select(e => new{ e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{Date=s.Key,Count=s.Count});
                    return Ok(userList);
                }   
                else
                {
                    var list=_context.LogHistories.Where(e=>e.UserId==id).GroupBy(e=>e.LogInTime.Date).OrderByDescending(e=>e.Key).Take(10)
                    .Select(e => new { e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{Date=s.Key,Count=s.Count});
                    return Ok(userList);
                }                                                                      
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get month wise Login summary
        ///</summary>
        [AllowAnonymous] 
        [HttpGet("{id}")]
        public ActionResult GetLogInSummaryByMonth(int id)
        {
            try
            {              
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.LogHistories.Where(q=>q.LogInTime.Year==DateTime.Now.Year).GroupBy(e=>e.LogInTime.Month)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Month=MonthName.ReturnMonthName(s.Key),Count=s.Count});              
                    return Ok(userList);
                }
                else
                {
                    var list=_context.LogHistories.Where(e=>e.UserId==id).GroupBy(e=>e.LogInTime.Month)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Month=MonthName.ReturnMonthName(s.Key),Count=s.Count});              
                    return Ok(userList);
                }              
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get Browser Login summary
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult GetBrowserCount(int id)
        {
            try
            {     
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.LogHistories.Where(e=>e.Browser!=null).GroupBy(e=>e.Browser)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Browser=s.Key,Count=s.Count});             
                    return Ok(userList);
                }
                else
                {
                    var list=_context.LogHistories.Where(e=>e.UserId==id && e.Browser!=null).GroupBy(e=>e.Browser)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Browser=s.Key,Count=s.Count});             
                    return Ok(userList);
                }                     
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get Platform Login summary
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult GetPlatformCount(int id)
        {
            try
            {     
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.LogHistories.Where(e=>e.Platform!=null).GroupBy(e=>e.Platform)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Platform=s.Key,Count=s.Count});             
                    return Ok(userList);
                }
                else
                {
                    var list=_context.LogHistories.Where(e=>e.UserId==id && e.Platform!=null).GroupBy(e=>e.Platform)
                        .Select(e => new { e.Key, Count = e.Count() });                       
                    var userList=list.Select(s=>new UserLog{Platform=s.Key,Count=s.Count});             
                    return Ok(userList);
                }                     
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get date wise Quiz Attendance count
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{id}")]      
        public ActionResult GetQuizCountByDate(int id)
        {
            try
            {
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.QuizResponseInitials.GroupBy(e=>e.DateAdded.Date).OrderByDescending(e=>e.Key).Take(10)
                    .Select(e => new{ e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{Date=s.Key,Count=s.Count});
                    return Ok(userList);
                }   
                else
                {
                    var list=_context.QuizResponseInitials.Where(e=>e.UserId==id).GroupBy(e=>e.DateAdded.Date).OrderByDescending(e=>e.Key).Take(10)
                    .Select(e => new { e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{Date=s.Key,Count=s.Count});
                    return Ok(userList);
                }                                                                      
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get Quiz wise user attendance count
        ///</summary>
        [AllowAnonymous]
        [HttpGet("{id}")]      
        public ActionResult GetUserCountByQuiz(int id)
        {
            try
            {
                var objUser=_context.Users.Where(e=>e.UserId==id).FirstOrDefault();
                if(objUser.UserRoleId==1)
                {
                    var list=_context.QuizResponseInitials.GroupBy(e=>e.QuizTitle)
                    .Select(e => new{ e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{QuizTitle=s.Key,Count=s.Count});
                    return Ok(userList);
                }   
                else
                {
                    var list=_context.QuizResponseInitials.Where(e=>e.UserId==id).GroupBy(e=>e.QuizTitle)
                    .Select(e => new { e.Key, Count = e.Count() });
                    var userList=list.Select(s=>new UserLog{QuizTitle=s.Key,Count=s.Count});
                    return Ok(userList);
                }                                                                      
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }
    }
}