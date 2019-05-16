using System;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationCore.Controllers
{
    [ViewComponent(Name = "ViewComponentCurrentTime")]
    public class ViewComponentCurrentTime : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var time = DateTime.Now.ToString("h:mm:ss");
            return Content($"The current time is {time}");
        }
    }


}