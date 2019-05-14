using Autofac;
using Caiba.IRepositories;
using Caiba.MvcUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestCore.Common.Helper;
using TestCore.Common.Ioc;
using TestCore.Domain.Enums;
using TestCore.Domain.SysEntity;
using TestCore.IRepository.SysAdmin;
using TestCore.Repositories;

namespace TestCore.MvcUtils
{

    [AdminLogFilter]
    [AdminAuthorize]
    public class AdminBaseController : BaseController
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        protected readonly IRoleRightRepository _roleRightRepos = IoCBootstrapper.AutoContainer.Resolve<IRoleRightRepository>();
        protected readonly IHostingEnvironment _hostingEnvironment = IoCBootstrapper.AutoContainer.Resolve<IHostingEnvironment>();

        public IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 跳转到没有权限
        /// </summary>
        /// <returns></returns>
        public IActionResult RedirectNoRight()
        {
            return new RedirectResult("/Account/NoRight");
        }

        /// <summary>
        /// 返回没有的权限
        /// </summary>
        /// <param name="id"> Node Id</param>
        /// <returns></returns>
        [ActionRight(NoAudit = true)]
        [ActionType(ActType = ActionTypeEnum.View)]
        [HttpGet]
        public async Task<IActionResult> GetRights(int id)
        {
            try
            {
                if (AdminWebUtil.UserRight == null)
                {
                    throw new Exception("会话已经过期，请重新登录！");
                }
                ///用户在当前栏目的权限
                List<int> nodeRightIds = null;

                if (AdminWebUtil.UserRight.IsSupper)
                {
                    var node = await this._roleRightRepos.GetModelAsync<SysAdminNode>(new { id });
                    nodeRightIds = node.RightIdList;
                }
                else
                {
                    nodeRightIds = AdminWebUtil.UserRight.Rights.Where(c => c.NodeId == id)
                        .Select(c => c.RightId).Distinct().ToList();
                }

                var allRights = AdminWebUtil.RightSelectList;

                this.ResponseResult.Data = allRights.Where(c => !nodeRightIds.Any(r => r.ToString() == c.Value)).ToArray();
            }
            catch (Exception ex)
            {
                this.ResponseResult.Result = 0;
                this.ResponseResult.Message = ex.Message;
            }
            return Json(this.ResponseResult);

        }


        [ActionRight(RightType = RightTypeEnum.edit)]
        [HttpPost]
        public async Task<IActionResult> ChangeSortIndex(int id, int sortIndex, int sortType, string tableName, int parentId)
        {
            var type = TableHelper.GetTypeByTable(tableName);

            var cond = new Condition();

            if (type.HasProperty("ParentId"))
            {
                cond.Where("ParentId", parentId);
            }

            string[] tableNames = { "memaccount", "sysaccount","sitegameconfig" };

            var updateRes = !tableNames.IsContains(tableName.ToLower());

            var row = await this._roleRightRepos.ChangeSortIndexAsync(type, id, sortIndex, sortType, cond, updateRes);

            if (row > 0)
            {
                this.ResponseResult.Result = 1;
            }
            else
            {
                this.ResponseResult.Result = 0;
            }
            return Json(this.ResponseResult);
        }


        [ActionRight(NoAudit = true)]
        [ActionType(ActType = ActionTypeEnum.View)]
        [HttpGet]
        public async Task<IActionResult> GetSites(int? operId)
        {
            try
            {
                var pkIds = await this._roleRightRepos.GetSingleListAsync<int, SysWebConfig>(nameof(SysWebConfig.Id),
                          new { operId, Status = 1 }, "SortIndex");

                this.ResponseResult.Result = 1;
                this.ResponseResult.Data = ResourceHelper.GetSelectList(TableEnum.SysWebConfig, nameof(SysWebConfig.SiteTitle), pkIds.ToArray());

            }
            catch (Exception ex)
            {
                this.ResponseResult.Result = 0;
                this.ResponseResult.Message = ex.Message;
            }
            return Json(this.ResponseResult);
        }


