using System;
using Microsoft.EntityFrameworkCore;
using QuizplusApi.Models.Menu;
using QuizplusApi.Models.Others;
using QuizplusApi.Models.Question;
using QuizplusApi.Models.Quiz;
using QuizplusApi.Models.User;

namespace QuizplusApi.ViewModels.Helper
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(b=>
            {
                b.HasKey(e=>e.UserRoleId);  
                b.Property(b=>b.UserRoleId).HasIdentityOptions(startValue:3);        
                b.HasData(
                    new UserRole
                    {
                        UserRoleId=1,
                        RoleName="Admin",
                        DisplayName="Admin",
                        RoleDesc="Application Admin",
                        IsActive=true,
                        DateAdded=DateTime.Now,
                        AddedBy=1,
                        IsMigrationData=true
                    },
                    new UserRole
                    {
                        UserRoleId=2,
                        RoleName="Student",
                        DisplayName="Candidate",
                        RoleDesc="All Students",
                        IsActive=true,
                        DateAdded=DateTime.Now,
                        AddedBy=1,
                        IsMigrationData=true
                    });          
            });

            modelBuilder.Entity<Users>(b=>{
                b.HasKey(e=>e.UserId);  
                b.Property(b=>b.UserId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new Users
                    {
                        UserId=1,
                        UserRoleId=1,
                        FullName="John Doe",
                        Email="admin@exam-systems.com",                       
                        Password="12345678",
                        IsActive=true,
                        DateAdded=DateTime.Now,                       
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new Users
                    {
                        UserId=2,
                        UserRoleId=2,
                        FullName="Mark Wood",
                        Email="candidate@exam-systems.com",                       
                        Password="12345678",
                        IsActive=true,
                        DateAdded=DateTime.Now,                       
                        AddedBy=1,                       
                        IsMigrationData=true
                    });
            });

            modelBuilder.Entity<AppMenu>(b=>{
                b.HasKey(e=>e.AppMenuId);  
                b.Property(b=>b.AppMenuId).HasIdentityOptions(startValue:10);              
                b.HasData(
                    new AppMenu
                    {
                        AppMenuId=1,
                        MenuTitle="Dashboard",
                        Url="/dashboard",
                        SortOrder=1,                       
                        IconClass="dashboard",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=2,
                        MenuTitle="Menus",
                        Url="/menu/menus",
                        SortOrder=2,                       
                        IconClass="menu_open",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=3,
                        MenuTitle="Roles",
                        Url="/user/roles",
                        SortOrder=3,                       
                        IconClass="supervised_user_circle",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=4,
                        MenuTitle="Users",
                        Url="/user/users",
                        SortOrder=4,                       
                        IconClass="mdi-account-multiple",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=5,
                        MenuTitle="Category",
                        Url="/question/category",
                        SortOrder=5,                       
                        IconClass="category",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=6,
                        MenuTitle="Assessments",
                        Url="/quiz/topics",
                        SortOrder=6,                       
                        IconClass="emoji_objects",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=7,
                        MenuTitle="Questions",
                        Url="/question/quizes",
                        SortOrder=7,                       
                        IconClass="help_center",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=8,
                        MenuTitle="Reports",
                        Url="/report/students",
                        SortOrder=8,                       
                        IconClass="description",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=9,
                        MenuTitle="Certificate Template",
                        Url="/report/certificates",
                        SortOrder=9,                       
                        IconClass="card_giftcard",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=10,
                        MenuTitle="App Settings",
                        Url="/settings/appSettings",
                        SortOrder=11,                       
                        IconClass="settings",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new AppMenu
                    {
                        AppMenuId=11,
                        MenuTitle="Examine & Reports",
                        Url="/report/admin",
                        SortOrder=10,                       
                        IconClass="description",
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    });
            });

            modelBuilder.Entity<MenuMapping>(b=>{
                b.HasKey(e=>e.MenuMappingId);  
                b.Property(b=>b.MenuMappingId).HasIdentityOptions(startValue:12);              
                b.HasData(
                    new MenuMapping
                    {
                        MenuMappingId=1,
                        UserRoleId=1,
                        AppMenuId=1,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=2,
                        UserRoleId=1,
                        AppMenuId=2,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=3,
                        UserRoleId=1,
                        AppMenuId=3,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=4,
                        UserRoleId=1,
                        AppMenuId=4,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=5,
                        UserRoleId=1,
                        AppMenuId=5,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=6,
                        UserRoleId=1,
                        AppMenuId=6,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=7,
                        UserRoleId=1,
                        AppMenuId=7,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=8,
                        UserRoleId=1,
                        AppMenuId=11,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=9,
                        UserRoleId=1,
                        AppMenuId=9,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=10,
                        UserRoleId=1,
                        AppMenuId=10,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=11,
                        UserRoleId=2,
                        AppMenuId=1,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new MenuMapping
                    {
                        MenuMappingId=12,
                        UserRoleId=2,
                        AppMenuId=8,
                        IsActive=true,
                        DateAdded=DateTime.Now,                      
                        AddedBy=1,                       
                        IsMigrationData=true
                    });
            });

            modelBuilder.Entity<QuizMarkOption>(b=>{
                b.HasKey(e=>e.QuizMarkOptionId);  
                b.Property(b=>b.QuizMarkOptionId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new QuizMarkOption
                    {
                        QuizMarkOptionId=1,
                        QuizMarkOptionName="Equal distribution"
                    },
                    new QuizMarkOption
                    {
                        QuizMarkOptionId=2,
                        QuizMarkOptionName="No marks(Survey)"
                    },
                    new QuizMarkOption
                    {
                        QuizMarkOptionId=3,
                        QuizMarkOptionName="Question wise set"
                    });
            });

            modelBuilder.Entity<QuizParticipantOption>(b=>{
                b.HasKey(e=>e.QuizParticipantOptionId);  
                b.Property(b=>b.QuizParticipantOptionId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new QuizParticipantOption
                    {
                        QuizParticipantOptionId=1,
                        QuizParticipantOptionName="All registered candidates"
                    },
                    new QuizParticipantOption
                    {
                        QuizParticipantOptionId=2,
                        QuizParticipantOptionName="Custom Input"
                    });
            });

            modelBuilder.Entity<ReportType>(b=>{
                b.HasKey(e=>e.ReportTypeId);  
                b.Property(b=>b.ReportTypeId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new ReportType
                    {
                        ReportTypeId=1,
                        ReportTypeName="Pending Examine"
                    },
                    new ReportType
                    {
                        ReportTypeId=2,
                        ReportTypeName="Reports"
                    });
            });

            modelBuilder.Entity<QuestionType>(b=>{
                b.HasKey(e=>e.QuestionTypeId);  
                b.Property(b=>b.QuestionTypeId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new QuestionType
                    {
                        QuestionTypeId=1,
                        QuestionTypeName="MCQ"
                    },
                    new QuestionType
                    {
                        QuestionTypeId=2,
                        QuestionTypeName="Descriptive"
                    });
            });

            modelBuilder.Entity<QuestionLavel>(b=>{
                b.HasKey(e=>e.QuestionLavelId);  
                b.Property(b=>b.QuestionLavelId).HasIdentityOptions(startValue:4);              
                b.HasData(
                    new QuestionLavel
                    {
                        QuestionLavelId=1,
                        QuestionLavelName="Easy"
                    },
                    new QuestionLavel
                    {
                        QuestionLavelId=2,
                        QuestionLavelName="Medium"
                    },
                    new QuestionLavel
                    {
                        QuestionLavelId=3,
                        QuestionLavelName="Hard"
                    });
            });

            modelBuilder.Entity<SiteSettings>(b=>{
                b.HasKey(e=>e.SiteSettingsId);  
                b.Property(b=>b.SiteSettingsId).HasIdentityOptions(startValue:2);              
                b.HasData(
                    new SiteSettings
                    {
                        SiteSettingsId=1,
                        SiteTitle="Quiz Plus",
                        WelComeMessage="Hello there,Sign in to start your task!",                       
                        CopyRightText="© 2021 Copyright Quiz Plus",
                        DefaultEmail="",
                        Host="smtp.gmail.com",
                        Port=587, 
                        LogoPath="",
                        FaviconPath="", 
                        AppBarColor="",
                        FooterColor="", 
                        BodyColor="",
                        AllowWelcomeEmail=true,
                        AllowFaq=true,
                        AllowRightClick=true,
                        EndExam=false,
                        LogoOnExamPage=true,
                        PaidRegistration=true,
                        RegistrationPrice=0,
                        Currency="",
                        CurrencySymbol="",
                        StripePubKey="",
                        StripeSecretKey="",
                        IsActive=true,
                        DateAdded=DateTime.Now,                       
                        AddedBy=1,                       
                        IsMigrationData=true
                    });
            });

            modelBuilder.Entity<Faq>(b=>{
                b.HasKey(e=>e.FaqId);  
                b.Property(b=>b.FaqId).HasIdentityOptions(startValue:3);              
                b.HasData(
                    new Faq
                    {
                        FaqId=1,
                        Title="What are the purposes of this app?",
                        Description="Quiz Plus will fulfill your need to take online Exams,Quizes as well as you can perform surveys by this app.",                                             
                        IsActive=true,
                        DateAdded=DateTime.Now,                       
                        AddedBy=1,                       
                        IsMigrationData=true
                    },
                    new Faq
                    {
                        FaqId=2,
                        Title="What will be requirements to take a Exam?",
                        Description="Nothing at all! You just need an active Email.",                                             
                        IsActive=true,
                        DateAdded=DateTime.Now,                       
                        AddedBy=1,                       
                        IsMigrationData=true
                    });
            });
        }
    }
}