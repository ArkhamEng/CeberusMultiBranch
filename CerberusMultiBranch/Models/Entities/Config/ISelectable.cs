﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerberusMultiBranch.Models.Entities.Config
{
    public interface ISelectable
    {
        int Id { get; }

        string Name { get;  }
    }
}