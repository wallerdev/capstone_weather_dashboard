using System.Collections.Generic;

namespace InsurancePolicyRepository
{
    public interface IPolicyProvider
    {
        List<PolicyInfo> GetPoliciesThatMatchNameFragment(string nameFragment);
        PolicyInfo GetPolicyThatMatchesNameOrNumber(string policyNumber, string policyHolderName);
    }
}
