using Disco.Services.Plugins;
using NN.DiscoWarrantyPlugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NN.DiscoWarrantyPlugin
{
    // This class represents the plugin configuration handler within Disco. A plugin
    //  configuration class inherits from the 'PluginConfigurationHandler' abstract
    //  class found in 'Disco.Services.Plugins'.
    // Each plugin must have one Configuration Handler class to represent it. It
    //  can be named anything, but following the convention of '{PluginId}Configuration'
    //  is recommended.

    public class NNPluginConfiguration : PluginConfigurationHandler
    {
        /// <summary>
        /// This method is called by the Disco plugin host whenever the
        /// configuration page is requested. A response is required including
        /// a configuration 'model' and the type of a compiled Razor view.
        /// </summary>
        public override PluginConfigurationHandler.PluginConfigurationHandlerGetResponse Get(Data.Repository.DiscoDataContext dbContext, System.Web.Mvc.Controller controller)
        {
            // Get View type
            Type view = typeof(Views.Configuration);

            // Build Model
            ConfigurationModel model = new ConfigurationModel()
            {
        
            };

            // Return Response
            return GetResponse(view, model);
        }

        /// <summary>
        /// This method is called by the Disco plugin host whenever the configuration
        /// page is posted back by clicking the 'Save Configuration' button.
        /// You may validate the returned configuration, save it and then
        /// return a boolean to indicate success or failure.
        /// </summary>
        public override bool Post(Data.Repository.DiscoDataContext dbContext, System.Web.Mvc.FormCollection form, System.Web.Mvc.Controller controller)
        {
          

            return true;
        }
    }
}