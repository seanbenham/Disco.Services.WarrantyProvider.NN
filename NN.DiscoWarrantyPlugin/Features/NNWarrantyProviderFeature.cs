using Disco.Data.Repository;
using Disco.Models.BI.Config;
using Disco.Models.Repository;
using Disco.Services.Plugins;
using Disco.Services.Plugins.Features.WarrantyProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NN.DiscoWarrantyPlugin.Features
{
    [PluginFeature(Id = "NNWarrantyProvider", Name = "Network Neighborhood", PrimaryFeature = true)]
    public class NNWarrantyProviderFeature : WarrantyProviderFeature
    {
        public override string WarrantyProviderId { get { return "NN"; } }

        #region Submit Job
        public override Type SubmitJobViewType
        {

            //  Log Warranty Page
            get { return typeof(Views.NNOptions); }

        }

        private void SubmitJobValidateEnvironment(DiscoDataContext dbContext, Controller controller, User TechUser)
        {



            // Validate TechUser Email Address
            if (string.IsNullOrEmpty(TechUser.EmailAddress))
                controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Email Address (Update your Email Address in Active Directory)");
            if (string.IsNullOrEmpty(TechUser.PhoneNumber))
                controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Phone Number (Update your Telephone Number in Active Directory)");

           


        }



        public override dynamic SubmitJobViewModel(DiscoDataContext dbContext, Controller controller, Job Job, OrganisationAddress Address, User TechUser)
        {
            SubmitJobValidateEnvironment(dbContext, controller, TechUser);

            var model = new ViewModels.NNOptionsViewModel()
            {
                NTPDevice = null,
                Issue = 0
            };

            return model;

            //return null;
        }

        public override Dictionary<string, string> SubmitJobParseProperties(DiscoDataContext dbContext, FormCollection form, Controller controller, Job Job, OrganisationAddress Address, User TechUser, string FaultDescription)
        {
            SubmitJobValidateEnvironment(dbContext, controller, TechUser);

            ViewModels.NNOptionsViewModel model = new ViewModels.NNOptionsViewModel();
            controller.TryUpdateModel(model);

            return new Dictionary<string, string>() 
            {
                {"NTPDevice", model.NTPDevice.ToString()},
                {"Issue", model.Issue.ToString()}
            };
        }



        public override Dictionary<string, string> SubmitJobDiscloseInfo(DiscoDataContext dbContext, Job Job, OrganisationAddress Address, User TechUser, string FaultDescription, Dictionary<string, string> WarrantyProviderProperties)
        {
            Dictionary<String, String> info = new Dictionary<string, string>();

            // If the device is a part of the NTP then change contact details to them
            bool ntpDevice = bool.Parse(WarrantyProviderProperties["NTPDevice"]);

            if (ntpDevice == true)
            {
                // Victorian Teacher notebook
                info.Add("Contact Name", Job.User.DisplayName);
                info.Add("Contact Email", Job.User.EmailAddress);
            }
            else
            {
                // School owned devices
                info.Add("Contact Name", TechUser.DisplayName);
                info.Add("Contact Email", TechUser.EmailAddress);
            }



            info.Add("Contact Company", Address.Name);
            info.Add("Contact Address", Address.Address);
            info.Add("Contact Suburb", Address.Suburb);
            info.Add("Contact Postcode", Address.Postcode);
            info.Add("Contact Phone", TechUser.PhoneNumber);
            info.Add("Device Serial Number", Job.DeviceSerialNumber);
            info.Add("Device Product Description", String.Format("{0} {1}", Job.Device.DeviceModel.Manufacturer, Job.Device.DeviceModel.Model));
            info.Add("Disco Task", String.Format("Customer Job Id: {0}", Job.Id));
            info.Add("NTP Device", WarrantyProviderProperties["NTPDevice"]);
            info.Add("Suspected Issue", WarrantyProviderProperties["Issue"]);


            return info;
        }

        public override string SubmitJob(DiscoDataContext dbContext, Job Job, OrganisationAddress Address, User TechUser, string FaultDescription, Dictionary<string, string> WarrantyProviderProperties)
        {
            // Send Job to NN

            var httpBody = new StringBuilder("Automated=1&");

            bool ntpDevice = bool.Parse(WarrantyProviderProperties["NTPDevice"]);
            // Determine if DEECD NTP job is to be processed or not.
            if (ntpDevice == true)
            {
                httpBody.Append("&txtContact=");
                httpBody.Append(HttpUtility.UrlEncode(Job.User.DisplayName));
                httpBody.Append("&txtemail=");
                httpBody.Append(HttpUtility.UrlEncode(Job.User.EmailAddress));

            }
            else
            {
                httpBody.Append("&txtContact=");
                httpBody.Append(HttpUtility.UrlEncode(TechUser.DisplayName));
                httpBody.Append("&txtemail=");
                httpBody.Append(HttpUtility.UrlEncode(TechUser.EmailAddress));
            }

            httpBody.Append("&txtorg=");
            httpBody.Append(HttpUtility.UrlEncode(Address.Name));
            httpBody.Append("&txtorgaddress=");
            httpBody.Append(HttpUtility.UrlEncode(Address.Address));
            httpBody.Append("&txtorgsub=");
            httpBody.Append(HttpUtility.UrlEncode(Address.Suburb));
            httpBody.Append("&txtorgpost=");
            httpBody.Append(HttpUtility.UrlEncode(Address.Postcode));
            httpBody.Append("&txtorgstate=");
            httpBody.Append("VIC");
            httpBody.Append("&txtphone=");
            httpBody.Append(HttpUtility.UrlEncode(TechUser.PhoneNumber));
            httpBody.Append("&txtemail=");
            httpBody.Append(HttpUtility.UrlEncode(TechUser.EmailAddress));
            httpBody.Append("&txtSerial=");
            httpBody.Append(HttpUtility.UrlEncode(Job.DeviceSerialNumber));
            httpBody.Append("&txtModel=");
            httpBody.Append(HttpUtility.UrlEncode(Job.Device.DeviceModel.Model));
            httpBody.Append("&txtProblemDesc=");
            httpBody.Append(HttpUtility.UrlEncode(FaultDescription));
            httpBody.Append("&nttpUnit=");
            httpBody.Append(ntpDevice ? "1" : "0");
            httpBody.Append("&sltSuspectedIssue=");
            httpBody.Append(WarrantyProviderProperties["Issue"]);


            // Return Job Reference



            string stringResponse = null;

            HttpWebRequest wreq = HttpWebRequest.Create("http://portal.nn.net.au/ajax/warranty/ajaxWarrantyCreateJob.php") as HttpWebRequest;
            // Added: 2013-02-08 G#
            // Fix for Proxy Servers which dont support KeepAlive
            wreq.KeepAlive = false;
            // End Added: 2013-02-08 G#
            wreq.Method = WebRequestMethods.Http.Post;
            wreq.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter sw = new StreamWriter(wreq.GetRequestStream()))
            {
                sw.Write(httpBody.ToString());
            }

            HttpWebResponse wres = wreq.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(wres.GetResponseStream()))
            {
                stringResponse = sr.ReadToEnd();
            }



            // NN Get last 6 Chars to gather jobID
            stringResponse = stringResponse.Substring(stringResponse.Length - 6);
            // remove end bracket to make it a proper number
            char[] remove = { '}' };
            stringResponse = stringResponse.TrimEnd(remove);

            int jobReference = default(int);
            if (int.TryParse(stringResponse, out jobReference))
            {
                return jobReference.ToString();
            }
            else
            {
                // Throw error if unable to get Job task ID
                throw new WarrantyProviderSubmitJobException(stringResponse);
            }
        }
        #endregion

        #region Job Details
        public override Type JobDetailsViewType
        {
            get { return typeof(Views.JobDetails); }
        }
        public override dynamic JobDetailsViewModel(DiscoDataContext dbContext, Controller controller, Job Job)
        {
            return null;
        }
        #endregion
    }
}