using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Henk5.Data;
using Henk5.Models;
using Microsoft.AspNetCore.Authorization;

namespace Henk5.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Items
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {

            IEnumerable<Item> items = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.GetAsync("items");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Item>>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    items = Enumerable.Empty<Item>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(items);
        }

        // GET: Items/Details/5
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            Item items = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/Items/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Item>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(items);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item items)
        {
            //database.Items.Add(item);
            //database.SaveChanges();
            //return RedirectToAction("Index");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.PostAsJsonAsync("items", items);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Item>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return RedirectToAction("Index");


        }


        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Item items = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/Items/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Item>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View(items);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item)
        {
            Item items = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/Items/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.PutAsJsonAsync(item.Id.ToString(), item);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Item>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Item? items;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7100/api/Items/");

                //Called Member default GET All records  
                //GetAsync to send a GET request   
                // PutAsync to send a PUT request  
                var responseTask = client.DeleteAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {

                    var readTask = result.Content.ReadAsAsync<Item>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
