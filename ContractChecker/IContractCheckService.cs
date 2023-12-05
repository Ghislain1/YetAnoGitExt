using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContractChecker
{
    public interface IContractCheckService
    {
        Task<ContractResult[]> ValidateContractAsync(ContractRule[] rules,string[] lines);

        IList<ContractRule> Rules { get; }
    }
}