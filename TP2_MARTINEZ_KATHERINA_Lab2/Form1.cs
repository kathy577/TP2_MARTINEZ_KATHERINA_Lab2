using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TP2_MARTINEZ_KATHERINA_Lab2
{
    public partial class frmRegistro : Form
    {
        public frmRegistro()
        {
            InitializeComponent();
        }

        List<Producto> productos = new List<Producto>();
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = File.Exists("productos.txt")
                   ? File.ReadAllLines("productos.txt").Length + 1 : 1;
             
                string nombre = txtNombre.Text;
                double precio = double.Parse(txtPrecio.Text);
                int stock = int.Parse(txtStock.Text);

                Producto p = new Producto(id, nombre, precio, stock);
                File.AppendAllText("productos.txt", p.ToString() + Environment.NewLine);
                
                foreach (var linea in File.ReadAllLines("productos.txt"))

               txtNombre.Clear();
                txtPrecio.Clear();
                txtStock.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message);



            }
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            productos.Clear();


            foreach (var linea in File.ReadAllLines("productos.txt"))
            {
                if (!string.IsNullOrWhiteSpace(linea))
                {
                    var datos = linea.Split(',');
                    productos.Add(new Producto(
                        int.Parse(datos[0]),
                        datos[1],
                        double.Parse(datos[2]),
                        int.Parse(datos[3])
                    ));

                }
            }
            dgvProductos.Rows.Clear();
            foreach (var p in productos)
            {
                dgvProductos.Rows.Add(p.Id, p.Nombre, p.Precio, p.Stock);
            }

            double total = productos.Sum(p => p.Precio * p.Stock);
            lblTotal.Text = total.ToString("N2");
        
            var productoCaro = productos.OrderByDescending(p => p.Precio).FirstOrDefault();
            if (productoCaro != null)
            {
                lblProductoCaro.Text = productoCaro.Nombre + " $" + productoCaro.Precio.ToString("N2") + "";
            }

            var productoMasStock = productos.OrderByDescending(p => p.Stock).FirstOrDefault();
            if (productoMasStock != null)
            {
                lblProductoMasStock.Text =  productoMasStock.Nombre + " Stock: " + productoMasStock.Stock + "";
            }

        }

        private void frmRegistro_Load(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            File.WriteAllText("productos.txt", string.Empty);
            productos.Clear();
            dgvProductos.Rows.Clear();
            lblTotal.Text = "0.00";
            lblProductoCaro.Text = "...";
            lblProductoMasStock.Text = "...";
        }
    }
}