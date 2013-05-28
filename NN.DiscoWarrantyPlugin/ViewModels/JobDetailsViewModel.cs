using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Web.Script.Serialization;

namespace Disco.Services.Plugins.NN.ViewModels
{
    public class JobDetailsViewModel
    {
 
        internal static JobDetailsViewModel EmptyJobDetails()
        {
                 return new JobDetailsViewModel();
        }
    
    }
    
}