﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionPassControlSystem.BLL
{
    public interface IStringMessage
    {
        void StringMessageInEnglish(string message);
        void StringMessageInRussian(string message);
    }
}
