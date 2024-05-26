using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Core.Interfaces
{
    public interface IProductCodeService
    {
        string GenerateUniqueCode();
        bool ValidateCode(string code);
    }
}
