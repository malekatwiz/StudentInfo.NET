﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StudentInfo.Data;
using PagedList;
using StudentInfo.Enums;
using StudentInfo.WebClient.Helpers;
using StudentInfo.WebClient.Models;
using StudentInfo.Users.Dto;

namespace StudentInfo.WebClient.Controllers
{
    [RequireHttps]
    [Authorize]
    public class CourseController : Controller
    {
        [AuthorizeRoles(SystemRoles.Administrator, SystemRoles.Student)]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var db = new StudentInfoContext();

            var courses = db.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(x => x.Code.Contains(searchString) ||
                x.Name.Contains(searchString));
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(courses.OrderBy(x => x.Code).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(Guid id, bool allowEdit = false)
        {
            var db = new StudentInfoContext();

            var courseDetails = db.Courses.FirstOrDefault(x => x.Id == id);

            if (courseDetails != null)
            {
                if (allowEdit && User.IsInRole(SystemRoles.Administrator))
                {
                    return PartialView("_EditDetails", courseDetails);
                }
                return PartialView("_Details", courseDetails);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(CourseEnrollModel model)
        {
            var result = false;
            var message = "Sorry, something bad happened.";
            var db = new StudentInfoContext();
            var student = db.UserDetails.FirstOrDefault(x => x.ApplicationUser.Id == User.Identity.GetUserId());

            //if (student != null)
            //{
            //    var hasCourse = db.StudentCourses.Any(x => x.Course.Id == model.CourseId &&
            //    x.CourseDate.Year == model.CourseDate.Year);

            //    if (hasCourse)
            //    {
            //        message = "You are registered for this course.";
            //    }
            //    else
            //    {
            //        db.StudentCourses.Add(new Users.Dto.StudentCourse
            //        {
            //            CourseDate = model.CourseDate,
            //            CourseState = CourseRegistrationState.Added,
            //            Term = model.Term,
            //            CreateDate = DateTime.UtcNow,
            //            LastUpdate = DateTime.UtcNow
            //        });

            //        try
            //        {
            //            db.SaveChanges();

            //            message = "You have been registered for this course.";
            //            result = true;
            //        }
            //        catch(Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }
            //}

            return Json(new { success = result, message = message },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(SystemRoles.Administrator)]
        public JsonResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var db = new StudentInfoContext();

                var courseToUpdate = db.Courses.FirstOrDefault(x => x.Id == course.Id);

                if (courseToUpdate != null)
                {

                    courseToUpdate.Name = course.Name;
                    courseToUpdate.Description = course.Description;
                    courseToUpdate.NumberOfCredits = course.NumberOfCredits;
                    courseToUpdate.Level = course.Level;
                    db.SaveChanges();
                }
                return Json(new { success = true });
            }

            var errors = new List<string>();
            foreach(var e in ModelState.Values)
            {
                errors.AddRange(e.Errors.Select(x => x.ErrorMessage));
            }
            return Json(errors);
        }
    }
}