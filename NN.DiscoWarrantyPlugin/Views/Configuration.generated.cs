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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Configuration.cshtml")]
    public partial class Configuration : System.Web.Mvc.WebViewPage<NN.DiscoWarrantyPlugin.ViewModels.ConfigurationModel>
    {
        public Configuration()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"clearfix\"");

WriteLiteral(" style=\"width: 600px; margin: 20px auto;\"");

WriteLiteral(">\r\n    <img");

WriteAttribute("src", Tuple.Create(" src=\"", 134), Tuple.Create("\"", 182)
            
            #line 3 "..\..\Views\Configuration.cshtml"
, Tuple.Create(Tuple.Create("", 140), Tuple.Create<System.Object, System.Int32>(this.DiscoPluginResourceUrl("NNLogo.png")
            
            #line default
            #line hidden
, 140), false)
);

WriteLiteral(" alt=\"NN\"");

WriteLiteral(" style=\"float: left; width: 192px; height: 202px;\"");

WriteLiteral(" />\r\n    <div");

WriteLiteral(" style=\"float: left; margin-top: 80px; margin-left: 20px;\"");

WriteLiteral(">\r\n        <h3>This plugin doesn\'t require any configuration.</h3>\r\n    </div>\r\n<" +
"/div>\r\n<div");

WriteLiteral(" class=\"code\"");

WriteLiteral(" style=\"width: 760px; margin: 0 auto; font-size: 0.9em;\"");

WriteLiteral(@">
    <h4>DISCLAIMER</h4>
    The software is provided ""as is"", without warranty of any kind, express or implied, including but not
        limited to the warranties of merchantability, fitness for a particular purpose and noninfringement.
        In no event shall Network Neighborhood, the authors or copyright holders be liable for any claim,
        damages or other liability, whether in an action of contract, tort or otherwise, arising from,
        out of or in connection with the software or the use or other dealings in the software.
        <br />
    <br />
    This plugin was developed by <a");

WriteLiteral(" href=\"http://discoict.com.au/member/1257.aspx\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(@">Sean Benham</a>. If requested by Network Neighborhood, this plugin could be disabled at anytime.
</div>
<script>
    $(function () {
        var $actionBar = $('.actionBar');

        // Remove the 'Save Configuration' button
        $actionBar.find('input[type=""submit""][value=""Save Configuration""]').remove();

        // Add 'Return to Plugins' button
        var pluginsUrl = '");

            
            #line 27 "..\..\Views\Configuration.cshtml"
                      Write(Url.Content("~/Config/Plugins"));

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var $buttonPlugins = $(\'<a>\').attr(\'href\', pluginsUrl).addClass(\'butt" +
"on\').text(\'Return to Plugins\').prependTo($actionBar);\r\n    });\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
