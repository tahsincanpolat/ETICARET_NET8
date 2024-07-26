using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Extensions;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using OrderItem = ETICARET.Entities.OrderItem;

namespace ETICARET.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private IProductService _productService;
        private UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService, IOrderService orderService, IProductService productService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _productService = productService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            return View(
                new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = i.Product.Price,
                        ImageUrl = i.Product.Images[0].ImageUrl, // TODO: Düzeltilecek.
                        Quantity = i.Quantity
                    }).ToList()
                }
                
            );
        }

        public IActionResult AddToCart(int productId, int quantity) {

            _cartService.AddToCart(_userManager.GetUserId(User),productId,quantity);

            return RedirectToAction("Index");
        
        }

        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(_userManager.GetUserId(User),productId);

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            OrderModel orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    ImageUrl = i.Product.Images[0].ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model, string paymentMethod)
        {
            ModelState.Remove("CartModel");

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.ProductId,
                        Name = i.Product.Name,
                        Price = i.Product.Price,
                        ImageUrl = i.Product.Images[0].ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()

                };

                if(paymentMethod == "credit")
                {
                    // ödeme
                    var payment = PaymentProccess(model);

                    if(payment.Status == "success")
                    {
                        SaveOrder(model, payment, userId);
                        ClearCart(cart.Id.ToString());
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Order Completed",
                            Message = "Congratulations. your order has been received.",
                            Css = "success"
                        });

                        return View("Success");
                    }
                    else
                    {
                        TempData.Put("message", new ResultModel()
                        {
                            Title = "Hata",
                            Message = payment.ErrorMessage,
                            Css = "danger"
                        });

                    }
                  

                }
                else
                {
                    SaveOrder(model,userId);
                    ClearCart(cart.Id.ToString());
                    TempData.Put("message", new ResultModel()
                    {
                        Title = "Order Completed",
                        Message = "Congratulations. Your order has been received.",
                        Css = "success"
                    });

                    return View("Success");

                }
            }
            return View(model);
        }

        private void ClearCart(string cartId)
        {
            _cartService.ClearCart(cartId);
        }


        // EFT için çalışan sipariş kaydet methodu
        private void SaveOrder(OrderModel model, string userId)
        {
            var order = new Order()
            {
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState = Order.EnumOrderState.completed,
                PaymentTypes = Order.EnumPaymentTypes.Eft,
                PaymentToken = Guid.NewGuid().ToString(),
                ConversionId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                OrderNote = model.OrderNote,
                OrderDate = DateTime.Now,
                FirstName = model.FirstName,    
                LastName = model.LastName,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                City = model.City,
                UserId = userId
            };

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                };

                order.OrderItems.Add(orderItem);

            }

            _orderService.Create(order);
        }

        // Kredi Kartı için çalışan sipariş kaydet methodu
        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order()
            {
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState = Order.EnumOrderState.completed,
                PaymentTypes = Order.EnumPaymentTypes.CreditCard,
                PaymentToken = Guid.NewGuid().ToString(),
                ConversionId = payment.ConversationId,
                PaymentId = payment.PaymentId,
                OrderNote = model.OrderNote,
                OrderDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                Email = model.Email,
                Phone = model.Phone,
                City = model.City,
                UserId = userId
            };

            foreach (var item in model.CartModel.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                };

                order.OrderItems.Add(orderItem);

            }

            _orderService.Create(order);
        }

        private Payment PaymentProccess(OrderModel model)
        {
            Options options = new Options()
            {
                BaseUrl = "https://sandbox-api.iyzipay.com",
                ApiKey = "sandbox-cNnJEaoyNt0sCREL4nOq8PajTLQwWeXz",
                SecretKey = "sandbox-cmJxJfaGlVarqNV3c5ZQcMTwVNh8qswx"
            };

            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0];
            request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0];
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = model.CartModel.CartId.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();


            PaymentCard paymentCard = new PaymentCard()
            {
                CardHolderName = model.CardName,
                CardNumber = model.CardNumber,
                ExpireMonth = model.ExprationMonth,
                ExpireYear = model.ExprationYear,
                Cvc = model.CVV,
                RegisterCard = 0
            };

            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer()
            {
                Id = _userManager.GetUserId(User),
                Name = model.FirstName,
                Surname = model.LastName,
                GsmNumber = model.Phone,
                Email = model.Email,
                IdentityNumber = "11111111111", 
                RegistrationAddress = model.Address,
                Ip = externalIp.ToString(),
                City = model.City,
                Country = "TURKEY",
                ZipCode = "34000"
            };

            request.Buyer = buyer;

            Address shippingAddress = new Address()
            {
                ContactName = model.FirstName,
                City = model.City,
                Country = "TURKEY",
                Description = model.Address,
                ZipCode = "34000"
            };

            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address()
            {
                ContactName = model.FirstName,
                City = model.City,
                Country = "TURKEY",
                Description = model.Address,
                ZipCode = "34000"
            };

            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var cartItem in model.CartModel.CartItems)
            {
                basketItem = new BasketItem()
                {
                    Id = cartItem.ProductId.ToString(),
                    Name = cartItem.Name,
                    Category1 = _productService.GetProductDetail(cartItem.ProductId).ProductCategories.FirstOrDefault().CategoryId.ToString(),
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (cartItem.Price * cartItem.Quantity).ToString().Split(",")[0]
                };
                basketItems.Add(basketItem);

            }

            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request,options);

            return payment;

        }

        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            var orderListModel = new List<OrderListModel>();

            OrderListModel orderModel;

            foreach (var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.OrderNote = order.OrderNote;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentTypes = order.PaymentTypes;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;   
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.Images[0].ImageUrl
                }).ToList();    

                orderListModel.Add(orderModel);
            }

            return View(orderListModel);
        }
    }
}
