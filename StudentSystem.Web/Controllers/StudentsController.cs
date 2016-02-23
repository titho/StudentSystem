﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentSystem.Data;
using StudentSystem.Web.Models;
using StudentSystem.DatabaseModels;

namespace StudentSystem.Web.Controllers
{
    //[Authorize(Users="deevvil_pz@abv.bg")]
    public class StudentsController : BaseController
    {

        // GET: Students
        public ActionResult Index()
        {
            var students = this.data.Students.All().Select(StudentViewModel.FromStudentModel).ToList();
            return View(students);

        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student foundStudentDM = this.data.Students.Find(id);
            if (foundStudentDM == null)
            {
                return HttpNotFound();
            }
            StudentViewModel foundStudentVM = new StudentViewModel()
            {
                StudentID = foundStudentDM.StudentID,
                FirstName = foundStudentDM.FirstName,
                SecondName = foundStudentDM.SecondName,
                LastName = foundStudentDM.LastName,
                Number = foundStudentDM.Number,
                StudentClass = foundStudentDM.StudentClass.ClassName
            };
            return View(foundStudentVM);


        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var studentClassesList = StudentClassesSelectList();
            ViewBag.StudentClasses = studentClassesList;
            return View();
        }

     

        // тука вече се POST-ват данните, попълнени във View-то от потребителя
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,SecondName,LastName,Number,StudentClassID")] StudentViewModel studentViewModel)
        {
            

            if (!ModelState.IsValid)
            {
                // ако нещо не е наред, връщаме за да се попълни/коригира
                return View(studentViewModel);
            }
            var studentClassToAssign = this.data.StudentClasses.Find(studentViewModel.StudentClassID);
            if(studentClassToAssign ==null)
            {
                return HttpNotFound("Student class not found");
            }

            Student newStudent = new Student()
            {
                // само името - StudentClassID се създава от EntityFramework
                // оценките и учениците се попълват от друг контролер!!!
                FirstName = studentViewModel.FirstName,
                SecondName = studentViewModel.SecondName,
                LastName = studentViewModel.LastName,
                Number = studentViewModel.Number,
                StudentClass=studentClassToAssign
            };
            this.data.Students.Add(newStudent);
            this.data.SaveChanges();

            // отиваме на индекса за класовете (или където искаме след успешно създаден студент)
            return RedirectToAction("Index");
        }
        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToEdit= this.data.Students.Find(id);
            if (studentToEdit == null)
            {
                return HttpNotFound();
            }
            StudentViewModel studentViewModel = new StudentViewModel()
                {
                    StudentClassID = studentToEdit.StudentClass.StudentClassID,
                    StudentID = studentToEdit.StudentID,
                    FirstName = studentToEdit.FirstName,
                    SecondName = studentToEdit.SecondName,
                    LastName = studentToEdit.LastName,
                    Number = studentToEdit.Number,
                    StudentClass = studentToEdit.StudentClass.ClassName

                };
            ViewBag.StudentClasses = StudentClassesSelectList();
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,SecondName,LastName,Number,StudentClassID")] StudentViewModel studentViewModel)
        {
            if (!ModelState.IsValid)
            {
                // ако нещо не е наред, връщаме за да се попълни/коригира
                return View(studentViewModel);
            }
            var studentToEditClass = this.data.StudentClasses.Find(studentViewModel.StudentClassID);
            var studentToEdit = this.data.Students.Find(studentViewModel.StudentID);
            if (studentToEdit==null)
            {
                return HttpNotFound("Student Not Found !");
            }
            studentToEdit.FirstName = studentViewModel.FirstName;
            studentToEdit.SecondName = studentViewModel.SecondName;
            studentToEdit.LastName = studentViewModel.LastName;
            studentToEdit.Number = studentViewModel.Number;
            studentToEdit.StudentClass = studentToEditClass;
            this.data.Students.Update(studentToEdit);
            this.data.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Students/Delete/5
        //Nedovursheno 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentViewModel studentViewModel = null;
            if (studentViewModel == null)
            {
                return HttpNotFound();
            }
            return View(studentViewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            return RedirectToAction("Index");
        }

      
    }
}
