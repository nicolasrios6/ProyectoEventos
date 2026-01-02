using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoEventos.Data;
using ProyectoEventos.Models;
using ProyectoEventos.Service;

namespace ProyectoEventos.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private readonly EventosDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ImagenStorage _imagenStorage;

        public EventoController(EventosDbContext context, UserManager<Usuario> userManager, ImagenStorage imagenStorage)
        {
            _context = context;
            _userManager = userManager;
            _imagenStorage = imagenStorage;
        }

        // GET: Evento
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var eventos = await _context.Eventos
                .Where(e => e.UsuarioId == userId)
                .OrderByDescending(e => e.FechaHora)
                .ToListAsync();
            return View(eventos);
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Evento/Create
        public IActionResult Create()
        {
            //ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel evento)
        {
            if (!ModelState.IsValid)
                return View(evento);

            var userId = _userManager.GetUserId(User);

            string? imagenUrl = null;

            if (evento.Imagen != null)
            {
                try
                {
                    imagenUrl = await _imagenStorage.SaveAsync(userId, evento.Imagen);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Imagen", $"Error al procesar la imagen: {ex.Message}");
                    return View(evento);
                }
            }
            var nuevoEvento = new Evento
            {
                Titulo = evento.Titulo,
                Descripcion = evento.Descripcion,
                FechaHora = evento.FechaHora,
                Ubicacion = evento.Ubicacion,
                LinkCompra = evento.LinkCompra,
                ImagenUrl = imagenUrl,
                Estado = EstadoEvento.Pendiente,
                UsuarioId = userId,
            };

            _context.Eventos.Add(nuevoEvento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Evento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            var vm = new EventoViewModel
            {
                Id = evento.Id,
                Titulo = evento.Titulo,
                Descripcion = evento.Descripcion,
                FechaHora = evento.FechaHora,
                Ubicacion = evento.Ubicacion,
                LinkCompra = evento.LinkCompra,
                ImagenUrl = evento.ImagenUrl
            };

            return View(vm);

            //ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", evento.UsuarioId);
            //return View(evento);
        }

        //// POST: Evento/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, EventoViewModel evento)
        //{
        //    if (id != evento.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(evento);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EventoExists(evento.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", evento.UsuarioId);
        //    return View(evento);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var evento = await _context.Eventos.FindAsync(vm.Id);
            if (evento == null) return NotFound();

            evento.Titulo = vm.Titulo;
            evento.Descripcion = vm.Descripcion;
            evento.FechaHora = vm.FechaHora;
            evento.Ubicacion = vm.Ubicacion;
            evento.LinkCompra = vm.LinkCompra;

            if (vm.Imagen != null)
            {
                await _imagenStorage.DeleteAsync(evento.ImagenUrl);

                var userId = _userManager.GetUserId(User);
                evento.ImagenUrl = await _imagenStorage.SaveAsync(userId, vm.Imagen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Evento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Evento/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var evento = await _context.Eventos.FindAsync(id);
        //    if (evento != null)
        //    {
        //        _context.Eventos.Remove(evento);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.Id == id);
        }
    }
}
