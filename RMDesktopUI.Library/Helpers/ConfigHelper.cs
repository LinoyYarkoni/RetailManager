using System;
using System.Configuration;

namespace RMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(taxRate, out decimal output);

            return isValidTaxRate ? output : throw new ConfigurationErrorsException("Tax rate is not set up properly");
        }
    }
}
