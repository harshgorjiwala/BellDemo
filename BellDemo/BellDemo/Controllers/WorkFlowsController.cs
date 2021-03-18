using BellDemo.Data;
using BellDemo.Data.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var list = await _context.WorkFlows.ToListAsync();
            return View(list);
        }

        // GET: WorkFlows/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new WorkFlow());
            else
                return View(_context.WorkFlows.Find(id));
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
            return View(workFlow);
        }

        // GET: WorkFlows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var workflow = await _context.WorkFlows.FindAsync(id);
            _context.WorkFlows.Remove(workflow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
