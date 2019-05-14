﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCore.Admin.Controllers;
using TestCore.Domain.CommonEntity;
using TestCore.Domain.Entity;
using TestCore.Domain.InputEntity;
using TestCore.Domain.ViewEntity;
using TestCore.Framework;
using TestCore.IRepository.User;
using TestCore.IService.User;

namespace TestCore.Admin.Areas.Users.Controllers
{
    [Area(AreaNames.UsersManage)]
    public class UserController : BaseController
    {
        private readonly IUsersSvc _usersSvc;
       
        public UserController(IUsersSvc usersSvc)
        {
            this._usersSvc = usersSvc;
        }
        public IActionResult Index()
        {
            var userList = _usersSvc.GetList<ViewUser>(null).ToList().OrderByDescending(t => t.Id);
            ViewBag.userList = userList;
            return View();
        }

        [HttpGet]
        public IActionResult Woodyapp(string route)
        {
            ViewBag.url = route;
            return View();
        }

        #region GetUserDetials 用户基本信息
        [HttpGet]
        public IActionResult GetUserDetials(int Id)
        {
            ViewUser userInfo = _usersSvc.GetModel<ViewUser>(new { Id });
            return View(userInfo);
        }

        #endregion

        #region GetUserPrice 获取用户分成设置
        [HttpGet]
        public IActionResult GetUserPrice(int Id)
        {
            List<ReturnUserPrice> list = new List<ReturnUserPrice>();
            var userPrice = _usersSvc.GetList<Userprice>(new { Userid = Id });
            var acc = _usersSvc.GetList<Acc>(new { Is_State = 0 }, "Is_display desc");
            if (userPrice == null)
            {
                foreach (var item in acc)
                {
                    ReturnUserPrice mode = new ReturnUserPrice
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Acwid = item.Acwid,
                        Uprice_default = item.Uprice,
                        Is_state = item.Is_state,
                        Is_display = item.Is_display
                    };
                    list.Add(mode);
                }               
            }
            else
            {
                foreach (var item in userPrice)
                {
                    ReturnUserPrice mode = new ReturnUserPrice();
                    if (item.Channelid > 0)
                    {
                        mode.Id = item.Channelid;
                    }
                    var name = acc.Where(t => t.Id == item.Channelid).ToList();
                    if (name.Count == 0)
                    {
                        continue;
                    }
                    if (list.Where(x => x.Acwid == name.FirstOrDefault().Acwid).Count()>0)
                    {
                        continue;
                    }
                    mode.Name = name.FirstOrDefault().Name;
                    mode.Acwid =name.FirstOrDefault().Acwid;
                    mode.Uprice_default = name.FirstOrDefault().Uprice;
                    mode.Is_state = item.Is_state;
                    mode.Is_display =name.FirstOrDefault().Is_display;
                    list.Add(mode);
                }
            }
            ViewBag.List = list;
            ViewBag.Userid = userPrice.FirstOrDefault().Userid;
            ViewBag.otherList = acc;
            return View();
        }

        #endregion

        #region SaveUserPrice 更改用户分成设置
        [HttpPost]
        public IActionResult SaveUserPrice([FromBody]JObject obj)
        {
            var str = obj["postJson"].ToString();
            var priceList = JsonConvert.DeserializeObject<List<JsonUprice>>(str);
            if (priceList != null)
            {
                List<Userprice> list = new List<Userprice>();
                foreach (var item in priceList)
                {
                    Userprice userprice = new Userprice() {
                        Userid = item.Userid,
                        Channelid = item.Channelid,
                        Uprice = item.Userid,
                        Gprice = item.Gprice,
                        Is_state = item.Is_state,
                        Addtime = DateTime.Now
                    };
                    list.Add(userprice);
                }
                var res = _usersSvc.InsertUpriceList(list);
                if (res.IsSuccess)
                {
                    this.ResponseResult.Result = 1;
                    this.ResponseResult.Message = res.Message;
                    this.ResponseResult.Data = "Users/User";
                }
                else
                {
                    this.ResponseResult.Result = 0;
                    this.ResponseResult.Message = res.Message;
                    this.ResponseResult.Data = "Users/User";
                }
            }
            else
            {
                this.ResponseResult.Result = 0;
                this.ResponseResult.Message = "更新失败";
                this.ResponseResult.Data = "Users/User";
            }
            return Json(this.ResponseResult);
        }
        #endregion

        #region ResetUserPrice 重置用户分成设置
        [HttpPost]
        public IActionResult ResetUserPrice()
        {
            return Ok();
        }
        #endregion
    }
}