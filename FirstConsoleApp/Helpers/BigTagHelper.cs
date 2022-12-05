using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FirstConsoleApp.Helpers
{
    // big element
    [HtmlTargetElement("big")]
    // big attribute
    [HtmlTargetElement(Attributes = "big")]
    public class BigTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // convert big tag to h3
            output.TagName = "h3";
            // remove big attribute
            output.Attributes.RemoveAll("big");
            output.Attributes.SetAttribute("class", "h3");
        }
    }
}
