
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeBudgetSystem.Web.Models;
using HomeBudgetSystem.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


public class TransactionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransactionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TRANSACTIONS
    public async Task<IActionResult> Index()
    {
        var transactions = _context.Transactions
        .Include(t => t.Category)
        .OrderByDescending(t => t.Date);


        return View(await transactions.ToListAsync());
    }

    // GET: TRANSACTIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // GET: TRANSACTIONS/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

    // POST: TRANSACTIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Amount,Date,Description,IsIncome,CategoryId,Category")] Transaction transaction)
    {
        if (ModelState.IsValid)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", transaction.CategoryId);

        return View(transaction);
    }

    // GET: TRANSACTIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }
        return View(transaction);
    }

    // POST: TRANSACTIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Amount,Date,Description,IsIncome,CategoryId,Category")] Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.Id))
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
        return View(transaction);
    }

    // GET: TRANSACTIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // POST: TRANSACTIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransactionExists(int? id)
    {
        return _context.Transactions.Any(e => e.Id == id);
    }
}
