﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentInfo.WebClient.Models
{
    public class CourseSearchModel
    {
        public string Keyword { get; set; }

        [Display(Name = "Department")]
        public Guid DepartmentId { get; set; }

        [Display(Name = "Faculty")]
        public Guid FacultyId { get; set; }
    }
}