using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ETICARET.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IProductService _productService;
        private ICommentService _commentService;

        public CommentController(UserManager<ApplicationUser> userManager, IProductService productService, ICommentService commentService)
        {
            _userManager = userManager;
            _productService = productService;
            _commentService = commentService;
        }
        public IActionResult ShowProductComments(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = _productService.GetProductDetail(id.Value);

            if(product is null)
            {
                return NotFound();
            }

            return PartialView("_PartialComments",product.Comments);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            Comment comment = _commentService.GetById(id.Value);

            if (comment is null)
            {
                return NotFound();
            }

            _commentService.Delete(comment);

            return Json(new { result = true });

        }

        public IActionResult Edit(int?id, string text)
        {
            if(id is null)
            {
                return BadRequest();
            }

            Comment comment = _commentService.GetById(id.Value);

            if(comment is null)
            {
                return NotFound();
            }

            comment.Text = text;
            comment.CreateOn = DateTime.Now;

            _commentService.Update(comment);

            return Json(new { result = true });

        }

        public IActionResult Create(CommentModel model, int? productId)
        {
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                if(productId is null)
                {
                    return BadRequest();
                }

                Product product = _productService.GetById(productId.Value);

                if(product is null)
                {
                    return BadRequest();
                }

                Comment comment = new Comment()
                {
                    Text = model.Text.Trim('\n').Trim(' '),
                    CreateOn = DateTime.Now,
                    ProductId = product.Id,
                    UserId = _userManager.GetUserId(User) ?? "0"
                };

                _commentService.Create(comment);

                return Json(new { result = true });
            }

            return View(model);
        }
    }
}
