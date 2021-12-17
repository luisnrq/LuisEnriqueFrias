using LuisEnriqueFrias.Context;
using LuisEnriqueFrias.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        ClientesContext context;
        public FormPractica()
        {
            InitializeComponent();
            this.context = new ClientesContext();
            this.CargarClientes();
        }

        public void CargarClientes()
        {
            this.cmbclientes.Items.Clear();
            List<Cliente> clientes = this.context.GetClientes();
            foreach(Cliente cl in clientes)
            {
                this.cmbclientes.Items.Add(cl.Contacto);
            }
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            string clienteAEditar = this.cmbclientes.SelectedItem.ToString();
            string empresa = this.txtempresa.Text;
            string contacto = this.txtcontacto.Text;
            string cargo = this.txtcargo.Text;
            string ciudad = this.txtciudad.Text;
            string telefono = this.txttelefono.Text;
            this.context.Updatecliente(clienteAEditar, empresa,contacto,cargo,ciudad,telefono);
            MessageBox.Show("Cliente Editado!");
            this.CargarClientes();

        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empresa = this.cmbclientes.SelectedItem.ToString();
            Cliente cl = this.context.GetCliente(empresa);
            this.txtempresa.Text = cl.Empresa;
            this.txtcontacto.Text = cl.Contacto;
            this.txtcargo.Text = cl.Cargo;
            this.txtciudad.Text = cl.Ciudad;
            this.txttelefono.Text = cl.Telefono;

            this.lstpedidos.Items.Clear();

            List<Pedido> pedidos = this.context.GetPedidos(cl.CodigoCliente);

            foreach (Pedido pd in pedidos)
            {
                this.lstpedidos.Items.Add(pd.CodigoPedido);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pedido = this.lstpedidos.SelectedItem.ToString();
            string empresa = this.txtempresa.Text;
            Pedido pd = this.context.GetPedido(empresa, pedido);
            this.txtcodigopedido.Text = pd.CodigoPedido;
            this.txtfechaentrega.Text = pd.FechaEntrega;
            this.txtformaenvio.Text = pd.FormaEnvio;
            this.txtimporte.Text = pd.Importe.ToString();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string pedido = this.lstpedidos.SelectedItem.ToString();
            string empresa = this.txtempresa.Text;
            this.context.EliminarPedido(pedido, empresa);
            //this.lstpedidos.Items.Clear();
        }

        


    }
}
