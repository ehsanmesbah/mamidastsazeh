using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using mamidastsazeh.Extensions;

namespace mamidastsazeh.TagHelpers
{
    public class VideoCreatedTagHelper : TagHelper
    {
        public DateTime Created { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("class", "video-created");
            output.Content.SetContent(Created.ToPersianInterval());
        }
    }
}
