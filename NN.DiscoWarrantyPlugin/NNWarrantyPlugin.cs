using Disco.Data.Repository;
using Disco.Services.Plugins;
using Disco.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NN.DiscoWarrantyPlugin
{
    [Plugin(Id = "NNWarrantyPlugin", Name = "Network Neighborhood", Author = "Sean Benham",
         Url = "http://nn.net.au/", HostVersionMin = "1.2.0606.0000")]
    public class NNWarrantyPlugin : Plugin
    {
    }
}