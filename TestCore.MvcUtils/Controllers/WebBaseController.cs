using Autofac;
using Microsoft.AspNetCore.Localization;
using TestCore.Common;
using TestCore.Common.Extensions;
using TestCore.Common.Ioc;
using TestCore.Domain.Enums;
using TestCore.IService.SysAdmin;

namespace TestCore.MvcUtils
{

    [WebLogFilter(ProjectType = ProjectTypeEnum.Web)]
    public  class WebBaseController : BaseController
    {
        public string MemId
        {
            get
            {
                if (User != null && User.Identity != null)
                    return User.Identity.Name;
                return string.Empty;
            }
        }

        public int SiteId
        {
            get
            {
                int siteId = WebConfig.AppSettings.SiteId;
               
                return siteId;
            }
        }

        public TestCore.Domain.SysEntity.Admin CurrentMember
        {
            get {

                if (string.IsNullOrEmpty(MemId))
                    return null;
                return (IoCBootstrapper.AutoContainer.Resolve<IAdminSvc>()).GetModel(new { id= MemId });
            }
        }

        public string IP
        {
            get {
                
                return HttpContext.GetUserIp();
            }
        }

        public string Lang
        {
            get
            {
                var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>();

                return requestCulture.RequestCulture.Culture.Name;
            }
        }
    }
}
