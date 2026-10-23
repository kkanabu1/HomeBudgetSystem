
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeBudgetSystem.Web.Models;
using HomeBudgetSystem.Web.Data;

public class BudgetsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BudgetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: BUDGETS
    public async Task<IActionResult> Index()    
    {
        var budgets = _context.Budgets
        .OrderByDescending(b => b.Year)
        .ThenByDescending(b => b.Month);

        return View(await budgets.ToListAsync());
    }

    // GET: BUDGETS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(m => m.Id == id);
        if (budget == null)
        {
            return NotFound();
        }

        return View(budget);
    }

    // GET: BUDGETS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BUDGETS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Month,Year,PlannedIncome,PlannedExpenses")] Budget budget)
    {
        if (ModelState.IsValid)
        {
            _context.Add(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(budget);
    }

    // GET: BUDGETS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets.FindAsync(id);
        if (budget == null)
        {
            return NotFound();
        }
        return View(budget);
    }

    // POST: BUDGETS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Month,Year,PlannedIncome,PlannedExpenses")] Budget budget)
    {
        if (id != budget.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(budget);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(budget.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(budget);
    }

    // GET: BUDGETS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(m => m.Id == id);
        if (budget == null)
        {
            return NotFound();
        }

        return View(budget);
    }

    // POST: BUDGETS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var budget = await _context.Budgets.FindAsync(id);
        if (budget != null)
        {
            _context.Budgets.Remove(budget);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BudgetExists(int? id)
    {
        return _context.Budgets.Any(e => e.Id == id);
    }
}
