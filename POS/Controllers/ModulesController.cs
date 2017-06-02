using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POS;
using POS.Models;

namespace POS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private UPOSEntities db = new UPOSEntities();

        // GET: Modules/Details/5
        public ActionResult ActivitiesByStaff(string StaffList, string isSearched)
        {
            int assignedTotalHours = 0;

            List<SelectListItem> staffItems = new List<SelectListItem>();
            List<Staff> dbStaffList = db.Staffs.ToList();
            SelectListItem sli0 = new SelectListItem();
            sli0.Text = "Select Staff";
            sli0.Value = "0";
            staffItems.Add(sli0);
            foreach (Staff stf in dbStaffList)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = stf.Name;
                sli.Value = stf.Id.ToString();
                staffItems.Add(sli);
            }
            ViewBag.StaffList = staffItems;


            int myStaff = 0;
            if (!String.IsNullOrEmpty(StaffList))
            {
                bool b0 = int.TryParse(StaffList.Trim(), out myStaff);
            }


            //if (!String.IsNullOrEmpty(isSearched))
            //{
            //    Session["myStaff"] = myStaff.ToString();

            //    if (Session["myStaff"] != null)
            //    {
            //        bool b1 = int.TryParse(Session["myStaff"].ToString(), out myStaff);

            //        staffItems[myStaff].Selected = true;
            //    }
            //}
            //else
            //{
            //    // ??????????
            //}

            var currentUser = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "382" });
            }

            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "383" });
            }

            Department department = db.Departments.FirstOrDefault(x => x.Id == currentUser.DepartmentId);
            if (department == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "384" });
            }

            Staff selectedStaff = null;

            if (myStaff == 0)
            {
                ViewBag.StaffId = "0";
                ViewBag.StaffName = "NA";
            }
            else
            {
                selectedStaff = (Staff)db.Staffs.Find(myStaff);
                if (selectedStaff != null)
                {
                    ViewBag.StaffId = selectedStaff.Id.ToString();
                    ViewBag.StaffName = selectedStaff.Name;
                }
                else
                {
                    ViewBag.StaffId = "0";
                    ViewBag.StaffName = "NA";
                }
            }

            ViewBag.AssignedTotalHours = "0";

            var modules = db.Modules.Include(m => m.PreparedYear).Include(m => m.AcademicYear).Include(m => m.CourseType).Include(m => m.DegreeType).Include(m => m.Department).Include(m => m.Language).Include(m => m.Semester).Include(m => m.Staff).Include(m => m.StudyType).Include(m => m.ModuleStatusSet).OrderBy(m => m.Name); //.Where(x => x.DepartmentId.Equals(int.Parse(id.ToString())));

            List<Module> moduleList = new List<Module>();
            List<Module> staffModuleList = new List<Module>();

            if (modules == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "386" });
            }

            foreach (var item in modules)
            {
                moduleList.Add((Module)item);
            }

            moduleList = moduleList.Where(x => x.CoordinatorDeptId == department.Id).ToList();

            foreach (var module in moduleList)
            {
                bool isExist = false;
                foreach (var activity in module.Activities)
                {
                    if (activity.StaffId == myStaff && activity.StaffId > 0)
                    {
                        isExist = true;
                        assignedTotalHours += activity.TotalHours;
                    }
                }
                if (isExist == true)
                {
                    staffModuleList.Add((Module)module);
                    isExist = false;
                }
            }

            ViewBag.AssignedTotalHours = assignedTotalHours.ToString();

            return View(staffModuleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name));

        }


        // GET: Modules/Details/5
        public ActionResult StaffModules()
        {
            int assignedTotalHours = 0;

            if (!MyFunctions.CheckUserRole(User.Identity.Name, "staff"))
            {
                return RedirectToAction("Index", "Warning", new { id = "371" });
            }

            var staff = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (staff == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "372" });
            }

            ViewBag.StaffId = staff.Id.ToString();
            ViewBag.StaffName = staff.Name;
            ViewBag.AssignedTotalHours = "0";

            var modules = db.Modules.Include(m => m.PreparedYear).Include(m => m.AcademicYear).Include(m => m.CourseType).Include(m => m.DegreeType).Include(m => m.Department).Include(m => m.Language).Include(m => m.Semester).Include(m => m.Staff).Include(m => m.StudyType).Include(m => m.ModuleStatusSet).OrderBy(m => m.Name); //.Where(x => x.DepartmentId.Equals(int.Parse(id.ToString())));

            List<Module> moduleList = new List<Module>();
            List<Module> staffModuleList = new List<Module>();

            if (modules == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "373" });
            }

            foreach (var item in modules)
            {
                moduleList.Add((Module)item);
            }

            foreach (var module in moduleList)
            {
                bool isExist = false;
                foreach (var activity in module.Activities)
                {
                    if (activity.StaffId == staff.Id && module.ModuleStatusSet.Status == "Confirmed")
                    {
                        isExist = true;
                        assignedTotalHours += activity.TotalHours;
                    }
                }
                if (isExist == true)
                {
                    staffModuleList.Add((Module)module);
                    isExist = false;
                }
            }

            ViewBag.AssignedTotalHours = assignedTotalHours.ToString();

            return View(staffModuleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name));

        }


        // GET: Modules
        public ActionResult Index(string DepartmentList, string DegreeTypeList, string SemesterList, string StudyTypeList, string LanguageList, string isSearched)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "man"))
            {
                return RedirectToAction("Index", "Warning", new { id = "301" });
            }

            Session["BackLink"] = "Index";

            List<SelectListItem> departmentItems = new List<SelectListItem>();
            List<Department> dbDeptList = db.Departments.ToList();
            SelectListItem sli0 = new SelectListItem();
            sli0.Text = "Select Department";
            sli0.Value = "0";
            departmentItems.Add(sli0);
            foreach (Department dept in dbDeptList)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = dept.Name;
                sli.Value = dept.Id.ToString();
                departmentItems.Add(sli);
            }
            ViewBag.DepartmentList = departmentItems;

            List<SelectListItem> degreeTypeItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Degree Type",Value="0" },
                new SelectListItem {Text="Licencjat (Bachelor)",Value="1" },
                new SelectListItem {Text="Inżynieria (Engineering)",Value="2"},
                new SelectListItem {Text="Magister (Master)",Value="3"},
            };
            ViewBag.DegreeTypeList = degreeTypeItems;

            List<SelectListItem> semesterItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Semester",Value="0" },
                new SelectListItem {Text="1st Semester",Value="1" },
                new SelectListItem {Text="2nd Semester",Value="2"},
                new SelectListItem {Text="3rd Semester",Value="3"},
                new SelectListItem {Text="4th Semester",Value="4" },
                new SelectListItem {Text="5th Semester",Value="5" },
                new SelectListItem {Text="6th Semester",Value="6"},
                new SelectListItem {Text="7th Semester",Value="7"},
                new SelectListItem {Text="8th Semester",Value="8" },
            };
            ViewBag.SemesterList = semesterItems;

            List<SelectListItem> studyTypeItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Study Type",Value="0" },
                new SelectListItem {Text="Stacjonarne",Value="1" },
                new SelectListItem {Text="Niestacjonarne",Value="2"},
            };
            ViewBag.StudyTypeList = studyTypeItems;

            List<SelectListItem> languageItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Language",Value="0" },
                new SelectListItem {Text="Polish",Value="1" },
                new SelectListItem {Text="English",Value="2"},
            };
            ViewBag.LanguageList = languageItems;


            int myDepartment = 0;
            if (!String.IsNullOrEmpty(DepartmentList))
            {
                bool b0 = int.TryParse(DepartmentList.Trim(), out myDepartment);
            }

            int myDegreeType = 0;
            if (!String.IsNullOrEmpty(DegreeTypeList))
            {
                bool b2 = int.TryParse(DegreeTypeList.Trim(), out myDegreeType);
            }

            int mySemester = 0;
            if (!String.IsNullOrEmpty(SemesterList))
            {
                bool b1 = int.TryParse(SemesterList.Trim(), out mySemester);
            }

            int myStudyType = 0;
            if (!String.IsNullOrEmpty(StudyTypeList))
            {
                bool b2 = int.TryParse(StudyTypeList.Trim(), out myStudyType);
            }

            int myLanguage = 0;
            if (!String.IsNullOrEmpty(LanguageList))
            {
                bool b2 = int.TryParse(LanguageList.Trim(), out myLanguage);
            }

            if (!String.IsNullOrEmpty(isSearched))
            {
                Session["myDepartment"] = myDepartment.ToString();
                Session["mySemester"] = mySemester.ToString();
                Session["myDegreeType"] = myDegreeType.ToString();
                Session["myStudyType"] = myStudyType.ToString();
                Session["myLanguage"] = myLanguage.ToString();
            }
            else
            {
                if (Session["myDepartment"] != null)
                {
                    bool b4 = int.TryParse(Session["myDepartment"].ToString(), out myDepartment);
                }

                if (Session["myDegreeType"] != null)
                {
                    bool b4 = int.TryParse(Session["myDegreeType"].ToString(), out myDegreeType);
                }

                if (Session["mySemester"] != null)
                {
                    bool b4 = int.TryParse(Session["mySemester"].ToString(), out mySemester);
                }

                if (Session["myStudyType"] != null)
                {
                    bool b4 = int.TryParse(Session["myStudyType"].ToString(), out myStudyType);
                }

                if (Session["myLanguage"] != null)
                {
                    bool b4 = int.TryParse(Session["myLanguage"].ToString(), out myLanguage);
                }

                departmentItems[myDepartment].Selected = true;
                degreeTypeItems[myDegreeType].Selected = true;
                semesterItems[mySemester].Selected = true;
                studyTypeItems[myStudyType].Selected = true;
                languageItems[myLanguage].Selected = true;
            }

            var modules = db.Modules.Include(m => m.AcademicYear).Include(m => m.CourseType).Include(m => m.DegreeType).Include(m => m.Department).Include(m => m.Language).Include(m => m.Semester).Include(m => m.Staff).Include(m => m.StudyType).Include(m => m.ModuleStatusSet).OrderBy(m => m.Name); //.Where(x => x.DepartmentId.Equals(int.Parse(id.ToString())));

            List<Module> moduleList = new List<Module>();

            if (modules == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "302" });
            }
            foreach (var item in modules)
            {
                moduleList.Add((Module)item);
            }

            bool isFiltered = false;

            if (myDepartment > 0)
            {
                moduleList = moduleList.Where(x => x.DepartmentId == myDepartment).ToList();
                //isFiltered = true;
            }
            if (myDegreeType > 0)
            {
                moduleList = moduleList.Where(x => x.DegreeTypeId == myDegreeType).ToList();
                //isFiltered = true;
            }
            if (mySemester > 0)
            {
                moduleList = moduleList.Where(x => x.SemesterId == mySemester).ToList();
                //isFiltered = true;
            }
            if (myStudyType > 0)
            {
                moduleList = moduleList.Where(x => x.StudyTypeId == myStudyType).ToList();
                //isFiltered = true;
            }
            if (myLanguage > 0)
            {
                moduleList = moduleList.Where(x => x.LanguageId == myLanguage).ToList();
                //isFiltered = true;
            }
            if (myDepartment > 0 && myDegreeType > 0 && mySemester > 0 && myStudyType > 0 && myLanguage > 0)
            {
                isFiltered = true;
            }

            if (isFiltered)
            {
                //moduleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name);
                return View(moduleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name));
            }
            else
            {
                moduleList = moduleList.Where(x => x.Id == 99999).ToList();
                moduleList.OrderBy(x => x.Name);
                return View(moduleList);
            }
        }


        // GET: Modules/Details/5
        public ActionResult DeptModules(string DegreeTypeList, string SemesterList, string StudyTypeList, string LanguageList, string isSearched)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "303" });
            }

            Session["BackLink"] = "DeptModules";

            var currentUser = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "304" });
            }

            Department department = db.Departments.FirstOrDefault(x => x.Id == currentUser.DepartmentId);
            if (department == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "305" });
            }

            ViewBag.DeptId = department.Id.ToString();
            ViewBag.DepartmentName = department.Name;

            List<SelectListItem> degreeTypeItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Degree Type",Value="0" },
                new SelectListItem {Text="Licencjat (Bachelor)",Value="1" },
                new SelectListItem {Text="Inżynieria (Engineering)",Value="2"},
                new SelectListItem {Text="Magister (Master)",Value="3"},
            };
            ViewBag.DegreeTypeList = degreeTypeItems;

            List<SelectListItem> semesterItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Semester",Value="0" },
                new SelectListItem {Text="1st Semester",Value="1" },
                new SelectListItem {Text="2nd Semester",Value="2"},
                new SelectListItem {Text="3rd Semester",Value="3"},
                new SelectListItem {Text="4th Semester",Value="4" },
                new SelectListItem {Text="5th Semester",Value="5" },
                new SelectListItem {Text="6th Semester",Value="6"},
                new SelectListItem {Text="7th Semester",Value="7"},
                new SelectListItem {Text="8th Semester",Value="8" },
            };
            ViewBag.SemesterList = semesterItems;

            List<SelectListItem> studyTypeItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Study Type",Value="0" },
                new SelectListItem {Text="Stacjonarne",Value="1" },
                new SelectListItem {Text="Niestacjonarne",Value="2"},
            };
            ViewBag.StudyTypeList = studyTypeItems;

            List<SelectListItem> languageItems = new List<SelectListItem>()
            {
                new SelectListItem {Text="Select Language",Value="0" },
                new SelectListItem {Text="Polish",Value="1" },
                new SelectListItem {Text="English",Value="2"},
            };
            ViewBag.LanguageList = languageItems;

            int myDegreeType = 0;
            if (!String.IsNullOrEmpty(DegreeTypeList))
            {
                bool b2 = int.TryParse(DegreeTypeList.Trim(), out myDegreeType);
            }

            int mySemester = 0;
            if (!String.IsNullOrEmpty(SemesterList))
            {
                bool b1 = int.TryParse(SemesterList.Trim(), out mySemester);
            }

            int myStudyType = 0;
            if (!String.IsNullOrEmpty(StudyTypeList))
            {
                bool b2 = int.TryParse(StudyTypeList.Trim(), out myStudyType);
            }

            int myLanguage = 0;
            if (!String.IsNullOrEmpty(LanguageList))
            {
                bool b2 = int.TryParse(LanguageList.Trim(), out myLanguage);
            }

            if (!String.IsNullOrEmpty(isSearched))
            {
                Session["myDegreeType"] = myDegreeType.ToString();
                Session["mySemester"] = mySemester.ToString();
                Session["myStudyType"] = myStudyType.ToString();
                Session["myLanguage"] = myLanguage.ToString();
            }
            else
            {
                if (Session["myDegreeType"] != null)
                {
                    bool b4 = int.TryParse(Session["myDegreeType"].ToString(), out myDegreeType);
                }

                if (Session["mySemester"] != null)
                {
                    bool b4 = int.TryParse(Session["mySemester"].ToString(), out mySemester);
                }

                if (Session["myStudyType"] != null)
                {
                    bool b4 = int.TryParse(Session["myStudyType"].ToString(), out myStudyType);
                }

                if (Session["myLanguage"] != null)
                {
                    bool b4 = int.TryParse(Session["myLanguage"].ToString(), out myLanguage);
                }

                degreeTypeItems[myDegreeType].Selected = true;
                semesterItems[mySemester].Selected = true;
                studyTypeItems[myStudyType].Selected = true;
                languageItems[myLanguage].Selected = true;
            }

            var modules = db.Modules.Include(m => m.PreparedYear).Include(m => m.AcademicYear).Include(m => m.CourseType).Include(m => m.DegreeType).Include(m => m.Department).Include(m => m.Language).Include(m => m.Semester).Include(m => m.Staff).Include(m => m.StudyType).Include(m => m.ModuleStatusSet).OrderBy(m => m.Name); //.Where(x => x.DepartmentId.Equals(int.Parse(id.ToString())));

            List<Module> moduleList = new List<Module>();

            //var modules = db.Modules.Where(x => x.DepartmentId.Equals(int.Parse(id.ToString())));
            if (modules == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "306" });
            }

            foreach (var item in modules)
            {
                moduleList.Add((Module)item);
            }

            bool isFiltered = false;

            if (myDegreeType > 0)
            {
                moduleList = moduleList.Where(x => x.DegreeTypeId == myDegreeType).ToList();
                //isFiltered = true;
            }
            if (mySemester > 0)
            {
                moduleList = moduleList.Where(x => x.SemesterId == mySemester).ToList();
                //isFiltered = true;
            }
            if (myStudyType > 0)
            {
                moduleList = moduleList.Where(x => x.StudyTypeId == myStudyType).ToList();
                //isFiltered = true;
            }
            if (myLanguage > 0)
            {
                moduleList = moduleList.Where(x => x.LanguageId == myLanguage).ToList();
                //isFiltered = true;
            }

            if (myDegreeType > 0 && mySemester > 0 && myStudyType > 0 && myLanguage > 0)
            {
                isFiltered = true;
            }

            moduleList = moduleList.Where(x => x.DepartmentId == currentUser.DepartmentId || x.CoordinatorDeptId == currentUser.DepartmentId).ToList();

            moduleList = moduleList.Where(x => x.IsActive == true).ToList();

            if (isFiltered)
            {
                //moduleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name);
                return View(moduleList.OrderBy(x => x.CourseTypeId).ThenBy(x => x.Name));
            }
            else
            {
                moduleList = moduleList.Where(x => x.Id == 99999).ToList();
                moduleList.OrderBy(x => x.Name);
                return View(moduleList);
            }
        }


        // GET: Modules/Create
        public ActionResult CreateHop(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "307" });
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "308" });
            }

            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "309" });
            }
            ViewBag.DepartmentName = department.Name;

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name");
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name");
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name");
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name");
            //ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode");

            List<SelectListItem> departmentItems = new List<SelectListItem>();
            List<Department> dbDeptList = db.Departments.ToList();
            foreach (Department dept in dbDeptList)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = dept.Name;
                sli.Value = dept.Id.ToString();
                if (dept.Id == id)
                {
                    sli.Selected = true;
                }
                departmentItems.Add(sli);
            }
            ViewBag.DepartmentId = departmentItems;

            //ViewBag.DepartmentId[id].Selected = true;

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name");
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name");
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHop([Bind(Include = "Id,Name,NameEN,ModuleCode,DepartmentId,Specialization,Coordinator,PreparedYearId,AcademicYearId,SemesterId,CourseTypeId,DegreeTypeId,StudyTypeId,LanguageId,ECTS,IsFL,Note,IsActive,StatusId,CoordinatorDeptId")] Module module)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "310" });
            }

            var currentUser = db.Staffs.FirstOrDefault(x => x.EmailAddress == User.Identity.Name);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Warning", new { id = "319" });
            }
            module.DepartmentId = currentUser.DepartmentId;

            if (ModelState.IsValid)
            {
                try
                {
                    module.StatusId = 0;
                    if (String.IsNullOrEmpty(module.ModuleCode))
                    {
                        module.ModuleCode = "Unknown";
                    }
                    db.Modules.Add(module);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Warning", new { id = "324" });
                }

                Module newModule = db.Modules.Include(m => m.Department).Include(m => m.DegreeType).Include(m => m.Semester).Include(m => m.StudyType).Include(m => m.Language).FirstOrDefault(x => x.Id == module.Id);
                if (newModule != null && newModule.ModuleCode == "Unknown")
                {
                    string moduleCode = "";
                    try
                    {
                        moduleCode += newModule.Department.DeptCode.ToString();
                        moduleCode += newModule.DegreeType.DTCode.ToString();
                        moduleCode += newModule.Semester.Name.ToString();
                        moduleCode += newModule.StudyType.STCode.ToString();
                        moduleCode += newModule.Language.LCode.ToString();
                    }
                    catch (Exception ex)
                    {
                        moduleCode = "Unknown";
                        return RedirectToAction("Index", "Warning", new { id = "323" });
                    }
                    if (moduleCode.Length == 0)
                    {
                        moduleCode = "Unknown";
                    }
                    newModule.ModuleCode = moduleCode;
                    db.SaveChanges();
                }

                return RedirectToAction("DeptModules", new { Id = module.DepartmentId });
            }

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            //ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);


            List<SelectListItem> departmentItems = new List<SelectListItem>();
            List<Department> dbDeptList = db.Departments.ToList();
            foreach (Department dept in dbDeptList)
            {
                SelectListItem sli = new SelectListItem();
                sli.Text = dept.Name;
                sli.Value = dept.Id.ToString();
                if (dept.Id == module.Id)
                {
                    sli.Selected = true;
                }
                departmentItems.Add(sli);
            }
            ViewBag.DepartmentId = departmentItems;

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            return View(module);
        }


        // GET: Modules/Create
        public ActionResult Create()
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "man"))
            {
                return RedirectToAction("Index", "Warning", new { id = "311" });
            }

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name");
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name");
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name");
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode");
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name");
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name");
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status");

            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,NameEN,ModuleCode,DepartmentId,Specialization,Coordinator,PreparedYearId,AcademicYearId,SemesterId,CourseTypeId,DegreeTypeId,StudyTypeId,LanguageId,ECTS,IsFL,Note,IsActive,StatusId,CoordinatorDeptId")] Module module)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "man"))
            {
                return RedirectToAction("Index", "Warning", new { id = "312" });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    module.StatusId = 0;
                    if (String.IsNullOrEmpty(module.ModuleCode))
                    {
                        module.ModuleCode = "Unknown";
                    }
                    db.Modules.Add(module);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Warning", new { id = "324" });
                }

                Module newModule = db.Modules.Include(m => m.Department).Include(m => m.DegreeType).Include(m => m.Semester).Include(m => m.StudyType).Include(m => m.Language).FirstOrDefault(x => x.Id == module.Id);
                if (newModule != null && newModule.ModuleCode == "Unknown")
                {
                    string moduleCode = "";
                    try
                    {
                        moduleCode += newModule.Department.DeptCode.ToString();
                        moduleCode += newModule.DegreeType.DTCode.ToString();
                        moduleCode += newModule.Semester.Name.ToString();
                        moduleCode += newModule.StudyType.STCode.ToString();
                        moduleCode += newModule.Language.LCode.ToString();
                    }
                    catch (Exception ex)
                    {
                        moduleCode = "Unknown";
                        return RedirectToAction("Index", "Warning", new { id = "323" });
                    }
                    if (moduleCode.Length == 0)
                    {
                        moduleCode = "Unknown";
                    }
                    newModule.ModuleCode = moduleCode;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status", module.StatusId);

            return View(module);
        }

        // GET: Modules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "313" });
            }

            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status", module.StatusId);

            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,NameEN,ModuleCode,DepartmentId,Specialization,Coordinator,PreparedYearId,AcademicYearId,SemesterId,CourseTypeId,DegreeTypeId,StudyTypeId,LanguageId,ECTS,IsFL,Note,IsActive,StatusId,CoordinatorDeptId")] Module module)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "314" });
            }

            if (ModelState.IsValid)
            {
                //db.Entry(module).Entity.Modified = DateTime.Now;
                //db.Entry(module).Entity.ModifiedBy = User.Identity.Name;
                module.Modified = DateTime.Now;
                module.ModifiedBy = User.Identity.Name;

                db.Entry(module).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("DeptModules", new { Id = module.DepartmentId } );
            }
            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status", module.StatusId);

            return View(module);
        }


        // Admin Edit
        public ActionResult AdminEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "315" });
            }

            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }

            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status", module.StatusId);

            return View(module);
        }

        // POST: Admin Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEdit([Bind(Include = "Id,Name,NameEN,ModuleCode,DepartmentId,Specialization,Coordinator,PreparedYearId,AcademicYearId,SemesterId,CourseTypeId,DegreeTypeId,StudyTypeId,LanguageId,ECTS,IsFL,Note,IsActive,StatusId,CoordinatorDeptId")] Module module)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "hop"))
            {
                return RedirectToAction("Index", "Warning", new { id = "316" });
            }

            if (ModelState.IsValid)
            {
                //db.Entry(module).Entity.Modified = DateTime.Now;
                //db.Entry(module).Entity.ModifiedBy = User.Identity.Name;
                module.Modified = DateTime.Now;
                module.ModifiedBy = User.Identity.Name;

                db.Entry(module).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PreparedYearId = new SelectList(db.AcademicYears, "Id", "Name", module.PreparedYearId);
            ViewBag.AcademicYearId = new SelectList(db.AcademicYears, "Id", "Name", module.AcademicYearId);
            ViewBag.CourseTypeId = new SelectList(db.CourseTypes, "Id", "Name", module.CourseTypeId);
            ViewBag.DegreeTypeId = new SelectList(db.DegreeTypes, "Id", "Name", module.DegreeTypeId);
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", module.DepartmentId);
            ViewBag.CoordinatorDeptId = new SelectList(db.Departments, "Id", "DeptCode", module.CoordinatorDeptId);
            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", module.LanguageId);
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Name", module.SemesterId);
            ViewBag.Coordinator = new SelectList(db.Staffs, "Id", "Name", module.Coordinator);
            ViewBag.StudyTypeId = new SelectList(db.StudyTypes, "Id", "Name", module.StudyTypeId);
            ViewBag.StatusId = new SelectList(db.ModuleStatusSets, "Id", "Status", module.StatusId);

            return View(module);
        }


        // GET: Modules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "man"))
            {
                return RedirectToAction("Index", "Warning", new { id = "317" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!MyFunctions.CheckUserRole(User.Identity.Name, "man"))
            {
                return RedirectToAction("Index", "Warning", new { id = "318" });
            }

            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
