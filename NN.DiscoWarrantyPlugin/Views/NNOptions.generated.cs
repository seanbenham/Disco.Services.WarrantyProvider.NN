﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NN.DiscoWarrantyPlugin.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Disco.Services.Plugins;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.4.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/NNOptions.cshtml")]
    public partial class NNOptions : System.Web.Mvc.WebViewPage<NN.DiscoWarrantyPlugin.ViewModels.NNOptionsViewModel>
    {
        public NNOptions()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\NNOptions.cshtml"
  
    // NN Options
    // Last Updated 20/05/2013
    // Sean Benham

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" id=\"warrantyJobNNOptions\"");

WriteLiteral(" class=\"form\"");

WriteLiteral(" style=\"width: 650px; margin-top: 15px;\"");

WriteLiteral(">\r\n    <h2>Network Neighborhood Options</h2>\r\n    <table>\r\n        <tr>\r\n        " +
"    <td>\r\n                <p>\r\n                    Please state the type of clai" +
"m you wish to submit:\r\n                    <br />\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 16 "..\..\Views\NNOptions.cshtml"
               Write(Html.RadioButtonFor(m => m.NTPDevice, false));

            
            #line default
            #line hidden
WriteLiteral("<label> School Owned Device</label>\r\n                    <br />\r\n");

WriteLiteral("                    ");

            
            #line 18 "..\..\Views\NNOptions.cshtml"
               Write(Html.RadioButtonFor(m => m.NTPDevice, true));

            
            #line default
            #line hidden
WriteLiteral("<label>  Victorian NTP Staff Notebook</label>\r\n                    <br />\r\n");

WriteLiteral("                    ");

            
            #line 20 "..\..\Views\NNOptions.cshtml"
               Write(Html.ValidationMessageFor(m => m.NTPDevice));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </p>\r\n                <p>\r\n                    What is the prim" +
"ary suspected issue with the device:<br />\r\n");

WriteLiteral("                    ");

            
            #line 24 "..\..\Views\NNOptions.cshtml"
               Write(Html.DropDownListFor(m => m.Issue, new SelectList(NN.DiscoWarrantyPlugin.Features.NNWarrantyProviderFeature.NNSuspectedIssues, "Key", "Value")));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 25 "..\..\Views\NNOptions.cshtml"
               Write(Html.ValidationMessageFor(m => m.Issue));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
