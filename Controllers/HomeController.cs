using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        
        private IBowlersRepository _repo { get; set; }

        //Constructor
        public HomeController(IBowlersRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(int id = 0)
        {
            ViewBag.Teams = _repo.Teams.Distinct().ToList();

            if (id == 0)
            {
                ViewBag.SelectedTeam = "All Bowlers"; //if no team is selected
                var blah = _repo.Bowlers.ToList(); //compile list of all bowlers
                return View(blah);
            }
            else
            {
                ViewBag.SelectedTeam = _repo.Teams.Single(x => x.TeamID == id).TeamName; //if a team filter is seleceted, find the team name where TeamID in the table matches the id passed in the URL
                var blah = _repo.Bowlers.Where(x => x.TeamID == id).ToList(); //compile list of bowlers from the specified team
                return View(blah);
            }
        }

        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = _repo.Teams.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            _repo.AddBowler(b);
            ViewBag.ActionString = "Successfully Added Bowler Record:";

            return View("Confirmation", b); //pass it to the view
        }

        //EDIT:
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Teams = _repo.Teams.Distinct().ToList();
            var bowler = _repo.getBowler(id);
            return View(bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            _repo.EditBowler(b);
            return View("Confirmation", b);
        }

        //DELETE:
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var elBowlerMoribundo = _repo.getBowler(id);
            return View(elBowlerMoribundo);
        }



        [HttpPost]
        public IActionResult Delete(Bowler b)
        {
            _repo.DeleteBowler(b);
            return View("Confirmation");
        }
    }
}
