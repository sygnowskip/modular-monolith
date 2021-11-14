using Hexure.Results;
using ModularMonolith.Orders.Domain.ValueObjects;

namespace ModularMonolith.Orders.Domain.Policies
{
    public interface IGrossPricePolicy
    {
        Result IsGrossPriceSumOfNetAndTax(Price net, Tax tax, Price gross);
    }

    public class GrossPricePolicy : IGrossPricePolicy
    {
        public Result IsGrossPriceSumOfNetAndTax(Price net, Tax tax, Price gross)
        {
            return Result.Create(net.Value + tax.Value == gross.Value, Errors.GrossPriceHasToBeSumOfNetAndTax.Build());
        }

        public static class Errors
        {
            public static readonly Error.ErrorType GrossPriceHasToBeSumOfNetAndTax =
                new Error.ErrorType(nameof(GrossPriceHasToBeSumOfNetAndTax),
                    "Gross price has to be equal to sum of net and tax value");
        }
    }
}