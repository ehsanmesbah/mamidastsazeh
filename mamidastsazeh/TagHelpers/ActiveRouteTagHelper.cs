using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("is-active-route");

            if (IsActive())
            {
                MakeActive(output);
            }
        }

        private bool IsActive()
        {
            var currentController = ViewContext.RouteData.Values["controller"] as string;
            var currentAction = ViewContext.RouteData.Values["action"] as string;

            if (string.IsNullOrEmpty(currentController))
            {
                return false;
            }

            if (string.IsNullOrEmpty(currentAction))
            {
                return false;
            }

            if (currentController.ToLower().Equals(Controller.ToLower()))
            {
                if (currentAction.ToLower().Equals(Action.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.SetAttribute(classAttr);
            }
            else
            {
                if (classAttr.Value == null)
                {
                    output.Attributes.SetAttribute("class", "active");
                }
                else
                {
                    if (classAttr.Value.ToString().ToLower().IndexOf("active") < 0)
                    {
                        output.Attributes.SetAttribute("class", classAttr.Value + " active");
                    }
                }
            }
        }
    }
}
