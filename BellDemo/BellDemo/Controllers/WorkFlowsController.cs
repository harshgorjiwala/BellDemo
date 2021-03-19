using BellDemo.Data;
using BellDemo.Data.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BellDemo.Controllers
{
    [Authorize]
    public class WorkFlowsController : Controller
    {
        private readonly AppDBContext _context;
        

        public WorkFlowsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: WorkFlows
        public async Task<IActionResult> Index(bool newlogin = false)
        {
            ViewBag.newlogin = newlogin;

            var list = await _context.WorkFlows.ToListAsync();

            var flow = new WorkFlow
            {
                ServiceCategoryTypes = _context.serviceCategoryTypes.Select(a => new SelectListItem()
                {
                    Text = a.ServiceCategoryTypeName,
                    Value = a.ServiceCategoryTypeID.ToString()
                }).ToList(),
                ServiceCategories = new List<SelectListItem>()
            };
            flow.Date = null;

            ViewBag.workorder = flow;
            return View(list);
        }

        // GET: WorkFlows/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var flow = new WorkFlow
                {
                    ServiceCategoryTypes = _context.serviceCategoryTypes.Select(a => new SelectListItem()
                    {
                        Text = a.ServiceCategoryTypeName, Value = a.ServiceCategoryTypeID.ToString()
                    }).ToList(),
                    ServiceCategories = new List<SelectListItem>()
                };

                return View(flow);
            }

            else
            {
                var flow = _context.WorkFlows.Find(id);

                flow.ServiceCategoryTypes = _context.serviceCategoryTypes.Select(a => new SelectListItem()
                {
                    Text = a.ServiceCategoryTypeName,
                    Value = a.ServiceCategoryTypeID.ToString()
                }).ToList();

                flow.ServiceCategories = _context.ServiceCategories
                    .Where(a => a.ServiceCategoryType.ServiceCategoryTypeID == flow.ServiceCategoryTypeID).Select(a => new SelectListItem()
                    {
                        Text = a.ServiceCategoryName,
                        Value = a.ServiceCategoryId.ToString()
                    }).ToList();

                return View(flow);
            }
        }

        // POST: WorkFlows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(WorkFlow workFlow)
        {
            if (ModelState.IsValid)
            {
                if (workFlow.WorkFLowID == 0)
                {
                    _context.Add(workFlow);
                    
                }
                else
                {
                    _context.Update(workFlow);
                   
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                workFlow.ServiceCategoryTypes = _context.serviceCategoryTypes.Select(a => new SelectListItem()
                {
                    Text = a.ServiceCategoryTypeName,
                    Value = a.ServiceCategoryTypeID.ToString()
                }).ToList();

                workFlow.ServiceCategories = workFlow.WorkFLowID > 0
                    ? _context.ServiceCategories
                        .Where(a => a.ServiceCategoryType.ServiceCategoryTypeID == workFlow.ServiceCategoryTypeID)
                        .Select(a => new SelectListItem()
                        {
                            Text = a.ServiceCategoryName,
                            Value = a.ServiceCategoryId.ToString()
                        }).ToList()
                    : new List<SelectListItem>();

                return View(workFlow);
            }
        }

        // GET: WorkFlows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var workflow = await _context.WorkFlows.FindAsync(id);
            _context.WorkFlows.Remove(workflow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //this will return categories depends on category type
        [HttpPost]
        public JsonResult GetCategory(int id)
        {
            return Json(new SelectList(_context.ServiceCategories.Where(a => (a.ServiceCategoryType.ServiceCategoryTypeID == id)), "ServiceCategoryId", "ServiceCategoryName"));
        }
    }
}
