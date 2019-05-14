using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TestCore.Common.Helper;
using TestCore.Domain.CommonEntity;

namespace TestCore.MvcUtils
{
    public class BaseController : Controller
    {
        public ResponseResult ResponseResult = new ResponseResult { Result = 1,Message=string.Empty };
        
        /// <summary>
        /// ��������
        /// </summary>
        public string CultureName
        {
            get
            {
                var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

                return requestCulture.RequestCulture.Culture.Name;
            }
        }

        public string SessionId => User.FindFirstValue(ClaimTypes.PrimarySid);

        /// <summary>
        /// ��֤�Ĵ�����Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetModelStateErrorString( )
        {
            var items = ModelState.Where(c => c.Value.Errors.Count > 0);
            if (!items.Any()) return string.Empty;
            StringBuilder strb = new StringBuilder();
            foreach (var item in items)
            {
                var error = item.Value.Errors.FirstOrDefault();

                
                strb.Append(string.Format("{0} : {1}; ",  item.Key,  error.ErrorMessage ));
            }
            return strb.ToString();
        }

        /// <summary>
        /// ��ȡ�༭������ֶ�
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetExpelFields(object model)
        {
            var type = model.GetType();

            var keyNames = type.GetKeyNames();

            var pros = type.GetProperties();

            var exFields = new List<string>();

            foreach (var p in pros)
            {
                if (keyNames.Item2.IsContains(p.Name) && !keyNames.Item1.IsContains(p.Name))
                {
                    exFields.Add(p.Name);
                    continue;
                }
                if (keyNames.Item3.IsContains(p.Name))
                {
                    exFields.Add(p.Name);
                    continue;
                }
                if (Request.Form[p.Name].Count == 0)
                {
                    var obj = p.GetValue(model);
                    if (obj == null)
                    {
                        exFields.Add(p.Name);
                    }
                    else if (obj is DateTime)
                    {
                        if (Convert.ToDateTime(obj) == DateTime.MinValue)
                        {
                            exFields.Add(p.Name);
                        }
                    }
                    else if (obj is ValueType)
                    {
                        if (Convert.ToInt32(obj) == 0)
                        {
                            exFields.Add(p.Name);
                        }
                    }
                }
            }
            if (exFields.Count == 0) return null;

            return string.Join(",", exFields);
        }

  
        #region  UserLog or MemLog

        /// <summary>
        /// ������־��������Ϣ
        /// </summary>
        /// <param name="logText"></param>
        /// <param name="orgs"></param>
        public void SetLogContentFormat(string logText, params object[] args)
        {
            // var text = string.Format(logText, args);
            ///Request.SetHeaderValue(Names.LogContent, text);
            Request.HttpContext.Items["Caiba.GamePlatform.LogContent"] = string.Format(logText, args);
        }

        /// <summary>
        /// ������־��������Ϣ
        /// </summary>
        /// <param name="model">ʵ�������������Ķ���</param>
        /// <param name="expelFields"></param>
        public void SetLogContent(object model, string expelFields = null) 
        {
            var pros = model.GetType().GetProperties();

            if (!string.IsNullOrEmpty(expelFields))
            {
                var exFieldList = expelFields.Split(',');
                pros = pros.Where(p => !exFieldList.IsContains(p.Name)).ToArray();
            }
            var nameVals = pros.Select(c => string.Format("{0}={1}", c.Name, c.GetValue(model)));

            Request.HttpContext.Items["Caiba.GamePlatform.LogContent"] = string.Join(",", nameVals);
        }

        //public string GetLogContent( )
        //{
        //    return Request.GetHeaderValue(Names.LogContent);
        //}

        #endregion

        #region GameId

        /// <summary>
        /// ���� ��Ա��־����ϷId
        /// </summary>
        /// <param name="gameId"></param>
        public virtual void SetLogGameId(object gameId)
        {
            Request.HttpContext.Items["Caiba.GamePlatform.GameId"] = gameId;
        }

        /// <summary>
        ///  ��ȡ ��Ա��־����ϷId
        /// </summary>
        /// <returns></returns>
        //public virtual int GetLogGameId()
        //{
        //    return TypeHelper.TryParse(Request.GetHeaderValue(Names.GameId), 0);
        //}

        #endregion


    }
}