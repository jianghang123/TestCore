using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestCore.Admin.Infrastructure;
using TestCore.Admin.Models;
using TestCore.Domain.CommonEntity;
using TestCore.IService.SysAdmin;

namespace TestCore.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IAdminSvc _adminSvc;
        private StringBuilder _menuHtml = new StringBuilder();
        public HomeController(IWorkContext workContext, IAdminSvc adminSvc)
        {
            this._workContext = workContext;
            this._adminSvc = adminSvc;
        }
        public async Task<IActionResult> Index()
        {
            TestCore.Domain.SysEntity.Admin admInfo = await _workContext.GetCurrentUser();
            //区域
            List<string> list = new List<string>{"users","orders","channels","articles", "agents"};
            if (admInfo != null)
            {
                string roleIds = _adminSvc.GetModel(new { admInfo.Id }).Limits;
                JObject jObject = JObject.Parse(roleIds.ToString());
                var data = jObject.SelectToken("data");
                for (int i = 0; i < data.Count(); i++)
                {
                    JObject roleData = JObject.Parse(data[i].ToString());
                    var actions = string.Empty;
                    foreach (JProperty jProperty in roleData.Properties())
                    {
                        if (list.Contains(jProperty.Name)) 
                        {
                            actions = jProperty.Name;
                            _menuHtml.Append("<dl>");
                            _menuHtml.AppendFormat("<dt><span class=\"glyphicon glyphicon-user\"></span>&nbsp;{0}</dt>", jProperty.Value);
                        }
                        else
                        {
                            _menuHtml.AppendFormat("<dd><a href=\"javascript:;\" name=\"/{0}/{1}\">{2}</a></dd>", actions,jProperty.Name,jProperty.Value);
                        }
                    }
                }
                ViewBag.MenuNav = _menuHtml.ToString();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View(admInfo);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
