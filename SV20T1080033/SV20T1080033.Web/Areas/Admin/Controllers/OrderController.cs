using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV20T1080033.BusinessLayer;
using SV20T1080033.BusinessLayers;
using SV20T1080033.DomainModels;
using SV20T1080033.Web;
using SV20T1080033.Web.AppCodes;
using SV20T1080033.Web.Models;
using System.Drawing.Printing;
using System.Reflection;

namespace LiteCommerce.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]// chuyển đến đăng nhập
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    public class OrderController : Controller
    {
        private const string Product_Search = "Product_Search";
        private const string Shopping_Cart = "Shopping_Cart";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const string Order_Search = "Order_Search";
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// Hiển thị danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchOrderInput>(Order_Search);
            if (input == null)
            {
                input = new PaginationSearchOrderInput()
                {
                    Page = 1,
                    PageSize = Page_Size,
                    SearchValue = "",
                    Status = 0,
                };
            }

            return View(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult Search(PaginationSearchOrderInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(input.Page, input.PageSize, input.Status, input.SearchValue, out rowCount);
            var model = new PaginationSearchOrder()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };
            ApplicationContext.SetSessionData(Order_Search, input);//lưu lại điều kiện tìm kiếm

            return View(model);
        }

        /// <summary>
        /// Giao diện trang tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }
        /// <summary>
        /// Giao diện trang chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id = 0)
        {
            //Code chức năng lấy và hiển thị thông tin của đơn hàng và chi tiết của đơn hàng
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            // lấy thông tin của một đơn hàng và chi tiết đơn hàng đó theo mã đơn hàng
            Order order = OrderDataService.GetOrder(id);
            List<OrderDetail> orderDetails = OrderDataService.ListOrderDetails(id);

            PaginationSearchOrderDetail result = new PaginationSearchOrderDetail()
            {
                Order = order,
                OrderDetails = orderDetails
            };
            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(result);
        }
        /// <summary>
        /// Giao diện cập nhật thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>        
        [HttpGet]
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            //Code chức năng để lấy chi tiết đơn hàng cần edit
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productId < 0)
            {
                return RedirectToAction("Details", new {id = productId });
            }
            OrderDetail orderDetail = OrderDataService.GetOrderDetail(id, productId);
            if (orderDetail == null)
            {
                return RedirectToAction("Index");
            }
            return View(orderDetail);
        }

        public IActionResult CreateDetail(int id = 0)
        {
           var model = new OrderDetail()
           {
               OrderID = id,
               ProductID = 0
           };
            return View(model);
        }


        /// <summary>
        /// Cập nhật chi tiết đơn hàng (trong giỏ hàng)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        [HttpPost]         
        public IActionResult UpdateDetail(OrderDetail data)
        {
            // mã đặt hàng
            if (data.ProductID <= 0)
            {
                TempData[ERROR_MESSAGE] = "mã đặt hàng không tồn tại";
                return RedirectToAction("Details", new {id = data.OrderID });
            }
            // Số lượng
            if (data.Quantity < 1)
            {
                TempData[ERROR_MESSAGE] = "Số lượng không tồn tại";
                return RedirectToAction("Details", new {id = data.OrderID });
            }

            // Đơn giá
            if (data.SalePrice < 1)
            {
                TempData[ERROR_MESSAGE] = "Đơn giá không tồn tại";
                return RedirectToAction("Details", new { id = data.OrderID });
            }

            // Cập nhật chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            OrderDataService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);
            return RedirectToAction("Details", new { id = data.OrderID });
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>        
        public IActionResult DeleteDetail(int id = 0, int productID = 0)
        {
            //DONE: Code chức năng xóa 1 chi tiết trong đơn hàng
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (productID < 0)
            {
                return RedirectToAction("Details", new { id = id });
            }

            // Xoá chi tiết 1 đơn hàng nếu kiểm tra đúng hết
            bool isDeleted = OrderDataService.DeleteOrderDetail(id, productID);
            if (!isDeleted)
            {
                TempData[ERROR_MESSAGE] = "Không thể xoá mặt hàng này";
                return RedirectToAction("Details", new { id = id });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            //DONE: Code chức năng để xóa đơn hàng (nếu được phép xóa)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            // Xoá đơn hàng ở trạng thái vừa tạo, bị huỷ hoặc bị từ chối
            if (data.Status == OrderStatus.INIT
                || data.Status == OrderStatus.CANCEL
                || data.Status == OrderStatus.REJECTED)
            {
                OrderDataService.DeleteOrder(id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new {id = id});
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            //TODO: Duyệt chấp nhận đơn hàng

            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            bool isAccepted = OrderDataService.AcceptOrder(id);
            if (!isAccepted)
            {
                TempData[ERROR_MESSAGE] = $"Chấp nhận đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction($"Details/{data.OrderID}");
            }
            return RedirectToAction("Details", new {id = id});
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Shipping(int id = 0, int shipperID = 0, int countProducts = 0)
        {
            //Code chức năng chuyển đơn hàng sang trạng thái đang giao hàng (nếu được phép)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }
            if (Request.Method == "GET")
            {
                ViewBag.OrderID = id;
                ViewBag.CountProducts = countProducts;
                return View();
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            if (shipperID <= 0)
            {
                TempData[ERROR_MESSAGE] = "Bạn phải chọn đơn vị vận chuyển";
                return RedirectToAction("Details", new {id = id});
            }
            if (countProducts <= 0)
            {
                TempData[ERROR_MESSAGE] = "Không có mặt hàng nào để chuyển giao";
                return RedirectToAction("Details", new { id = id });
            }
            bool isShipped = OrderDataService.ShipOrder(id, shipperID);
            if (!isShipped)
            {
                TempData[ERROR_MESSAGE] = $"Xác nhận chuyển đơn hàng cho người giao hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            //Code chức năng ghi nhận hoàn tất đơn hàng (nếu được phép)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            bool isFinished = OrderDataService.FinishOrder(id);
            if (!isFinished)
            {
                TempData[ERROR_MESSAGE] = $"Xác nhận hoàn tất đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details",new {id = data.OrderID});
            }
            return RedirectToAction("Details", new {id = id});
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            //Code chức năng hủy đơn hàng (nếu được phép)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            bool isCanceled = OrderDataService.CancelOrder(id);
            if (!isCanceled)
            {
                TempData[ERROR_MESSAGE] = $"Hủy bỏ đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            //Code chức năng từ chối đơn hàng (nếu được phép)
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            Order data = OrderDataService.GetOrder(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }

            bool isRejected = OrderDataService.RejectOrder(id);
            if (!isRejected)
            {
                TempData[ERROR_MESSAGE] = $"Từ chối đơn hàng thất bại vì trạng thái đơn hàng hiện tại là: {data.StatusDescription}";
                return RedirectToAction("Details", new { id = data.OrderID });
            }
            return RedirectToAction("Details", new { id = id });
        }

        /// <summary>
        /// Sử dụng 1 biến session để lưu tạm giỏ hàng (danh sách các chi tiết của đơn hàng) trong quá trình xử lý.
        /// Hàm này lấy giỏ hàng hiện đang có trong session (nếu chưa có thì tạo mới giỏ hàng rỗng)
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {
            List<OrderDetail> shoppingCart = ApplicationContext.GetSessionData<List<OrderDetail>>(Shopping_Cart) ;
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                ApplicationContext.SetSessionData(Shopping_Cart, shoppingCart);
            }
            return shoppingCart;
        }

        /// <summary>
        /// Lấy danh sách mặt hàng để hiển thị ở trang tạo đơn hàng
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public IActionResult SearchProducts(int page = 1, string searchValue = "")
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchProductInput>(Product_Search);
            if (input == null)
            {
                input = new PaginationSearchProductInput()
                {
                    Page = 1,
                    PageSize = Page_Size,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0,
                };
            }
            int rowCount = 0;
            var data = ProductDataService.ListProducts(page, Page_Size, searchValue, 0, 0, 0, 0, out rowCount);
            ViewBag.Page = page;
            return View(data);
        }
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(OrderDetail data)
        {
            if (data == null)
            {
                TempData[ERROR_MESSAGE] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Create");
            }
            if (data.SalePrice <= 0 || data.Quantity <= 0)
            {
                TempData[ERROR_MESSAGE] = "Giá bán và số lượng không hợp lệ";
                return RedirectToAction("Create");
            }

            List<OrderDetail> shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == data.ProductID);

            if (existsProduct == null) //Nếu mặt hàng cần được bổ sung chưa có trong giỏ hàng thì bổ sung vào giỏ
            {
                shoppingCart.Add(data);
            }
            else //Trường hợp mặt hàng cần bổ sung đã có thì tăng số lượng và thay đổi đơn giá
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice = data.SalePrice;
            }
            ApplicationContext.SetSessionData(Shopping_Cart, shoppingCart);
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>        
        /// <returns></returns>
        public ActionResult RemoveFromCart(int id = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(Shopping_Cart, shoppingCart);
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa toàn bộ dữ liệu trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(Shopping_Cart, shoppingCart);
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Khởi tạo đơn hàng và chuyển đến trang Details sau khi khởi tạo xong để tiếp tục quá trình xử lý đơn hàng
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            if (customerID == 0 || employeeID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng và nhân viên phụ trách";
                return RedirectToAction("Create");
            }

            int orderID = OrderDataService.InitOrder(customerID, employeeID, DateTime.Now, shoppingCart);

            HttpContext.Session.Remove("Shopping_Cart");
            return RedirectToAction("Details", new {id = orderID});
        }
    }
}
