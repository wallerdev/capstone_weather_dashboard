using System;
using System.Collections.Generic;
using System.Linq;
using WeatherStation;

namespace InsurancePolicyRepository
{
    public class MockPolicyProvider : IPolicyProvider
    {
        public List<PolicyInfo> GetPoliciesThatMatchNameFragment(string nameFragment)
        {
            var list = from p in GetAllPolicies()
                       where p.PolicyHolderName.ToLower().Contains(nameFragment)
                       orderby p.PolicyNumber
                       select p;
            return list.ToList();
        }

        public PolicyInfo GetPolicyThatMatchesNameOrNumber(string policyNumber, string policyHolderName)
        {
            string lowerPolicyHolderName = policyHolderName.ToLower();
            var list = from p in GetAllPolicies()
                       where p.PolicyHolderName.ToLower() == lowerPolicyHolderName
                             || p.PolicyNumber == policyNumber
                       select p;
            return list.First();
        }

        private List<PolicyInfo> GetAllPolicies()
        {
            return new List<PolicyInfo>
                       {
                            new PolicyInfo("George Smith", "654321", new Address(null, "Lansing", "Mi", "48910", "Ingham")),
                            new PolicyInfo("Alex Brown", "65548987", new Address("2475 Houlihan Rd", "Saginaw", "Mi", "48601", "Saginaw")),
                            new PolicyInfo("Peggy Green", "3587154", new Address("800 E Erie Rd", "Temperance", "Mi", "48182", "Monroe")),
                            new PolicyInfo("Harry Reed", "7896348", new Address("808 S 24th St", "Springfield", "Il", "62703", "Sangamon"))
                       };
        }
    }
}
