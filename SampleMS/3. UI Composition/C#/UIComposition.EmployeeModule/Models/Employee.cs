// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel;

namespace UIComposition.EmployeeModule.Models
{
    /// <summary>
    /// Employee entity class.
    /// </summary>
    public class Employee : INotifyPropertyChanged
    {
        private string id;
        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.NotifyPropertyChanged("Id");
            }
        }
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged("Name");
            }
        }
        private string lastName;
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
                this.NotifyPropertyChanged("LastName");
            }
        }
        private string email;
        public string Email
        {
            get { return this.email; }
            set
            {
                this.email = value;
                this.NotifyPropertyChanged("Email");
            }
        }
        private string phone;
        public string Phone
        {
            get { return this.phone; }
            set
            {
                this.phone = value;
                this.NotifyPropertyChanged("Phone");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}