using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuizplusApi.Models.Question;
using QuizplusApi.Models.Quiz;
using QuizplusApi.ViewModels.Helper;
using QuizplusApi.Models;

namespace QuizplusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReportController:ControllerBase
    {
        private readonly AppDbContext _context;
        public ReportController(AppDbContext context)
        {
            _context=context;
        }

        ///<summary>
        ///Get finished exam status
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Admin,Student")]
        public ActionResult GetFinishedExamInfo(int id)
        {
            try
            {
                var info=_context.QuizResponseInitials.SingleOrDefault(q=>q.QuizResponseInitialId==id);
                return Ok(info);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get finished exam result
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Admin,Student")]
        public ActionResult GetFinishedExamResult(int id)
        {
            try
            {
                var result=from d in _context.QuizResponseDetails join i in _context.QuizResponseInitials 
                on d.QuizResponseInitialId equals i.QuizResponseInitialId  join
                u in _context.Users on i.UserId equals u.UserId join t in _context.QuizTopics on i.QuizTopicId equals t.QuizTopicId
                where i.QuizResponseInitialId.Equals(id)
                select new{u.FullName,u.Email,u.Mobile,u.Address,i.AttemptCount,i.QuizTitle,i.QuizMark,i.QuizTime,i.UserObtainedQuizMark,
                i.TimeTaken,d.QuestionDetail,d.UserAnswer,d.IsAnswerSkipped,d.CorrectAnswer,d.AnswerExplanation,d.QuestionMark,
                d.UserObtainedQuestionMark,t.CertificateTemplateId,t.AllowCorrectOption,t.QuizPassMarks,t.QuizMarkOptionId};

                if(result.Count()==0)
                {
                    var basicResult=from i in _context.QuizResponseInitials 
                    join u in _context.Users on i.UserId equals u.UserId 
                    join t in _context.QuizTopics on i.QuizTopicId equals t.QuizTopicId
                    where i.QuizResponseInitialId.Equals(id)
                    select new{u.FullName,u.Email,u.Mobile,u.Address,i.AttemptCount,i.QuizTitle,i.QuizMark,i.QuizTime,i.UserObtainedQuizMark,
                    i.TimeTaken,t.CertificateTemplateId,t.AllowCorrectOption,t.QuizPassMarks,t.QuizMarkOptionId};
                    return Ok(basicResult);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get pending examine data
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult GetPendingExamine(int id)
        {
            try
            {
                var result=from d in _context.QuizResponseDetails join i in _context.QuizResponseInitials 
                on d.QuizResponseInitialId equals i.QuizResponseInitialId  join
                u in _context.Users on i.UserId equals u.UserId join q in _context.QuizQuestions on d.QuizQuestionId equals q.QuizQuestionId
                where i.QuizResponseInitialId==id && q.QuestionTypeId==2 && d.IsAnswerSkipped==false && d.IsExamined==false
                select new{
                    u.FullName,u.Email,u.Mobile,u.Address,i.AttemptCount,i.QuizTitle,i.QuizMark,i.QuizTime,i.UserObtainedQuizMark,
                    i.TimeTaken,d.QuizResponseDetailId,d.QuizQuestionId,d.QuestionDetail,d.UserAnswer,d.IsAnswerSkipped,d.CorrectAnswer,d.AnswerExplanation,d.QuestionMark,
                    d.UserObtainedQuestionMark
                };
                return Ok(result);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Update Quiz Marks Obtain
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut("{id}/{marks}")]       
        public ActionResult UpdateMarksObtain(int id,decimal marks)
        {
            try
            {
                using var transaction = _context.Database.BeginTransaction();

                var objDetail=_context.QuizResponseDetails.SingleOrDefault(q=>q.QuizResponseDetailId==id);
                objDetail.UserObtainedQuestionMark=marks;
                objDetail.IsExamined=true;
                _context.SaveChanges();

                decimal sumOfQuizMark=0;
                var objInitial=_context.QuizResponseInitials.SingleOrDefault(q=>q.QuizResponseInitialId==objDetail.QuizResponseInitialId);
                sumOfQuizMark=_context.QuizResponseDetails.Where(q=>q.QuizResponseInitialId==objDetail.QuizResponseInitialId).Sum(q=>q.UserObtainedQuestionMark);
                objInitial.UserObtainedQuizMark=sumOfQuizMark;
                if(_context.QuizResponseDetails.Where(q=>q.QuizResponseInitialId==objDetail.QuizResponseInitialId  && q.IsExamined==false).Count()==0)
                {
                    objInitial.IsExamined=true;
                }
                _context.SaveChanges();

                transaction.Commit();
                return Ok(new Confirmation { Status = "success", ResponseMsg = objDetail.QuizResponseInitialId.ToString() });
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get all exam result
        ///</summary>
        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult GetResults()
        {
            try
            {
                var results=from i in _context.QuizResponseInitials join u in _context.Users on i.UserId equals u.UserId join t in _context.QuizTopics on i.QuizTopicId equals t.QuizTopicId
                orderby i.DateAdded descending select new{u.FullName,u.Email,i.AttemptCount,i.QuizTopicId,i.QuizResponseInitialId,i.QuizTitle,i.QuizMark,i.QuizTime,
                i.UserObtainedQuizMark,i.TimeTaken,i.IsExamined,i.DateAdded,t.CertificateTemplateId,t.AllowCorrectOption,t.QuizPassMarks,t.QuizMarkOptionId};
                
                return Ok(results);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get result by user id
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Student")]
        public ActionResult GetResultsById(int id)
        {
            try
            {
                var results=from i in _context.QuizResponseInitials join u in _context.Users on i.UserId equals u.UserId join t in _context.QuizTopics on i.QuizTopicId equals t.QuizTopicId
                where i.UserId.Equals(id) && i.IsExamined.Equals(true) orderby i.DateAdded descending select new{u.FullName,u.Email,u.Mobile,u.Address,u.DateOfBirth,i.AttemptCount,i.QuizTopicId,i.QuizResponseInitialId,
                i.QuizTitle,i.QuizMark,i.QuizTime,i.UserObtainedQuizMark,i.TimeTaken,i.IsExamined,i.DateAdded,t.CertificateTemplateId,t.AllowCorrectOption,t.QuizPassMarks,t.QuizMarkOptionId};
                return Ok(results);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get filtered Quiz list for individual user
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Student")]
        public ActionResult GetFilteredQuiz(int id)
        {
            try
            {
                var results=_context.QuizResponseInitials.Select(s=>new{QuizTopicId=s.QuizTopicId,QuizTitle=s.QuizTitle,UserId=s.UserId,IsExamined=s.IsExamined}).Where(q=>q.UserId==id && q.IsExamined==true).Distinct();              
                return Ok(results);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get results by topic
        ///</summary>
        [HttpGet("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult GetResultByTopic(int id)
        {
            try
            {
                var results=from i in _context.QuizResponseInitials join u in _context.Users on i.UserId equals u.UserId
                where i.QuizTopicId.Equals(id) orderby i.UserId select new{u.FullName,u.Email,u.Mobile,u.Address,i.AttemptCount,i.QuizTopicId,i.QuizResponseInitialId,
                i.QuizTitle,i.QuizMark,i.QuizTime,i.UserObtainedQuizMark,i.TimeTaken};
                return Ok(results);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get Report Types
        ///</summary>
        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult GetReportType()
        {
            try
            {
                var list=_context.ReportTypes.ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });             
            }
        }
    }
}