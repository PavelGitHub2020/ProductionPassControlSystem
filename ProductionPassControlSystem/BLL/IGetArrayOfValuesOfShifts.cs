using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IGetArrayOfValuesOfShifts
    {
        List<string> GetArrayOfValuesOfShifts(int dayNumber);
    }
}
