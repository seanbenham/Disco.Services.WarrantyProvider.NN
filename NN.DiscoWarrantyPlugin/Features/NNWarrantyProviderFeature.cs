using Disco.Data.Repository;
using Disco.Models.BI.Config;
using Disco.Models.Repository;
using Disco.Services.Plugins;
using Disco.Services.Plugins.Features.WarrantyProvider;
using Newtonsoft.Json;
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

        #region Suspected Issues
        private static Dictionary<int, string> _NNSuspectedIssues = new Dictionary<int, string>()
            {
                { 0, "Please Select"},
                { 1, "Hard Drive"},
                { 2, "CPU"},
                { 3, "Charger"},
                { 4, "RAM"},
                { 5, "Motherboard"},
                { 6, "Keyboard"},
                { 7, "Palmrest"},
                { 8, "DVD Drive"},
                { 9, "Screen"},
                { 10, "Physical Screen Damage"},
                { 11, "Physical Base"},
                { 12, "Physical Top Cover"},
                { 13, "Physical Keyboard"},
                { 14, "Physical Charger"},
                { 15, "Battery"},
                { 16, "Physical Battery"},
                { 17, "Basecover"},
                { 18, "Screen Hinges"},
                { 19, "Fan"},
                { 20, "Internal Power"}
            };
        public static Dictionary<int, string> NNSuspectedIssues
        {
            get
            {
                return _NNSuspectedIssues;
            }
        }
        #endregion

        public override Type SubmitJobViewType
        {
            //  Log Warranty Page
            get { return typeof(Views.NNOptions); }

        }

        private void SubmitJobValidateEnvironment(DiscoDataContext dbContext, Controller controller, User TechUser, Job Job, ViewModels.NNOptionsViewModel OptionsModel)
        {
            // Validate TechUser Email Address
            if (OptionsModel == null || (OptionsModel.NTPDevice.HasValue && !OptionsModel.NTPDevice.Value))
            {
                // Non-NTP Device (or Unknown)
                // Check for Technicians Details
                if (string.IsNullOrEmpty(TechUser.EmailAddress))
                    controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Email Address (Update your Email Address in Active Directory)");
                if (string.IsNullOrEmpty(TechUser.PhoneNumber))
                    controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Phone Number (Update your Telephone Number in Active Directory)");
            }
            else
            {
                if (!NNSuspectedIssues.ContainsKey(OptionsModel.Issue))
                    controller.ModelState.AddModelError("Issue", "Unknown Suspected Issue Id");

                if (OptionsModel.NTPDevice.HasValue && OptionsModel.NTPDevice.Value)
                {
                    // NTP Device (Use Users Details)
                    if (string.IsNullOrEmpty(Job.User.EmailAddress))
                        controller.ModelState.AddModelError(string.Empty, "NN NTP Device requires the User's Email Address (Update the Email Address in Active Directory)");
                    if (string.IsNullOrEmpty(Job.User.PhoneNumber))
                        controller.ModelState.AddModelError(string.Empty, "NN NTP Device requires the User's Phone Number (Update the Phone Number in Active Directory)");
                }
                else
                {
                    // Non-NTP Device (or Unknown)
                    // Check for Technicians Details
                    if (string.IsNullOrEmpty(TechUser.EmailAddress))
                        controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Email Address (Update your Email Address in Active Directory)");
                    if (string.IsNullOrEmpty(TechUser.PhoneNumber))
                        controller.ModelState.AddModelError(string.Empty, "NN Requires a Technician Phone Number (Update your Telephone Number in Active Directory)");
                }
            }
        }

        public override dynamic SubmitJobViewModel(DiscoDataContext dbContext, Controller controller, Job Job, OrganisationAddress Address, User TechUser)
        {
            SubmitJobValidateEnvironment(dbContext, controller, TechUser, Job, null);

            var model = new ViewModels.NNOptionsViewModel()
            {
                NTPDevice = null,
                Issue = 0
            };

            return model;
        }

        public override Dictionary<string, string> SubmitJobParseProperties(DiscoDataContext dbContext, FormCollection form, Controller controller, Job Job, OrganisationAddress Address, User TechUser, string FaultDescription)
        {
            ViewModels.NNOptionsViewModel model = new ViewModels.NNOptionsViewModel();
            controller.TryUpdateModel(model);

            SubmitJobValidateEnvironment(dbContext, controller, TechUser, Job, model);

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
            info.Add("Contact State", NNCompatibleContactState(Address.State));
            info.Add("Contact Postcode", Address.Postcode);
            info.Add("Contact Phone", TechUser.PhoneNumber);
            info.Add("Device Serial Number", Job.DeviceSerialNumber);
            info.Add("Device Product Description", String.Format("{0} {1}", Job.Device.DeviceModel.Manufacturer, Job.Device.DeviceModel.Model));
            info.Add("NTP Device", ntpDevice ? "Yes" : "No");
            info.Add("Suspected Issue", NNSuspectedIssues[int.Parse(WarrantyProviderProperties["Issue"])]);

            return info;
        }

        public override string SubmitJob(DiscoDataContext dbContext, Job Job, OrganisationAddress Address, User TechUser, string FaultDescription, Dictionary<string, string> WarrantyProviderProperties)
        {
            // Send Job to NN

            var httpBody = new StringBuilder("Automated=1");

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
            httpBody.Append(NNCompatibleContactState(Address.State));
            httpBody.Append("&txtphone=");
            httpBody.Append(HttpUtility.UrlEncode(TechUser.PhoneNumber));
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


            string stringResponse = null;
            try
            {
                HttpWebRequest wreq = HttpWebRequest.Create("https://portal.nn.net.au/ajax/warranty/ajaxWarrantyCreateJob.php") as HttpWebRequest;
                wreq.KeepAlive = false;
                wreq.Method = WebRequestMethods.Http.Post;
                wreq.ContentType = "application/x-www-form-urlencoded";

                using (StreamWriter sw = new StreamWriter(wreq.GetRequestStream()))
                {
                    sw.Write(httpBody.ToString());
                }

                using (HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wres.GetResponseStream()))
                    {
                        stringResponse = sr.ReadToEnd();
                    }
                }

                // TODO: Parse the JSON Correctly - http://en.wikipedia.org/wiki/Json
                // JSON.net is used in Disco, eg:
                //      dynamic response = JsonConvert.DeserializeObject(stringResponse);
                //      string jobReference = response.SomeChildObject.SomeOtherObject.PropertyWhichHoldsJobReference;
                //      return jobReference;
                // END TODO

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
            catch (Exception ex)
            {
                string errorMessage = string.Format("Unable to successfully log warranty:{0}{1}{0}{2}{0}Server Responded: {3}",
                                                        Environment.NewLine, ex.GetType().Name, ex.Message, stringResponse ?? "Unknown/None");
                throw new WarrantyProviderSubmitJobException(errorMessage);
            }
        }

        private string NNCompatibleContactState(string ContactState)
        {
            if (!string.IsNullOrWhiteSpace(ContactState))
            {
                switch (ContactState.ToLower())
                {
                    case "australian capital territory":
                    case "act":
                        return "ACT";
                    case "new south wales":
                    case "nsw":
                        return "NSW";
                    case "northern territory":
                    case "nt":
                        return "NT";
                    case "queensland":
                    case "qld":
                        return "QLD";
                    case "south australia":
                    case "sa":
                        return "SA";
                    case "tasmania":
                    case "tas":
                        return "TAS";
                    case "victoria":
                    case "vic":
                        return "VIC";
                    case "western australia":
                    case "wa":
                        return "WA";
                }
            }
            // Default to Victoria
            return "VIC";
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
