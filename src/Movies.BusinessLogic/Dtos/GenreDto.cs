﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.BusinessLogic;

public readonly struct GenreDto
{
    public int Id { get; init; }
    public string Name { get; init; }
}
