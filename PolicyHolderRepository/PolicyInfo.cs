using WeatherStation;

namespace InsurancePolicyRepository
{
    public class PolicyInfo
    {
        public string PolicyHolderName
        {
            get;
            set;
        }

        public string PolicyNumber
        {
            get;
            set;
        }

        public Address PolicyHomeAddress
        {
            get;
            set;
        }

        public PolicyInfo(string policyHolderName, string policyNumber, Address policyHomeAddress)
        {
            PolicyHolderName = policyHolderName;
            PolicyNumber = policyNumber;
            PolicyHomeAddress = policyHomeAddress;
        }
    }
}
