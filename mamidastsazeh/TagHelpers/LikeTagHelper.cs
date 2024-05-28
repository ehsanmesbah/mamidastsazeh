using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.TagHelpers
{
    [HtmlTargetElement("like")]
    public class LikeTagHelper : TagHelper
    {
      
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

        }

    }
}
