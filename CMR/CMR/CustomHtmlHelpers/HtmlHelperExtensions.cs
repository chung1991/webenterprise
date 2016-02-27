using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CMR.CustomHtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static String ActivePage(this HtmlHelper helper,String controller,String action=null)
        {
            String classValue = "";
            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();
            if (action != null)
            {
                if (currentController == controller && currentAction == action)
                {
                    classValue = "active";
                }
            }
            else
            {
                if (currentController == controller)
                {
                    classValue = "activeByController";
                }
            }
            
            return classValue;
        }
    }
}