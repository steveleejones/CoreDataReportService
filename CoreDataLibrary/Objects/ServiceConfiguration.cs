using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDataLibrary.Objects
{
    public class ServiceConfiguration
    {
        public string ServiceConfigurationName { get; set; }
        public string ServiceConfigurationItem { get; set; }
        public string ServiceConfigurationValue { get; set; }

        public ServiceConfiguration(string name, string item, string value)
        {
            ServiceConfigurationName = name;
            ServiceConfigurationItem = item;
            ServiceConfigurationValue = value;
        }
    }
}
