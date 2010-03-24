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
//added
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace chapter5
{
    [CustomValidation(typeof(ValidateEmployee), "EmailNotNull")]
    public class Employee : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string email; 
        [Required]
        public string Email
        {
            get { return email; }
            set
            {   if (value != email)
                {
                    email = value;
                    NotifyPropertyChanged("email");
                }
            }
        }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string State { get; set; }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
    public static class ValidateEmployee
    {
        public static bool EmailNotNull(object employeeObject, ValidationContext context, out ValidationResult validationResult)
        {
            validationResult = null;

            Employee emp = employeeObject as Employee;

            string email= emp.Email;
            if (email== null)
            {
                validationResult = new ValidationResult("Email cannot empty");
            }
            return !(email == null);
        }
    }
}