        private ExcelPackage CreateExcelPackage<T>(List<string> fields, IEnumerable<T> models)
        {
            try
            {
                var dic = new Dictionary<string, string>();

                fields.Select(c =>
                {
                    var valueText = c.Split('|');
                    dic[valueText[0].ToLower().Trim()] = valueText[1];
                    return 0;
                }).ToList();


                var tType = models.First().GetType();
                var tableName = tType.Name.ToLower();

                var activities = new List<SelectListItem>();

                //if (tableName == "viewwalletlog")
                //{
                //    activities = AdminWebUtil.GetActivitySelectList();
                //}

                //var childGames = new List<GameChildList>();

                //if (dic.Keys.Any(c => c == "childgameid"))
                //{
                //    childGames = _roleRightRepos.GetList<GameChildList>(null).ToList();
                //}

                var tPros = tType.GetProperties().Where(c => dic.Keys.Any(x => x.ToLower() == c.Name.ToLower())).ToList();

                var package = new ExcelPackage();

                package.Workbook.Properties.Author = User.Identity.Name;

                var worksheet = package.Workbook.Worksheets.Add("FirstSheet");

                //First add the headers
                var fieldCount = tPros.Count();
                for (int i = 0; i < fieldCount; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dic[tPros[i].Name.ToLower()];
                }

                worksheet.Row(1).Style.Font.Size = 14;
                worksheet.Row(1).Style.Font.Bold = true;

                var numberformat = "#,##0";
                var dateFormat = "yyyy/MM/dd hh:mm:ss";


                var row = 2;
                foreach (var model in models)
                {
                    var col = 1;
                    foreach (PropertyInfo tPro in tPros)
                    {
                        var v = tPro.GetValue(model);

                        var pName = tPro.Name.ToLower();

                        if (tPro.PropertyType.FullName.ToLower().Contains("decimal"))
                        {
                            worksheet.Cells[row, col].Style.Numberformat.Format = numberformat;
                        }
                        if (tPro.PropertyType.FullName.ToLower().Contains("datetime"))
                        {
                            worksheet.Cells[row, col].Style.Numberformat.Format = dateFormat;
                        }

                        //if (pName == "rankid")
                        //{
                        //    v = ResourceHelper.GetDisplayName(TableEnum.MemRank, nameof(MemRank.Name), (int)v);
                        //}
                        //if (pName == "status" && (tType.Name.ToLower() == "membase" || tType.Name.ToLower() == "ViewMemLevel"))
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(StatusEnum), v);
                        //}

                        //if (pName == "gender")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(GenderEnum), v);
                        //}


                        //if (pName == "status" && (tType.Name.ToLower() == "viewmemorder"))
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(OrderStautsEnum), v);
                        //}
                        //if (pName == "banktype")
                        //{
                        //    v = ResourceHelper.GetDisplayName(TableEnum.SysBankType, nameof(SysBankType.Name), (int)v);
                        //}

                        //if (pName == "flowtype")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(FundFlowTypeEnum), v);
                        //}
                        //if (pName.Contains("gameid"))
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(GameEnum), v);
                        //}
                        //if (pName.Contains("childgameid"))
                        //{
                        //    v = childGames.Where(c => c.GameCode == v.ToString());
                        //}
                        //if (pName == "status" && tableName == "fundgameTrade")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(GameTradeStatusEnum), v);
                        //}
                        //if (pName == "sourcetype")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(SourceTypeEnum), v);
                        //}
                        //if (pName == "activityid")
                        //{
                        //    v = activities.GetText(v);
                        //}

                        //if (pName == "status" && tableName.Contains("betlog"))
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(BetLogStatusEnum), v);
                        //}
                        //if (pName == "rebatetype")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(RebateTypeEnum), v);
                        //}
                        //if (pName == "paystatus")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(PayStatusEnum), v);
                        //}
                        //if (pName == "agentType")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(AgentTypeEnum), v);
                        //}
                        //if (pName == "status" && tableName == "fundagentbrokeragestat")
                        //{
                        //    v = EnumHelper.GetDisplayName(typeof(AgentBrokerageStatusEnum), v);
                        //}

                        worksheet.Cells[row, col].Value = v;

                        col++;
                    }

                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                //// Add to table / Add summary row
                //var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: models.Count, toColumn: fieldCount), "Data");
                //tbl.ShowHeader = true;
                //tbl.TableStyle = TableStyles.Dark1;
                //tbl.ShowTotal = true;
                //tbl.Columns[0].DataCellStyleName = dataCellStyleName;
                //tbl.Columns[0].TotalsRowFunction = RowFunctions.Sum;
                //worksheet.Cells[models.Count, 2].Style.Numberformat.Format = numberformat;

                //// AutoFitColumns
                //worksheet.Cells[1, 1, 4, 4].AutoFitColumns();

                //worksheet.HeaderFooter.OddFooter.InsertPicture(
                //    new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, "images", "captcha.jpg")),
                //    PictureAlignment.Right);

                return package;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <param name="models"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateExcel<T>(List<string> fields, IEnumerable<T> models)
        {
            if (fields.Count < 1)
            {
                throw new Exception("fields参数---导出数据标题不能为空");
            }
            if (models.Count() < 1)
            {
                throw new Exception("models参数---导出数据记录数小于1条");
            }

            var fileGuid = Guid.NewGuid().ToString();
            var package = CreateExcelPackage<T>(fields, models);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                package.SaveAs(new FileInfo("ssss.xlsx"));
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[fileGuid] = memoryStream.ToArray();
            }
            return fileGuid;
        }

        /// <summary>
        /// 下载请求处理
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, XlsxContentType, (string.IsNullOrEmpty(fileName) ? DateTime.Now.ToString() : fileName) + ".xlsx");
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
    }
}