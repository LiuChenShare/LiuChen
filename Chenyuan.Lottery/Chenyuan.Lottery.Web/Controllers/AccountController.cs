using Chenyuan.Lottery.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Chenyuan.Lottery.Web.Controllers
{
    public class AccountController : ChenyuanControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录HttpPost
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CheckLogin(string userNamea, string userPas)
        {
            //验证用户登录
            var login = _service.Login(userNamea.Trim(), userPas);
            if (!login)
            {
                return Json(new { Success = false, Messages = "用户名或密码错误" });
            }
            var account = _workContext.CurrentUser.AccountId;
            ////存储Session
            //Session["userName"] = Request.Form["txtUid"].ToString();  //把用户id保存到session中
            ////读取Session
            //if (Session["userName"] != null)
            //{
            //    Response.Write(Session["userName"].ToString() + "---点击获取session"); //获取session，并写入页面
            //}

            return Json(new { success = true});
        }
    }
}