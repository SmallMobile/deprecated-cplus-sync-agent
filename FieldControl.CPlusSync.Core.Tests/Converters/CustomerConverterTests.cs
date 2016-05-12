using AssertProperties;
using FieldControl.CPlusSync.Core.Converters;
using FieldControl.CPlusSync.Core.CPlus.Models;
using NUnit.Framework;

namespace FieldControl.CPlusSync.Core.Tests.Converters
{
    /*
        [  ] C-Plus Customer should be converted to Field Control Customer 
    */

     [TestFixture]
    public class CustomerConverterTests
    {
        [Test]
        public void ConvertFrom_GivenACustomer_ShouldBeConvertedAsExpected()
        {
            var converter = new CustomerConverter();

            var customer = converter.ConvertFrom(new CPlus.Models.Customer()
            {
                Name = "Luiz Freneda",
                City = "Sao Paulo",
                Email = "lfreneda@gmail.com",
                Number = "123",
                Street = "Rua dos Pinheiros",
                Phone = "11963427199",
                State = "SP",
                ZipCode = "05422010"
            });

            customer
                .AssertProperties()
                    .EnsureThat(c => c.Name).ShouldBe("Luiz Freneda")
                    .And(c => c.City).ShouldBe("Sao Paulo")
                    .And(c => c.Email).ShouldBe("lfreneda@gmail.com")
                    .And(c => c.Number).ShouldBe("123")
                    .And(c => c.Street).ShouldBe("Rua dos Pinheiros")
                    .And(c => c.Phone).ShouldBe("11963427199")
                    .And(c => c.State).ShouldBe("SP")
                    .And(c => c.ZipCode).ShouldBe("05422010")
                .Assert();
        }
    }
}
