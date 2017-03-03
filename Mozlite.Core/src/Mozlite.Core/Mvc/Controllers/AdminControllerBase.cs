using Microsoft.AspNetCore.Authorization;
using Mozlite.Extensions.Identity;

namespace Mozlite.Mvc.Controllers
{
    /// <summary>
    /// 后台管理控制器。
    /// </summary>
    [Authorize(Roles = IdentitySettings.Administrator)]
    public abstract class AdminControllerBase : ControllerBase
    {
        private int _pageIndex = -1;
        /// <summary>
        /// 当前页码。
        /// </summary>
        protected override int PageIndex
        {
            get
            {
                if (_pageIndex == -1)
                {
                    string page = Request.Query["page"];
                    _pageIndex = page.AsInt32() ?? 1;
                    if (_pageIndex < 1)
                        _pageIndex = 1;
                }
                return _pageIndex;
            }
        }
    }
}