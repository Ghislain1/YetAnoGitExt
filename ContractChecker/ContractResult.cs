namespace ContractChecker
{
    public record ContractResult(string Rule, int LineId, ResultStatus ResultStatus);
}