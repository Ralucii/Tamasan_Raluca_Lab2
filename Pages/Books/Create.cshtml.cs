using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tamasan_Raluca_Lab2.Data;
using Tamasan_Raluca_Lab2.Models;

namespace Tamasan_Raluca_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]

    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Tamasan_Raluca_Lab2.Data.Tamasan_Raluca_Lab2Context _context;

        public CreateModel(Tamasan_Raluca_Lab2.Data.Tamasan_Raluca_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            });

            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");

            var book = new Book();

            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);


            return Page();
        }

        [BindProperty]
        public Book Book { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newBook = new Book();
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }

                _context.Book.Add(Book);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAssignedCategoryData(_context, newBook);
            return Page();
        }
    }
}