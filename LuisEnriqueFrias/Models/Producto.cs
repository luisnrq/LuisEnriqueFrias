using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuisEnriqueFrias.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public String Nombre { get; set; }
        public String Precio { get; set; }
        public int Stock { get; set; }
    }
}
