using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuizplusApi.Models;
using QuizplusApi.Models.Menu;
using QuizplusApi.ViewModels.Helper;
using QuizplusApi.ViewModels.Menu;

namespace QuizplusApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MenuController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly ISqlRepository<AppMenu> _menuRepo;
        private readonly ISqlRepository<MenuMapping> _menuMappingRepo;

        public MenuController(
                            IConfiguration config,
                            AppDbContext context,                           
                            ISqlRepository<AppMenu> menuRepo,                          
                            ISqlRepository<MenuMapping> menuMappingRepo)
        { 
            _config=config;        
            _context = context;          
            _menuRepo = menuRepo;
            _menuMappingRepo=menuMappingRepo;
        }

        ///<summary>
        ///Get App Menu List
        ///</summary>
        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult GetMenuList()
        {
            try
            {              
                var menuList=_menuRepo.SelectAll();
                return Ok(menuList);           
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Delete Menu by Id
        ///</summary>
        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public ActionResult DeleteSingleMenu(int id)
        {
            try
            {   
                if(id>=1 && id<=9)
                {
                    return Accepted(new Confirmation { Status = "restricted", ResponseMsg = "This Menu is restricted to delete." });
                }
                else
                {
                    _menuRepo.Delete(id);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully deleted" });
                }                                                
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Create App Menu
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPost]       
        public ActionResult CreateMenu(AppMenu model)
        {
            try
            {      
                var objCheck=_context.AppMenus.Where(q=>q.MenuTitle.ToLower()==model.MenuTitle.ToLower()||
                q.Url.ToLower()==model.Url.ToLower()||model.SortOrder<=0||q.SortOrder==model.SortOrder).FirstOrDefault();
                if(objCheck==null)
                {                  
                    model.DateAdded=DateTime.Now;
                    model.IsActive=true;
                    var obj=_menuRepo.Insert(model);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully saved" });                                     
                }
                else if(objCheck.MenuTitle.ToLower()==model.MenuTitle.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Menu Title!" });
                }
                else if(objCheck.Url.ToLower()==model.Url.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate URL!" });
                }
                else if(objCheck.SortOrder==model.SortOrder)
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Order No.!" });
                }
                else if(model.SortOrder<=0)
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Order No. must greater than 0!" });
                }
                else
                {
                    return Accepted(new Confirmation { Status = "error", ResponseMsg = "Something unexpected!" });
                }
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Update App Menu
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpPut]       
        public ActionResult UpdateMenu(AppMenu model)
        {
            try
            {
                var objMenu=_context.AppMenus.SingleOrDefault(opt=>opt.AppMenuId==model.AppMenuId);
                var objMenuTitleCheck=_context.AppMenus.SingleOrDefault(opt=>opt.MenuTitle.ToLower()==model.MenuTitle.ToLower());
                var objUrlCheck=_context.AppMenus.SingleOrDefault(opt=>opt.Url.ToLower()==model.Url.ToLower());
                var objSortOrderCheck=_context.AppMenus.SingleOrDefault(opt=>opt.SortOrder==model.SortOrder);

                if(model.SortOrder<=0)
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Order No. must greater than 0!" });
                }
                else if(objMenuTitleCheck!=null && objMenuTitleCheck.MenuTitle.ToLower()!=objMenu.MenuTitle.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Menu Title!" });
                }
                else if(objUrlCheck!=null && objUrlCheck.Url.ToLower()!=objMenu.Url.ToLower())
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Url!" });
                }
                else if(objSortOrderCheck!=null && objSortOrderCheck.SortOrder!=objMenu.SortOrder)
                {
                    return Accepted(new Confirmation { Status = "duplicate", ResponseMsg = "Duplicate Order No.!" });
                }
                else
                { 
                    objMenu.MenuTitle=model.MenuTitle;
                    objMenu.Url=model.Url;
                    objMenu.SortOrder=model.SortOrder;
                    objMenu.IconClass=model.IconClass;              
                    objMenu.LastUpdatedBy=model.LastUpdatedBy;
                    objMenu.LastUpdatedDate=DateTime.Now;
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
        ///App Side bar menus
        ///</summary>
        [Authorize(Roles="Admin,Student")]
        [HttpGet("{roleId}")]
        public ActionResult GetSidebarMenu(int roleId)
        {
            try
            {
                var menuList=(from m in _context.MenuMappings join a in _context.AppMenus
                on m.AppMenuId equals a.AppMenuId orderby a.SortOrder where m.UserRoleId.Equals(roleId)  
                select new {a.AppMenuId,a.MenuTitle,a.Url,a.IconClass});
                return Ok(menuList);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Get all menus for assinging to Role
        ///</summary>
        [Authorize(Roles="Admin")]
        [HttpGet("{userRoleId}")]
        public ActionResult GetAllMenu(int userRoleId)
        {
            try
            {
                var allMenus=from m in _context.AppMenus select new {m.AppMenuId,m.MenuTitle,m.Url,m.IconClass,
                IsSelected=m.AppMenuId.Equals((from mm in _context.MenuMappings where mm.UserRoleId.Equals(userRoleId)
                && mm.AppMenuId.Equals(m.AppMenuId) select new {mm.AppMenuId}).FirstOrDefault().AppMenuId)?true:false};
                return Ok(allMenus);
            }
            catch (Exception ex)
            {              
                return Accepted(new Confirmation { Status = "error", ResponseMsg = ex.Message });
            }
        }

        ///<summary>
        ///Assign App Menu
        ///</summary>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public ActionResult MenuAssign(MenuOperation model)
        {
            try
            {              
                var objCheck=_context.MenuMappings.SingleOrDefault(opt=>opt.UserRoleId==model.UserRoleId&&opt.AppMenuId==model.MenuId);  
                if(objCheck==null)
                {
                    var objAssign=new MenuMapping{UserRoleId=model.UserRoleId,
                    AppMenuId=model.MenuId,IsActive=true,DateAdded=DateTime.Now,AddedBy=model.AddedBy};

                    _menuMappingRepo.Insert(objAssign);
                    return Ok(new Confirmation { Status = "success", ResponseMsg = "Successfully assigned" });                          
                }
                else
                {
                   _menuMappingRepo.Delete(objCheck.MenuMappingId); 
                    return Ok(new Confirmation { Status = "delete", ResponseMsg = "Successfully un-assigned" }); 
                }           
            }
            catch(Exception ex)
            {
                return Accepted(new Confirmation { Status = "unknown", ResponseMsg = ex.Message });  
            }
            
        }
    }
}