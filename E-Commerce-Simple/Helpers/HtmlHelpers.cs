using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce_Simple.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsActive(this IHtmlHelper html, string controller, string action = "")
        {
            var routeData = html.ViewContext.RouteData;
            var currentController = routeData.Values["controller"].ToString();
            var currentAction = routeData.Values["action"].ToString();

            if (currentController == controller && (string.IsNullOrEmpty(action) || currentAction == action))
            {
                return "active";
            }
            return "";
        }
    }
}
