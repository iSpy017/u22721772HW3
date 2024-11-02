using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using u22721772_HW3.Models;
using PagedList;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls;
using System.Web.UI;

using jsreport;

namespace u22721772_HW3.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();

        // Home/Index action

        public async Task<ActionResult> Index(int? studentPage, int? bookPage)
        {
            int pageSize = 10; // Number of items to display per page


            var students = await db.students.OrderBy(s => s.studentId).ToListAsync();
            var studentsPaged = students.ToPagedList(studentPage ?? 1, pageSize);

            var books = await db.books.Include(b => b.author).Include(b => b.type).ToListAsync();
            var booksPaged = books.ToPagedList(bookPage ?? 1, pageSize);

            var homeViewModel = new HomeViewModel
            {
                Students = studentsPaged,
                Books = booksPaged
            };

            return View(homeViewModel);
        }   


        public async Task<ActionResult> Maintain(int? authorPage, int? typesPage, int? borrowsPage)
        {
            int pageSize = 10; // Number of items to display per page

            // Author paged list
            var authors = await db.authors.OrderBy(a => a.authorId).ToListAsync();
            var authorsPaged = authors.ToPagedList(authorPage ?? 1, pageSize);

            // Type paged list
            var types = await db.types.OrderBy(t => t.typeId).ToListAsync();
            var typesPaged = types.ToPagedList(typesPage ?? 1, pageSize);

            // Borrow paged list
            var borrows = await db.borrows.Include(b => b.book).Include(b => b.student).ToListAsync();
            var borrowsPaged = borrows.ToPagedList(borrowsPage ?? 1, pageSize);

            var maintainViewModel = new MaintainViewModel
            {
                authors = authorsPaged,
                types = typesPaged,
                borrows = borrowsPaged
            };

            return View(maintainViewModel);
        }






        // Home/Report action
        public async Task<ActionResult> Report()
        {
            // Generate and display the report (customize this logic)
            var reportData = await GeneratePopularBooksReport();
            return View(reportData);
        }

        // Logic to generate the report data (customize as per your requirement)
        private async Task<List<ReportModel>> GeneratePopularBooksReport()
        {
            // Implement the logic to calculate the report data
            // Example: Count the number of borrows for each book
            var reportData = await db.borrows
                .GroupBy(b => b.bookId)
                .Select(g => new ReportModel
                {
                    BookId = (int)g.Key,
                    BorrowCount = g.Count()
                })
                .ToListAsync();

            // Retrieve book titles for the report data
            foreach (var item in reportData)
            {
                var book = await db.books.FindAsync(item.BookId);
                if (book != null)
                {
                    item.BookTitle = book.name;
                }
            }

            return reportData;
        }




        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student students)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(students);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(students);
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
