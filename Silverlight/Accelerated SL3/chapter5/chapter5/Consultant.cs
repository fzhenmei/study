using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace chapter5
{
    [Bindable(true, BindingDirection.TwoWay)] 
    public class Consultant 
    {
        [Display(Name = "First Name :", Description = "Enter Your First Name", Order = 1)] 
        public string FirstName { get; set; }
        [Display(Name = "Last Name :", Description = "Enter Your Last Name", Order = 2)] 
        public string LastName { get; set; }
        [Display(Name = "Email :", Description = "Enter Your Email", Order = 4)] 
        public string Email { get; set; }
        [Required(ErrorMessage = "This field cannot be blank")]
        [Display(Name = "Website :", Description = "Enter Your Website Url", Order = 3)] 
        public string Website { get; set; }  
      


    }

    
}

