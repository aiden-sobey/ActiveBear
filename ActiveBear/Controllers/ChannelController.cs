using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveBear.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ActiveBearContext _context;

        public ChannelController(ActiveBearContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public void CreateChannel(string title, string key)
        {
            var channel = ChannelService.CreateChannel(title, key, _context);
        }

        public IActionResult Engage(Guid? id)
        {
            // Check the passed ID represents a valid channel
            Channel activeChannel;

            if (id == null)
                return NotFound();

            var channels = _context.Channels.Where(x => x.Id == id);
            switch (channels.Count())
            {
                case 0:
                    return NotFound();
                case 1:
                    activeChannel = channels.FirstOrDefault();
                    break;
                default:
                    // This would actually be a very serious error - GUID double up
                    return NotFound(); 
            }

            // Check the current user is authorised
            var currentUser = _context.Users.FirstOrDefault();
            if (!activeChannel.AuthorisedUsers.Contains(currentUser))
                return NotFound();

            // Gather relevant messages
            var channelMessages = _context.Messages.Where(m => m.Channel == id).ToList();

            // Push our information to the view
            ViewBag.Channel = activeChannel;
            ViewBag.Messages = channelMessages;
            ViewBag.UserNames = ChannelService.LinkMessagesToUsers(channelMessages, _context);

            return View();
        }
    }
}
