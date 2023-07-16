using System;
using System.Configuration;

namespace DataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(taxRate, out decimal output);

            return isValidTaxRate ? output : throw new ConfigurationErrorsException("Tax rate is not set up properly");
        }

    }

}
