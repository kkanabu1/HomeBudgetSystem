using HomeBudgetSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HomeBudgetSystem.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            var totalIncome = transactions
                .Where(t => t.IsIncome)
                .Sum(t => t.Amount);

            var totalExpenses = transactions
                .Where(t => !t.IsIncome)
                .Sum(t => t.Amount);

            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.Balance = totalIncome - totalExpenses;
            ViewBag.RecentTransactions = transactions.Take(5).ToList();

            var expenseData = transactions
                .Where(t => !t.IsIncome)
                .GroupBy(t => t.Category.Name)
                .Select(g => new
    {
        Category = g.Key,
        Total = g.Sum(t => t.Amount)
    })
    .ToList();

            ViewBag.ChartLabels = expenseData
                .Select(e => e.Category)
                .ToList();

            ViewBag.ChartData = expenseData
                .Select(e => e.Total)
                .ToList();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var currentBudget = await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.Month == currentMonth &&
                    b.Year == currentYear);

            var monthlyExpenses = transactions
                .Where(t =>
                 !t.IsIncome &&
                t.Date.Month == currentMonth &&
                 t.Date.Year == currentYear)
                .GroupBy(t => t.Category.Name)
                 .Select(g => new



                 {
                     Category = g.Key,
                     Total = g.Sum(t => t.Amount)
                 })

    .ToList();

            var monthlyTotalExpenses = transactions
    .Where(t =>
        !t.IsIncome &&
        t.Date.Month == currentMonth &&
        t.Date.Year == currentYear)
    .Sum(t => t.Amount);

            ViewBag.MonthlyExpenses = monthlyTotalExpenses;

            ViewBag.MonthlyChartLabels =
                monthlyExpenses.Select(x => x.Category).ToList();

            ViewBag.MonthlyChartData =
                monthlyExpenses.Select(x => x.Total).ToList();

            if (currentBudget != null)
            {
                ViewBag.PlannedExpenses = currentBudget.PlannedExpenses;
                ViewBag.PlannedIncome = currentBudget.PlannedIncome;

                ViewBag.ExpenseDifference =
                    currentBudget.PlannedExpenses - monthlyTotalExpenses;

                if (monthlyTotalExpenses > currentBudget.PlannedExpenses)
                {
                    ViewBag.BudgetWarning =
                        "WARNING: Monthly budget exceeded!";
                }
                else
                {
                    ViewBag.BudgetWarning =
                        "You are within your monthly budget.";
                }

                ViewBag.IncomeDifference =
                    totalIncome - currentBudget.PlannedIncome;
            }

            var currentDay = DateTime.Now.Day;
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            decimal forecastExpenses = 0;

            if (currentDay > 0)
            {
                forecastExpenses = (totalExpenses / currentDay) * daysInMonth;
            }

            ViewBag.ForecastExpenses = Math.Round(forecastExpenses, 2);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}