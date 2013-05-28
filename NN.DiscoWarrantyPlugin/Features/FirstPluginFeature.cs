using Disco.Services.Plugins;
using Disco.Services.Plugins.Features.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disco.PluginSample.Features
{
    // This class represents a feature within Disco. A plugin feature class
    //  inherits from one of the core feature types, which intern inherit the
    //  root 'PluginFeature' abstract class found in 'Disco.Services.Plugins'.
    //  The current feature types include:
    //    - CertificateProviderFeature
    //    - InteroperabilityProviderFeature
    //    - OtherFeature
    //    - UIExtensionFeature<Model>
    //    - WarrantyProviderFeature
    // A 'PluginFeature' attribute is attached to provide metadata to the Manifest Generator.
    // Each plugin must have at least one feature - but may have many more.
    //  They can be named anything, but following the convention of
    //  '{PluginId}FeatureName' is recommended.

    [PluginFeature(Id = "FirstPluginFeature", Name = "Samples - First Plugin Feature", PrimaryFeature = true)]
    public class FirstPluginFeature : OtherFeature
    {

        /// <summary>
        /// This method is called each time the Disco plugin host initializes the plugin.
        /// This occurs after Disco calls the initialize method on the Plugin core reference.
        /// The order that features are initialized cannot be guaranteed, thus, if multiple
        /// features are present they should not rely on each other.
        /// </summary>
        public override void Initialize(Data.Repository.DiscoDataContext dbContext)
        {
            // eg. Perform some work to 'setup' the plugin feature for use.
            // Keep the work in this method to a minimum! Delays is this
            //  method will increase the time Disco takes to startup.
        }
    }
}