﻿using FakeXiecheng.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Dtos
{
    public class TouristForCreationDto:TouristForManipulationDto
    {
        

        //public IEnumerable<ValidationResult> Validate(
        //    ValidationContext validationContext)
        //{
        //    if (Title == Description) {
        //        yield return new ValidationResult(
        //            "路线名称必须与路线描述不一样",
        //            new[] { "TouristRouteForCreationDto" }
        //        );
        //    }
        //}
    }
}
