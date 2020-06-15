using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class ResidentGeneralInformationModel
    {
        public UserModel User { get; set; }

        public SettingModel Setting { get; set; }
    }
}