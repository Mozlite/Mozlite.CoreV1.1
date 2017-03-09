#if DEBUG
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = Mozlite.Mvc.Controllers.ControllerBase;

namespace Mozlite.Mvc.Tools
{
    /// <summary>
    /// 工具控制器。
    /// </summary>
    public class ToolController : ControllerBase
    {
        private readonly IStyleManager _styleManager;
        public ToolController(IStyleManager styleManager)
        {
            _styleManager = styleManager;
        }

        [Route("csstool")]
        public async Task<IActionResult> Index(string file, string dir = null)
        {
            if (dir == null)
                dir = Path.GetFileNameWithoutExtension(file);
            if (await _styleManager.ParseAsync(file, dir))
                return Content("成功！");
            return Content("错误！");
        }
    }
}
#endif