using Disco.Services.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NN.DiscoWarrantyPlugin
{
    // This class represents the core plugin reference within Disco. A plugin core reference
    //  class inherits from the 'Plugin' abstract class found in 'Disco.Services.Plugins'.
    //  It provides (optional) override methods to capture plugin maintenance tasks. A
    //  'Plugin' attribute is attached to provide metadata to the Manifest Generator.
    // Each plugin must have one class to represent it. It can be named anything,
    //  but following the convention of '{PluginId}' is recommended.

    [Plugin(Id = "NNWntyPlugin", Name = "Network Neighborhood-  Warranty Plugin", Author = "Sean Benham",
         Url = "http://nn.net.au/", HostVersionMin = "1.2.0521.1121")]
    public class NNPlugin : Plugin
    {
        // The below methods are completely optional, and can be removed. They're here to
        //  illustrate how a plugin might hook-in to plugin life-cycle events.


        /// <summary>
        /// This method is called by the Disco plugin host immediately after installing
        /// the plugin.
        /// </summary>
        public override void Install(Data.Repository.DiscoDataContext dbContext, Services.Tasks.ScheduledTaskStatus Status)
        {
            
        }
        /// <summary>
        /// This method is called by the Disco plugin host after removing a previous
        /// version of the plugin and installing this version.
        /// </summary>
        public override void AfterUpdate(Data.Repository.DiscoDataContext dbContext, PluginManifest PreviousManifest)
        {
            
        }
        /// <summary>
        /// This method is called by the Disco plugin host immediately before
        /// uninstalling the plugin.
        /// </summary>
        public override void Uninstall(Data.Repository.DiscoDataContext dbContext, bool UninstallData, Services.Tasks.ScheduledTaskStatus Status)
        {
            
        }

        /// <summary>
        /// This method is called each time the Disco plugin host initializes the plugin.
        /// This occurs each time Disco is started up (normally every 24 hours; eg. when
        /// the Application Pool is recycled), and immediately after completing the plugins
        /// initial install (is called after the 'Install' method above is called).
        /// </summary>
        public override void Initialize(Data.Repository.DiscoDataContext dbContext)
        {
            // eg. Perform some work to 'setup' the plugin for use.
            // Keep the work in this method to a minimum! Delays is this
            //  method will increase the time Disco takes to startup.
        }

    }
}