using System.ComponentModel.DataAnnotations;

namespace NN.DiscoWarrantyPlugin.ViewModels
{
    public class NNOptionsViewModel
    {
        // gathers radio button input for school owned/staff notebook
        [Required(ErrorMessage = "You must select if the computer is School Owned or DEECD NTP device")]
        public bool? NTPDevice { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "You must select one primary issue to submit to NN")]
        public int Issue { get; set; }
    }
}