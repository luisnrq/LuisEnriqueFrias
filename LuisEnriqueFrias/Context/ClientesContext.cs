using LuisEnriqueFrias.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region PROCEDURES
/*
CREATE PROCEDURE mostrarCliente(@empresa nvarchar(50))
AS
	SELECT * FROM clientes where Empresa = @empresa
GO

CREATE PROCEDURE pedidosCliente(@codCliente nvarchar(50))
AS
	SELECT * FROM pedidos WHERE CodigoCliente = @codCliente
GO

CREATE PROCEDURE selecionarPedidos(@nombreCliente nvarchar(50), @codPedido nvarchar(100))
AS
	declare @codCliente nvarchar(50)
	select @codCliente = CodigoCliente FROM clientes WHERE empresa = @nombreCliente
	SELECT * FROM pedidos WHERE CodigoCliente = @codCliente AND CodigoPedido = @codPedido
GO

CREATE PROCEDURE eliminarPedido(@nombreCliente nvarchar(50), @codPedido nvarchar(100))
AS
	declare @codCliente nvarchar(50)
	select @codCliente = CodigoCliente FROM clientes WHERE empresa = @nombreCliente
	delete from pedidos where CodigoCliente = @codCliente AND CodigoPedido = @codPedido
GO
 */
#endregion

namespace LuisEnriqueFrias.Context
{
    public class ClientesContext
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public ClientesContext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json");
            IConfigurationRoot config = builder.Build();
            string cadenaconexion = config["CadenaHospitalTajamar"];
            this.cn = new SqlConnection(cadenaconexion);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

        }

        public List<Cliente> GetClientes()
        {
            string sql = "SELECT * FROM clientes";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Cliente> clientes = new List<Cliente>();
            while (this.reader.Read())
            {
                Cliente cl = new Cliente();
                string empresa = this.reader["Empresa"].ToString();
                cl.Contacto = empresa;
                clientes.Add(cl);
            }

            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public Cliente GetCliente(string empresa)
        {
            string sql = "mostrarCliente";
            this.com.Parameters.AddWithValue("@empresa", empresa);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            Cliente cl = new Cliente();
            while (this.reader.Read())
            {
                cl.CodigoCliente = this.reader["CodigoCliente"].ToString();
                cl.Empresa = this.reader["Empresa"].ToString();
                cl.Contacto = this.reader["Contacto"].ToString();
                cl.Cargo = this.reader["Cargo"].ToString();
                cl.Ciudad = this.reader["Ciudad"].ToString();
                cl.Telefono = this.reader["Telefono"].ToString();
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return cl;
        }

        public List<Pedido> GetPedidos(string codigoCliente)
        {
            string sql = "pedidosCliente";
            this.com.Parameters.AddWithValue("@codCliente", codigoCliente);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<Pedido> pedidos = new List<Pedido>();
            while (this.reader.Read())
            {
                Pedido pedido = new Pedido();
                pedido.CodigoPedido = this.reader["CodigoPedido"].ToString();
                pedido.CodigoCliente = this.reader["CodigoCliente"].ToString();
                pedido.FechaEntrega = this.reader["FechaEntrega"].ToString();
                pedido.FormaEnvio = this.reader["FormaEnvio"].ToString();
                pedido.Importe = int.Parse(this.reader["Importe"].ToString());
                pedidos.Add(pedido);
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public int Updatecliente(string clienteAEditar, string empresa, string contacto, string cargo, string ciudad, string telefono)
        {
            string sql = "update clientes set Empresa = @Empresa, Contacto = @contacto, Cargo = @cargo, Ciudad = @ciudad, telefono = @telefono WHERE Empresa = @clienteAEditar";
            this.com.Parameters.AddWithValue("@Empresa", empresa);
            this.com.Parameters.AddWithValue("@contacto", contacto);
            this.com.Parameters.AddWithValue("@cargo", cargo);
            this.com.Parameters.AddWithValue("@ciudad", ciudad);
            this.com.Parameters.AddWithValue("@telefono", telefono);
            this.com.Parameters.AddWithValue("@clienteAEditar", clienteAEditar);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int modificados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return modificados;
        }

        public Pedido GetPedido(string empresa, string pedido)
        {
            string sql = "selecionarPedidos";
            this.com.Parameters.AddWithValue("@nombreCliente", empresa);
            this.com.Parameters.AddWithValue("@codPedido", pedido);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            Pedido pd = new Pedido();
            while (this.reader.Read())
            {
                pd.CodigoPedido = this.reader["CodigoPedido"].ToString();
                pd.CodigoCliente = this.reader["CodigoCliente"].ToString();
                pd.FechaEntrega = this.reader["FechaEntrega"].ToString();
                pd.FormaEnvio = this.reader["FormaEnvio"].ToString();
                pd.Importe = int.Parse(this.reader["Importe"].ToString());
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pd;
        }

        public int EliminarPedido(string pedido, string empresa)
        {
            string sql = "eliminarPedido";
            this.com.Parameters.AddWithValue("@pedido", pedido);
            this.com.Parameters.AddWithValue("@empresa", empresa);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();

            int eliminados = this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();

            return eliminados;
        }
    }
}
