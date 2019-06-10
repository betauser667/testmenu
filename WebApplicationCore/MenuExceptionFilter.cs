using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationCore
{
    public class MenuExceptionFilter : IExceptionFilter
    {
        public MenuExceptionFilter()
        {

        }

        public void OnException(ExceptionContext context)
        {
            int x = 1;

        }
    }
}
